using HomeBoxLanding.Api.Features.Deploys;
using HomeBoxLanding.Api.Features.Deploys.Types;
using Moq;
using NUnit.Framework;

namespace HomeBoxLanding.Api.Tests.Features.Deploys.GivenARequestToGetAllDeploys
{
    [TestFixture]
    public class WhenRequestIsValid
    {
        private GetAllDeploysResponse _result;

        [OneTimeSetUp]
        public void Setup()
        {
            var deployRepository = new Mock<IDeployRepository>();
            deployRepository.Setup(x => x.GetAllDeploys()).Returns(new GetDeploysResponse
            {
                Deploys = new List<DeployRecord>
                {
                    new DeployRecord
                    {
                        Identifier = Guid.Parse("F37CE5FB-6BCD-4FF1-9439-6B7DF25FB352"),
                        CommitId = "2A437AB2-9A3F-4560-A3B4-891025C59F1F",
                        StartedAt = DateTime.Now.AddMinutes(-1),
                        FinishedAt = DateTime.Now.AddMinutes(1)
                    }
                }
            });
            
            var subject = new DeployService(null, deployRepository.Object, null);
            _result = subject.GetAllDeploys();
        }

        [Test]
        public void ThenResponseContainsCorrectlyMappedDeploys()
        {
            Assert.That(_result.Deploys.Count, Is.EqualTo(1));
        }
    }
}