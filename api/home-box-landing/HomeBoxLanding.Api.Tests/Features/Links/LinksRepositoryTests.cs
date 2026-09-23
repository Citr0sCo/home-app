using HomeBoxLanding.Api.Data;
using HomeBoxLanding.Api.Features.Columns.Types;
using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.Links.Types;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace HomeBoxLanding.Api.Tests.Features.Links;

[TestFixture]
public class LinksRepositoryTests
{
    private const string DatabasePath = "assets/home-app.db";

    [SetUp]
    public async Task SetUp()
    {
        DeleteDatabase();

        await using var context = new DatabaseContext();
        await context.Database.MigrateAsync();
    }

    [TearDown]
    public void TearDown()
    {
        DeleteDatabase();
    }

    [Test]
    public async Task UpdateLink_UpdatesIpHostWhenOptionalFieldsAreMissing()
    {
        var linkIdentifier = Guid.NewGuid();
        var columnIdentifier = Guid.NewGuid();

        await using (var context = new DatabaseContext())
        {
            var column = new ColumnRecord
            {
                Identifier = columnIdentifier,
                Name = "Services",
                Icon = "server",
                SortOrder = 0
            };

            context.Add(column);
            context.Add(new LinkRecord
            {
                Identifier = linkIdentifier,
                Name = "Router",
                Url = "http://192.168.1.10",
                Host = "192.168.1.10",
                Port = 80,
                IconUrl = null,
                Column = column
            });

            await context.SaveChangesAsync();
        }

        var response = await new LinksRepository().UpdateLink(new UpdateLinkRequest
        {
            Link = new Link
            {
                Identifier = linkIdentifier,
                Host = "192.168.1.11"
            }
        });

        Assert.That(response.HasError, Is.False);
        Assert.That(response.Link?.Host, Is.EqualTo("192.168.1.11"));
    }

    private static void DeleteDatabase()
    {
        if (File.Exists(DatabasePath))
            File.Delete(DatabasePath);
    }
}
