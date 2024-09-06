using Loudenvier.Utils;
using CommandLine;
using CommandLine.Text;

namespace Loudenvier.CommandLineFramework;

public class ConsolePrinter {
    static void Println(string? s = null, ConsoleColor? f = null, ConsoleColor? b = null, bool restoreColor = true) =>
        Print(s + Environment.NewLine, f, b, restoreColor);
    static void Print(string? s = null, ConsoleColor? f = null, ConsoleColor? b = null, bool restoreColor = true) {
        var (bck, fgr) = (Console.BackgroundColor, Console.ForegroundColor);
        if (f.HasValue)
            Console.ForegroundColor = f.Value;
        if (b.HasValue)
            Console.BackgroundColor = b.Value;
        Console.Write(s);
        if (restoreColor) 
            (Console.BackgroundColor, Console.ForegroundColor) = (bck, fgr);
    }

    public string Prompt { get; set; } = "cmd";
    public string GetPrompt() => $"({Prompt})> ";
    public virtual void DisplayPrompt() => Print($"({Prompt})> ", ConsoleColor.DarkGray, restoreColor: true);
    public virtual void Clear() => Console.Clear();
    public virtual void Error(string s) => Println(" ! " + s, ConsoleColor.Red);
    public virtual void Info(string s) => Println(" # " + s, ConsoleColor.DarkYellow);
    public virtual void Resp(string s) => Println(s, ConsoleColor.Yellow);
    public virtual void Label(string s) => Println(s, ConsoleColor.Green);
    public virtual void Ondata(string s) => Println(s, ConsoleColor.DarkGray);
    public virtual void Warn(string s) {
        Print(" ! WARNING: ", ConsoleColor.Cyan);
        Println(s, ConsoleColor.DarkCyan);
    }
    public virtual void Attention(string s) => Println(s, ConsoleColor.DarkCyan);
    public virtual void Problem(string s) => Println(s, ConsoleColor.Magenta);

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
