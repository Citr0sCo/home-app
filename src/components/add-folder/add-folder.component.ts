import { Component, EventEmitter, Input, Output, signal, WritableSignal } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { LinkService } from '../../services/link-service/link.service';
import { IColumn } from '../../services/link-service/types/column.type';
import { IFolder } from '../../services/link-service/types/folder.type';

@Component({
    selector: 'add-folder',
    templateUrl: './add-folder.component.html',
    styleUrls: ['./add-folder.component.scss'],
    standalone: false
})
export class AddFolderComponent {

    @Input()
    public column: IColumn | null = null;

    @Input()
    public sortOrder: number = 0;

    @Output()
    public item: EventEmitter<IFolder> = new EventEmitter<IFolder>();

    public isAddingFolder: WritableSignal<boolean> = signal<boolean>(false);
    public isLoading: WritableSignal<boolean> = signal<boolean>(false);
    public successMessage: WritableSignal<string | null> = signal<string | null>(null);
    public errorMessage: WritableSignal<string | null> = signal<string | null>(null);

    public form: FormGroup = new FormGroup<any>({
        name: new FormControl('', Validators.required),
        icon: new FormControl('fas fa-folder', Validators.required)
    });

    private _linkService: LinkService;

    constructor(linkService: LinkService) {
        this._linkService = linkService;
    }

    public addFolder(): void {
        this.isLoading.set(true);

        this._linkService.createFolder({
            identifier: null,
            name: this.form.get('name')!.value,
            icon: this.form.get('icon')!.value,
            sortOrder: this.sortOrder,
            columnId: this.column!.identifier!,
            links: []
        }).subscribe((folder: IFolder) => {
            this.isLoading.set(false);
            this.successMessage.set('Successfully added folder.');
            this.item.emit(folder);

        }, () => {
            this.isLoading.set(false);
            this.errorMessage.set('Failed to add a folder.');
        });
    }
}
