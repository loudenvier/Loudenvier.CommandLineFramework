using CommandLine;

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

    class TestCommandsWithOptions
    {
        class TestOptions
        {
            [Value(0, MetaName = "Value 1")]
            public int Value { get; set; }
            [Value(1, MetaName = "Value 2")]
            public string? Name { get; set; }
        }
        public static bool didItRun = false;
        static void TestCommand(TestOptions options, CommandLoop loop) {
            didItRun = options.Value != 0;
        }
    }

    [Fact]
    public void CanRegisterImplicitCommandOptions() {
        var set = new CommandSet(autoRegisterCommands: false);
        set.RegisterCommandSet<TestCommandsWithOptions>();
        var commands = set.AvailableCommands;
        var cmdName = commands.Single(c => c == "Test");
        Assert.True(cmdName == "Test");
        var runner = set.GetCommandRunner("Test")!;
        Assert.True(runner.Options?.Name == "TestOptions");
    }

    class TestCommandsWithVerbs
    {
        class TestVerbs
        {
            [Verb("Verb1")]
            public class Verb1;
            [Verb("Verb2")]
            public class Verb2;
        }
        public static bool didItRunVerb1 = false;
        public static bool ditItRunVerb2 = false;
        static void TestCommand(object options, CommandLoop loop) {
            didItRunVerb1 = options is TestVerbs.Verb1;
            ditItRunVerb2 = options is TestVerbs.Verb2;
        }
    }

    [Fact]
    public void CanRegisterImplicitCommandVerbs() {
        var set = new CommandSet(autoRegisterCommands: false);
        set.RegisterCommandSet<TestCommandsWithVerbs>();
        var commands = set.AvailableCommands;
        var cmdName = commands.Single(c => c == "Test");
        Assert.True(cmdName == "Test");
        var runner = set.GetCommandRunner("Test")!;
        Assert.Null(runner.Options);
        Assert.Equal(2, runner.Verbs.Length);
    }

    [Fact]
    public void CanAutoRegisterImplicitCommandVerbs() {
        var set = new CommandSet(autoRegisterCommands: false);
        set.AutoRegisterCommands();
        var commands = set.AvailableCommands;
        var cmdName = commands.Single(c => c == "Get");
        Assert.True(cmdName == "Get");
        var runner = set.GetCommandRunner("Get")!;
        Assert.Null(runner.Options);
        Assert.Equal(8, runner.Verbs.Length);
    }

}

static class IntelbrasCommandSet
{
    internal class GetVerbs
    {
        [Verb("general-config", HelpText = "Retrieves the current general configuration")]
        internal class GeneralConfig;
        [Verb("network-config", HelpText = "Retrieves the current network configuration")]
        internal class NetworkConfig;
        [Verb("wlan-config", HelpText = "Retrieves the current WLAN configuration")]
        internal class WLanConfig;

        [Verb("upload-config", HelpText = "Gets the HTTP UPLOAD (online mode) configuration")]
        internal class GetHttpUploadConfig;
        [Verb("intelbras-config", HelpText = "Gets the Intelbras mode configuration")]
        internal class GetIntelbrasConfig;
        [Verb("time", HelpText = "Retrieves the device's current time")]
        internal class GetCurrentTime;
        [Verb("users", HelpText = "List users stored on the device")]
        internal class GetUsers
        {
            [Value(0, MetaName = "count", HelpText = "The ammount of users to retrieve")]
            public int Count { get; set; }
            [Value(1, MetaName = "offset", HelpText = "An offset to start retrieving users")]
            public int? Offset { get; set; }
        }
        [Verb("face", HelpText = "Retrieves the facial record for a user")]
        internal class GetFace
        {
            [Value(0, MetaName = "user id", HelpText = "The id of the user")]
            public string UserId { get; set; }
        }
    }
    internal static async Task GetCommand(object value, CommandLoop loop) {
        await Task.CompletedTask;
    }
}
