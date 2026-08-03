import { LocationMapper } from './location-service/location.mapper';
import { NotepadMapper } from './notepad-service/notepad.mapper';
import { WeatherMapper } from './weather-service/weather.mapper';
import { PiHoleMapper } from './pihole-service/pi-hole.mapper';
import { PlexMapper } from './plex-service/plex.mapper';
import { UptimeKumaMapper } from './uptime-kuma-service/uptime-kuma.mapper';
import { StatMapper } from './stats-service/stat.mapper';

describe('data mappers', () => {
    it('maps browser and API location shapes, including an empty location', () => {
        const empty = LocationMapper.map(null);
        expect(empty.latitude).toBe(0);
        expect(empty.longitude).toBe(0);
        expect(LocationMapper.map({ coords: { latitude: 1, longitude: 2 } }))
            .toMatchObject({ latitude: 1, longitude: 2 });
        expect(LocationMapper.map({ latitude: 3, longitude: 4 }))
            .toMatchObject({ latitude: 3, longitude: 4 });
    });

    it('maps notes and weather responses', () => {
        const note = NotepadMapper.map({
            Identifier: 'note-id', Note: 'Remember this',
            CreatedAt: '2026-01-01T00:00:00.000Z', UpdatedAt: '2026-01-02T00:00:00.000Z'
        });
        expect(note.identifier).toBe('note-id');
        expect(note.createdAt.toISOString()).toBe('2026-01-01T00:00:00.000Z');
        expect(note.updatedAt.toISOString()).toBe('2026-01-02T00:00:00.000Z');

        const weather = WeatherMapper.map({
            name: 'London', weather: [{ main: 'Clouds', description: 'broken clouds' }],
            coord: { lat: 51.5, lon: -0.1 }, main: {
                feels_like: 12, humidity: 80, pressure: 1012, temp: 13, temp_max: 15, temp_min: 10
            }, wind: { deg: 180, gust: 5, speed: 3 }
        });
        expect(weather).toMatchObject({
            name: 'London', weatherShortDescription: 'Clouds', weatherDescription: 'broken clouds',
            latitude: 51.5, longitude: -0.1, temperature: 13, windSpeed: 3
        });
    });

    it('maps monitoring and resource payloads', () => {
        const uptime = UptimeKumaMapper.mapActivities({ Response: { Data: {
            Activities: [{ Metrics: [{ Name: 'Website', IsUp: true }] }]
        } } });
        expect(uptime).toEqual([{ metrics: [{ name: 'Website', isUp: true }] }]);

        const pihole = PiHoleMapper.mapActivities({ Response: { Data: {
            Activities: [{ Identifier: 'pi', Queries: { Total: 10, Blocked: 2, PercentBlocked: 20 }, Clients: { Total: 3 } }]
        } } });
        expect(pihole[0]).toEqual({ identifier: 'pi', queriesToday: 10, blockedToday: 2, blockedPercentage: 20, clients: 3 });
        expect(PiHoleMapper.mapActivity({ Identifier: 'pi' })).toEqual({
            identifier: 'pi', queriesToday: undefined, blockedToday: undefined,
            blockedPercentage: undefined, clients: undefined
        });

        const plex = PlexMapper.mapActivity({ Response: { Data: { Sessions: [
            { User: 'Alex', Duration: null, FullTitle: 'Live', State: 'playing', ViewOffset: null,
                ProgressPercentage: 0, VideoDecision: 'directplay' }
        ] } } });
        expect(plex[0]).toMatchObject({ user: 'Alex', progressPercentage: 100, isLiveTv: true });

        const stats = StatMapper.map({ Stats: [{ Name: 'app', CpuUsage: { Percentage: 1 } }] });
        expect(stats.stats[0]).toEqual({
            name: 'app', cpuUsage: { percentage: 1, total: undefined, used: undefined },
            memoryUsage: { percentage: undefined, total: undefined, used: undefined },
            diskUsage: { percentage: undefined, total: undefined, used: undefined }
        });
    });
});
