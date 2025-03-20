using HomeBoxLanding.Api.Core.Types;
using HomeBoxLanding.Api.Features.Links.Types;
using Minio;
using Minio.DataModel.Args;

namespace HomeBoxLanding.Api.Features.Links;

public class LinksService
{
    private readonly ILinksRepository _linksRepository;
    private readonly IMinioClient _minioClient;

    private readonly string? _bucketName;
    private readonly string? _cdnUrl;

    public LinksService(ILinksRepository linksRepository, IMinioClient minioClient)
    {
        _linksRepository = linksRepository;
        _minioClient = minioClient;

        _cdnUrl = Environment.GetEnvironmentVariable("MINIO_CDN_URL");
        _bucketName = Environment.GetEnvironmentVariable("ASPNETCORE_MINIO_BUCKET_NAME");
    }

    public LinksResponse GetAllLinks()
    {
        var links = _linksRepository.GetAll();

        return new LinksResponse
        {
            Links = links.ConvertAll(LinkMapper.Map)
        };
    }

    public ImportLinksResponse ImportLinks(ImportLinksRequest request)
    {
        var response = new ImportLinksResponse();

        var addLinkResponse = _linksRepository.ImportLinks(request);

        if (addLinkResponse.HasError)
        {
            response.AddError(addLinkResponse.Error);
            return response;
        }

        response.Links = addLinkResponse.Links;
        return response;
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

        response.Link = addLinkResponse.Link;
        return response;
    }

    public UpdateLinkResponse UpdateLink(UpdateLinkRequest request)
    {
        var response = new UpdateLinkResponse();

        var updateLinkResponse = _linksRepository.UpdateLink(request);

        if (updateLinkResponse.HasError)
        {
            response.AddError(updateLinkResponse.Error);
            return response;
        }

        response.Link = updateLinkResponse.Link;
        return response;
    }

    public CommunicationResponse DeleteLink(Guid linkReference)
    {
        var response = new CommunicationResponse();

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

    public async Task<UploadLinkLogoResponse> UploadLogo(Guid linkReference, IFormFileCollection request)
    {
        var response = new UploadLinkLogoResponse();

        if (request.Count == 0)
        {
            response.AddError(new Error
            {
                Code = ErrorCode.DatabaseError,
                UserMessage = "Something went wrong attempting to save a link.",
                TechnicalMessage = "Something went wrong attempting to save a link."
            });
            return response;
        }

        var existingLink = _linksRepository.GetLinkByReference(linkReference);

        if (existingLink == null)
        {
            response.AddError(new Error
            {
                Code = ErrorCode.DatabaseError,
                UserMessage = "Something went wrong attempting to save a link.",
                TechnicalMessage = "Something went wrong attempting to save a link."
            });
            return response;
        }
        
        var beArgs = new BucketExistsArgs()
            .WithBucket(_bucketName);
        
        var bucketExists = await _minioClient.BucketExistsAsync(beArgs).ConfigureAwait(false);

        if (!bucketExists)
        {
            response.AddError(new Error
            {
                Code = ErrorCode.DatabaseError,
                UserMessage = "Something went wrong attempting to save a link.",
                TechnicalMessage = "Something went wrong attempting to save a link."
            });
            return response;
        }

        var file = request.First();

        var newFileLink = string.Empty;

        using (var stream = file.OpenReadStream())
        {
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject($"public/images/app-logos/{Guid.NewGuid()}-{file.FileName}")
                .WithStreamData(stream)
                .WithObjectSize(file.Length)
                .WithContentType(file.ContentType);

            var saveFileResponse = await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);

            newFileLink = $"{_cdnUrl}/{_bucketName}/{saveFileResponse.ObjectName}";
        }

        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(existingLink.IconUrl.Replace(_cdnUrl + "/", "").Replace(_bucketName + "/", ""));
        
        await _minioClient.RemoveObjectAsync(removeObjectArgs).ConfigureAwait(false);

        existingLink.IconUrl = newFileLink;

        var updateLinkResponse = _linksRepository.UpdateLink(new UpdateLinkRequest
        {
            Link = LinkMapper.Map(existingLink)
        });

        if (updateLinkResponse == null)
        {
            response.AddError(new Error
            {
                Code = ErrorCode.DatabaseError,
                UserMessage = "Something went wrong attempting to save a link.",
                TechnicalMessage = "Something went wrong attempting to save a link."
            });
            return response;
        }

        response.IconUrl = existingLink.IconUrl;
        return response;
    }
}

public class LinkMapper
{
    public static LinkRecord Map(Link link)
    {
        return new LinkRecord
        {
            Identifier = link.Identifier ?? Guid.Empty,
            Name = link.Name,
            IconUrl = link.IconUrl,
            IsSecure = link.IsSecure,
            Port = link.Port,
            Host = link.Host,
            Url = link.Url,
            Category = link.Category,
            SortOrder = link.SortOrder
        };
    }

    public static Link Map(LinkRecord record)
    {
        return new Link
        {
            Identifier = record.Identifier,
            Name = record.Name,
            IconUrl = record.IconUrl,
            IsSecure = record.IsSecure,
            Port = record.Port,
            Host = record.Host,
            Url = record.Url,
            Category = record.Category,
            SortOrder = record.SortOrder
        };
    }
}