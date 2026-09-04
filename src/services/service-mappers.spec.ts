import { BuildMapper } from './build-service/build.mapper';
import { BuildConclusion } from './build-service/types/build-conclusion.enum';
import { BuildStatus } from './build-service/types/build-status.enum';
import { ConfigsMapper } from './configs-service/configs.mapper';
import { FuelPriceMapper } from './fuel-price-service/fuel-price.mapper';
import { LidarrMapper } from './lidarr-service/lidarr.mapper';
import { ColumnMapper } from './link-service/column.mapper';
import { FolderMapper } from './link-service/folder.mapper';
import { LinkMapper } from './link-service/link.mapper';
import { LocationMapper } from './location-service/location.mapper';
import { NotepadMapper } from './notepad-service/notepad.mapper';
import { PiHoleMapper } from './pihole-service/pi-hole.mapper';
import { QBitTorrentMapper } from './qbittorrent-service/qbittorrent.mapper';
import { PlexMapper } from './plex-service/plex.mapper';
import { RadarrMapper } from './radarr-service/radarr.mapper';
import { ReadarrMapper } from './readarr-service/readarr.mapper';
import { SonarrMapper } from './sonarr-service/sonarr.mapper';
import { StatMapper } from './stats-service/stat.mapper';
import { TautulliMapper } from './tautulli-service/tautulli.mapper';
import { UptimeKumaMapper } from './uptime-kuma-service/uptime-kuma.mapper';
import { WeatherMapper } from './weather-service/weather.mapper';

