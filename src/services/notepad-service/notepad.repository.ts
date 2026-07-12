import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable()
export class NotepadRepository {

    private _httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public getNotepad(): Observable<any> {
        return this._httpClient.get(`${environment.apiBaseUrl}/api/notepad`, {})
            .pipe(
                map((response: any) => {
                    return response.Notepad;
                })
            );
    }

    public createNotepad(): Observable<any> {
        return this._httpClient.post(`${environment.apiBaseUrl}/api/notepad`, {})
            .pipe(
                map((response: any) => {
                    return response.Notepad;
                })
            );
    }

    public updateNotepad(note: string): Observable<any> {
        return this._httpClient.patch(`${environment.apiBaseUrl}/api/notepad`, { Note: note })
            .pipe(
                map((response: any) => {
                    return response.Notepad;
                })
            );
    }

}