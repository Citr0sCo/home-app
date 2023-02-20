import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {map, Observable} from "rxjs";
import {LinkMapper} from "./link.mapper";

@Injectable()
export class LinkRepository {

    private _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public getAllLinks(): Observable<any> {
        return this._httpClient.get('/api/links')
            .pipe(map((response:any) => {
                return LinkMapper.map(response.Links);
            }));
    }
}