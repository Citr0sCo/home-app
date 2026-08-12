import { Subject, of } from 'rxjs';
import { QBitTorrentDetailsComponent } from './qbittorrent-details.component';
import { QBitTorrentService } from '../../../../services/qbittorrent-service/qbittorrent.service';
import { IQBitTorrentStats } from '../../../../services/qbittorrent-service/types/qbittorrent-stats.type';

describe('QBitTorrentDetailsComponent', () => {
    it('formats upload and download rates with lowercase kilobytes', () => {
        const activities = new Subject<Array<IQBitTorrentStats>>();
        const service = {
            activities,
            getStats: () => of({
                identifier: 'qbittorrent-1',
                totalTorrents: 12,
                uploadRate: 10240,
                downloadRate: 1048576,
                totalLeeches: 4
            }),
            ngOnInit: vi.fn(),
            ngOnDestroy: vi.fn()
        } as unknown as QBitTorrentService;
        const component = new QBitTorrentDetailsComponent(service);
        component.item = { identifier: 'qbittorrent-1' } as any;

        component.ngOnInit();

        expect(component.formattedUploadRate()).toBe('10 kB/s');
        expect(component.formattedDownloadRate()).toBe('1.0 MB/s');
        expect(component.isLoading()).toBe(false);

        activities.next([{
            identifier: 'qbittorrent-1',
            totalTorrents: 14,
            uploadRate: 1024,
            downloadRate: 1073741824,
            totalLeeches: 5
        }]);

        expect(component.formattedUploadRate()).toBe('1.0 kB/s');
        expect(component.formattedDownloadRate()).toBe('1.0 GB/s');
        expect(component.stats()?.totalTorrents).toBe(14);

        component.ngOnDestroy();
        expect(service.ngOnInit).toHaveBeenCalledOnce();
        expect(service.ngOnDestroy).toHaveBeenCalledOnce();
    });
});
