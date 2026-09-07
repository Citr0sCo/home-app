using HomeBoxLanding.Api.Data;
using HomeBoxLanding.Api.Features.CustomLinkWidgets.Types;
using Microsoft.EntityFrameworkCore;

namespace HomeBoxLanding.Api.Features.CustomLinkWidgets;

public interface IWidgetCacheRepository
{
    WidgetCacheRecord? Get(Guid linkIdentifier, string widgetType);
    void Save(WidgetCacheRecord record);
}

public class WidgetCacheRepository : IWidgetCacheRepository
{
    public WidgetCacheRecord? Get(Guid linkIdentifier, string widgetType)
    {
        using var context = new DatabaseContext();
        return context.WidgetCache
            .AsNoTracking()
            .FirstOrDefault(record => record.LinkIdentifier == linkIdentifier && record.WidgetType == widgetType);
    }

    public void Save(WidgetCacheRecord record)
    {
        using var context = new DatabaseContext();
        var existingRecord = context.WidgetCache
            .FirstOrDefault(item => item.LinkIdentifier == record.LinkIdentifier && item.WidgetType == record.WidgetType);

        if (existingRecord is null)
        {
            context.WidgetCache.Add(record);
        }
        else
        {
            existingRecord.Value = record.Value;
            existingRecord.UpdatedAt = record.UpdatedAt;
        }

        context.SaveChanges();
    }
}
