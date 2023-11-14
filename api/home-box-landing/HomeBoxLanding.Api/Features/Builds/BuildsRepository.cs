using HomeBoxLanding.Api.Core.Types;
using HomeBoxLanding.Api.Data;
using HomeBoxLanding.Api.Features.Builds.Types;

namespace HomeBoxLanding.Api.Features.Builds;

public interface IBuildsRepository
{
    List<BuildRecord> GetAll();
    BuildRecord? GetBuild(string buildIdentifier);
    SaveBuildResponse SaveBuild(SaveBuildRequest request);
    UpdateBuildResponse UpdateBuild(UpdateBuildRequest request);
}

public class BuildsRepository : IBuildsRepository
{
    public List<BuildRecord> GetAll()
    {
        using (var context = new DatabaseContext())
        {
            try
            {
                return context.Builds
                    .ToList();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return new List<BuildRecord>();
            }
        }
    }
    public BuildRecord? GetBuild(string githubBuildReference)
    {
        var response = new UpdateBuildResponse();

        using (var context = new DatabaseContext())
        {
            try
            {
                return context.Builds.FirstOrDefault(x => x.GithubBuildReference == githubBuildReference);
            }
            catch (Exception exception)
            {
                response.AddError(new Error
                {
                    Code = ErrorCode.DatabaseError,
                    UserMessage = "Something went wrong attempting to update a build log.",
                    TechnicalMessage = $"The following exception was thrown: {exception.Message}"
                });
            }
        }

        return null;
    }
    public SaveBuildResponse SaveBuild(SaveBuildRequest request)
    {
        var response = new SaveBuildResponse();

        using (var context = new DatabaseContext())
        {
            try
            {
                var buildRecord = new BuildRecord
                {
                    StartedAt = request.StartedAt,
                    FinishedAt = request.FinishedAt,
                    Conclusion = request.Conclusion,
                    Status = request.Status,
                    GithubBuildReference = request.GithubBuildReference
                };

                context.Add(buildRecord);
                context.SaveChanges();

                response.BuildIdentifier = buildRecord.Identifier;
            }
            catch (Exception exception)
            {
                response.AddError(new Error
                {
                    Code = ErrorCode.DatabaseError,
                    UserMessage = "Something went wrong attempting to save a build log.",
                    TechnicalMessage = $"The following exception was thrown: {exception.Message}"
                });
            }
        }

        return response;
    }
    public UpdateBuildResponse UpdateBuild(UpdateBuildRequest request)
    {
        var response = new UpdateBuildResponse();

        using (var context = new DatabaseContext())
        {
            try
            {
                var existingBuildRecord = context.Builds.FirstOrDefault(x => x.Identifier == request.Identifier);

                if (existingBuildRecord == null)
                {
                    response.AddError(new Error
                    {
                        Code = ErrorCode.BuildNotFound,
                        UserMessage = "Build not found when trying to update.",
                        TechnicalMessage = $"Could not find a build with identifier of '{request.Identifier}'."
                    });
                    return response;
                }

                existingBuildRecord.FinishedAt = request.FinishedAt;
                existingBuildRecord.Conclusion = request.Conclusion;
                existingBuildRecord.Status = request.Status;

                context.SaveChanges();

                response.BuildIdentifier = request.Identifier;
            }
            catch (Exception exception)
            {
                response.AddError(new Error
                {
                    Code = ErrorCode.DatabaseError,
                    UserMessage = "Something went wrong attempting to update a build log.",
                    TechnicalMessage = $"The following exception was thrown: {exception.Message}"
                });
            }
        }

        return response;
    }
}