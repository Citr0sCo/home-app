using HomeBoxLanding.Api.Core.Types;
using HomeBoxLanding.Api.Data;
using HomeBoxLanding.Api.Features.Links.Types;
using Microsoft.EntityFrameworkCore;

namespace HomeBoxLanding.Api.Features.Links
{
    public interface ILinksRepository
    {
        List<LinkRecord> GetAll();
        AddLinkResponse AddLink(AddLinkRequest request);
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
}