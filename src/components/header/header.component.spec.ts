import { CommonModule } from '@angular/common';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { LinkRepository } from '../../services/link-service/link.repository';
import { LinkService } from '../../services/link-service/link.service';
import { PendingRequestCancellationService } from '../../services/pending-request-cancellation.service';
import { HeaderComponent } from './header.component';

interface IVisualViewportStub {
    height: number;
    offsetTop: number;
    listenerCount(): number;
    dispatch(eventType: string): void;
}

// jsdom does not implement window.visualViewport, which the component uses to
// stay inside the area left visible by the on-screen keyboard.
function stubVisualViewport(height: number, offsetTop: number): IVisualViewportStub {
    const listeners: Map<string, Set<EventListener>> = new Map();

    const viewport = {
        height,
        offsetTop,
        addEventListener(type: string, listener: EventListener): void {
            const typeListeners: Set<EventListener> = listeners.get(type) ?? new Set();
            typeListeners.add(listener);
            listeners.set(type, typeListeners);
        },
        removeEventListener(type: string, listener: EventListener): void {
            listeners.get(type)?.delete(listener);
        },
        listenerCount(): number {
            return Array.from(listeners.values()).reduce((count, typeListeners) => count + typeListeners.size, 0);
        },
        dispatch(eventType: string): void {
            listeners.get(eventType)?.forEach((listener) => listener(new Event(eventType)));
        }
    };

    Object.defineProperty(window, 'visualViewport', { configurable: true, value: viewport });

    return viewport;
}

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
        Reflect.deleteProperty(window, 'visualViewport');
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

    it('keeps the search overlay inside the visual viewport left by the keyboard', () => {
        const viewport: IVisualViewportStub = stubVisualViewport(800, 0);

        searchBar().click();
        fixture.detectChanges();

        const overlay: HTMLElement = fixture.nativeElement.querySelector('.search-overlay');
        expect(overlay.style.getPropertyValue('--search-overlay-height')).toBe('800px');
        expect(overlay.style.getPropertyValue('--search-overlay-top')).toBe('0px');

        viewport.height = 420;
        viewport.offsetTop = 96;
        viewport.dispatch('resize');

        expect(overlay.style.getPropertyValue('--search-overlay-height')).toBe('420px');
        expect(overlay.style.getPropertyValue('--search-overlay-top')).toBe('96px');
    });

    it('stops tracking the visual viewport once the search is closed', () => {
        const viewport: IVisualViewportStub = stubVisualViewport(800, 0);

        searchBar().click();
        fixture.detectChanges();
        expect(viewport.listenerCount()).toBe(2);

        fixture.nativeElement.querySelector('.search-overlay').click();
        fixture.detectChanges();

        expect(dialogInput()).toBeNull();
        expect(viewport.listenerCount()).toBe(0);
    });
});
