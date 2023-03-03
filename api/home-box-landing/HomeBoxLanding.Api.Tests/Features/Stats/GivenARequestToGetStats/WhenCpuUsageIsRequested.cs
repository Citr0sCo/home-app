using HomeBoxLanding.Api.Core.Shell;
using HomeBoxLanding.Api.Features.Stats;
using HomeBoxLanding.Api.Features.Stats.Types;
using Moq;
using NUnit.Framework;

namespace HomeBoxLanding.Api.Tests.Features.Stats.GivenARequestToGetStats
{
    [TestFixture]
    public class WhenCpuUsageIsRequested
    {
        private StatsResponse _result;

        [SetUp]
        public void SetUp()
        {
            var shellService = new Mock<IShellService>();
            shellService.Setup(x => x.RunOnHost("docker stats home-app --no-stream")).Returns("CONTAINER ID   NAME       CPU %     MEM USAGE / LIMIT     MEM %     NET I/O         BLOCK I/O     PIDS\ne5b9df6f7579   home-app   0.01%     89.39MiB / 7.764GiB   1.12%     956kB / 1.4MB   1.11MB / 0B   22");
            
            var subject = new StatsService(shellService.Object);
            _result = subject.GetServerStats();
        }
        
        [Test]
        public void ThenMemoryUsageIsReturnedCorrectly()
        {
            Assert.That(_result.CpuUsage.Percentage, Is.EqualTo(0.01));
        }
    
    }
}