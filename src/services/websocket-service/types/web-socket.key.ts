export enum WebSocketKey {
    Unknown = 'Unknown',
    Handshake = 'Handshake',
    BuildStarted = 'BuildStarted',
    BuildUpdated = 'BuildUpdated',
    DeployStarted = 'DeployStarted',
    DeployUpdated = 'DeployUpdated',
    ServerStats = 'ServerStats',
    PlexActivity = 'PlexActivity',
    PiHoleActivity = 'PiHoleActivity',
    RadarrActivity = 'RadarrActivity',
    SonarrActivity = 'SonarrActivity',
    DockerAppUpdateProgress = 'DockerAppUpdateProgress',
}