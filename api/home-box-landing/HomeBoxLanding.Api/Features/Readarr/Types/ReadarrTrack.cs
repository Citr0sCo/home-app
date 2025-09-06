using Newtonsoft.Json;

namespace HomeBoxLanding.Api.Features.Readarr.Types;

public class ReadarrTrack
{
    [JsonProperty("authorMetadataId")]
    public int AuthorMetadataId { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("ended")]
    public bool Ended { get; set; }

    [JsonProperty("authorName")]
    public string AuthorName { get; set; }

    [JsonProperty("authorNameLastFirst")]
    public string AuthorNameLastFirst { get; set; }

    [JsonProperty("foreignAuthorId")]
    public string ForeignAuthorId { get; set; }

    [JsonProperty("titleSlug")]
    public string TitleSlug { get; set; }

    [JsonProperty("overview")]
    public string Overview { get; set; }

    [JsonProperty("links")]
    public List<Link> Links { get; set; }

    [JsonProperty("nextBook")]
    public ReadarrNextBook NextBook { get; set; }

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
    public List<object> Genres { get; set; }

    [JsonProperty("cleanName")]
    public string CleanName { get; set; }

    [JsonProperty("sortName")]
    public string SortName { get; set; }

    [JsonProperty("sortNameLastFirst")]
    public string SortNameLastFirst { get; set; }

    [JsonProperty("tags")]
    public List<int> Tags { get; set; }

    [JsonProperty("added")]
    public DateTime Added { get; set; }

    [JsonProperty("ratings")]
    public ReadarrRatings Ratings { get; set; }

    [JsonProperty("statistics")]
    public ReadarrStatistics Statistics { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("lastBook")]
    public ReadarrLastBook LastBook { get; set; }
    
    public class AddOptions
    {
        [JsonProperty("addType")]
        public string AddType { get; set; }

        [JsonProperty("searchForNewBook")]
        public bool SearchForNewBook { get; set; }
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

    public class ReadarrLastBook
    {
        [JsonProperty("authorMetadataId")]
        public int AuthorMetadataId { get; set; }

        [JsonProperty("foreignBookId")]
        public string ForeignBookId { get; set; }

        [JsonProperty("titleSlug")]
        public string TitleSlug { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("releaseDate")]
        public DateTime ReleaseDate { get; set; }

        [JsonProperty("links")]
        public List<Link> Links { get; set; }

        [JsonProperty("genres")]
        public List<string> Genres { get; set; }

        [JsonProperty("relatedBooks")]
        public List<object> RelatedBooks { get; set; }

        [JsonProperty("ratings")]
        public ReadarrRatings Ratings { get; set; }

        [JsonProperty("lastSearchTime")]
        public DateTime LastSearchTime { get; set; }

        [JsonProperty("cleanTitle")]
        public string CleanTitle { get; set; }

        [JsonProperty("monitored")]
        public bool Monitored { get; set; }

        [JsonProperty("anyEditionOk")]
        public bool AnyEditionOk { get; set; }

        [JsonProperty("lastInfoSync")]
        public DateTime LastInfoSync { get; set; }

        [JsonProperty("added")]
        public DateTime Added { get; set; }

        [JsonProperty("addOptions")]
        public AddOptions AddOptions { get; set; }

        [JsonProperty("authorMetadata")]
        public object AuthorMetadata { get; set; }

        [JsonProperty("author")]
        public object Author { get; set; }

        [JsonProperty("editions")]
        public object Editions { get; set; }

        [JsonProperty("bookFiles")]
        public object BookFiles { get; set; }

        [JsonProperty("seriesLinks")]
        public object SeriesLinks { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }

    public class Link
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ReadarrNextBook
    {
        [JsonProperty("authorMetadataId")]
        public int AuthorMetadataId { get; set; }

        [JsonProperty("foreignBookId")]
        public string ForeignBookId { get; set; }

        [JsonProperty("titleSlug")]
        public string TitleSlug { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("releaseDate")]
        public DateTime ReleaseDate { get; set; }

        [JsonProperty("links")]
        public List<Link> Links { get; set; }

        [JsonProperty("genres")]
        public List<string> Genres { get; set; }

        [JsonProperty("relatedBooks")]
        public List<object> RelatedBooks { get; set; }

        [JsonProperty("ratings")]
        public ReadarrRatings Ratings { get; set; }

        [JsonProperty("cleanTitle")]
        public string CleanTitle { get; set; }

        [JsonProperty("monitored")]
        public bool Monitored { get; set; }

        [JsonProperty("anyEditionOk")]
        public bool AnyEditionOk { get; set; }

        [JsonProperty("lastInfoSync")]
        public DateTime LastInfoSync { get; set; }

        [JsonProperty("added")]
        public DateTime Added { get; set; }

        [JsonProperty("addOptions")]
        public AddOptions AddOptions { get; set; }

        [JsonProperty("authorMetadata")]
        public object AuthorMetadata { get; set; }

        [JsonProperty("author")]
        public object Author { get; set; }

        [JsonProperty("editions")]
        public object Editions { get; set; }

        [JsonProperty("bookFiles")]
        public object BookFiles { get; set; }

        [JsonProperty("seriesLinks")]
        public object SeriesLinks { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }

    public class ReadarrRatings
    {
        [JsonProperty("votes")]
        public int Votes { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }

        [JsonProperty("popularity")]
        public string Popularity { get; set; }
    }

    public class ReadarrStatistics
    {
        [JsonProperty("bookFileCount")]
        public int BookFileCount { get; set; }

        [JsonProperty("bookCount")]
        public int BookCount { get; set; }

        [JsonProperty("availableBookCount")]
        public int AvailableBookCount { get; set; }

        [JsonProperty("totalBookCount")]
        public int TotalBookCount { get; set; }

        [JsonProperty("sizeOnDisk")]
        public object SizeOnDisk { get; set; }

        [JsonProperty("percentOfBooks")]
        public double PercentOfBooks { get; set; }
    }
}
