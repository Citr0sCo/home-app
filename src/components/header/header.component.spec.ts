import { CommonModule } from '@angular/common';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { LinkRepository } from '../../services/link-service/link.repository';
import { LinkService } from '../../services/link-service/link.service';
import { PendingRequestCancellationService } from '../../services/pending-request-cancellation.service';
import { HeaderComponent } from './header.component';

describe('HeaderComponent', () => {
    let fixture: ComponentFixture<HeaderComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [HeaderComponent],
            imports: [CommonModule, FormsModule],
            providers: [
                LinkService,
                LinkRepository,
                PendingRequestCancellationService,
                provideHttpClient(),
                provideHttpClientTesting()
            ]
        }).compileComponents();

        fixture = TestBed.createComponent(HeaderComponent);
        fixture.detectChanges();
    });

    afterEach(() => {
        fixture.destroy();
        TestBed.resetTestingModule();
    });

    function searchBar(): HTMLElement {
        return fixture.nativeElement.querySelector('.header-component__search');
    }

    function dialogInput(): HTMLInputElement | null {
        return fixture.nativeElement.querySelector('.search-dialog input');
    }

    it('opens the search and focuses its input when the floating bar is tapped', () => {
        expect(dialogInput()).toBeNull();

        searchBar().click();
        fixture.detectChanges();

        expect(dialogInput()).not.toBeNull();
        expect(document.activeElement).toBe(dialogInput());
    });

    it('opens the search and focuses its input when the search icon is tapped', () => {
        const icon: HTMLElement = fixture.nativeElement.querySelector('.header-component__search .c-icon');

        icon.click();
        fixture.detectChanges();

        expect(dialogInput()).not.toBeNull();
        expect(document.activeElement).toBe(dialogInput());
    });

    it('focuses the input synchronously while the tap is handled', () => {
        searchBar().click();

        expect(document.activeElement).toBe(dialogInput());
    });
});
