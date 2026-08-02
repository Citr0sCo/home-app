import { Stack } from './stack';

describe('Stack', () => {
    it('stores and removes items in last-in-first-out order', () => {
        const stack = new Stack<string>();

        stack.push('first');
        stack.push('second');

        expect(stack.peek()).toBe('second');
        expect(stack.size()).toBe(2);
        expect(stack.pop()).toBe('second');
        expect(stack.pop()).toBe('first');
        expect(stack.size()).toBe(0);
    });

    it('returns undefined when reading an empty stack', () => {
        const stack = new Stack<number>();

        expect(stack.peek()).toBeUndefined();
        expect(stack.pop()).toBeUndefined();
    });
});
