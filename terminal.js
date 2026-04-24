window.setupTerminal = (containerId) => {
    const term = new Terminal({
        cursorBlink: true,
        theme: {
            background: '#000000',
            foreground: '#00FF00' // Matrix Green
        },
        fontFamily: 'Courier New'
    });
    const container = document.getElementById(containerId);
    term.open(container);
    term.writeln('Welcome to Red Tower OS...');
    term.writeln('Connecting to T4Dungeon engine...');

    // Store the terminal instance globally so Blazor can call it
    window.terminalInstance = term;
};

window.writeToTerminal = (text) => {
    if (window.terminalInstance) {
        window.terminalInstance.writeln(text);
    }
};