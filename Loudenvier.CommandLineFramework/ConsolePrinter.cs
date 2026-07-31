using Loudenvier.Utils;
using CommandLine;
using CommandLine.Text;

namespace Loudenvier.CommandLineFramework;

public class ConsolePrinter {
    static void Println(string? s = null, ConsoleColor? f = null, ConsoleColor? b = null, bool restoreColor = true, bool showTimeStamp = false) 
        => Print(s + Environment.NewLine, f, b, restoreColor, showTimeStamp);
    static void Print(string? s = null, ConsoleColor? f = null, ConsoleColor? b = null, bool restoreColor = true, bool showTimeStamp = false) {
        var (bck, fgr) = (Console.BackgroundColor, Console.ForegroundColor);
        if (f.HasValue)
            Console.ForegroundColor = f.Value;
        if (b.HasValue)
            Console.BackgroundColor = b.Value;
        if (s is not null && showTimeStamp)
            s = $"[{DateTime.Now:HH:mm:ss.fff}] {s}";
        Console.Write(s);
        if (restoreColor) 
            (Console.BackgroundColor, Console.ForegroundColor) = (bck, fgr);
    }

    public bool ShowTimestamp { 
        get; 
        set {
            field = value;
            Info($"Timestamp display is now turned {field.ToString("on", "off")}.");
        }
    } = false;
    public string Prompt { get; set; } = "cmd";
    public string GetPrompt() => $"({Prompt})> ";
    public virtual void DisplayPrompt() => Print($"({Prompt})> ", ConsoleColor.DarkGray, restoreColor: true);
    public virtual void Clear() => Console.Clear();
    public virtual void Error(string s) => Println(" ! " + s, ConsoleColor.Red, showTimeStamp: ShowTimestamp);
    public virtual void Error(string prefix, string msg, 
        ConsoleColor prefixColor = ConsoleColor.DarkRed, 
        ConsoleColor msgColor = ConsoleColor.Red) {
        Print(prefix, prefixColor);
        Println($" {msg}", msgColor);
    }
    public virtual void Info(string s) => Println(" # " + s, ConsoleColor.DarkYellow, showTimeStamp: ShowTimestamp);
    public virtual void Resp(string s) => Println(s, ConsoleColor.Yellow, showTimeStamp: ShowTimestamp);
    public virtual void Label(string s) => Println(s, ConsoleColor.Green, showTimeStamp: ShowTimestamp);
    public virtual void Ondata(string s) => Println(s, ConsoleColor.DarkGray, showTimeStamp: ShowTimestamp);
    public virtual void Warn(string s) {
        Print(" ! WARNING: ", ConsoleColor.Cyan, showTimeStamp: ShowTimestamp);
        Println(s, ConsoleColor.DarkCyan);
    }
    public virtual void Attention(string s) => Println(s, ConsoleColor.DarkCyan, showTimeStamp: ShowTimestamp);
    public virtual void Problem(string s) => Println(s, ConsoleColor.Magenta, showTimeStamp: ShowTimestamp);

    public void DisplayHelp<T>(ParserResult<T> result) {
        var ht = HelpText.AutoBuild(result, h => {
            h.AutoVersion = false;
            h.AutoHelp = false;
            h.AddNewLineBetweenHelpSections = false;
            h.AdditionalNewLineAfterOption = false;
            return h;
        });
        string help = ht.ToString();
        if (result is NotParsed<object> notParsed && notParsed.Errors.Any(e => e is HelpVerbRequestedError or HelpRequestedError)) {
            Info($"HELP{Environment.NewLine}{help.RemoveLines(2)}");
        } else {
            Error($"ERROR(S){Environment.NewLine}{Environment.NewLine}{help.RemoveLines(4)}");
        }
    }

}
