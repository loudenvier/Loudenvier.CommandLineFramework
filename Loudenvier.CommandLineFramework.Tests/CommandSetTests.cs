namespace Loudenvier.CommandLineFramework.Tests;

public class CommandSetTests {

    class TestCommands
    {
        internal static bool voidCommandExecuted;
        internal static bool commanExplicitExecuted;
        public static void VoidCommand() => voidCommandExecuted = true;
        [Command("Explicit")]
        public static void CommandExplicit() => commanExplicitExecuted = true;  
    }

    [Fact]
    public void CanRegisterImplicitCommand() {
        var set = new CommandSet(autoRegisterCommands: false);
        set.RegisterCommandSet<TestCommands>();
        var commands = set.AvailableCommands;
        var cmdName = commands.Single(c => c == "Void");
        Assert.True(cmdName == "Void");
    }
    [Fact]
    public void CanRegisterExplicitCommand() {
        var set = new CommandSet(autoRegisterCommands: false);
        set.RegisterCommandSet<TestCommands>();
        var commands = set.AvailableCommands;
        var cmdName = commands.Single(c => c == "Explicit");
        Assert.True(cmdName == "Explicit");
    }
    [Fact]
    public async Task CanRunImplicitlyRegisteredCommand() {
        var set = new CommandSet(autoRegisterCommands: false);
        set.RegisterCommandSet<TestCommands>();
        var runner = set.GetCommandRunner("Void")!;
        await runner.RunAsync("", new CommandLoop());
        Assert.True(TestCommands.voidCommandExecuted);
    }
    [Fact]
    public async Task CanRunExplictlyRegisteredCommand() {
        var set = new CommandSet(autoRegisterCommands: false);
        set.RegisterCommandSet<TestCommands>();
        var runner = set.GetCommandRunner("Explicit")!;
        await runner.RunAsync("", new CommandLoop());
        Assert.True(TestCommands.commanExplicitExecuted);
    }

}
