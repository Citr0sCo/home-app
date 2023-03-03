import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { ILink } from '../../services/link-service/types/link.type';
import { Subscription } from 'rxjs';
import { PlexService } from '../../services/plex-service/plex.service';
import { IPlexSession } from '../../services/plex-service/types/plex-session.type';
import { IBuild } from '../../services/build-service/types/build.type';

@Component({
    selector: 'plex-link',
    templateUrl: './plex-link.component.html',
    styleUrls: ['./plex-link.component.scss']
})
export class PlexLinkComponent implements OnInit, OnDestroy {

    @Input()
    public item: ILink | null = null;

    public plexSessions: Array<IPlexSession> = [];

    private _subscriptions: Subscription = new Subscription();
    private readonly _plexService: PlexService;

    constructor(plexService: PlexService) {
        this._plexService = plexService;
    }

    public ngOnInit() {
        this._subscriptions.add(
            this._plexService.getActivity()
                .subscribe((response: Array<IPlexSession>) => {
                    this.plexSessions = response;
                })
        );

        this._subscriptions.add(
            this._plexService.sessions
                .asObservable()
                .subscribe((response: Array<IPlexSession>) => {
                    this.plexSessions = response;
                })
        );

        setInterval(() => {
            for (const session of this.plexSessions) {
                if (session.state === 'playing') {
                    session.viewOffset += 1000;
                }
            }
        }, 1000);

        this._plexService.ngOnInit();
    }

    public getTimeFromDuration(duration: number): string {

        const date = new Date(duration);

        let displayText = '';

        const hours = date.getUTCHours();
        if (hours > 0) {
            displayText += `${hours}:`;
        }

        const minutes = date.getMinutes();
        if (minutes < 10) {
            displayText += `0${minutes}:`;
        } else {
            displayText += `${minutes}:`;
        }

        const seconds = date.getSeconds();
        if (seconds < 10) {
            displayText += `0${seconds}`;
        } else {
            displayText += `${seconds}`;
        }

        return displayText;
    }

    public ngOnDestroy() {
        this._plexService.ngOnDestroy();

        this._subscriptions.unsubscribe();
    }
}
