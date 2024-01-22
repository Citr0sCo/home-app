using HomeBoxLanding.Api.Core.Types;
using HomeBoxLanding.Api.Data;
using HomeBoxLanding.Api.Features.Deploys.Types;

namespace HomeBoxLanding.Api.Features.Deploys;

public interface IDeployRepository
{
    GetDeploysResponse GetAllDeploys();
    SaveDeployResponse SaveDeploy(string commitId);
    CommunicationResponse SetDeployAsFinished(Guid deployId, DateTime finishedAt);
}

public class DeployRepository : IDeployRepository
{
    public SaveDeployResponse SaveDeploy(string commitId)
    {
        var response = new SaveDeployResponse();

        using (var context = new DatabaseContext())
        {
            try
            {
                var deployRecord = new DeployRecord
                {
                    Identifier = Guid.NewGuid(),
                    CommitId = commitId,
                    StartedAt = DateTime.UtcNow
                };

                context.Add(deployRecord);
                context.SaveChanges();

                response.DeployIdentifier = deployRecord.Identifier;
                response.CommitId = deployRecord.CommitId;
                response.StartedAt = deployRecord.StartedAt;
            }
            catch (Exception exception)
            {
                response.AddError(new Error
                {
                    Code = ErrorCode.DatabaseError,
                    UserMessage = "Something went wrong attempting to save a deploy log.",
                    TechnicalMessage = $"The following exception was thrown: {exception.Message}"
                });
            }
        }

        return response;
    }

    public CommunicationResponse SetDeployAsFinished(Guid deployId, DateTime finishedAt)
    {
        var response = new CommunicationResponse();

        using (var context = new DatabaseContext())
        {
            try
            {
                var deployRecord = context.Deploys.FirstOrDefault(x => x.Identifier == deployId);

                if (deployRecord == null)
                {
                    response.AddError(new Error
                    {
                        Code = ErrorCode.DeployNotFound,
                        UserMessage = "Could not find deploy with a given identifier.",
                        TechnicalMessage = $"Could not find deploy with the following identifier: {deployId}"
                    });
                    return response;
                }

                deployRecord.FinishedAt = finishedAt;
                context.SaveChanges();
            }
            catch (Exception exception)
            {
                response.AddError(new Error
                {
                    Code = ErrorCode.DatabaseError,
                    UserMessage = "Something went wrong attempting to save a pin.",
                    TechnicalMessage = $"The following exception was thrown: {exception.Message}"
                });
            }
        }

        return response;
    }

    public GetDeploysResponse GetAllDeploys()
    {
        var response = new GetDeploysResponse();

        using (var context = new DatabaseContext())
        {
            try
            {
                response.Deploys = context.Deploys.OrderByDescending(x => x.StartedAt).Take(5).ToList();
                context.SaveChanges();
            }
            catch (Exception exception)
            {
                response.AddError(new Error
                {
                    Code = ErrorCode.DatabaseError,
                    UserMessage = "Something went wrong attempting to save a pin.",
                    TechnicalMessage = $"The following exception was thrown: {exception.Message}"
                });
            }
        }

        return response;
    }
}