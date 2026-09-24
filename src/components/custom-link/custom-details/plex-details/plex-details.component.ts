import { Component, Input, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { catchError, of, Subject, switchMap, takeUntil, timer } from 'rxjs';
import { ILink } from '../../../../services/link-service/types/link.type';
import { IPlexSession } from '../../../../services/plex-service/types/plex-session.type';
import { PlexService } from '../../../../services/plex-service/plex.service';

@Component({
    selector: 'plex-details',
    templateUrl: './plex-details.component.html',
    styleUrls: ['./plex-details.component.scss'],
    standalone: false
})
export class PlexDetailsComponent implements OnInit, OnDestroy {

    @Input()
    public item: ILink | null = null;

    public plexSessions: WritableSignal<Array<IPlexSession>> = signal<Array<IPlexSession>>([]);
    public isLoading: WritableSignal<boolean> = signal<boolean>(true);

    private readonly _destroy: Subject<void> = new Subject();
    private readonly _plexService: PlexService;

    constructor(plexService: PlexService) {
        this._plexService = plexService;
    }

    public ngOnInit() {
        timer(0, 5000)
            .pipe(
                switchMap(() => this._plexService.getActivity().pipe(
                    catchError(() => of(new Array<IPlexSession>()))
                )),
                takeUntil(this._destroy)
            )
            .subscribe((response: Array<IPlexSession>) => {
                this.plexSessions.set(response);
                this.isLoading.set(false);
            });
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

    public trimTitle(title: string): string {
        if (title.length > 18) {
            return `${title.slice(0, 18).trim()}...`;
        }
        return title;
    }

    public ngOnDestroy(): void {
        this._destroy.next();
        this._destroy.complete();
    }
}
