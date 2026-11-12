using System.Text;

namespace Loudenvier.CommandLineFramework.Parsing;

public static class Tokenizer
{
    /// <summary>
    /// Splits a command line into arguments and skips the first token, the executable name.
    /// Whitespace separates arguments outside double quotes. Inside a quoted argument,
    /// doubled double quotes represent one literal quote. Backslashes are literal.
    /// </summary>
    /// <exception cref="FormatException">The input contains an unclosed double quote.</exception>
    public static List<string> TokenizeCommandLineToList(string commandLine)
    {
        var tokens = new List<string>();
        var token = new StringBuilder();
        bool tokenStarted = false;
        bool inQuotes = false;

        for (int index = 0; index < commandLine.Length; index++)
        {
            char current = commandLine[index];
            if (current == '"')
            {
                tokenStarted = true;
                if (inQuotes && index + 1 < commandLine.Length && commandLine[index + 1] == '"')
                {
                    token.Append('"');
                    index++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (!inQuotes && char.IsWhiteSpace(current))
            {
                if (!tokenStarted)
                    continue;

                tokens.Add(token.ToString());
                token.Clear();
                tokenStarted = false;
            }
            else
            {
                token.Append(current);
                tokenStarted = true;
            }
        }

        if (inQuotes)
            throw new FormatException("Unclosed double quote in command line.");

        if (tokenStarted)
            tokens.Add(token.ToString());
        if (tokens.Count > 0)
            tokens.RemoveAt(0);

        return tokens;
    }

    /// <inheritdoc cref="TokenizeCommandLineToList"/>
    public static string[] TokenizeCommandLineToStringArray(string commandLine)
        => TokenizeCommandLineToList(commandLine).ToArray();
}
