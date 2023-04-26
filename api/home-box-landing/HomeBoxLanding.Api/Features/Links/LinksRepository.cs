using HomeBoxLanding.Api.Core.Types;
using HomeBoxLanding.Api.Data;
using HomeBoxLanding.Api.Features.Links.Types;

namespace HomeBoxLanding.Api.Features.Links;

public interface ILinksRepository
{
    List<LinkRecord> GetAll();
    LinkRecord GetLinkByReference(Guid linkReference);
    LinkRecord GetLinkAbove(Guid linkReference);
    LinkRecord GetLinkBelow(Guid linkReference);
    AddLinkResponse AddLink(AddLinkRequest request);
    UpdateLinkResponse UpdateLink(UpdateLinkRequest request);
    CommunicationResponse DeleteLink(Guid linkReference);
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
                    .OrderBy(x => x.Category)
                    .ThenBy(x => x.SortOrder)
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
                    .FirstOrDefault(x => x.Identifier == linkReference);
            }
            catch (Exception exception)
            {
                return null;
            }
        }
    }
    
    public LinkRecord GetLinkAbove(Guid linkReference)
    {
        using (var context = new DatabaseContext())
        {
            try
            {
                return context.Links
                    .Where(x => x.Identifier == linkReference)
                    .FirstOrDefault();
            }
            catch (Exception exception)
            {
                return null;
            }
        }
    }
    
    public LinkRecord GetLinkBelow(Guid linkReference)
    {
        using (var context = new DatabaseContext())
        {
            try
            {
                return context.Links
                    .FirstOrDefault(x => x.Identifier == linkReference);
            }
            catch (Exception exception)
            {
                return null;
            }
        }
    }

    public AddLinkResponse AddLink(AddLinkRequest request)
    {
        var response = new AddLinkResponse();
            
        var link = request.Link;

        using (var context = new DatabaseContext())
        using (var transaction = context.Database.BeginTransaction())
        {
            try
            {
                var linkRecord = new LinkRecord
                {
                    Identifier = Guid.NewGuid(),
                    Name = link.Name,
                    Url = link.Url,
                    Host = link.Host,
                    Port = link.Port,
                    IconUrl = link.IconUrl,
                    IsSecure = link.IsSecure,
                    Category = link.Category,
                    SortOrder = link.SortOrder
                };

                context.Add(linkRecord);
                    
                context.SaveChanges();
                transaction.Commit();

                response.Link = new Link
                {
                    Identifier = linkRecord.Identifier,
                    Name = linkRecord.Name,
                    IconUrl = linkRecord.IconUrl,
                    IsSecure = linkRecord.IsSecure,
                    Port = linkRecord.Port,
                    Host = linkRecord.Host,
                    Url = linkRecord.Url,
                    Category = linkRecord.Category,
                    SortOrder = linkRecord.SortOrder
                };
                return response;
            }
            catch (Exception exception)
            {
                transaction.Rollback();
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

    public UpdateLinkResponse UpdateLink(UpdateLinkRequest request)
    {
        var response = new UpdateLinkResponse();
            
        var link = request.Link;

        using (var context = new DatabaseContext())
        using (var transaction = context.Database.BeginTransaction())
        {
            try
            {
                var linkRecord = context.Links.FirstOrDefault(x => x.Identifier == request.Link.Identifier);
                    
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

                if (link.Category.Length > 0 && link.Category != linkRecord.Category)
                    linkRecord.Category = link.Category;

                if (link.SortOrder != linkRecord.SortOrder)
                    linkRecord.SortOrder = link.SortOrder;
 
                context.Update(linkRecord);
                    
                context.SaveChanges();
                transaction.Commit();

                response.Link = new Link
                {
                    Identifier = linkRecord.Identifier,
                    Name = linkRecord.Name,
                    IconUrl = linkRecord.IconUrl,
                    IsSecure = linkRecord.IsSecure,
                    Port = linkRecord.Port,
                    Host = linkRecord.Host,
                    Url = linkRecord.Url,
                    Category = linkRecord.Category,
                    SortOrder = linkRecord.SortOrder
                };
                return response;
            }
            catch (Exception exception)
            {
                transaction.Rollback();
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

    public CommunicationResponse DeleteLink(Guid linkReference)
    {
        var response = new CommunicationResponse();
            
        using (var context = new DatabaseContext())
        using (var transaction = context.Database.BeginTransaction())
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
                    
                context.SaveChanges();
                transaction.Commit();

                return response;
            }
            catch (Exception exception)
            {
                transaction.Rollback();
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