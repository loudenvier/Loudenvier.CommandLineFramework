namespace Loudenvier.CommandLineFramework.Tests;

public class CommandConventionsTests {
    [Fact]
    public void DefaultConventionHasExpectedValues() {
        var def = CommandConventions.Default;
        Assert.Equal("CommandSet", def.CommandSetSuffix);
        Assert.Equal("Command", def.CommandSuffix);
        Assert.Equal("Options", def.OptionsSuffix);
        Assert.Equal("Verbs", def.VerbSetSuffix);
    }

    [Fact]
    public void GetCommandNameWorks() {
        var commandName = CommandConventions.Default.GetCommandName("MyCommandCommand");
        Assert.Equal("MyCommand", commandName);
    }
}