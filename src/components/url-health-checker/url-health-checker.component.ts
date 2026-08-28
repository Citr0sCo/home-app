import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnDestroy, OnInit, Output, signal, WritableSignal } from '@angular/core';
import { first, Subject, takeUntil } from 'rxjs';
import { HealthCheckService } from '../../services/healthcheck-service/healthcheck.service';

@Component({
    selector: 'url-health-checker',
    templateUrl: './url-health-checker.component.html',
    styleUrls: ['./url-health-checker.component.scss'],
    standalone: false,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class UrlHealthCheckerComponent implements OnInit, OnDestroy {

    @Input()
    public url: string = '';

    @Input()
    public host: string = '';

    @Input()
    public port: number = 0;

    @Output()
    public statusChanged: EventEmitter<string> = new EventEmitter<string>();

    public isSecure: WritableSignal<boolean> = signal<boolean>(false);

    public status: WritableSignal<string> = signal<string>('');

    public statusDescription: WritableSignal<string> = signal<string>('Unknown state');

    public responseTime: WritableSignal<number> = signal<number>(0);

    public isLoading: WritableSignal<boolean> = signal<boolean>(true);

    private readonly _destroy: Subject<void> = new Subject();
    private readonly _healthCheckService: HealthCheckService;

    constructor(healthCheckService: HealthCheckService) {
        this._healthCheckService = healthCheckService;
    }

    public ngOnInit(): void {

        this.isSecure.set(this.url.toLowerCase().startsWith('https://'));

        const target = this.port > 0 ? `${this.host}:${this.port}` : this.host;

        this._healthCheckService.check(target, this.isSecure())
            .pipe(
                first(),
                takeUntil(this._destroy)
            )
            .subscribe({
                next: (response: any) => {
                    if (response.StatusCode.toString()[0] === '2' || response.StatusCode.toString()[0] === '3') {
                        this.status.set('up');
                        this.statusDescription.set('Service is reachable.');
                    } else if (response.StatusCode.toString()[0] === '4') {
                        this.status.set('warning');
                        this.statusDescription.set(`Service has returned an '${response.StatusDescription}' response.`);
                    } else {
                        this.status.set('down');
                        this.statusDescription.set(response.StatusDescription);
                    }
                    this.responseTime.set(response.DurationInMilliseconds);
                    this.isLoading.set(false);
                    this.statusChanged.emit(this.status());
                },
                error: (error) => {
                    this.status.set('down');
                    this.statusDescription.set('Service is down.');
                    this.responseTime.set(0);
                    this.isLoading.set(false);
                    this.statusChanged.emit(this.status());
                    console.error(error);
                }
            });
    }

    public determineResponseTime(responseTime: number): string {

        if (responseTime >= 1000) {
            return `${Math.round((responseTime / 1000) * 100) / 100} s`;
        }

        return `${responseTime} ms`;
    }

    public ngOnDestroy(): void {
        this._destroy.next();
        this._destroy.complete();
    }
}
