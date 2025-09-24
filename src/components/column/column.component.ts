import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { IColumn } from '../../services/link-service/types/column.type';
import { IStatModel } from '../../services/stats-service/types/stat-model.type';
import { BehaviorSubject, Subject, takeUntil } from 'rxjs';
import { LinkService } from '../../services/link-service/link.service';
import { ILink } from '../../services/link-service/types/link.type';
import { CdkDragDrop } from '@angular/cdk/drag-drop';
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
    public columns: Array<IColumn> = new Array<IColumn>();

    @Input()
    public isEditModeEnabled: boolean = false;

    @Input()
    public allStats: Array<IStatModel> = new Array<IStatModel>();

    @Input()
    public showWidgets: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

    @Output()
    public updated: EventEmitter<void> = new EventEmitter<void>();

    public isEditing: boolean = false;
    public isDeleting: boolean = false;
    public isLoading: boolean = false;
    public successMessage: string | null = null;
    public errorMessage: string | null = null;

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

    public getLastSortOrder(links: Array<ILink>): number {
        return links.length;
    }

    public refreshLinkCache(): void {
        this.updated.next();
    }

    public drop(targetColumn: IColumn, $event: CdkDragDrop<Array<string>>): void {

        const item = $event.item.data as ILink;

        this.columns = this.columns.map((column) => {

            if (column.identifier === item.columnId) {
                column.links.splice($event.previousIndex, 1);
            }

            return column;
        });

        item.columnId = targetColumn.identifier!;
        targetColumn.links.splice($event.currentIndex, 0, item);
    }

    public getOtherColumnIds(column: IColumn): Array<string> {
        return this.columns
            .filter((x) => x.identifier !== column.identifier)
            .map((x) => {
                return x.identifier!;
            });
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
}
