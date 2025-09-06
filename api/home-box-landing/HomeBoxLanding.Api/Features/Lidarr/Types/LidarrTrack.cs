using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.Lidarr.Types;

public class LidarrTrack
{
    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("ended")]
    public bool Ended { get; set; }

    [JsonProperty("artistName")]
    public string ArtistName { get; set; }

    [JsonProperty("foreignArtistId")]
    public string ForeignArtistId { get; set; }

    [JsonProperty("tadbId")]
    public int TadbId { get; set; }

    [JsonProperty("discogsId")]
    public int DiscogsId { get; set; }

    [JsonProperty("overview")]
    public string Overview { get; set; }

    [JsonProperty("artistType")]
    public string ArtistType { get; set; }

    [JsonProperty("disambiguation")]
    public string Disambiguation { get; set; }

    [JsonProperty("links")]
    public List<Link> Links { get; set; }

    [JsonProperty("nextAlbum")]
    public object NextAlbum { get; set; }

    [JsonProperty("lastAlbum")]
    public LidarrLastAlbum LastAlbum { get; set; }

    [JsonProperty("images")]
    public List<Image> Images { get; set; }

    [JsonProperty("path")]
    public string Path { get; set; }

    [JsonProperty("qualityProfileId")]
    public int QualityProfileId { get; set; }

    [JsonProperty("metadataProfileId")]
    public int MetadataProfileId { get; set; }

    [JsonProperty("monitored")]
    public bool Monitored { get; set; }

    [JsonProperty("monitorNewItems")]
    public string MonitorNewItems { get; set; }

    [JsonProperty("rootFolderPath")]
    public string RootFolderPath { get; set; }

    [JsonProperty("genres")]
    public List<string> Genres { get; set; }

    [JsonProperty("cleanName")]
    public string CleanName { get; set; }

    [JsonProperty("sortName")]
    public string SortName { get; set; }

    [JsonProperty("tags")]
    public List<int> Tags { get; set; }

    [JsonProperty("added")]
    public DateTime Added { get; set; }

    [JsonProperty("ratings")]
    public LidarrRatings Ratings { get; set; }

    [JsonProperty("statistics")]
    public LidarrStatistics Statistics { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    public class Artist
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("ended")]
        public bool Ended { get; set; }

        [JsonProperty("artistName")]
        public string ArtistName { get; set; }

        [JsonProperty("foreignArtistId")]
        public string ForeignArtistId { get; set; }

        [JsonProperty("tadbId")]
        public int TadbId { get; set; }

        [JsonProperty("discogsId")]
        public int DiscogsId { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("artistType")]
        public string ArtistType { get; set; }

        [JsonProperty("disambiguation")]
        public string Disambiguation { get; set; }

        [JsonProperty("links")]
        public List<Link> Links { get; set; }

        [JsonProperty("nextAlbum")]
        public object NextAlbum { get; set; }

        [JsonProperty("lastAlbum")]
        public object LastAlbum { get; set; }

        [JsonProperty("images")]
        public List<Image> Images { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("qualityProfileId")]
        public int QualityProfileId { get; set; }

        [JsonProperty("metadataProfileId")]
        public int MetadataProfileId { get; set; }

        [JsonProperty("monitored")]
        public bool Monitored { get; set; }

        [JsonProperty("monitorNewItems")]
        public string MonitorNewItems { get; set; }

        [JsonProperty("genres")]
        public List<string> Genres { get; set; }

        [JsonProperty("cleanName")]
        public string CleanName { get; set; }

        [JsonProperty("sortName")]
        public string SortName { get; set; }

        [JsonProperty("tags")]
        public List<int> Tags { get; set; }

        [JsonProperty("added")]
        public DateTime Added { get; set; }

        [JsonProperty("ratings")]
        public LidarrRatings Ratings { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }

    public class Image
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("coverType")]
        public string CoverType { get; set; }

        [JsonProperty("extension")]
        public string Extension { get; set; }

        [JsonProperty("remoteUrl")]
        public string RemoteUrl { get; set; }
    }

    public class LidarrLastAlbum
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("disambiguation")]
        public string Disambiguation { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("artistId")]
        public int ArtistId { get; set; }

        [JsonProperty("foreignAlbumId")]
        public string ForeignAlbumId { get; set; }

        [JsonProperty("monitored")]
        public bool Monitored { get; set; }

        [JsonProperty("anyReleaseOk")]
        public bool AnyReleaseOk { get; set; }

        [JsonProperty("profileId")]
        public int ProfileId { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("albumType")]
        public string AlbumType { get; set; }

        [JsonProperty("secondaryTypes")]
        public List<object> SecondaryTypes { get; set; }

        [JsonProperty("mediumCount")]
        public int MediumCount { get; set; }

        [JsonProperty("ratings")]
        public LidarrRatings Ratings { get; set; }

        [JsonProperty("releaseDate")]
        public DateTime ReleaseDate { get; set; }

        [JsonProperty("releases")]
        public List<Release> Releases { get; set; }

        [JsonProperty("genres")]
        public List<string> Genres { get; set; }

        [JsonProperty("media")]
        public List<Medium> Media { get; set; }

        [JsonProperty("artist")]
        public Artist Artist { get; set; }

        [JsonProperty("images")]
        public List<Image> Images { get; set; }

        [JsonProperty("links")]
        public List<Link> Links { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("lastSearchTime")]
        public DateTime? LastSearchTime { get; set; }
    }

    public class Link
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Medium
    {
        [JsonProperty("mediumNumber")]
        public int MediumNumber { get; set; }

        [JsonProperty("mediumName")]
        public string MediumName { get; set; }

        [JsonProperty("mediumFormat")]
        public string MediumFormat { get; set; }
    }

    public class LidarrRatings
    {
        [JsonProperty("votes")]
        public int Votes { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    }

    public class Release
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("albumId")]
        public int AlbumId { get; set; }

        [JsonProperty("foreignReleaseId")]
        public string ForeignReleaseId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("trackCount")]
        public int TrackCount { get; set; }

        [JsonProperty("media")]
        public List<Medium> Media { get; set; }

        [JsonProperty("mediumCount")]
        public int MediumCount { get; set; }

        [JsonProperty("disambiguation")]
        public string Disambiguation { get; set; }

        [JsonProperty("country")]
        public List<string> Country { get; set; }

        [JsonProperty("label")]
        public List<string> Label { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("monitored")]
        public bool Monitored { get; set; }
    }

    public class LidarrStatistics
    {
        [JsonProperty("albumCount")]
        public int AlbumCount { get; set; }

        [JsonProperty("trackFileCount")]
        public int TrackFileCount { get; set; }

        [JsonProperty("trackCount")]
        public int TrackCount { get; set; }

        [JsonProperty("totalTrackCount")]
        public int TotalTrackCount { get; set; }

        [JsonProperty("sizeOnDisk")]
        public long SizeOnDisk { get; set; }

        [JsonProperty("percentOfTracks")]
        public double PercentOfTracks { get; set; }
    }
}