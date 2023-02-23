import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { StatRepository } from './stat.repository';
import { IStatResponse } from './types/stat.response';

@Injectable()
export class StatService {

    private _statRepository: StatRepository;

    constructor(deployRepository: StatRepository) {
        this._statRepository = deployRepository;
    }

    public getAll(): Observable<IStatResponse> {
        return this._statRepository.getAll();
    }

}