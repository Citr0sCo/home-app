export class TerminalParser {

    private readonly _terminalOutput: string;
    private _html = '';

    constructor(terminalOutput: string) {
        this._terminalOutput = terminalOutput;
    }

    public toHtml(): string {

        this._html = this._terminalOutput;

        this._html = this._terminalOutput
            .replaceAll('\u001b[0;36m', '<span class="text-cyan">')
            .replaceAll('\u001b[0;32m', '<span class="text-green">')
            .replaceAll('\u001b[0m', '</span>')
            .replaceAll('\\n', '<br />')
            .replaceAll('\\"', '"')
            .replaceAll('\\\\"', '"');

        return this._html;
    }
}