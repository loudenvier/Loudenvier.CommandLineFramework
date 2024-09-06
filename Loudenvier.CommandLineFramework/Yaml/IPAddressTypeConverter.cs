using System.Net;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Loudenvier.CommandLineFramework.Yaml;

public class IPAddressTypeConverter : IYamlTypeConverter
{
    public bool Accepts(Type type) => type.Equals(typeof(IPAddress));

    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
        => IPAddress.Parse(parser.Consume<Scalar>().Value);

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        var ip = (IPAddress)value!;
        emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, ip.ToString(), ScalarStyle.Any, true, false));
    }
}

