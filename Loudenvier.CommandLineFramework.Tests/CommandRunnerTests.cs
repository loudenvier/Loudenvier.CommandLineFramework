using System.Reflection;

namespace Loudenvier.CommandLineFramework.Tests
{
    public class CommandRunnerTests
    {
        static bool didItRun;
        static async Task RunItWithoutParamsAsync() {
            didItRun = true;
            await Task.CompletedTask;
        }
        static async Task RunItWithGenericOptionsOnly(GenericOptions? genericOptions) {
            ArgumentNullException.ThrowIfNull(genericOptions);
            didItRun = true;
            await Task.CompletedTask;
        }
        static async Task RunItWithAllParasmAsync(GenericOptions genericOptions, CommandLoop loop) {
            ArgumentNullException.ThrowIfNull(genericOptions);
            ArgumentNullException.ThrowIfNull(loop);
            didItRun = true;
            await Task.CompletedTask;
        }
        static void RunItWithoutParamsVoid() => didItRun = true;
        static void RunItWithGenericOptionsOnlyVoid(GenericOptions genericOptions) {
            ArgumentNullException.ThrowIfNull(genericOptions);
            didItRun = true;
        }
        static void RunItWithAllParamsVoid(GenericOptions genericOptions, CommandLoop loop) {
            ArgumentNullException.ThrowIfNull(genericOptions);
            ArgumentNullException.ThrowIfNull(loop);
            didItRun = true;
        }
        static Task RunItWithoutParamsTask() {
            didItRun = true;
            return Task.CompletedTask;
        }
        static Task RunItWithGenericOptionsOnlyTask(GenericOptions genericOptions) {
            ArgumentNullException.ThrowIfNull(genericOptions);
            didItRun = true;
            return Task.CompletedTask;
        }
        static Task RunItWithAllParamsTask(GenericOptions genericOptions, CommandLoop loop) {
            ArgumentNullException.ThrowIfNull(genericOptions);
            ArgumentNullException.ThrowIfNull(loop);
            didItRun = true;
            return Task.CompletedTask;
        }

        static readonly BindingFlags bindings = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        [Fact]
        public async Task CanRunAsyncMethodWithoutParams() {
            var loop = new CommandLoop();
            var method = typeof(CommandRunnerTests).GetMethod(nameof(RunItWithoutParamsAsync), bindings)!;
            var runner = new CommandRunner(method, ["runit"], null, []);
            didItRun = false;
            await runner.RunAsync("teste.exe runit felipe machado", loop);
            Assert.True(didItRun);
        }
        [Fact]
        public async Task CanRunAsyncMethodWithOnlyGenericOptionsParam() {
            var loop = new CommandLoop();
            var method = typeof(CommandRunnerTests).GetMethod(nameof(RunItWithGenericOptionsOnly), bindings)!;
            var runner = new CommandRunner(method, ["runit"], null, []);
            didItRun = false;
            await runner.RunAsync("teste.exe runit felipe machado", loop);
            Assert.True(didItRun);
        }
        [Fact]
        public async Task CanRunAsyncMethodWithAllParams() {
            var loop = new CommandLoop();
            var method = typeof(CommandRunnerTests).GetMethod(nameof(RunItWithAllParasmAsync), bindings)!;
            var runner = new CommandRunner(method, ["runit"], null, []);
            didItRun = false;
            await runner.RunAsync("teste.exe runit felipe machado", loop);
            Assert.True(didItRun);
        }
        [Fact]
        public async Task CanRunMethodWithoutParamsVoid() {
            var loop = new CommandLoop();
            var method = typeof(CommandRunnerTests).GetMethod(nameof(RunItWithoutParamsVoid), bindings)!;
            var runner = new CommandRunner(method, ["runit"], null, []);
            didItRun = false;
            await runner.RunAsync("teste.exe runit felipe machado", loop);
            Assert.True(didItRun);
        }
        [Fact]
        public async Task CanRunMethodWithGenericOptionsOnlyVoid() {
            var loop = new CommandLoop();
            var method = typeof(CommandRunnerTests).GetMethod(nameof(RunItWithGenericOptionsOnlyVoid), bindings)!;
            var runner = new CommandRunner(method, ["runit"], null, []);
            didItRun = false;
            await runner.RunAsync("teste.exe runit felipe machado", loop);
            Assert.True(didItRun);
        }
        [Fact]
        public async Task CanRunMethodWithAllParamsVoid() {
            var loop = new CommandLoop();
            var method = typeof(CommandRunnerTests).GetMethod(nameof(RunItWithAllParamsVoid), bindings)!;
            var runner = new CommandRunner(method, ["runit"], null, []);
            didItRun = false;
            await runner.RunAsync("teste.exe runit felipe machado", loop);
            Assert.True(didItRun);
        }
        [Fact]
        public async Task CanRunMethodWithoutParamsTask() {
            var loop = new CommandLoop();
            var method = typeof(CommandRunnerTests).GetMethod(nameof(RunItWithoutParamsTask), bindings)!;
            var runner = new CommandRunner(method, ["runit"], null, []);
            didItRun = false;
            await runner.RunAsync("teste.exe runit felipe machado", loop);
            Assert.True(didItRun);
        }
        [Fact]
        public async Task CanRunMethodWithGenericOptionsOnlyTask() {
            var loop = new CommandLoop();
            var method = typeof(CommandRunnerTests).GetMethod(nameof(RunItWithGenericOptionsOnlyTask), bindings)!;
            var runner = new CommandRunner(method, ["runit"], null, []);
            didItRun = false;
            await runner.RunAsync("teste.exe runit felipe machado", loop);
            Assert.True(didItRun);
        }
        [Fact]
        public async Task CanRunMethodWithAllParamsTask() {
            var loop = new CommandLoop();
            var method = typeof(CommandRunnerTests).GetMethod(nameof(RunItWithAllParamsTask), bindings)!;
            var runner = new CommandRunner(method, ["runit"], null, []);
            didItRun = false;
            await runner.RunAsync("teste.exe runit felipe machado", loop);
            Assert.True(didItRun);
        }

    }
}