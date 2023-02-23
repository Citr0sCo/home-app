import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BuildRepository } from './build.repository';
import { IBuild } from './types/build.type';

@Injectable()
export class BuildService {

    private _statRepository: BuildRepository;

    constructor(deployRepository: BuildRepository) {
        this._statRepository = deployRepository;
    }

    public getAll(): Observable<Array<IBuild>> {
        return this._statRepository.getAll();
    }

}