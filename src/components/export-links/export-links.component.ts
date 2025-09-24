import { Component } from '@angular/core';
import { LinkService } from '../../services/link-service/link.service';

@Component({
    selector: 'export-links',
    templateUrl: './export-links.component.html',
    styleUrls: ['./export-links.component.scss'],
    standalone: false
})
export class ExportLinksComponent {

    private _linkService: LinkService;

    constructor(linkService: LinkService) {
        this._linkService = linkService;
    }

    public handleClick(): void {
        this._linkService.getAllColumns()
            .subscribe((columns) => {
                const element = document.createElement('a');
                element.setAttribute('href', `data:text/plain;charset=utf-8,${encodeURIComponent(JSON.stringify(columns))}`);
                element.setAttribute('download', 'columns.json');

                element.style.display = 'none';
                document.body.appendChild(element);

                element.click();

                document.body.removeChild(element);
            });
    }
}
