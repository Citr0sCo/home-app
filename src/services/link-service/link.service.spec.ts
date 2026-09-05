import { LinkRepository } from './link.repository';
import { LinkService } from './link.service';

describe('LinkService click tracking', () => {
    const now = 1_700_000_000_000;
    let service: LinkService;
    let currentTime: number;

    beforeEach(() => {
        service = new LinkService({} as LinkRepository);
        currentTime = now;
        vi.spyOn(Date, 'now').mockImplementation(() => currentTime);
    });

    afterEach(() => {
        vi.restoreAllMocks();
    });

    it('returns never when a link has not been clicked', () => {
        expect(service.getLastClickedLabel(null)).toBe('never');
    });

    it('reports elapsed hours from the server timestamp', () => {
        const lastClickedAt = new Date(now - (3 * 60 * 60 * 1000)).toISOString();

        expect(service.getLastClickedLabel(lastClickedAt)).toBe('3 hours ago');
    });

    it('treats server timestamps without an offset as UTC', () => {
        const lastClickedAt = new Date(now - (5 * 60 * 1000)).toISOString().slice(0, -1);

        expect(service.getLastClickedLabel(lastClickedAt)).toBe('0 hours ago');
    });

    it('reports elapsed days after a full day', () => {
        const lastClickedAt = new Date(now - (2 * 24 * 60 * 60 * 1000)).toISOString();

        expect(service.getLastClickedLabel(lastClickedAt)).toBe('2 days ago');
    });

    it('returns never for an invalid server timestamp', () => {
        expect(service.getLastClickedLabel('not-a-date')).toBe('never');
    });

    it('returns never for missing or invalid timestamps', () => {
        expect(service.getLastClickedStatus(null)).toBe('never');
        expect(service.getLastClickedStatus('not-a-date')).toBe('never');
    });

    it('returns recent for clicks less than one month ago', () => {
        const recentClick = new Date(now - (29 * 24 * 60 * 60 * 1000)).toISOString();

        expect(service.getLastClickedStatus(recentClick)).toBe('recent');
    });

    it('returns month for clicks at least one month but less than one year ago', () => {
        const monthOldClick = new Date(now - (30 * 24 * 60 * 60 * 1000)).toISOString();

        expect(service.getLastClickedStatus(monthOldClick)).toBe('month');
    });

    it('returns year for clicks at least one year ago', () => {
        const yearOldClick = new Date(now - (365 * 24 * 60 * 60 * 1000)).toISOString();

        expect(service.getLastClickedStatus(yearOldClick)).toBe('year');
    });
});
