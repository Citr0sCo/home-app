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
