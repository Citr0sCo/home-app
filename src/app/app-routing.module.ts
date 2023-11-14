import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomePageComponent } from '../pages/home-page/home-page.component';
import { FuelPricesPageComponent } from '../pages/fuel-prices-page/fuel-prices-page.component';

const routes: Routes = [
    {
        path: 'home',
        component: HomePageComponent
    },
    {
        path: 'fuel-prices',
        component: FuelPricesPageComponent
    },
    { path: '**', pathMatch: 'full', redirectTo: 'home' }
];

@NgModule({
    imports: [
        RouterModule.forRoot(routes)
    ],
    exports: [
        RouterModule
    ]
})
export class AppRoutingModule {
}
