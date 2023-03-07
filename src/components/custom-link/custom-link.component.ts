import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { ILink } from '../../services/link-service/types/link.type';
import { LinkService } from '../../services/link-service/link.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { IStatResponse } from '../../services/stats-service/types/stat.response';
import { Subscription } from 'rxjs';
import { StatService } from '../../services/stats-service/stat.service';
import { IStatModel } from '../../services/stats-service/types/stat-model.type';

@Component({
    selector: 'custom-link',
    templateUrl: './custom-link.component.html',
    styleUrls: ['./custom-link.component.scss']
})
export class CustomLinkComponent implements OnInit, OnDestroy {

    @Input()
    public item: ILink | null = null;

    @Input()
    public stats: Array<IStatModel> = new Array<IStatModel>();

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
        isSecure: new FormControl('', Validators.required),
        iconUrl: new FormControl('', Validators.required)
    });

    private readonly _linkService: LinkService;
    private readonly _subscriptions: Subscription = new Subscription();

    constructor(linkService: LinkService) {
        this._linkService = linkService;
    }

    public ngOnInit(): void {
        this.form = new FormGroup<any>({
            name: new FormControl(this.item?.name ?? '', Validators.required),
            url: new FormControl(this.item?.url ?? '', Validators.required),
            host: new FormControl(this.item?.host ?? '', Validators.required),
            port: new FormControl(this.item?.port ?? '', Validators.required),
            isSecure: new FormControl(this.item?.isSecure ?? false, Validators.required),
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
            containerName: this.item?.containerName ?? '',
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

    public ngOnDestroy(): void {
        this._subscriptions.unsubscribe();
    }
}
