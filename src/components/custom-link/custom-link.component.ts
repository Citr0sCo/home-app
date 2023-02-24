import { Component, Input } from '@angular/core';
import { ILink } from '../../services/link-service/types/link.type';
import { LinkService } from '../../services/link-service/link.service';

@Component({
    selector: 'custom-link',
    templateUrl: './custom-link.component.html',
    styleUrls: ['./custom-link.component.scss']
})
export class CustomLinkComponent {

    @Input()
    public item: ILink | null = null;

    public isDeleting: boolean = false;

    public isDeleted: boolean = false;

    private _linkService: LinkService;

    constructor(linkService: LinkService) {
        this._linkService = linkService;
    }

    public deleteLink(): void {
        this._linkService.deleteLink(this.item?.identifier ?? '')
            .subscribe(() => {
                this.isDeleted = true;
            });
    }
}
