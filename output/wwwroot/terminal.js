window.term = null;

window.setupTerminal = (elementId) => {
    const container = document.getElementById(elementId);

    if (!container) {
        console.error("Terminal container not found");
        return;
    }

    // Prevent double initialization
    if (window.term) {
        return;
    }

    window.term = new Terminal();
    window.term.open(container);
};

window.writeToTerminal = (text) => {
    if (window.term) {
        window.term.writeln(text);
    }
};