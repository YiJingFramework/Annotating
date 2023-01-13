using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Xml.Linq;
using YiJingFramework.Annotating.Entities;
using YiJingFramework.Annotating.Serialization;
using YiJingFramework.Core;

namespace YiJingFramework.Annotating
{
    /// <summary>
    /// 一个注解仓库，通常对应一个 json 文件。
    /// An annotation store, corresponding to a json file usually.
    /// </summary>
    public sealed class AnnotationStore
    {
        // Used JsonPropertyNames:
        // gl gp gs n t

        /// <summary>
        /// 仓库标题。
        /// Title of the store.
        /// </summary>
        [JsonPropertyName("n")]
        public string? Title { get; set; }

        /// <summary>
        /// 仓库标签。
        /// Tags of the store.
        /// </summary>
        [JsonPropertyName("t")]
        public IList<string> Tags { get; }

        /// <summary>
        /// 创建一个空的注解仓库。
        /// Create an empty annotation store.
        /// </summary>
        public AnnotationStore() : this(null, null, null, null, null)
        { }

        /// <summary>
        /// 创建 <seealso cref="AnnotationStore"/> 的实例。
        /// Create a new instance of <seealso cref="AnnotationStore"/>.
        /// 一般在反序列化时使用。
        /// Only used during deserialization usually.
        /// </summary>
        /// <param name="title">
        /// 仓库标题。
        /// Title of the store.
        /// </param>
        /// <param name="tags">
        /// 仓库标签。
        /// Tags of the store.
        /// </param>
        /// <param name="stringGroups">
        /// 以 <seealso cref="string"/> 为目标的一系列注解组。
        /// Annotation groups target <seealso cref="string"/>s.
        /// 此参数会被复制而非直接引用。
        /// This will be copied rather than directly referenced.
        /// </param>
        /// <param name="paintingGroups">
        /// 以 <seealso cref="Painting"/> 为目标的一系列注解组。
        /// Annotation groups target <seealso cref="Painting"/>s.
        /// 此参数会被复制而非直接引用。
        /// This will be copied rather than directly referenced.
        /// </param>
        /// <param name="paintingLinesGroups">
        /// 以 <seealso cref="PaintingLines"/> 为目标的一系列注解组。
        /// Annotation groups target <seealso cref="PaintingLines"/>s.
        /// 此参数会被复制而非直接引用。
        /// This will be copied rather than directly referenced.
        /// </param>
        [JsonConstructor]
        public AnnotationStore(string? title,
            IList<string>? tags,
            IList<AnnotationGroup<string>>? stringGroups,
            IList<AnnotationGroup<Painting>>? paintingGroups,
            IList<AnnotationGroup<PaintingLines>>? paintingLinesGroups)
        {
            static List<T> CreateList<T>(IList<T>? e)
            {
                if (e is null)
                    return new List<T>();
                return new List<T>(e);
            }

            this.Title = title;
            this.Tags = CreateList(tags);

            this.StringGroups = CreateList(stringGroups);
            this.PaintingGroups = CreateList(paintingGroups);
            this.PaintingLinesGroups = CreateList(paintingLinesGroups);
        }

        /// <summary>
        /// 序列化为 json 字符串。
        /// Serialize to a json string.
        /// </summary>
        /// <param name="serializerOptions">
        /// 序列化器选项。
        /// Serializer options.
        /// </param>
        /// <returns>
        /// 结果。
        /// The result.
        /// </returns>
        public string SerializeToJsonString(JsonSerializerOptions? serializerOptions = null)
        {
            var context = serializerOptions is null ?
                AnnotationStoreSerializerContext.Default :
                new AnnotationStoreSerializerContext(serializerOptions);

            return JsonSerializer.Serialize(this, context.AnnotationStore);
        }

