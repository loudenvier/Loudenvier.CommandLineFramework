#pragma warning disable IDE1006 // Naming Styles

namespace Loudenvier.CommandLineFramework;

public class ConsoleState {
    readonly Dictionary<string, object?> State = [];
    public Dictionary<string, object?> Current => State;
    public T? Get<T>(string key) => State.TryGetValue(key, out var value) ? (T?)value : default;
    public T Set<T>(string key, T value) {
        State[key] = value;
        return value;
    }
}

#pragma warning restore IDE1006 // Naming Styles
