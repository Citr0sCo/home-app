using HomeBoxLanding.Api.Core.Types;
using HomeBoxLanding.Api.Data;
using HomeBoxLanding.Api.Features.Links.Types;

namespace HomeBoxLanding.Api.Features.Links
{
    public class LinksRepository
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
    }
}