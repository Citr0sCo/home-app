using HomeBoxLanding.Api.Features.Links.Types;

namespace HomeBoxLanding.Api.Features.Links;

public class LinksService
{
    private readonly LinksRepository _linksRepository;

    public LinksService(LinksRepository linksRepository)
    {
        _linksRepository = linksRepository;
    }
    
    public LinksResponse GetAllLinks()
    {
        var links = _linksRepository.GetAll();

        return new LinksResponse
        {
            Links = links.ConvertAll(x => new Link
            {
                Identifier = x.Identifier,
                Name = x.Name,
                IconUrl = x.IconUrl,
                IsSecure = x.IsSecure,
                Port = x.Port,
                Host = x.Host,
                Url = x.Url,
                Category = x.Category
            })
        };
    }
}
