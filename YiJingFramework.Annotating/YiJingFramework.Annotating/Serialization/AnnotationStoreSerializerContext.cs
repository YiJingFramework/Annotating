using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YiJingFramework.Annotating.Serialization
{
    [JsonSerializable(typeof(AnnotationStore))]
    internal sealed partial class AnnotationStoreSerializerContext : JsonSerializerContext
    {
    }
}
