import { Component, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { ConfigsService } from '../../services/configs-service/configs.service';
import { ISetting } from '../../services/configs-service/types/setting.type';

@Component({
    selector: 'settings-page',
    templateUrl: './settings-page.component.html',
    styleUrls: ['./settings-page.component.scss'],
    standalone: false
})
export class SettingsPageComponent implements OnInit, OnDestroy {

    public settings: WritableSignal<Array<ISetting>> = signal<Array<ISetting>>([]);
    public isLoading: WritableSignal<boolean> = signal<boolean>(true);
    public isSaving: WritableSignal<boolean> = signal<boolean>(false);
    public hasError: WritableSignal<boolean> = signal<boolean>(false);
    public isSaved: WritableSignal<boolean> = signal<boolean>(false);
    public visibleSecrets: WritableSignal<Set<string>> = signal<Set<string>>(new Set<string>());

    private readonly _configsService: ConfigsService;
    private readonly _destroy: Subject<void> = new Subject();

    constructor(configsService: ConfigsService) {
        this._configsService = configsService;
    }

    public ngOnInit(): void {
        this.loadSettings();
    }

    public loadSettings(): void {
        this.isLoading.set(true);
        this.hasError.set(false);

        this._configsService.getAllSettings()
            .pipe(takeUntil(this._destroy))
            .subscribe({
                next: (settings) => {
                    this.settings.set(settings);
                    this.isLoading.set(false);
                },
                error: () => {
                    this.hasError.set(true);
                    this.isLoading.set(false);
                }
            });
    }

    public saveSettings(): void {
        this.isSaving.set(true);
        this.isSaved.set(false);
        this.hasError.set(false);

        this._configsService.updateSettings(this.settings())
            .pipe(takeUntil(this._destroy))
            .subscribe({
                next: (settings) => {
                    this.settings.set(settings);
                    this.isSaving.set(false);
                    this.isSaved.set(true);
                },
                error: () => {
                    this.hasError.set(true);
                    this.isSaving.set(false);
                }
            });
    }

    public toggleSecret(key: string): void {
        const visibleSecrets = new Set(this.visibleSecrets());
        if (visibleSecrets.has(key)) {
            visibleSecrets.delete(key);
        } else {
            visibleSecrets.add(key);
        }
        this.visibleSecrets.set(visibleSecrets);
    }

    public isSecretVisible(key: string): boolean {
        return this.visibleSecrets().has(key);
    }

    public ngOnDestroy(): void {
        this._destroy.next();
        this._destroy.complete();
    }
}
