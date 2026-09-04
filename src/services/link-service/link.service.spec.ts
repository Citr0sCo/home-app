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

    it('reports elapsed days after a full day', () => {
        const lastClickedAt = new Date(now - (2 * 24 * 60 * 60 * 1000)).toISOString();

        expect(service.getLastClickedLabel(lastClickedAt)).toBe('2 days ago');
    });

    it('returns never for an invalid server timestamp', () => {
        expect(service.getLastClickedLabel('not-a-date')).toBe('never');
    });
});
