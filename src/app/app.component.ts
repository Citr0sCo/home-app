import { Component, HostListener } from '@angular/core';
import { PendingRequestCancellationService } from '../services/pending-request-cancellation.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    standalone: false
})
export class AppComponent {
    public title = 'home-box-landing';

    private readonly _pendingRequestCancellationService: PendingRequestCancellationService;

    constructor(pendingRequestCancellationService: PendingRequestCancellationService) {
        this._pendingRequestCancellationService = pendingRequestCancellationService;
    }

    @HostListener('document:click', ['$event'])
    public handleDocumentClick(event: MouseEvent): void {
        const target = event.target;

        if (!(target instanceof Element)) {
            return;
        }

        const anchor = target.closest('a');

        if (anchor !== null && anchor.getAttribute('href') !== null) {
            this._pendingRequestCancellationService.cancelAll();
        }
    }
}
