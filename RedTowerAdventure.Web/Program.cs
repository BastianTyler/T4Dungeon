using System.Net.WebSockets;
using System.Text;
using T4Dungeon.Game.Core;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseWebSockets();
app.UseStaticFiles();

app.MapGet("/", () => Results.File("wwwroot/index.html", "text/html"));

app.Map("/ws", async context =>
{
    if (!context.WebSockets.IsWebSocketRequest) return;

    var ws = await context.WebSockets.AcceptWebSocketAsync();

    var writer = new WebConsoleWriter(text =>
    {
        var bytes = Encoding.UTF8.GetBytes(text.Replace("\n", "\r\n"));
        ws.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None).Wait();
    });

    var reader = new WebConsoleReader(async () =>
    {
        var buf = new byte[256];
        var result = await ws.ReceiveAsync(buf, CancellationToken.None);
        return Encoding.UTF8.GetString(buf, 0, result.Count);
    });

    Console.SetOut(writer);
    Console.SetIn(reader);

    await Task.Run(() =>
    {
        var engine = new GameEngine();
        engine.Run();
    });
});

app.Run();