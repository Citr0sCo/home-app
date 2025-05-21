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

    public LinksService(ILinksRepository linksRepository)
    {
        _linksRepository = linksRepository;
        
        _minioClient = new MinioClient()
            .WithEndpoint(Environment.GetEnvironmentVariable("ASPNETCORE_MINIO_ENDPOINT"))
            .WithCredentials(Environment.GetEnvironmentVariable("ASPNETCORE_MINIO_ACCESS_KEY"), Environment.GetEnvironmentVariable("ASPNETCORE_MINIO_SECRET_KEY"))
            .WithSSL()
            .Build();

        _cdnUrl = Environment.GetEnvironmentVariable("ASPNETCORE_MINIO_CDN_URL");
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

    public Link? GetLinkByReference(Guid linkReference)
    {
        var link = _linksRepository.GetLinkByReference(linkReference);

        if (link == null)
            return null;

        return LinkMapper.Map(link);
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

    public async Task<CommunicationResponse> DeleteLink(Guid linkReference)
    {
        var response = new CommunicationResponse();

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
        
        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(existingLink.IconUrl.Replace(_cdnUrl + "/", "").Replace(_bucketName + "/", ""));
        
        await _minioClient.RemoveObjectAsync(removeObjectArgs).ConfigureAwait(false);

        var deleteLinkResponse = _linksRepository.DeleteLink(linkReference);

        if (deleteLinkResponse == null)
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