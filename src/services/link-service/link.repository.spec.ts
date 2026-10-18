import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { environment } from '../../environments/environment';
import { LinkRepository } from './link.repository';

describe('LinkRepository', () => {
    let repository: LinkRepository;
    let http: HttpTestingController;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                LinkRepository,
                provideHttpClient(),
                provideHttpClientTesting()
            ]
        });

        repository = TestBed.inject(LinkRepository);
        http = TestBed.inject(HttpTestingController);
    });

    afterEach(() => {
        http.verify();
        TestBed.resetTestingModule();
    });

    it('attaches persisted last-access timestamps to column and folder links', () => {
        const columnLinkId = 'a3e832d1-1507-4abc-aebc-9d9bf640f001';
        const folderLinkId = 'a3e832d1-1507-4abc-aebc-9d9bf640f002';
        const columnTimestamp = '2026-10-01T10:00:00Z';
        const folderTimestamp = '2026-10-02T11:00:00Z';
        let result: any;

        const subscription = repository.getAllColumns().subscribe((columns) => result = columns);

        http.expectOne(`${environment.apiBaseUrl}/api/columns`).flush({
            Columns: [{
                Identifier: '9e1732b6-eef7-4a5b-8ceb-c9fc0c3b3001',
                Name: 'Services',
                SortOrder: 0,
                Icon: 'server',
                Links: [createLink(columnLinkId)],
                Folders: [{
                    Identifier: '9e1732b6-eef7-4a5b-8ceb-c9fc0c3b3002',
                    Name: 'Media',
                    Icon: 'folder',
                    SortOrder: 0,
                    ColumnId: '9e1732b6-eef7-4a5b-8ceb-c9fc0c3b3001',
                    Links: [createLink(folderLinkId)]
                }]
            }]
        });

        http.expectOne(`${environment.apiBaseUrl}/api/links/last-accessed`).flush({
            Links: [
                { Identifier: columnLinkId.toUpperCase(), LastAccessedAt: columnTimestamp },
                { Identifier: folderLinkId, LastAccessedAt: folderTimestamp }
            ]
        });

        expect(result[0].links[0].lastClickedAt).toBe(columnTimestamp);
        expect(result[0].folders[0].links[0].lastClickedAt).toBe(folderTimestamp);
        subscription.unsubscribe();
    });

    function createLink(identifier: string): any {
        return {
            Identifier: identifier,
            Name: 'Dashboard',
            Url: 'https://example.com',
            Host: 'example.com',
            Port: 443,
            IsSecure: true,
            IconUrl: 'icon.png',
            SortOrder: 0,
            ColumnId: '9e1732b6-eef7-4a5b-8ceb-c9fc0c3b3001'
        };
    }
});
