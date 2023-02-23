using HomeBoxLanding.Api.Data;
using HomeBoxLanding.Api.Features.Builds.Types;

namespace HomeBoxLanding.Api.Features.Builds
{
    public class BuildsRepository
    {
        public List<BuildRecord> GetAll()
        {
            using (var context = new DatabaseContext())
            {
                try
                {
                    return context.Builds
                        .ToList();
                }
                catch (Exception exception)
                {
                    return new List<BuildRecord>();
                }
            }
        }
    }
}