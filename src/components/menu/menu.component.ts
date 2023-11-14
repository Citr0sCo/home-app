import { Component } from '@angular/core';

@Component({
    selector: 'menu-component',
    templateUrl: './menu.component.html',
    styleUrls: ['./menu.component.scss']
})
export class MenuComponent {

    public showMenu: boolean = false;
}
