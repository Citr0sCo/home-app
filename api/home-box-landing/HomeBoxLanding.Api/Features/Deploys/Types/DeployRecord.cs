using System.ComponentModel.DataAnnotations;

namespace HomeBoxLanding.Api.Features.Deploys.Types
{
    public class DeployRecord
    {
        [Key]
        public Guid Identifier { get; set; }
        public string CommitId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
    }
}