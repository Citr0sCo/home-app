import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IFuelPrice } from '../../services/fuel-price-service/types/fuel-price.type';

@Component({
    selector: 'custom-map',
    templateUrl: './custom-map.component.html',
    styleUrls: ['./custom-map.component.scss']
})
export class CustomMapComponent implements OnInit {

    @Input()
    public latitude: number | null = null;

    @Input()
    public longitude: number | null = null;

    @Input()
    public fuelStations: Array<IFuelPrice> = [];

    @Output()
    public clicked: EventEmitter<IFuelPrice> = new EventEmitter<IFuelPrice>();

    public ngOnInit(): void {
        // @ts-ignore
        mapboxgl.accessToken = 'pk.eyJ1IjoiY2l0cjBzIiwiYSI6ImNsb3R5YXF1dDBlcmkybHMxbHJqbGJqYXQifQ.Hl5dzic3-jLQmOMeCZlQPw';

        // @ts-ignore
        const map = new mapboxgl.Map({
            container: 'map',
            style: 'mapbox://styles/mapbox/streets-v12',
            center: [this.longitude, this.latitude],
            zoom: 11
        });

        // @ts-ignore
        const userLocation = new mapboxgl.Marker({ color: 'red' })
            .setLngLat([this.longitude, this.latitude])
            .addTo(map);

        userLocation.getElement().addEventListener('click', () => {
            alert('Your location');
        });

        for (const fuelStation of this.fuelStations) {
            // @ts-ignore
            const fuelStationLocation = new mapboxgl.Marker({ color: fuelStation.colour })
                .setLngLat([fuelStation.longitude, fuelStation.latitude])
                .addTo(map);

            fuelStationLocation.getElement().addEventListener('click', () => {
                this.handlePinClick(fuelStation);
            });
        }
    }

    public handlePinClick(fuelPrice: IFuelPrice): void {
        this.clicked.emit(fuelPrice);
    }
}
