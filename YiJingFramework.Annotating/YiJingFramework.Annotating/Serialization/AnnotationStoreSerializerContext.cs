using System.Text.Json.Serialization;

namespace YiJingFramework.Annotating.Serialization
{
    [JsonSerializable(typeof(AnnotationStore))]
    internal sealed partial class AnnotationStoreSerializerContext : JsonSerializerContext
    {
    }
}