        /// <summary>
        /// 从 json 字符串反序列化。
        /// Deserialize from a json string.
        /// </summary>
        /// <param name="s">
        /// json 字符串。
        /// The json string.
        /// </param>
        /// <param name="serializerOptions">
        /// 序列化器选项。
        /// Serializer options.
        /// </param>
        /// <returns>
        /// 结果。
        /// The result.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="s"/> 是 <c>null</c> 。
        /// <paramref name="s"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="JsonException">
        /// 反序列化失败。
        /// Deserialization failed.
        /// </exception>
        public static AnnotationStore? DeserializeFromJsonString(
            string s, JsonSerializerOptions? serializerOptions = null)
        {
            ArgumentNullException.ThrowIfNull(s);

            var context = serializerOptions is null ?
                AnnotationStoreSerializerContext.Default :
                new AnnotationStoreSerializerContext(serializerOptions);

            return JsonSerializer.Deserialize(s, context.AnnotationStore);
        }

        /// <summary>
        /// 以 <seealso cref="string"/> 为目标的一系列注解组。
        /// Annotation groups target <seealso cref="string"/>s.
        /// 通常这些字符串不是真正的“目标”，而是目标的字符串表示形式。
        /// These strings are usually not the real "target", but its string representation.
        /// </summary>
        [JsonPropertyName("gs")]
        public IList<AnnotationGroup<string>> StringGroups { get; }

        /// <summary>
        /// 添加一个新 <seealso cref="string"/> 组。
        /// Add a new <seealso cref="string"/> group.
        /// </summary>
        /// <param name="title">
        /// 组标题。
        /// Title of the group.
        /// </param>
        /// <param name="comment">
        /// 一些关于这个组的内容。
        /// Something about this group.
        /// </param>
        /// <returns>
        /// 组。
        /// The group.
        /// </returns>
        public AnnotationGroup<string> AddStringGroup(
            string? title = null, string? comment = null)
        {
            var g = new AnnotationGroup<string>() {
                Title = title,
                Comment = comment
            };
            this.StringGroups.Add(g);
            return g;
        }

        /// <summary>
        /// 以 <seealso cref="Painting"/> 为目标的一系列注解组。
        /// Annotation groups target <seealso cref="Painting"/>s.
        /// </summary>
        [JsonPropertyName("gp")]
        public IList<AnnotationGroup<Painting>> PaintingGroups { get; }

        /// <summary>
        /// 添加一个新 <seealso cref="Painting"/> 组。
        /// Add a new <seealso cref="Painting"/> group.
        /// </summary>
        /// <param name="title">
        /// 组标题。
        /// Title of the group.
        /// </param>
        /// <param name="comment">
        /// 一些关于这个组的内容。
        /// Something about this group.
        /// </param>
        /// <returns>
        /// 组。
        /// The group.
        /// </returns>
        public AnnotationGroup<Painting> AddPaintingGroup(
            string? title = null, string? comment = null)
        {
            var g = new AnnotationGroup<Painting>() {
                Title = title,
                Comment = comment
            };
            this.PaintingGroups.Add(g);
            return g;
        }


        /// <summary>
        /// 以 <seealso cref="PaintingLines"/> 为目标的一系列注解组。
        /// Annotation groups target <seealso cref="PaintingLines"/>s.
        /// </summary>
        [JsonPropertyName("gl")]
        public IList<AnnotationGroup<PaintingLines>> PaintingLinesGroups { get; }

        /// <summary>
        /// 添加一个新 <seealso cref="PaintingLines"/> 组。
        /// Add a new <seealso cref="PaintingLines"/> group.
        /// </summary>
        /// <param name="title">
        /// 组标题。
        /// Title of the group.
        /// </param>
        /// <param name="comment">
        /// 一些关于这个组的内容。
        /// Something about this group.
        /// </param>
        /// <returns>
        /// 组。
        /// The group.
        /// </returns>
        public AnnotationGroup<PaintingLines> AddPaintingLinesGroup(
            string? title = null, string? comment = null)
        {
            var g = new AnnotationGroup<PaintingLines>() {
                Title = title,
                Comment = comment
            };
            this.PaintingLinesGroups.Add(g);
            return g;
        }
    }
}