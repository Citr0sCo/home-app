import { TerminalParser } from './terminal-parser';

describe('TerminalParser', () => {
    it('returns an empty string when no terminal output is provided', () => {
        expect(new TerminalParser('').toHtml()).toBe('');
    });

    it('converts terminal colors, escaped newlines, and escaped quotes to HTML', () => {
        const output = '\u001b[0;36mcyan\u001b[0m\\n\u001b[0;32mgreen\u001b[0m\\n\\"quoted\\"';

        expect(new TerminalParser(output).toHtml()).toBe(
            '<span class="text-cyan">cyan</span><br /><span class="text-green">green</span><br />"quoted"'
        );
    });
});
