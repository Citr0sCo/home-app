import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, signal, WritableSignal } from '@angular/core';
import { ILink } from '../../services/link-service/types/link.type';
import { LinkService } from '../../services/link-service/link.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { IStatModel } from '../../services/stats-service/types/stat-model.type';
import { IColumn } from '../../services/link-service/types/column.type';
import { PendingRequestCancellationService } from '../../services/pending-request-cancellation.service';
import { environment } from '../../environments/environment';

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
    public stats: WritableSignal<Array<IStatModel>> = signal<Array<IStatModel>>(new Array<IStatModel>());

    @Input()
    public isEditModeEnabled: WritableSignal<boolean> = signal<boolean>(false);

    @Input()
    public showWidgets: WritableSignal<boolean> = signal<boolean>(false);

    @Output()
    public updated: EventEmitter<ILink> = new EventEmitter<ILink>();

    @Output()
    public deleted: EventEmitter<ILink> = new EventEmitter<ILink>();

    @Output()
    public statusChanged: EventEmitter<string> = new EventEmitter<string>();

    public isDeleting: WritableSignal<boolean> = signal<boolean>(false);
    public isEditing: WritableSignal<boolean> = signal<boolean>(false);
    public isLoading: WritableSignal<boolean> = signal<boolean>(false);
    public isDeleted: WritableSignal<boolean> = signal<boolean>(false);
    public logoUpdated: WritableSignal<boolean> = signal<boolean>(false);
    public successMessage: WritableSignal<string | null> = signal<string | null>(null);
    public errorMessage: WritableSignal<string | null> = signal<string | null>(null);
    public showIcon: WritableSignal<boolean> = signal<boolean>(true);

    public form: FormGroup = new FormGroup<any>({
        name: new FormControl('', Validators.required),
        url: new FormControl('', Validators.required),
        host: new FormControl('', Validators.required),
        port: new FormControl('', Validators.required),
        iconUrl: new FormControl('', Validators.required)
    });

    private readonly _linkService: LinkService;
    private readonly _pendingRequestCancellationService: PendingRequestCancellationService;
    private readonly _destroy: Subject<void> = new Subject();

    constructor(linkService: LinkService, pendingRequestCancellationService: PendingRequestCancellationService) {
        this._linkService = linkService;
        this._pendingRequestCancellationService = pendingRequestCancellationService;
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

    public recordClick(event: MouseEvent): void {
        if (!this.item?.identifier) {
            return;
        }

        event.preventDefault();
        this._linkService.recordLinkClickLocally(this.item);
        this._pendingRequestCancellationService.cancelAll();
        this.recordClickWithoutBlocking(this.item.identifier);
        window.location.assign(this.item.url);
    }

    public getLastClickedStatus(): 'never' | 'recent' | 'week' | 'month' {
        return this._linkService.getLastClickedStatus(this.item?.lastClickedAt);
    }

    public deleteLink(): void {
        this.isLoading.set(true);

        this._linkService.deleteLink(this.item!.identifier!)
            .pipe(takeUntil(this._destroy))
            .subscribe(() => {
                this.isLoading.set(false);
                this.isDeleted.set(true);
                this.deleted.emit(this.item!);
            });
    }

    public updateLink(): void {
        this.isLoading.set(true);

        this._linkService.updateLink({
            identifier: this.item!.identifier,
            containerName: this.item!.containerName,
            name: this.form.get('name')!.value,
            url: this.form.get('url')!.value,
            host: this.form.get('host')!.value,
            port: this.form.get('port')!.value,
            sortOrder: this.item!.sortOrder,
            iconUrl: this.form.get('iconUrl')!.value,
            columnId: this.column!.identifier!,
            folderId: this.item!.folderId
        })
            .pipe(takeUntil(this._destroy))
            .subscribe((link) => {
                this.isLoading.set(false);
                if (this.item) {
                    Object.assign(this.item, link);
                }
                this.successMessage.set('Successfully updated link.');
                this.updated.emit(link);
            });
    }

    public handleFileUpload(e: any): void {

        if (e.target.files.length === 0) {
            return;
        }

        this.isLoading.set(true);

        const file = e.target.files[0] as File;

        const fileReader = new FileReader();
        fileReader.readAsArrayBuffer(file);

        fileReader.onload = () => {
            const arrayBuffer = fileReader.result as ArrayBuffer;
            const blob = new Blob([arrayBuffer], { type: file.type });

            const formData = new FormData();
            formData.append('Logo', blob, file.name);

            this.showIcon.set(false);
            this._linkService.uploadLogo(this.item!.identifier!, formData)
                .pipe(takeUntil(this._destroy))
                .subscribe((logoUrl: string) => {
                    this.isLoading.set(false);
                    this.logoUpdated.set(true);
                    this.showIcon.set(true);
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

    private recordClickWithoutBlocking(identifier: string): void {
        const endpoint = `${environment.apiBaseUrl}/api/links/${identifier}/click`;

        if (navigator.sendBeacon?.(endpoint)) {
            return;
        }

        void fetch(endpoint, {
            method: 'POST',
            body: '',
            keepalive: true
        }).catch(() => undefined);
    }
}
