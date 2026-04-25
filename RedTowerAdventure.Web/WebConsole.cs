using System.Text;

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
    private readonly Func<Task<string>> _read;
    public WebConsoleReader(Func<Task<string>> read) => _read = read;
    public override int Read()
    {
        var result = _read().GetAwaiter().GetResult();
        return result.Length > 0 ? result[0] : -1;
    }
}