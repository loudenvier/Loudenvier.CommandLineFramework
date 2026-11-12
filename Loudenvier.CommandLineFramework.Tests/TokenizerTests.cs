using Loudenvier.CommandLineFramework.Parsing;

namespace Loudenvier.CommandLineFramework.Tests;

public class TokenizerTests {
    [Theory]
    [InlineData("program.exe \"\" next", "", "next")]
    [InlineData("program.exe \"  Ana  \" next", "  Ana  ", "next")]
    [InlineData("program.exe --name=\"Ana Maria\" next", "--name=Ana Maria", "next")]
    [InlineData("program.exe one\ttwo", "one", "two")]
    [InlineData("program.exe \"A \"\"B\"\" C\" next", "A \"B\" C", "next")]
    [InlineData("\"C:\\Program Files\\program.exe\" \"C:\\My Files\\\" next", "C:\\My Files\\", "next")]
    public void PreservesArgumentBoundaries(string input, string first, string second) {
        Assert.Equal(new[] { first, second }, Tokenizer.TokenizeCommandLineToStringArray(input));
    }

    [Theory]
    [InlineData("")]
    [InlineData("program.exe")]
    public void ReturnsNoArgumentsWithoutValues(string input) {
        Assert.Empty(Tokenizer.TokenizeCommandLineToStringArray(input));
    }

    [Fact]
    public void ReportsAnUnclosedQuote() {
        Assert.Throws<FormatException>(() => Tokenizer.TokenizeCommandLineToList("program.exe \"unfinished"));
    }
}
