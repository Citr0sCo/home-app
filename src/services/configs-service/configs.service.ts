import { Injectable } from '@angular/core';
import { IConfigs } from './types/configs.type';
import { Observable, of, tap } from 'rxjs';
import { ConfigsRepository } from './configs.repository';
import { ISetting } from './types/setting.type';

@Injectable()
export class ConfigsService {

    private _configsRepository: ConfigsRepository;
    private _cachedConfigs: { configs: IConfigs; timestamp: Date } | null = null;

    constructor(configsRepository: ConfigsRepository) {
        this._configsRepository = configsRepository;
    }

    public refreshCache(): Observable<IConfigs> {
        return this._configsRepository.getAllConfigs()
            .pipe(
                tap((configs) => {
                    this._cachedConfigs = { configs: configs, timestamp: new Date() };
                    localStorage.setItem('cachedConfigs', JSON.stringify(configs));
                })
            );
    }

    public getAllConfigs(force: boolean = false): Observable<IConfigs> {

        if (localStorage.getItem('cachedConfigs')) {
            this._cachedConfigs = { configs: JSON.parse(`${localStorage.getItem('cachedConfigs')}`), timestamp: new Date() };
        }

        if (this._cachedConfigs !== null && this._cachedConfigs.configs !== null && !force) {
            const differenceInTime = new Date().getTime() - new Date(this._cachedConfigs.timestamp).getTime();

            const hourInMilliseconds = 1000 * 60 * 60;

            if (differenceInTime < hourInMilliseconds) {
                return of(this._cachedConfigs.configs);
            }
        }

        return this.refreshCache();
    }

    public getAllSettings(): Observable<Array<ISetting>> {
        return this._configsRepository.getAllSettings();
    }

    public updateSettings(settings: Array<ISetting>): Observable<Array<ISetting>> {
        return this._configsRepository.updateSettings(settings).pipe(
            tap(() => {
                this._cachedConfigs = null;
                localStorage.removeItem('cachedConfigs');
            })
        );
    }
}
