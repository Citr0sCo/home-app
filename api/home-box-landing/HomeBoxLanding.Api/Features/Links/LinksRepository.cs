using HomeBoxLanding.Api.Core.Types;
using HomeBoxLanding.Api.Data;
using HomeBoxLanding.Api.Features.Columns.Types;
using HomeBoxLanding.Api.Features.Links.Types;
using Microsoft.EntityFrameworkCore;

namespace HomeBoxLanding.Api.Features.Links;

public interface ILinksRepository
{
    List<LinkRecord> GetAll();
    LinkRecord? GetLinkByReference(Guid linkReference);
    Task<AddLinkResponse> AddLink(AddLinkRequest request);
    Task<ImportLinksResponse> ImportLinks(ImportLinksRequest request);
    Task<UpdateLinkResponse> UpdateLink(UpdateLinkRequest request);
    Task<CommunicationResponse> DeleteLink(Guid linkReference);
}

public class LinksRepository : ILinksRepository
{
    public List<LinkRecord> GetAll()
    {
        using (var context = new DatabaseContext())
        {
            try
            {
                return context.Links
                    .Include(x => x.Column)
                    .OrderBy(x => x.SortOrder)
                    .ToList();
            }
            catch (Exception exception)
            {
                return new List<LinkRecord>();
            }
        }
    }

    public LinkRecord GetLinkByReference(Guid linkReference)
    {
        using (var context = new DatabaseContext())
        {
            try
            {
                return context.Links
                    .Include(x => x.Column)
                    .FirstOrDefault(x => x.Identifier == linkReference);
            }
            catch (Exception exception)
            {
                return null;
            }
        }
    }

    public async Task<ImportLinksResponse> ImportLinks(ImportLinksRequest request)
    {
        var response = new ImportLinksResponse();

        var links = request.Links;

        await using (var context = new DatabaseContext())
        await using (var transaction = await context.Database.BeginTransactionAsync())
        {
            try
            {
                foreach (var linkRecord in context.Links)
                {
                    context.Remove(linkRecord);
                }

                var createdColumns = new Dictionary<string, ColumnRecord>();

                foreach (var link in links)
                {
                    var columnRecord = context.Columns.FirstOrDefault(x => x.Identifier == link.ColumnId);

                    if (columnRecord == null && link.Category != null)
                    {
                        if (createdColumns.TryGetValue(link.Category, out var column))
                        {
                            columnRecord = column;
                        }
                        else
                        {
                            columnRecord = context.Columns.FirstOrDefault(x => x.Name == link.Category);

                            if (columnRecord == null)
                            {
                                columnRecord = new ColumnRecord
                                {
                                    Identifier = Guid.NewGuid(),
                                    Name = link.Category,
                                    SortOrder = 0,
                                    Icon = "fa-file-alt fas"
                                };

                                createdColumns.Add(link.Category, columnRecord);
                                context.Add(columnRecord);
                            }
                        }
                    }

                    var linkRecord = new LinkRecord
                    {
                        Identifier = Guid.NewGuid(),
                        Name = link.Name,
                        Url = link.Url,
                        Host = link.Host,
                        Port = link.Port,
                        IconUrl = link.IconUrl,
                        IsSecure = link.IsSecure,
                        SortOrder = link.SortOrder,
                        Column = columnRecord
                    };

                    context.Add(linkRecord);
                }

                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                response.Links = links;
                return response;
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                response.AddError(new Error
                {
                    Code = ErrorCode.DatabaseError,
                    UserMessage = "Something went wrong attempting to update a build log.",
                    TechnicalMessage = $"The following exception was thrown: {exception.Message}"
                });
                return response;
            }
        }
    }

    public async Task<AddLinkResponse> AddLink(AddLinkRequest request)
    {
        var response = new AddLinkResponse();

        var link = request.Link;

        await using (var context = new DatabaseContext())
        await using (var transaction = await context.Database.BeginTransactionAsync())
        {
            try
            {
                var columnRecord = context.Columns.FirstOrDefault(x => x.Identifier == link.ColumnId);

                var linkRecord = new LinkRecord
                {
                    Identifier = Guid.NewGuid(),
                    Name = link.Name,
                    Url = link.Url,
                    Host = link.Host,
                    Port = link.Port,
                    IconUrl = link.IconUrl,
                    IsSecure = link.IsSecure,
                    SortOrder = link.SortOrder,
                    Column = columnRecord
                };

                context.Add(linkRecord);

                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                response.Link = response.Link = LinkMapper.Map(linkRecord);
                return response;
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                response.AddError(new Error
                {
                    Code = ErrorCode.DatabaseError,
                    UserMessage = "Something went wrong attempting to update a build log.",
                    TechnicalMessage = $"The following exception was thrown: {exception.Message}"
                });
                return response;
            }
        }
    }

    public async Task<UpdateLinkResponse> UpdateLink(UpdateLinkRequest request)
    {
        var response = new UpdateLinkResponse();

        var link = request.Link;

        await using (var context = new DatabaseContext())
        await using (var transaction = await context.Database.BeginTransactionAsync())
        {
            try
            {
                var linkRecord = context.Links
                    .Include(x => x.Column)
                    .FirstOrDefault(x => x.Identifier == request.Link.Identifier);

                if (linkRecord == null)
                {
                    response.AddError(new Error
                    {
                        Code = ErrorCode.DatabaseError,
                        UserMessage = "Something went wrong attempting to save a link.",
                        TechnicalMessage = "Something went wrong attempting to save a link."
                    });
                    return response;
                }

                if (link.Name.Length > 0 && link.Name != linkRecord.Name)
                    linkRecord.Name = link.Name;

                if (link.Url.Length > 0 && link.Url != linkRecord.Url)
                    linkRecord.Url = link.Url;

                if (link.Host.Length > 0 && link.Host != linkRecord.Host)
                    linkRecord.Host = link.Host;

                if (link.Port > 0 && link.Port != linkRecord.Port)
                    linkRecord.Port = link.Port;

                if (link.IconUrl.Length > 0 && link.IconUrl != linkRecord.IconUrl)
                    linkRecord.IconUrl = link.IconUrl;

                if (link.IsSecure != linkRecord.IsSecure)
                    linkRecord.IsSecure = link.IsSecure;

                context.Update(linkRecord);

                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                response.Link = LinkMapper.Map(linkRecord);
                return response;
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                response.AddError(new Error
                {
                    Code = ErrorCode.DatabaseError,
                    UserMessage = "Something went wrong attempting to update a link log.",
                    TechnicalMessage = $"The following exception was thrown: {exception.Message}"
                });
                return response;
            }
        }
    }

    public async Task<CommunicationResponse> DeleteLink(Guid linkReference)
    {
        var response = new CommunicationResponse();

        await using (var context = new DatabaseContext())
        await using (var transaction = await context.Database.BeginTransactionAsync())
        {
            try
            {
                var linkRecord = context.Links.FirstOrDefault(x => x.Identifier == linkReference);

                if (linkRecord == null)
                {
                    response.AddError(new Error
                    {
                        Code = ErrorCode.DatabaseError,
                        UserMessage = "Something went wrong attempting to save a link.",
                        TechnicalMessage = "Something went wrong attempting to save a link."
                    });
                    return response;
                }

                context.Links.Remove(linkRecord);

                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                return response;
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                response.AddError(new Error
                {
                    Code = ErrorCode.DatabaseError,
                    UserMessage = "Something went wrong attempting to update a build log.",
                    TechnicalMessage = $"The following exception was thrown: {exception.Message}"
                });
                return response;
            }
        }
    }
}