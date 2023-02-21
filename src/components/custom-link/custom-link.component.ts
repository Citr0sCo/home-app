import { Component, Input } from '@angular/core';
import { ILink } from '../../services/link-service/types/link.type';

@Component({
    selector: 'custom-link',
    templateUrl: './custom-link.component.html',
    styleUrls: ['./custom-link.component.scss']
})
export class CustomLinkComponent {

    @Input()
    public item: ILink | null = null;
}
