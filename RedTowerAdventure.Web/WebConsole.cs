using System.Text;

namespace RedTowerAdventure.Web
{
    public class WebConsoleWriter : TextWriter
    {
        private readonly Action<string> _write;
        public override Encoding Encoding => Encoding.UTF8;

        public WebConsoleWriter(Action<string> write) => _write = write;

        public override void WriteLine(string? value) => _write((value ?? "") + "\n");
        public override void Write(string? value) => _write(value ?? "");
        public override void Write(char value) => _write(value.ToString());
    }

    public class WebConsoleReader : TextReader
    {
        private readonly Func<Task<ConsoleKeyInfo>> _readKey;

        public WebConsoleReader(Func<Task<ConsoleKeyInfo>> readKey) => _readKey = readKey;

        public override int Read()
        {
            var key = _readKey().GetAwaiter().GetResult();
            return key.KeyChar;
        }
    }
}