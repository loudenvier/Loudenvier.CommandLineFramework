using Loudenvier.Utils;

namespace Loudenvier.CommandLineFramework.Tests;

public class CommandConventionsTest {
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
    [Fact]
    public void GetCommandNameWorksWithKebabCase() =>
        Assert.Equal("my-command", new CommandConventions {
            Casing = CaseConvention.Kebab
        }.GetCommandName("MyCommandCommand"));

    [Fact]
    public void GetCommandNameWorksWithSnakeCase() =>
        Assert.Equal("my_command", new CommandConventions {
            Casing = CaseConvention.Snake
        }.GetCommandName("MyCommandCommand"));

    [Fact]
    public void GetCommandNameWorksWithCamelCase() =>
        Assert.Equal("myCommand", new CommandConventions { 
            Casing = CaseConvention.Camel 
        }.GetCommandName("MyCommandCommand"));
    [Fact]
    public void GetCommandNameWorksWithPascalCase() =>
        Assert.Equal("MyCommand", new CommandConventions {
            Casing = CaseConvention.Pascal
        }.GetCommandName("my_commandCommand"));
    [Fact]
    public void GetCommandNameWorksWithFlatCase() =>
        Assert.Equal("mycommand", new CommandConventions {
            Casing = CaseConvention.Flat
        }.GetCommandName("my_commandCommand"));
    [Fact]
    public void GetCommandNameWorksWithCOBOLCase() =>
        Assert.Equal("MY-COMMAND", new CommandConventions {
            Casing = CaseConvention.Cobol
        }.GetCommandName("myCommandCommand"));
    [Fact]
    public void GetCommandNameWorksWithCONSTANTCase() =>
        Assert.Equal("MY_COMMAND", new CommandConventions {
            Casing = CaseConvention.Constant
        }.GetCommandName("myCommandCommand"));

}