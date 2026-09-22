import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { PendingRequestCancellationService } from '../services/pending-request-cancellation.service';
import { AppComponent } from './app.component';

describe('AppComponent', () => {
    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [
                RouterTestingModule
            ],
            declarations: [AppComponent],
            schemas: [CUSTOM_ELEMENTS_SCHEMA]
        }).compileComponents();
    });

    it('should create the app', () => {
        const fixture = TestBed.createComponent(AppComponent);
        const app = fixture.componentInstance;
        expect(app).toBeTruthy();
    });

    it('should have as title \'home-box-landing\'', () => {
        const fixture = TestBed.createComponent(AppComponent);
        const app = fixture.componentInstance;
        expect(app.title).toEqual('home-box-landing');
    });

    it('cancels pending requests when a destination anchor is clicked', () => {
        const fixture = TestBed.createComponent(AppComponent);
        const app = fixture.componentInstance;
        const cancellationService = TestBed.inject(PendingRequestCancellationService);
        const cancelAll = vi.spyOn(cancellationService, 'cancelAll');
        const anchor = document.createElement('a');
        const child = document.createElement('span');
        anchor.href = '/home';
        anchor.appendChild(child);

        app.handleDocumentClick({ target: child } as unknown as MouseEvent);

        expect(cancelAll).toHaveBeenCalledOnce();
    });
});
