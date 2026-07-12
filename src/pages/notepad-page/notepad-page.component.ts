import {Component, OnDestroy, OnInit, signal, WritableSignal} from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import {INotepad} from "../../services/notepad-service/types/notepad.type";
import {NotepadService} from "../../services/notepad-service/notepad.service";

@Component({
    selector: 'notepad-page',
    templateUrl: './notepad-page.component.html',
    styleUrls: ['./notepad-page.component.scss'],
    standalone: false
})
export class NotepadPageComponent implements OnInit, OnDestroy {

    public notepad: WritableSignal<INotepad | null> = signal<INotepad | null>(null);
    public isSynchronising: WritableSignal<boolean> = signal<boolean>(false);
    public isLoading: WritableSignal<boolean> = signal<boolean>(true);

    private readonly _notepadService: NotepadService;
    private readonly _destroy: Subject<void> = new Subject();

    constructor(notepadService: NotepadService) {
        this._notepadService = notepadService;
    }

    public ngOnInit(): void {

        this.isLoading.set(true);

        this._notepadService.getNotepad()
            .pipe(takeUntil(this._destroy))
            .subscribe((notepad: INotepad | null) => {

                if(notepad == null) {
                    this.createNotepad();
                } else {
                    this.notepad.set(notepad);
                    this.isLoading.set(false);
                }
            });
    }

    public updateNote(): void {

        this.isSynchronising.set(true);

        this._notepadService
            .updateNotepad(this.notepad()!.note)
            .pipe(takeUntil(this._destroy))
            .subscribe((notepad: INotepad | null) => {

                this.isSynchronising.set(false);

                this.notepad.set(notepad);
            });
    }

    public createNotepad(): void {

        this.isLoading.set(true);

        this._notepadService
            .createNotepad()
            .pipe(takeUntil(this._destroy))
            .subscribe((notepad: INotepad | null) => {
                this.isLoading.set(false);
                this.notepad.set(notepad);
            });
    }

    public ngOnDestroy(): void {
        this._destroy.next();
    }
}
