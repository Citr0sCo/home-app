import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { ILink } from '../../services/link-service/types/link.type';
import { Subscription } from 'rxjs';
import { PlexService } from '../../services/plex-service/plex.service';
import { IPlexSession } from '../../services/plex-service/types/plex-session.type';

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
                .subscribe((response) => {
                    this.plexSessions = response;
                })
        );

        setInterval(() => {
            this._plexService.getActivity()
                .subscribe((response) => {
                    this.plexSessions = response;
                });
        }, 15000);

        setInterval(() => {
            for (let session of this.plexSessions) {
                if (session.state === 'playing') {
                    session.viewOffset += 1000;
                }
            }
        }, 1000);
    }

    public getTimeFromDuration(duration: number): string {

        let date = new Date(duration);

        let displayText = '';

        let hours = date.getUTCHours();
        if (hours > 0)
            displayText += `${hours}:`;

        let minutes = date.getMinutes();
        if (minutes < 10)
            displayText += `0${minutes}:`;
        else
            displayText += `${minutes}:`;

        let seconds = date.getSeconds();
        if (seconds < 10)
            displayText += `0${seconds}`;
        else
            displayText += `${seconds}`;

        return displayText;
    }

    public ngOnDestroy() {
        this._subscriptions.unsubscribe();
    }
}
