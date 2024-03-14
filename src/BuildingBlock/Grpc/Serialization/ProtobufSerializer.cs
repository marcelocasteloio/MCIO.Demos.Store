using ProtoBuf.Meta;

namespace MCIO.Demos.Store.BuildingBlock.Grpc.Serialization;
public class ProtobufSerializer
{
    // Public Methods
    public static void ConfigureTypeCollection(IEnumerable<Type>? typeCollection)
    {
        if (typeCollection is null)
            return;

        foreach (var type in typeCollection)
        {
            var metaType = RuntimeTypeModel.Default.Add(
                type,
                applyDefaultBehaviour: false
            );

            foreach (var property in type.GetProperties())
                metaType.Add(property.Name);
        }
    }

    // Public Methods
    public static byte[] SerializeToProtobuf(object obj)
    {
        using var memoryStream = new MemoryStream();
        RuntimeTypeModel.Default.Serialize(memoryStream, obj);
        return memoryStream.ToArray();
    }
    public static T? DeserializeFromProtobuf<T>(byte[] byteArray)
    {
        using var memoryStream = new MemoryStream(byteArray);
        return RuntimeTypeModel.Default.Deserialize<T>(memoryStream);
    }
}
