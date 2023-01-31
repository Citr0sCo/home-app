using home_box_landing.api.Core.Shell;
using home_box_landing.api.Core.Types;
using home_box_landing.api.Features.Deploy.Types;

namespace home_box_landing.api.Features.Deploy
{
    public class DeployService
    {
        private readonly IShellService _shellService;
        private readonly IDeployRepository _deployRepository;

        public DeployService(IShellService shellService, IDeployRepository deployRepository)
        {
            _shellService = shellService;
            _deployRepository = deployRepository;
        }

        public GitlabBuildResponse Deploy(GithubBuildRequest request)
        {
            var response = new GitlabBuildResponse();

            if (request.Builds.Where(x => x.Stage == "build").Any(x => x.Status != "success"))
            {
                response.AddError(new Error
                {
                    Code = ErrorCode.Unauthorised,
                    UserMessage = "Not all tests have passed.",
                    TechnicalMessage = "Not all tests have passed."
                });
                return response;
            }
            
            var currentDeploys = _deployRepository.GetAllDeploys();

            if (currentDeploys.HasError || (currentDeploys.Deploys.Count > 0 && currentDeploys.Deploys.FirstOrDefault()?.FinishedAt == null))
            {
                response.AddError(new Error
                {
                    Code = ErrorCode.AppAlreadyDeploying,
                    UserMessage = "Application is already being deployed",
                    TechnicalMessage = "Application currently being deployed. Please add queueing to prevent this error from being returned."
                });
                return response;
            }

            
            var saveDeployResponse = _deployRepository.SaveDeploy(request.Commit.Id);

            if (saveDeployResponse.HasError)
            {
                response.AddError(saveDeployResponse.Error);
                return response;
            }

            Task.Run(() =>
            {
                _shellService.Run($"cd /var/www/live/project-trailblazer/ && sudo bash deploy.sh {request.Commit.Id} > /dev/null 2>&1");
                
                var setDeployAsFinished = _deployRepository.SetDeployAsFinished(saveDeployResponse.DeployIdentifier, DateTime.Now);

                if (setDeployAsFinished.HasError)
                    response.AddError(setDeployAsFinished.Error);
                
                _shellService.Run("cd /var/www/live/project-trailblazer/ && sudo bash recycle.sh > /dev/null 2>&1");
            });


            return response;
        }
    }
}