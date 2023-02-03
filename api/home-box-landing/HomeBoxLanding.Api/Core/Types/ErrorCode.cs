namespace HomeBoxLanding.Api.Core.Types
{
    public class ErrorCode
    {
        public static readonly int Unauthorised = 1;
        public static readonly int DatabaseError = 2;
        public static readonly int DeployNotFound = 3;
        public static readonly int AppAlreadyDeploying = 4;
    }
}