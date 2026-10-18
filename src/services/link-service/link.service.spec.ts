import { of } from 'rxjs';
import { LinkRepository } from './link.repository';
import { LinkService } from './link.service';
import { IColumn } from './types/column.type';

describe('LinkService', () => {
    const now = 1_700_000_000_000;
    let service: LinkService;
    let currentTime: number;

    beforeEach(() => {
        localStorage.clear();
        currentTime = now;
        vi.spyOn(Date, 'now').mockImplementation(() => currentTime);
    });

    afterEach(() => {
        localStorage.clear();
        vi.restoreAllMocks();
    });

    it('returns never for missing or invalid timestamps', () => {
        service = new LinkService({} as LinkRepository);

        expect(service.getLastClickedStatus(null)).toBe('never');
        expect(service.getLastClickedStatus('not-a-date')).toBe('never');
    });

    it('returns recent for clicks less than one week ago', () => {
        service = new LinkService({} as LinkRepository);
        const recentClick = new Date(now - (6 * 24 * 60 * 60 * 1000)).toISOString();

        expect(service.getLastClickedStatus(recentClick)).toBe('recent');
    });

    it('returns week for clicks at least one week but less than one month ago', () => {
        service = new LinkService({} as LinkRepository);
        const weekOldClick = new Date(now - (7 * 24 * 60 * 60 * 1000)).toISOString();

        expect(service.getLastClickedStatus(weekOldClick)).toBe('week');
    });

    it('returns month for clicks at least one month ago', () => {
        service = new LinkService({} as LinkRepository);
        const monthOldClick = new Date(now - (30 * 24 * 60 * 60 * 1000)).toISOString();

        expect(service.getLastClickedStatus(monthOldClick)).toBe('month');
    });

    it('loads cached columns without calling the repository', () => {
        const columns: Array<IColumn> = [];
        const repository = {
            getAllColumns: vi.fn(() => of(columns))
        } as unknown as LinkRepository;
        service = new LinkService(repository);
        localStorage.setItem('cachedColumns', JSON.stringify(columns));

        let result: Array<IColumn> | undefined;
        service.getAllColumns().subscribe((cachedColumns) => result = cachedColumns);

        expect(result).toEqual(columns);
        expect(repository.getAllColumns).not.toHaveBeenCalled();
    });

    it('records a click in the live link and cached columns', () => {
        const link = {
            identifier: 'link-1',
            lastClickedAt: null
        } as any;
        const columns: Array<IColumn> = [{
            identifier: 'column-1',
            name: 'Services',
            sortOrder: 0,
            icon: 'server',
            links: [link],
            folders: []
        }];
        service = new LinkService({} as LinkRepository);
        localStorage.setItem('cachedColumns', JSON.stringify(columns));

        service.recordLinkClickLocally(link);

        expect(link.lastClickedAt).toEqual(expect.any(String));
        expect(service.getLastClickedStatus(link.lastClickedAt)).toBe('recent');
        expect(JSON.parse(localStorage.getItem('cachedColumns')!)[0].links[0].lastClickedAt)
            .toBe(link.lastClickedAt);
    });

    it('ignores health and last-access values when comparing cached links', () => {
        service = new LinkService({} as LinkRepository);
        const localColumns = [createColumn('https://example.com', '2026-01-01T00:00:00Z', { status: 'down' })];
        const backendColumns = [createColumn('https://example.com', '2026-10-01T00:00:00Z', { status: 'up' })];

        expect(service.linksDiffer(localColumns, backendColumns)).toBe(false);
    });

    it('compares secure links consistently with older cached models', () => {
        service = new LinkService({} as LinkRepository);
        const cached = [createColumn('https://example.com', null, null)];
        const backend = [createColumn('https://example.com', null, null)];
        delete (cached[0].links[0] as any).isSecure;

        expect(service.linksDiffer(cached, backend)).toBe(false);
    });

    it('detects changes to link configuration', () => {
        service = new LinkService({} as LinkRepository);

        expect(service.linksDiffer(
            [createColumn('https://old.example.com', null, null)],
            [createColumn('https://new.example.com', null, null)]
        )).toBe(true);
    });

    function createColumn(url: string, lastClickedAt: string | null, health: any): IColumn {
        return {
            identifier: 'column-1',
            name: 'Services',
            sortOrder: 0,
            icon: 'server',
            links: [{
                identifier: 'link-1',
                containerName: null,
                name: 'Dashboard',
                url,
                host: 'example.com',
                port: 443,
                isSecure: true,
                iconUrl: 'icon.png',
                sortOrder: 0,
                columnId: 'column-1',
                folderId: null,
                lastClickedAt,
                health
            } as any],
            folders: []
        };
    }

    it('replaces cached access times with backend timestamps', () => {
        service = new LinkService({} as LinkRepository);
        const cached = [createColumn('https://example.com', '2026-01-01T00:00:00Z', null)];
        const backend = [createColumn('https://example.com', '2026-10-01T00:00:00Z', null)];

        const merged = service.mergeLastAccessedAt(cached, backend);

        expect(merged[0].links[0].lastClickedAt).toBe('2026-10-01T00:00:00Z');
    });

    it('preserves local clicks made while backend access data is loading', () => {
        service = new LinkService({} as LinkRepository);
        const cached = [createColumn('https://example.com', '2026-01-01T00:00:00Z', null)];
        const backend = [createColumn('https://example.com', '2026-10-01T00:00:00Z', null)];
        const link = cached[0].links[0];

        service.recordLinkClickLocally(link);
        const clickedAt = link.lastClickedAt;
        const merged = service.mergeLastAccessedAt(cached, backend);

        expect(merged[0].links[0].lastClickedAt).toBe(clickedAt);
    });

    it('caches columns returned by an explicit backend refresh', () => {
        const columns: Array<IColumn> = [];
        const repository = {
            getAllColumns: vi.fn(() => of(columns))
        } as unknown as LinkRepository;
        service = new LinkService(repository);

        service.getUpdatedColumns().subscribe();

        expect(repository.getAllColumns).toHaveBeenCalledTimes(1);
        expect(JSON.parse(localStorage.getItem('cachedColumns')!)).toEqual(columns);
    });
});
