import { Component, Input } from '@angular/core';

type WidgetSkeletonVariant = 'stats' | 'progress';

@Component({
    selector: 'widget-skeleton',
    templateUrl: './widget-skeleton.component.html',
    styleUrls: ['./widget-skeleton.component.scss'],
    standalone: false
})
export class WidgetSkeletonComponent {

    @Input()
    public variant: WidgetSkeletonVariant = 'stats';
}
