import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, signal, WritableSignal } from '@angular/core';
import { IColumn } from '../../services/link-service/types/column.type';
import { IStatModel } from '../../services/stats-service/types/stat-model.type';
import { Subject, takeUntil } from 'rxjs';
import { LinkService } from '../../services/link-service/link.service';
import { ILink } from '../../services/link-service/types/link.type';
import { IFolder } from '../../services/link-service/types/folder.type';
import { IColumnItem } from '../../services/link-service/types/column-item.type';
import { ColumnItems } from '../../services/link-service/column-items';
import { CdkDrag, CdkDragDrop } from '@angular/cdk/drag-drop';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'column',
    templateUrl: './column.component.html',
    styleUrls: ['./column.component.scss'],
    standalone: false
})
export class ColumnComponent implements OnInit, OnDestroy {

    @Input()
    public column: IColumn | null = null;

    @Input()
    public columns: WritableSignal<Array<IColumn>> = signal<Array<IColumn>>(new Array<IColumn>());

    @Input()
    public isEditModeEnabled: WritableSignal<boolean> = signal<boolean>(false);

    @Input()
    public allStats: WritableSignal<Array<IStatModel>> = signal<Array<IStatModel>>(new Array<IStatModel>());

    @Input()
    public showWidgets: WritableSignal<boolean> = signal<boolean>(false);

    @Output()
    public updated: EventEmitter<void> = new EventEmitter<void>();

    public isEditing: WritableSignal<boolean> = signal<boolean>(false);
    public isDeleting: WritableSignal<boolean> = signal<boolean>(false);
    public isLoading: WritableSignal<boolean> = signal<boolean>(false);
    public successMessage: WritableSignal<string | null> = signal<string | null>(null);
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
            name: new FormControl(this.column!.name, Validators.required),
            icon: new FormControl(this.column!.icon, Validators.required)
        });
    }

    public getItems(column: IColumn): Array<IColumnItem> {
        return ColumnItems.sort(column);
    }

    // Folders may sit alongside links in a column, but never inside another folder.
    public onlyLinksPredicate = (drag: CdkDrag): boolean => {
        return !ColumnItems.isFolder(drag.data);
    };

    public refreshLinkCache(): void {
        this.updated.next();
    }

    public dropIntoColumn(column: IColumn, $event: CdkDragDrop<Array<string>>): void {

        const dragged = $event.item.data as ILink | IFolder;
        const source = this.findDropContainer($event.previousContainer.id);

        if (source === null) {
            return;
        }

        this.removeFromSource(source, dragged);

        const items = ColumnItems.sort(column);
        items.splice($event.currentIndex, 0, ColumnItems.wrap(dragged));

        ColumnItems.write(column, items);

        this.columns.set([...this.columns()]);
    }

    public dropIntoFolder(folder: IFolder, $event: CdkDragDrop<Array<string>>): void {

        const dragged = $event.item.data as ILink | IFolder;

        if (ColumnItems.isFolder(dragged)) {
            return;
        }

        const source = this.findDropContainer($event.previousContainer.id);

        if (source === null) {
            return;
        }

        this.removeFromSource(source, dragged);

        folder.links.splice($event.currentIndex, 0, dragged);

        folder.links.forEach((link, index) => {
            link.sortOrder = index;
            link.columnId = folder.columnId;
            link.folderId = folder.identifier;
        });

        this.columns.set([...this.columns()]);
    }

    public getConnectedDropListIds(currentId: string | null): Array<string> {

        const ids: Array<string> = [];

        this.columns().forEach((column) => {
            ids.push(column.identifier!);
            column.folders.forEach((folder) => ids.push(folder.identifier!));
        });

        return ids.filter((id) => id !== currentId);
    }

    public updateColumn(): void {

        this.column!.name = this.form.get('name')!.value;
        this.column!.icon = this.form.get('icon')!.value;

        this._linkService.updateColumn(this.column!)
            .pipe(takeUntil(this._destroy))
            .subscribe(() => {
                this.refreshLinkCache();
            });
    }

    public deleteColumn(): void {
        this._linkService.deleteColumn(this.column!.identifier!)
            .pipe(takeUntil(this._destroy))
            .subscribe(() => {
                this.refreshLinkCache();
            });
    }

    public ngOnDestroy(): void {
        this._destroy.next();
    }

    private removeFromSource(source: IColumn | IFolder, dragged: ILink | IFolder): void {

        if (ColumnItems.isFolder(dragged)) {
            const folders = (source as IColumn).folders;
            folders.splice(folders.indexOf(dragged), 1);
            return;
        }

        source.links.splice(source.links.indexOf(dragged), 1);
    }

    private findDropContainer(identifier: string): IColumn | IFolder | null {

        for (const column of this.columns()) {

            if (column.identifier === identifier) {
                return column;
            }

            const folder = column.folders.find((x) => x.identifier === identifier);

            if (folder) {
                return folder;
            }
        }

        return null;
    }
}
