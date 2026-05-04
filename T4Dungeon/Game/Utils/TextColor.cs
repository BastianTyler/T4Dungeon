namespace T4Dungeon.Game.Utils;

public static class TextColor
{
    // =========================
    // BASIC FORMATTING
    // =========================
    public const string Reset = "\x1b[0m";
    public const string Bold = "\x1b[1m";
    public const string Dim = "\x1b[2m";
    public const string Underline = "\x1b[4m";
    public const string Reversed = "\x1b[7m";

    // =========================
    // STANDARD COLORS (BRIGHT)
    // =========================
    public const string Green = "\x1b[92m";
    public const string Cyan = "\x1b[96m";
    public const string Yellow = "\x1b[93m";
    public const string Red = "\x1b[91m";
    public const string Magenta = "\x1b[95m";
    public const string Blue = "\x1b[94m";
    public const string Gray = "\x1b[90m";
    public const string White = "\x1b[97m";

    // =========================
    // DARK / DIM COLORS
    // =========================
    public const string DimGreen = "\x1b[32m";
    public const string DimCyan = "\x1b[36m";
    public const string DimRed = "\x1b[31m";
    public const string DimYellow = "\x1b[33m";
    public const string DimBlue = "\x1b[34m";
    public const string DimMagenta = "\x1b[35m";

    // =========================
    // GAME THEMED COLORS
    // =========================
    public const string Gold = "\x1b[38;5;220m";    // Deep Gold
    public const string Orange = "\x1b[38;5;208m";  // Legendaries/Fire
    public const string Purple = "\x1b[38;5;135m";  // Epics/Magic
    public const string Brown = "\x1b[38;5;94m";   // Earth/Dirt
    public const string Pink = "\x1b[38;5;213m";    // Love/Hearts
    public const string Forest = "\x1b[38;5;34m";   // Nature/Poison
    public const string Sky = "\x1b[38;5;117m";    // Frost/Cold

    // =========================
    // BACKGROUND COLORS
    // =========================
    public const string BgRed = "\x1b[41m";
    public const string BgGreen = "\x1b[42m";
    public const string BgYellow = "\x1b[43m";
    public const string BgBlue = "\x1b[44m";
    public const string BgMagenta = "\x1b[45m";
    public const string BgCyan = "\x1b[46m";

    // =========================
    // UX ALERTS (SQUISHED)
    // =========================
    public const string Success = Green + Bold;
    public const string Info = Cyan + Bold;
    public const string Warning = Yellow + Bold;
    public const string Critical = Red + Reversed + Bold;
    public const string Tutorial = Magenta + Bold; // Perfect for your tutorial tag
}