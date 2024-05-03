import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { SpotifyService } from '../../services/spotify-service/spotify.service';

@Component({
    selector: 'spotify-page',
    templateUrl: './spotify-page.component.html',
    styleUrls: ['./spotify-page.component.scss']
})
export class SpotifyPageComponent implements OnInit, OnDestroy {

    private readonly _destroy: Subject<void> = new Subject();

    private _state: string = 'B5F46EB1354B4FB4';
    private _scope: string = 'playlist-modify-public';
    private _clientId: string = '495e9e3ad07f4c5b9a854df115e2cf43';

    private readonly _route: ActivatedRoute;
    private readonly _spotifyService: SpotifyService;
    private readonly _router: Router;

    constructor(route: ActivatedRoute, router: Router, spotifyService: SpotifyService) {
        this._route = route;
        this._router = router;
        this._spotifyService = spotifyService;
    }

    public ngOnInit(): void {
        this._route.queryParams
            .pipe(takeUntil(this._destroy))
            .subscribe((params: any) => {
                if (params.hasOwnProperty('state') && this._state === params.state && params.hasOwnProperty('code')) {
                    this._spotifyService.execute(params.code)
                        .pipe(takeUntil(this._destroy))
                        .subscribe(() => {
                            this._router.navigate([], {
                                queryParams: {
                                    state: null,
                                    code: null
                                },
                                queryParamsHandling: 'merge'
                            });
                        });
                }
            });
    }

    public openAuthLink(): void {
        location.href = `https://accounts.spotify.com/authorize?response_type=code&client_id=${this._clientId}&scope=${this._scope}&redirect_uri=http://localhost:4200/spotify&state=${this._state}`;
    }

    public ngOnDestroy(): void {
        this._destroy.next();
    }
}
