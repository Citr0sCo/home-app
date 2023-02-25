import { Component, Input, OnInit } from '@angular/core';
import { ILink } from '../../services/link-service/types/link.type';
import { LinkService } from '../../services/link-service/link.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'custom-link',
    templateUrl: './custom-link.component.html',
    styleUrls: ['./custom-link.component.scss']
})
export class CustomLinkComponent implements OnInit {

    @Input()
    public item: ILink | null = null;

    public isDeleting: boolean = false;
    public isEditing: boolean = false;
    public isLoading: boolean = false;

    public isDeleted: boolean = false;
    public successMessage: string | null = null;
    public errorMessage: string | null = null;

    public form: FormGroup = new FormGroup<any>({
        name: new FormControl('', Validators.required),
        url: new FormControl('', Validators.required),
        host: new FormControl('', Validators.required),
        port: new FormControl('', Validators.required),
        isSecure: new FormControl('', Validators,required),
        iconUrl: new FormControl('', Validators.required)
    });

    private _linkService: LinkService;

    constructor(linkService: LinkService) {
        this._linkService = linkService;
    }

    public ngOnInit(): void {
        this.form = new FormGroup<any>({
            name: new FormControl(this.item?.name ?? '', Validators.required),
            url: new FormControl(this.item?.url ?? '', Validators.required),
            host: new FormControl(this.item?.host ?? '', Validators.required),
            port: new FormControl(this.item?.port ?? '', Validators.required),
            isSecure: new FormControl(this.item?.isSecure ?? false, Validators,required),
            iconUrl: new FormControl(this.item?.iconUrl ?? '', Validators.required)
        });
    }

    public deleteLink(): void {
        this.isLoading = true;

        this._linkService.deleteLink(this.item?.identifier ?? '')
            .subscribe(() => {
                this.isLoading = false;
                this.isDeleted = true;
            });
    }

    public updateLink(): void {
        this.isLoading = true;

        this._linkService.updateLink({
            identifier: this.item?.identifier ?? '',
            name: this.form.get('name')?.value,
            url: this.form.get('url')?.value,
            isSecure: this.form.get('isSecure')?.value ?? false,
            host: this.form.get('host')?.value,
            port: this.form.get('port')?.value,
            category: this.item?.category ?? '',
            sortOrder: this.item?.sortOrder ?? 0,
            iconUrl: this.form.get('iconUrl')?.value
        }).subscribe((link) => {
            this.isLoading = false;
            this.item = link;
            this.successMessage = 'Successfully updated link.';
        });
    }
}
