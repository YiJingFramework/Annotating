using System.Text.Json;
using System.Text.Json.Serialization;
using YiJingFramework.Annotating.Serialization;

namespace YiJingFramework.Annotating;

/// <summary>
/// 一个注解仓库，通常对应一个 json 文件。
/// An annotation store, corresponding to a json file usually.
/// </summary>
public sealed class AnnotationStore
{
    // Used JsonPropertyNames:
    // g n t

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
    public AnnotationStore() : this(null, null, null)
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
    /// <param name="groups">
    /// 注解组。
    /// Annotation groups.
    /// 此参数会被复制而非直接引用。
    /// This will be copied rather than directly referenced.
    /// </param>
    [JsonConstructor]
    public AnnotationStore(string? title,
        IList<string>? tags,
        IList<AnnotationGroup>? groups)
    {
        static List<T> CopyList<T>(IList<T>? e)
        {
            if (e is null)
                return new List<T>();
            return new List<T>(e);
        }

        this.Title = title;
        this.Tags = CopyList(tags);

        this.Groups = CopyList(groups);
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
        AnnotationStoreSerializerContext context;
        if (serializerOptions is null)
            context = AnnotationStoreSerializerContext.Default;
        else
        {
            serializerOptions = new(serializerOptions);
            context = new AnnotationStoreSerializerContext(serializerOptions);
        }
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

        AnnotationStoreSerializerContext context;
        if (serializerOptions is null)
            context = AnnotationStoreSerializerContext.Default;
        else
        {
            serializerOptions = new(serializerOptions);
            context = new AnnotationStoreSerializerContext(serializerOptions);
        }
        return JsonSerializer.Deserialize(s, context.AnnotationStore);
    }

    /// <summary>
    /// 注解组。
    /// Annotation groups.
    /// </summary>
    [JsonPropertyName("g")]
    public IList<AnnotationGroup> Groups { get; }

    /// <summary>
    /// 添加一个新注解组。
    /// Add a new annotation group.
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
    public AnnotationGroup AddGroup(
        string? title = null, string? comment = null)
    {
        var g = new AnnotationGroup() {
            Title = title,
            Comment = comment
        };
        this.Groups.Add(g);
        return g;
    }

    /// <summary>
    /// 尝试获取注解组。
    /// Try to get an annotation group.
    /// </summary>
    /// <param name="title">
    /// 组标题。
    /// Title of the group.
    /// </param>
    /// <returns>
    /// 组。
    /// The group.
    /// </returns>
    public AnnotationGroup? GetGroup(string? title)
    {
        return this.Groups.FirstOrDefault(g => g.Title == title);
    }
}