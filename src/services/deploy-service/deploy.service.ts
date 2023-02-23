import { Injectable } from '@angular/core';
import { IDeploy } from './types/deploy.type';
import { Observable, of } from 'rxjs';
import { DeployRepository } from './deploy.repository';

@Injectable()
export class DeployService {

    private _deployRepository: DeployRepository;

    constructor(deployRepository: DeployRepository) {
        this._deployRepository = deployRepository;
    }

    public getAll(): Observable<Array<IDeploy>> {
        return this._deployRepository.getAll();
    }

}