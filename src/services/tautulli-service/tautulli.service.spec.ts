import { of } from 'rxjs';
import { TautulliRepository } from './tautulli.repository';
import { TautulliService } from './tautulli.service';
import { WebSocketService } from '../websocket-service/web-socket.service';

describe('TautulliService', () => {
    it('refreshes stats from the repository instead of returning a websocket snapshot', () => {
        const repository = {
            getStats: vi.fn(() => of({
                identifier: 'tautulli-1',
                totalMovies: 120,
                totalShows: 45,
                totalUsers: 8
            }))
        } as unknown as TautulliRepository;
        const webSocketService = {
            subscribe: vi.fn(),
            unsubscribe: vi.fn()
        } as unknown as WebSocketService;
        const service = new TautulliService(repository, webSocketService);
        service.handleNewActivity({
            Response: {
                Data: {
                    Activities: [{
                        Identifier: 'tautulli-1',
                        TotalMovies: 1,
                        TotalShows: 2,
                        TotalUsers: 3
                    }]
                }
            }
        });

        let result;
        service.getStats('tautulli-1').subscribe((stats) => result = stats);

        expect(repository.getStats).toHaveBeenCalledWith('tautulli-1');
        expect(result).toEqual({
            identifier: 'tautulli-1',
            totalMovies: 120,
            totalShows: 45,
            totalUsers: 8
        });
    });
});
