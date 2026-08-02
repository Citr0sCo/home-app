using HomeBoxLanding.Api.Features.Columns;
using HomeBoxLanding.Api.Features.Columns.Types;
using HomeBoxLanding.Api.Features.Folders.Types;
using HomeBoxLanding.Api.Features.Links.Types;
using NUnit.Framework;

namespace HomeBoxLanding.Api.Tests.Features.Columns;

[TestFixture]
public class ColumnMapperTests
{
    [Test]
    public void Map_OrdersLooseLinksAndExcludesLinksOwnedByFolders()
    {
        var folderIdentifier = Guid.NewGuid();
        var looseLink = CreateLink("loose", 1, null);
        var folderLink = CreateLink("folder-link", 0, folderIdentifier);
        var laterLooseLink = CreateLink("later", 2, null);
        var record = new ColumnRecord
        {
            Identifier = Guid.NewGuid(),
            Name = "Services",
            Icon = "server",
            Links = [laterLooseLink, folderLink, looseLink]
        };

        var result = ColumnMapper.Map(record);

        Assert.That(result.Links.Select(x => x.Name), Is.EqualTo(["loose", "later"]));
        Assert.That(result.Links.Any(x => x.Name == "folder-link"), Is.False);
    }

    [Test]
    public void Map_OrdersFoldersAndMapsTheirLinks()
    {
        var folderLink = CreateLink("folder-link", 1, Guid.NewGuid());
        var firstFolder = new FolderRecord
        {
            Identifier = Guid.NewGuid(),
            Name = "First",
            Icon = "folder",
            SortOrder = 1,
            Links = [folderLink]
        };
        var secondFolder = new FolderRecord
        {
            Identifier = Guid.NewGuid(),
            Name = "Second",
            Icon = "folder",
            SortOrder = 0
        };
        var record = new ColumnRecord
        {
            Identifier = Guid.NewGuid(),
            Name = "Services",
            Icon = "server",
            Folders = [firstFolder, secondFolder]
        };

        var result = ColumnMapper.Map(record);

        Assert.That(result.Folders.Select(x => x.Name), Is.EqualTo(["Second", "First"]));
        Assert.That(result.Folders[1].Links.Select(x => x.Name), Is.EqualTo(["folder-link"]));
    }

    private static LinkRecord CreateLink(string name, int sortOrder, Guid? folderIdentifier)
    {
        return new LinkRecord
        {
            Identifier = Guid.NewGuid(),
            Name = name,
            Url = $"https://{name}.example.com",
            Host = $"{name}.example.com",
            IconUrl = "icon.png",
            SortOrder = sortOrder,
            FolderIdentifier = folderIdentifier
        };
    }
}
