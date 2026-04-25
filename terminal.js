window.terminalInterop = {
    term: null,
    dotnetRef: null,
    inputBuffer: '',

    init: function (dotnetRef) {
        this.dotnetRef = dotnetRef;
        this.term = new Terminal({
            cursorBlink: true,
            fontSize: 16,
            fontFamily: 'Cascadia Code, Consolas, monospace',
            theme: {
                background: '#0a1628',
                foreground: '#00ff41',
                cursor: '#00ff41',
            },
            cols: 80,
            rows: 24,
        });

        const fitAddon = new FitAddon.FitAddon();
        this.term.loadAddon(fitAddon);
        this.term.open(document.getElementById('terminal-container'));
        fitAddon.fit();

        this.term.onKey(({ key, domEvent }) => {
            const code = domEvent.keyCode;

            if (code === 13) { // Enter
                this.term.write('\r\n');
                this.dotnetRef.invokeMethodAsync('OnTerminalInput', this.inputBuffer);
                this.inputBuffer = '';
            } else if (code === 8) { // Backspace
                if (this.inputBuffer.length > 0) {
                    this.inputBuffer = this.inputBuffer.slice(0, -1);
                    this.term.write('\b \b');
                }
            } else {
                this.inputBuffer += key;
                this.term.write(key);
            }
        });
    },

    write: function (text) {
        if (this.term) this.term.write(text);
    }
};