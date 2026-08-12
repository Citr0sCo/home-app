namespace HomeBoxLanding.Api.Features.QBitTorrent.Types;

public class QBitTorrentStatsResponse
{
    public Guid? Identifier { get; set; }
    public int TotalTorrents { get; set; }
    public long UploadRate { get; set; }
    public int TotalLeeches { get; set; }
}
