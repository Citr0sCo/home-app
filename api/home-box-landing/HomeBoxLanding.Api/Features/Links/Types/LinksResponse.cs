namespace HomeBoxLanding.Api.Features.Links.Types
{
    public class LinksResponse
    {
        public LinksResponse()
        {
            Links = new Dictionary<string, List<Link>>();
        }
        
        public Dictionary<string, List<Link>> Links { get; set; }
    }
}