describe('service mappers', () => {
    it('maps builds and preserves optional completion dates', () => {
        const result = BuildMapper.map({ Builds: [{
            Identifier: 'build-1',
            Status: BuildStatus.Completed,
            Conclusion: BuildConclusion.Success,
            StartedAt: '2026-08-01T10:00:00.000Z',
            FinishedAt: '2026-08-01T10:05:00.000Z',
            GithubBuildReference: 'run/1'
        }, {
            Identifier: 'build-2',
            Status: BuildStatus.InProgress,
            Conclusion: BuildConclusion.Unknown,
            StartedAt: '2026-08-01T11:00:00.000Z',
            FinishedAt: null,
            GithubBuildReference: 'run/2'
        }] });

        expect(result).toEqual([
            expect.objectContaining({
                identifier: 'build-1',
                startedAt: new Date('2026-08-01T10:00:00.000Z'),
                finishedAt: new Date('2026-08-01T10:05:00.000Z')
            }),
            expect.objectContaining({
                identifier: 'build-2',
                finishedAt: null
            })
        ]);
    });

    it('maps configuration keys to their frontend names', () => {
        expect(ConfigsMapper.map({ WeatherApiKey: 'weather-key', MapsApiKey: 'maps-key' })).toEqual({
            weatherApiKey: 'weather-key',
            mapsApiKey: 'maps-key'
        });
    });

    it('maps every known fuel brand and falls back for unknown brands', () => {
        const expectedBrands = [
            ['Tesco Extra', '#00539F', './assets/fuel-providers/tesco-logo.webp'],
            ['Sainsbury\'s', '#F06C00', './assets/fuel-providers/sainsburys-logo.png'],
            ['Texaco', '#e93330', './assets/fuel-providers/texaco-logo.png'],
            ['Esso', '#a50e91', './assets/fuel-providers/esso-logo.png'],
            ['ASDA', '#78BE20', './assets/fuel-providers/asda-logo.png'],
            ['Jet', '#f7c801', './assets/fuel-providers/jet-logo.png'],
            ['Shell', '#FFD500', './assets/fuel-providers/shell-logo.png'],
            ['Applegreen', '#6ebd00', './assets/fuel-providers/applegreen-logo.png'],
            ['Morrisons', '#00712f', './assets/fuel-providers/morrisons-logo.png'],
            ['BP', '#007f00', './assets/fuel-providers/bp-logo.png'],
            ['Essar', '#f03e35', './assets/fuel-providers/essar-logo.png']
        ];

        for (const [brand, colour, logo] of expectedBrands) {
            expect(FuelPriceMapper.generateColourFromBrand(`  ${brand}  `)).toEqual({ colour, logo });
        }
        expect(FuelPriceMapper.generateColourFromBrand('Independent Station')).toEqual({
            colour: '#414141',
            logo: ''
        });

        expect(FuelPriceMapper.map([{
            Identifier: 'station-1',
            Name: 'Station',
            Address: '1 Main Street',
            Postcode: 'AB1 2CD',
            Provider: 'Provider',
            Brand: 'Tesco',
            Latitude: 53.1,
            Longitude: -2.1,
            Petrol_E5_Price: 149.9,
            Petrol_E10_Price: 145.9,
            Diesel_B7_Price: 154.9,
            UpdatedAt: 'today',
            CreatedAt: 'yesterday',
            DistanceInMeters: 250
        }])).toEqual([expect.objectContaining({
            identifier: 'station-1',
            petrol_e5_price: 149.9,
            colour: '#00539F',
            logo: './assets/fuel-providers/tesco-logo.webp'
        })]);
    });

    it('maps media activity payloads and websocket envelopes', () => {
        const health = [{ Type: 'warning', Message: 'Missing metadata', WwkiUrl: '/wiki', Source: 'server' }];
        const payload = {
            TotalNumberOfTracks: 10,
            TotalNumberOfQueuedTracks: 2,
            TotalMissingTracks: 1,
            Health: health
        };
        expect(LidarrMapper.mapWebsocketActivities({ Response: { Data: payload } })).toEqual({
            totalNumberOfTracks: 10,
            totalNumberOfQueuedTracks: 2,
            totalMissingTracks: 1,
            health: [{ type: 'warning', message: 'Missing metadata', wikiUrl: '/wiki', source: 'server' }]
        });

        expect(RadarrMapper.mapActivity({
            TotalNumberOfMovies: 20,
            TotalNumberOfQueuedMovies: 3,
            TotalMissingMovies: 4,
            Health: health
        }).totalNumberOfMovies).toBe(20);
        expect(ReadarrMapper.mapActivity({
            TotalNumberOfBooks: 30,
            TotalNumberOfQueuedBooks: 4,
            TotalMissingBooks: 5,
            Health: health
        }).totalNumberOfBooks).toBe(30);
        expect(SonarrMapper.mapActivity({
            TotalNumberOfSeries: 40,
            TotalNumberOfQueuedEpisodes: 5,
            TotalNumberOfMissingEpisodes: 6,
            Health: health
        }).totalNumberOfSeries).toBe(40);
    });

    it('maps links, folders, columns, and their API representations', () => {
        const apiLink = {
            Identifier: 'link-1',
            containerName: 'container',
            Name: 'Dashboard',
            Url: 'https://example.test',
            Host: 'example.test',
            Port: 443,
            IconUrl: 'icon',
            SortOrder: 1,
            ColumnId: 'column-1',
            FolderId: undefined,
            Category: null,
            LastClickedAt: null
        };
        const link = LinkMapper.mapSingle(apiLink);
        expect(link.folderId).toBeNull();
        expect(LinkMapper.mapToApiSingle(link)).toEqual({
            Identifier: 'link-1',
            ContainerName: 'container',
            Name: 'Dashboard',
            Url: 'https://example.test',
            Host: 'example.test',
            Port: 443,
            IconUrl: 'icon',
            SortOrder: 1,
            ColumnId: 'column-1',
            FolderId: null,
            Category: null,
            LastClickedAt: null
        });

        const apiFolder = {
            Identifier: 'folder-1',
            Name: 'Media',
            Icon: 'folder',
            SortOrder: 2,
            ColumnId: 'column-1',
            Links: [apiLink]
        };
        const folder = FolderMapper.mapSingle(apiFolder);
        expect(FolderMapper.mapToApiSingle(folder).Links).toEqual([expect.objectContaining({ Identifier: 'link-1' })]);

        const apiColumn = {
            Identifier: 'column-1',
            Name: 'Apps',
            SortOrder: 1,
            Icon: 'apps',
            Links: [apiLink],
            Folders: [apiFolder]
        };
        const column = ColumnMapper.mapSingle(apiColumn);
        expect(column.links).toHaveLength(1);
        expect(column.folders).toHaveLength(1);
        expect(ColumnMapper.mapToApiSingle(column)).toEqual(expect.objectContaining({
            Identifier: 'column-1',
            Links: [expect.objectContaining({ Identifier: 'link-1' })],
            Folders: [expect.objectContaining({ Identifier: 'folder-1' })]
        }));
        expect(ColumnMapper.map([apiColumn])).toHaveLength(1);
        expect(FolderMapper.map([apiFolder])).toHaveLength(1);
        expect(LinkMapper.map([apiLink])).toHaveLength(1);
        expect(ColumnMapper.mapToApi([column])).toHaveLength(1);
        expect(FolderMapper.mapToApi([folder])).toHaveLength(1);
        expect(LinkMapper.mapToApi([link])).toHaveLength(1);
    });

    it('maps location values from browser and cached coordinate shapes', () => {
        expect(LocationMapper.map(null)).toEqual(expect.objectContaining({ latitude: 0, longitude: 0 }));
        expect(LocationMapper.map({ coords: { latitude: 1, longitude: 2 } })).toEqual(expect.objectContaining({
            latitude: 1,
            longitude: 2
        }));
        expect(LocationMapper.map({ latitude: 3, longitude: 4 })).toEqual(expect.objectContaining({
            latitude: 3,
            longitude: 4
        }));
    });

    it('maps notepad dates and weather details', () => {
        expect(NotepadMapper.map({
            Identifier: 'note-1',
            Note: 'Remember this',
            CreatedAt: '2026-08-01T10:00:00.000Z',
            UpdatedAt: '2026-08-01T11:00:00.000Z'
        })).toEqual({
            identifier: 'note-1',
            note: 'Remember this',
            createdAt: new Date('2026-08-01T10:00:00.000Z'),
            updatedAt: new Date('2026-08-01T11:00:00.000Z')
        });

        expect(WeatherMapper.map({
            name: 'London',
            weather: [{ main: 'Clouds', description: 'broken clouds' }],
            coord: { lat: 51.5, lon: -0.1 },
            main: {
                feels_like: 18,
                humidity: 70,
                pressure: 1012,
                temp: 19,
                temp_max: 21,
                temp_min: 16
            },
            wind: { deg: 180, gust: 5, speed: 3 }
        })).toEqual(expect.objectContaining({
            name: 'London',
            weatherShortDescription: 'Clouds',
            weatherDescription: 'broken clouds',
            latitude: 51.5,
            longitude: -0.1,
            temperature: 19,
            windDirection: 180
        }));
    });

    it('maps optional dashboard activity fields', () => {
        expect(PiHoleMapper.mapActivity({ Identifier: 'pihole-1' })).toEqual({
            identifier: 'pihole-1',
            queriesToday: undefined,
            blockedToday: undefined,
            blockedPercentage: undefined,
            clients: undefined
        });
        expect(PiHoleMapper.mapActivities({ Response: { Data: { Activities: [{
            Identifier: 'pihole-1',
            Queries: { Total: 100, Blocked: 20, PercentBlocked: 20 },
            Clients: { Total: 4 }
        }] } } })).toEqual([expect.objectContaining({ queriesToday: 100, clients: 4 })]);

        const sessions = PlexMapper.mapActivity({ Response: { Data: { Sessions: [
            {
                User: 'alice',
                Duration: 120,
                FullTitle: 'Movie',
                State: 'playing',
                ViewOffset: 30,
                ProgressPercentage: 25,
                VideoDecision: 'directplay'
            },
            {
                User: 'bob',
                Duration: null,
                FullTitle: 'Live',
                State: 'playing',
                ViewOffset: null,
                ProgressPercentage: 0,
                VideoDecision: 'copy'
            }
        ] } } });
        expect(sessions[0]).toEqual(expect.objectContaining({ progressPercentage: 25, isLiveTv: false }));
        expect(sessions[1]).toEqual(expect.objectContaining({ progressPercentage: 100, isLiveTv: true }));

        expect(StatMapper.map({ Stats: [{
            Name: 'host',
            CpuUsage: { Percentage: 10, Total: 100, Used: 10 },
            MemoryUsage: null,
            DiskUsage: { Percentage: 20, Total: 1000, Used: 200 }
        }] })).toEqual({
            stats: [expect.objectContaining({
                name: 'host',
                cpuUsage: { percentage: 10, total: 100, used: 10 },
                memoryUsage: { percentage: undefined, total: undefined, used: undefined }
            })]
        });

        expect(UptimeKumaMapper.mapActivities({ Response: { Data: { Activities: [{
            Metrics: [{ Name: 'homepage', IsUp: true }]
        }] } } })).toEqual([{ metrics: [{ name: 'homepage', isUp: true }] }]);
    });

    it('maps qBitTorrent totals and rates from websocket payloads', () => {
        expect(QBitTorrentMapper.mapActivities({ Response: { Data: { Activities: [{
            Identifier: 'qbittorrent-1',
            TotalTorrents: 12,
            UploadRate: 1048576,
            DownloadRate: 2097152,
            TotalLeeches: 4
        }] } } })).toEqual([{
            identifier: 'qbittorrent-1',
            totalTorrents: 12,
            uploadRate: 1048576,
            downloadRate: 2097152,
            totalLeeches: 4
        }]);

        expect(QBitTorrentMapper.mapStats({ Identifier: 'qbittorrent-1' })).toEqual({
            identifier: 'qbittorrent-1',
            totalTorrents: 0,
            uploadRate: 0,
            downloadRate: 0,
            totalLeeches: 0
        });
    });

    it('maps Tautulli library and user totals from websocket payloads', () => {
        expect(TautulliMapper.mapActivities({ Response: { Data: { Activities: [{
            Identifier: 'tautulli-1',
            TotalMovies: 125,
            TotalShows: 42,
            TotalUsers: 8
        }] } } })).toEqual([{
            identifier: 'tautulli-1',
            totalMovies: 125,
            totalShows: 42,
            totalUsers: 8
        }]);

        expect(TautulliMapper.mapStats({ Identifier: 'tautulli-1' })).toEqual({
            identifier: 'tautulli-1',
            totalMovies: 0,
            totalShows: 0,
            totalUsers: 0
        });
    });
});
