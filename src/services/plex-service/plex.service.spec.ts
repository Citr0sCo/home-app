import { of } from 'rxjs';
import { PlexRepository } from './plex.repository';
import { PlexService } from './plex.service';
import { IPlexSession } from './types/plex-session.type';

describe('PlexService', () => {
    it('requests current activity on every read instead of reusing a snapshot', () => {
        const firstSession = { fullTitle: 'First movie' } as IPlexSession;
        const secondSession = { fullTitle: 'Currently playing movie' } as IPlexSession;
        const repository = {
            getActivity: vi.fn()
                .mockReturnValueOnce(of([firstSession]))
                .mockReturnValueOnce(of([secondSession]))
        } as unknown as PlexRepository;
        const service = new PlexService(repository);
        let result: Array<IPlexSession> = [];

        service.getActivity().subscribe((sessions) => result = sessions);
        service.getActivity().subscribe((sessions) => result = sessions);

        expect(repository.getActivity).toHaveBeenCalledTimes(2);
        expect(result).toEqual([secondSession]);
    });
});
