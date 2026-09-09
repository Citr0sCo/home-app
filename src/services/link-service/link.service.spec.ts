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
