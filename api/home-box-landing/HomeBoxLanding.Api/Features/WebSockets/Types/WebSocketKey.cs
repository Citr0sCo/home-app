namespace HomeBoxLanding.Api.Features.WebSockets.Types;

public enum WebSocketKey
{
    Unknown,
    Handshake,
    BuildStarted,
    BuildUpdated,
    DeployStarted,
    DeployUpdated,
    ServerStats,
    PlexActivity,
    PiholeActivity,
    RadarrActivity,
    SonarrActivity,
    DockerAppUpdateProgress
}