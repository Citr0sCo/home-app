namespace HomeBoxLanding.Api.Features.Links.Types
{
    public class Link
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool IsSecure { get; set; }
        public string IconUrl { get; set; }
    }
}