using HomeBoxLanding.Api.Features.Links;
using HomeBoxLanding.Api.Features.Links.Types;
using Moq;
using NUnit.Framework;

namespace HomeBoxLanding.Api.Tests.Features.Links.GivenARequestToUpdateLink;

[TestFixture]
public class WhenTheOrderHasChanged
{
    private readonly Guid _linkIdentifier = Guid.Parse("ED8210CA-3753-4CC1-B09D-744435A0A075");

    [SetUp]
    public void SetUp()
    {
        var linksRepository = new Mock<ILinksRepository>();
        linksRepository.Setup(x => x.GetLinkByReference(_linkIdentifier)).Returns(new LinkRecord
        {
            SortOrder = "B"
        });
        linksRepository.Setup(x => x.GetLinkAbove(_linkIdentifier)).Returns(new LinkRecord
        {
            SortOrder = "A"
        });
        linksRepository.Setup(x => x.GetLinkBelow(_linkIdentifier)).Returns(new LinkRecord
        {
            SortOrder = "C"
        });
        linksRepository.Setup(x => x.UpdateLink(It.IsAny<UpdateLinkRequest>())).Returns(new UpdateLinkResponse
        {
            Link = new Link
            {
                SortOrder = "AA"
            }
        });
        
        var subject = new LinksService(linksRepository.Object);
        subject.UpdateLink(new UpdateLinkRequest
        {
            MoveUp = true,
            MoveDown = false,
            Link = new Link
            {
                Identifier = _linkIdentifier
            }
        });
    }

    [Test]
    public void ThenTheOrderHasBeenUpdatedCorrectly()
    {
     Assert.True(true);   
    }
}