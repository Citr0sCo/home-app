import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { ILink } from '../../services/link-service/types/link.type';
import { LinkService } from '../../services/link-service/link.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { IStatModel } from '../../services/stats-service/types/stat-model.type';
import { IColumn } from '../../services/link-service/types/column.type';

@Component({
    selector: 'custom-link',
    templateUrl: './custom-link.component.html',
    styleUrls: ['./custom-link.component.scss'],
    standalone: false
})
export class CustomLinkComponent implements OnInit, OnDestroy {

    @Input()
    public column: IColumn | null = null;

    @Input()
    public item: ILink | null = null;

    @Input()
    public stats: Array<IStatModel> = new Array<IStatModel>();

    @Input()
    public isEditModeEnabled: boolean = false;

    @Input()
    public showWidgets: boolean = false;

    @Output()
    public updated: EventEmitter<void> = new EventEmitter<void>();

    @Output()
    public deleted: EventEmitter<void> = new EventEmitter<void>();

    public isDeleting: boolean = false;
    public isEditing: boolean = false;
    public isLoading: boolean = false;
    public isDeleted: boolean = false;
    public logoUpdated: boolean = false;
    public successMessage: string | null = null;
    public errorMessage: string | null = null;
    public showIcon: boolean = true;

    public form: FormGroup = new FormGroup<any>({
        name: new FormControl('', Validators.required),
        url: new FormControl('', Validators.required),
        host: new FormControl('', Validators.required),
        port: new FormControl('', Validators.required),
        iconUrl: new FormControl('', Validators.required)
    });

    private readonly _linkService: LinkService;
    private readonly _destroy: Subject<void> = new Subject();

    constructor(linkService: LinkService) {
        this._linkService = linkService;
    }

    public ngOnInit(): void {
        this.form = new FormGroup<any>({
            name: new FormControl(this.item!.name, Validators.required),
            url: new FormControl(this.item!.url, Validators.required),
            host: new FormControl(this.item!.host, Validators.required),
            port: new FormControl(this.item!.port, Validators.required),
            iconUrl: new FormControl(this.item!.iconUrl, Validators.required)
        });
    }

    public deleteLink(): void {
        this.isLoading = true;

        this._linkService.deleteLink(this.item!.identifier!)
            .pipe(takeUntil(this._destroy))
            .subscribe(() => {
                this.isLoading = false;
                this.isDeleted = true;
                this.deleted.emit();
            });
    }

    public updateLink(): void {
        this.isLoading = true;

        this._linkService.updateLink({
            identifier: this.item!.identifier,
            containerName: this.item!.containerName,
            name: this.form.get('name')!.value,
            url: this.form.get('url')!.value,
            host: this.form.get('host')!.value,
            port: this.form.get('port')!.value,
            sortOrder: this.item!.sortOrder,
            iconUrl: this.form.get('iconUrl')!.value,
            columnId: this.column!.identifier!
        })
            .pipe(takeUntil(this._destroy))
            .subscribe((link) => {
                this.isLoading = false;
                this.item = link;
                this.successMessage = 'Successfully updated link.';
                this.updated.emit();
            });
    }

    public handleFileUpload(e: any): void {

        if (e.target.files.length === 0) {
            return;
        }

        this.isLoading = true;

        const file = e.target.files[0] as File;

        const fileReader = new FileReader();
        fileReader.readAsArrayBuffer(file);

        fileReader.onload = () => {
            const arrayBuffer = fileReader.result as ArrayBuffer;
            const blob = new Blob([arrayBuffer], { type: file.type });

            const formData = new FormData();
            formData.append('Logo', blob, file.name);

            this.showIcon = false;
            this._linkService.uploadLogo(this.item!.identifier!, formData)
                .pipe(takeUntil(this._destroy))
                .subscribe((logoUrl: string) => {
                    this.isLoading = false;
                    this.logoUpdated = true;
                    this.showIcon = true;
                    this.item!.iconUrl = logoUrl;
                });
        };
    }

    public handleIconError(): void {

        if (this.item!.iconUrl.indexOf('https://cdn.jsdelivr.net/') === -1) {
            this.item!.iconUrl = `https://cdn.jsdelivr.net/gh/selfhst/icons/png/${this.item!.name.replace(' ', '-').toLowerCase()}.png`;
            return;
        }

        this.item!.iconUrl = './assets/apps/default.png';
    }

    public ngOnDestroy(): void {
        this._destroy.next();
    }
}
