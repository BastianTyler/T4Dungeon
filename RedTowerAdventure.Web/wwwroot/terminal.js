window.terminalInterop = {
    term: null,
    fitAddon: null,

    init: function (dotnetRef) {
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

        this.fitAddon = new FitAddon.FitAddon();
        this.term.loadAddon(this.fitAddon);
        this.term.open(document.getElementById('terminal-container'));
        this.fitAddon.fit();

        this.term.onData(data => {
            dotnetRef.invokeMethodAsync('OnTerminalInput', data);
        });
    },

    write: function (text) {
        if (this.term) this.term.write(text);
    },

    writeLine: function (text) {
        if (this.term) this.term.write(text + '\r\n');
    }
};