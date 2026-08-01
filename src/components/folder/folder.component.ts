import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, signal, WritableSignal } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { CdkDrag, CdkDragDrop } from '@angular/cdk/drag-drop';
import { LinkService } from '../../services/link-service/link.service';
import { IColumn } from '../../services/link-service/types/column.type';
import { IFolder } from '../../services/link-service/types/folder.type';
import { IStatModel } from '../../services/stats-service/types/stat-model.type';

@Component({
    selector: 'folder',
    templateUrl: './folder.component.html',
    styleUrls: ['./folder.component.scss'],
    standalone: false
})
export class FolderComponent implements OnInit, OnDestroy {

    @Input()
    public folder: IFolder | null = null;

    @Input()
    public column: IColumn | null = null;

    @Input()
    public connectedTo: Array<string> = [];

    @Input()
    public isEditModeEnabled: WritableSignal<boolean> = signal<boolean>(false);

    @Input()
    public allStats: WritableSignal<Array<IStatModel>> = signal<Array<IStatModel>>(new Array<IStatModel>());

    @Input()
    public showWidgets: WritableSignal<boolean> = signal<boolean>(false);

    @Output()
    public updated: EventEmitter<void> = new EventEmitter<void>();

    @Output()
    public dropped: EventEmitter<CdkDragDrop<Array<string>>> = new EventEmitter<CdkDragDrop<Array<string>>>();

    public isExpanded: WritableSignal<boolean> = signal<boolean>(false);
    public isEditing: WritableSignal<boolean> = signal<boolean>(false);
    public isDeleting: WritableSignal<boolean> = signal<boolean>(false);
    public isLoading: WritableSignal<boolean> = signal<boolean>(false);
    public errorMessage: WritableSignal<string | null> = signal<string | null>(null);

    public form: FormGroup = new FormGroup<any>({
        name: new FormControl('', Validators.required),
        icon: new FormControl('', Validators.required)
    });

    private readonly _linkService: LinkService;
    private readonly _destroy: Subject<void> = new Subject();

    constructor(linkService: LinkService) {
        this._linkService = linkService;
    }

    public ngOnInit(): void {
        this.form = new FormGroup<any>({
            name: new FormControl(this.folder!.name, Validators.required),
            icon: new FormControl(this.folder!.icon, Validators.required)
        });
    }

    @Input()
    public enterPredicate: (drag: CdkDrag) => boolean = () => true;

    public updateFolder(): void {
        this.isLoading.set(true);

        this.folder!.name = this.form.get('name')!.value;
        this.folder!.icon = this.form.get('icon')!.value;

        this._linkService.updateFolder(this.folder!)
            .pipe(takeUntil(this._destroy))
            .subscribe(() => {
                this.isLoading.set(false);
                this.isEditing.set(false);
                this.updated.next();
            }, () => {
                this.isLoading.set(false);
                this.errorMessage.set('Failed to update the folder.');
            });
    }

    public deleteFolder(): void {
        this.isLoading.set(true);

        this._linkService.deleteFolder(this.folder!.identifier!)
            .pipe(takeUntil(this._destroy))
            .subscribe(() => {
                this.isLoading.set(false);
                this.updated.next();
            }, () => {
                this.isLoading.set(false);
                this.errorMessage.set('Failed to delete the folder.');
            });
    }

    public ngOnDestroy(): void {
        this._destroy.next();
    }
}
