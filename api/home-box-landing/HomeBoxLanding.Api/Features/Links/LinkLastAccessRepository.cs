using HomeBoxLanding.Api.Data;
using HomeBoxLanding.Api.Features.Links.Types;
using Microsoft.EntityFrameworkCore;

namespace HomeBoxLanding.Api.Features.Links;

public interface ILinkLastAccessRepository
{
    Task<List<LinkLastAccessRecord>> GetAllAsync();
    Task RecordAsync(Guid linkIdentifier, DateTime lastAccessedAt);
}

public class LinkLastAccessRepository : ILinkLastAccessRepository
{
    public async Task<List<LinkLastAccessRecord>> GetAllAsync()
    {
        await using var context = new DatabaseContext();
        return await context.LinkLastAccesses
            .AsNoTracking()
            .Join(context.Links, access => access.LinkIdentifier, link => link.Identifier, (access, link) => access)
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task RecordAsync(Guid linkIdentifier, DateTime lastAccessedAt)
    {
        await using var context = new DatabaseContext();
        var record = await context.LinkLastAccesses.FindAsync(linkIdentifier).ConfigureAwait(false);

        if (record is null)
        {
            context.LinkLastAccesses.Add(new LinkLastAccessRecord
            {
                LinkIdentifier = linkIdentifier,
                LastAccessedAt = lastAccessedAt
            });
        }
        else
        {
            record.LastAccessedAt = lastAccessedAt;
        }

        await context.SaveChangesAsync().ConfigureAwait(false);
    }
}
