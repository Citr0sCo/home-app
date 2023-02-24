using HomeBoxLanding.Api.Core.Types;
using HomeBoxLanding.Api.Features.Links.Types;

namespace HomeBoxLanding.Api.Features.Links;

public class LinksService
{
    private readonly ILinksRepository _linksRepository;

    public LinksService(ILinksRepository linksRepository)
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
                Category = x.Category,
                SortOrder = x.SortOrder
            })
        };
    }
    
    public AddLinkResponse AddLink(AddLinkRequest request)
    {
        var response = new AddLinkResponse();
        
        var addLinkResponse = _linksRepository.AddLink(request);

        if (addLinkResponse.HasError)
        {
            response.AddError(addLinkResponse.Error);
            return response;
        }

        response.Link = new Link
        {
            Identifier = addLinkResponse.Link.Identifier,
            Name = addLinkResponse.Link.Name,
            IconUrl = addLinkResponse.Link.IconUrl,
            IsSecure = addLinkResponse.Link.IsSecure,
            Port = addLinkResponse.Link.Port,
            Host = addLinkResponse.Link.Host,
            Url = addLinkResponse.Link.Url,
            Category = addLinkResponse.Link.Category,
            SortOrder = addLinkResponse.Link.SortOrder
        };
        return response;
    }
    
    public CommunicationResponse DeleteLink(Guid linkReference)
    {
        var response = new AddLinkResponse();
        
        var addLinkResponse = _linksRepository.DeleteLink(linkReference);

        if (addLinkResponse == null)
        {
            response.AddError(new Error
            {
                Code = ErrorCode.DatabaseError,
                UserMessage = "Something went wrong attempting to save a link.",
                TechnicalMessage = "Something went wrong attempting to save a link."
            });
            return response;
        }
        
        return response;
    }
}