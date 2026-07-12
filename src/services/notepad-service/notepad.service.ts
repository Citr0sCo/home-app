import { Injectable } from '@angular/core';
import { NotepadRepository } from './notepad.repository';
import {INotepad} from "./types/notepad.type";
import {map, Observable} from "rxjs";
import {NotepadMapper} from "./notepad.mapper";

@Injectable()
export class NotepadService {

    private _notepadRepository: NotepadRepository;

    constructor(notepadRepository: NotepadRepository) {
        this._notepadRepository = notepadRepository;
    }

    public getNotepad(): Observable<INotepad | null> {
        return this._notepadRepository
            .getNotepad()
            .pipe(
                map((response: any) => {

                    if(response == null) {
                        return response;
                    }

                    return NotepadMapper.map(response);
                })
            );
    }

    public createNotepad(): Observable<INotepad | null> {
        return this._notepadRepository
            .createNotepad()
            .pipe(
                map((response: any) => {

                    if(response == null) {
                        return response;
                    }

                    return NotepadMapper.map(response);
                })
            );
    }

    public updateNotepad(note: string): Observable<INotepad | null> {
        return this._notepadRepository
            .updateNotepad(note)
            .pipe(
                map((response: any) => {

                    if(response == null) {
                        return response;
                    }

                    return NotepadMapper.map(response);
                })
            );
    }
}