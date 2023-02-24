using HomeBoxLanding.Api.Core.Types;
using HomeBoxLanding.Api.Data;
using HomeBoxLanding.Api.Features.Links.Types;

namespace HomeBoxLanding.Api.Features.Links
{
    public interface ILinksRepository
    {
        List<LinkRecord> GetAll();
        LinkRecord? AddLink(AddLinkRequest request);
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

        public LinkRecord AddLink(AddLinkRequest request)
        {
            var link = request.Link;

            using (var context = new DatabaseContext())
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

                    return linkRecord;
                }
                catch (Exception exception)
                {
                    return null;
                }
            }
        }
    }
}