import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';
import { ErrorCodes, mapNetworkError, UserError } from './map-network-error';

describe('mapNetworkError', () => {
    it('maps common HTTP statuses to user-facing errors', () => {
        const cases = [
            {
                status: 0,
                code: ErrorCodes.OFFLINE,
                message: 'You\'re offline, unable to communicate with servers. Try checking your internet connection.'
            },
            {
                status: 503,
                code: ErrorCodes.OFFLINE,
                message: 'Apologies, servers can\'t be reached at this time. Try again in a few minutes.'
            },
            {
                status: 500,
                code: ErrorCodes.NETWORK_EXCEPTION,
                message: 'Apologies, servers were unable to complete that request due to an internal error. Please contact support if this error still persists.'
            },
            {
                status: 401,
                code: ErrorCodes.MISSING_PERMISSIONS,
                message: 'You do not have the correct permissions to complete this request. Please contact your local administrator for more information.'
            }
        ];

        for (const testCase of cases) {
            let receivedError: UserError | undefined;
            throwError(() => new HttpErrorResponse({ status: testCase.status }))
                .pipe(mapNetworkError())
                .subscribe({
                    error: (error: UserError) => {
                        receivedError = error;
                    }
                });

            expect(receivedError).toBeInstanceOf(UserError);
            expect(receivedError?.code).toBe(testCase.code);
            expect(receivedError?.message).toBe(testCase.message);
        }
    });

    it('uses the API message for invalid requests', () => {
        let receivedError: UserError | undefined;

        throwError(() => new HttpErrorResponse({
            status: 400,
            error: { UserMessage: 'The link could not be saved.' }
        }))
            .pipe(mapNetworkError())
            .subscribe({
                error: (error: UserError) => {
                    receivedError = error;
                }
            });

        expect(receivedError?.code).toBe(ErrorCodes.INVALID_ACTION);
        expect(receivedError?.message).toBe('The link could not be saved.');
    });

    it('uses a default message for invalid requests without an API message', () => {
        let receivedError: UserError | undefined;

        throwError(() => new HttpErrorResponse({ status: 406, error: {} }))
            .pipe(mapNetworkError())
            .subscribe({
                error: (error: UserError) => {
                    receivedError = error;
                }
            });

        expect(receivedError?.code).toBe(ErrorCodes.INVALID_ACTION);
        expect(receivedError?.message).toBe('Failed to perform action, there seems to be an error in your request. Please check your request is valid.');
    });

    it('maps unknown HTTP statuses to a network error', () => {
        const response = new HttpErrorResponse({ status: 418, statusText: 'I am a teapot' });
        let receivedError: UserError | undefined;

        throwError(() => response)
            .pipe(mapNetworkError())
            .subscribe({
                error: (error: UserError) => {
                    receivedError = error;
                }
            });

        expect(receivedError?.code).toBe(ErrorCodes.NETWORK_EXCEPTION);
        expect(receivedError?.message).toBe(`Network error [418] - ${response.message}`);
        expect(receivedError?.exception).toBe(response);
    });

    it('retains the original exception and exposes its message', () => {
        const cause = new Error('root cause');
        const userError = new UserError('Friendly message', ErrorCodes.APP_EXCEPTION, cause);

        expect(userError.toString()).toBe('Friendly message');
        expect(userError.name).toBe('UserError');
        expect(userError.code).toBe(ErrorCodes.APP_EXCEPTION);
        expect(userError.exception).toBe(cause);
        expect(userError.stack).toBe(cause.stack);
    });

    it('passes through non-HTTP errors unchanged', () => {
        const originalError = new Error('unexpected failure');
        let receivedError: unknown;

        throwError(() => originalError)
            .pipe(mapNetworkError())
            .subscribe({
                error: (error: unknown) => {
                    receivedError = error;
                }
            });

        expect(receivedError).toBe(originalError);
    });
});
