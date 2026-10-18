using HomeBoxLanding.Api.Data;
using HomeBoxLanding.Api.Features.Columns;
using HomeBoxLanding.Api.Features.Columns.Types;
using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.Links.Types;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace HomeBoxLanding.Api.Tests.Features.Links;

[TestFixture]
[NonParallelizable]
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

    [Test]
    public async Task ImportColumns_PreservesLinkIdentifierAndLastAccessTimestamp()
    {
        var linkIdentifier = Guid.NewGuid();
        var columnIdentifier = Guid.NewGuid();
        var lastAccessedAt = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);

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
                Name = "Dashboard",
                Url = "https://old.example.com",
                Host = "old.example.com",
                Port = 443,
                IsSecure = true,
                Column = column
            });
            await context.SaveChangesAsync();
        }

        var lastAccessRepository = new LinkLastAccessRepository();
        await lastAccessRepository.RecordAsync(linkIdentifier, lastAccessedAt);

        var response = await new ColumnsRepository().Import(new ImportColumnsRequest
        {
            Columns =
            [
                new Column
                {
                    Identifier = columnIdentifier,
                    Name = "Services",
                    Icon = "server",
                    SortOrder = 0,
                    Links =
                    [
                        new Link
                        {
                            Identifier = linkIdentifier,
                            Name = "Dashboard",
                            Url = "https://new.example.com",
                            Host = "new.example.com",
                            Port = 443,
                            IsSecure = true,
                            ColumnId = columnIdentifier
                        }
                    ]
                }
            ]
        });

        Assert.That(response.HasError, Is.False);
        var lastAccess = await lastAccessRepository.GetAllAsync();
        Assert.That(lastAccess.Single().LinkIdentifier, Is.EqualTo(linkIdentifier));
        Assert.That(lastAccess.Single().LastAccessedAt, Is.EqualTo(lastAccessedAt));

        await using var resultContext = new DatabaseContext();
        var updatedLink = await resultContext.Links.SingleAsync();
        Assert.That(updatedLink.Identifier, Is.EqualTo(linkIdentifier));
        Assert.That(updatedLink.Url, Is.EqualTo("https://new.example.com"));
    }

    private static void DeleteDatabase()
    {
        SqliteConnection.ClearAllPools();

        foreach (var path in new[] { DatabasePath, $"{DatabasePath}-shm", $"{DatabasePath}-wal" })
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
