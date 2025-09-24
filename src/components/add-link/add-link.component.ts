import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ILink } from '../../services/link-service/types/link.type';
import { LinkService } from '../../services/link-service/link.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { IColumn } from '../../services/link-service/types/column.type';

@Component({
    selector: 'add-link',
    templateUrl: './add-link.component.html',
    styleUrls: ['./add-link.component.scss'],
    standalone: false
})
export class AddLinkComponent {

    @Input()
    public column: IColumn | null = null;

    @Input()
    public sortOrder: number = 0;

    @Output()
    public item: EventEmitter<ILink> = new EventEmitter<ILink>();

    public isAddingLink: boolean = false;
    public isLoading: boolean = false;
    public successMessage: string | null = null;
    public errorMessage: string | null = null;

    public form: FormGroup = new FormGroup<any>({
        name: new FormControl('', Validators.required),
        url: new FormControl('', Validators.required),
        host: new FormControl('', Validators.required),
        port: new FormControl('', Validators.required),
        iconUrl: new FormControl('./assets/apps/default.png', Validators.required)
    });

    private _linkService: LinkService;

    constructor(linkService: LinkService) {
        this._linkService = linkService;
    }

    public addLink(): void {
        this.isLoading = true;

        this._linkService.addLink({
            identifier: null,
            containerName: this.form.get('containerName')?.value,
            name: this.form.get('name')?.value,
            url: this.form.get('url')?.value,
            host: this.form.get('host')?.value,
            port: this.form.get('port')?.value,
            sortOrder: this.sortOrder,
            iconUrl: this.form.get('iconUrl')?.value,
            columnId: this.column!.identifier!
        }).subscribe((link: ILink) => {
            this.isLoading = false;
            this.successMessage = 'Successfully added link.';
            this.item.emit(link);

        }, () => {
            this.isLoading = false;
            this.errorMessage = 'Failed to add a link.';
        });
    }
}
