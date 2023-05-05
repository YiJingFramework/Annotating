using System.Text.Json.Serialization;

namespace YiJingFramework.Annotating;

/// <summary>
/// 一组注解。
/// A group of annotation.
/// </summary>
/// <typeparam name="T">
/// 注解所针对内容的类型。
/// Type of target of the entry.
/// 应该使用可以（被 <seealso cref="System.Text.Json"/> ）（反）序列化的类型。
/// It should be (de)serializable (with <seealso cref="System.Text.Json"/>).
/// </typeparam>
public sealed class AnnotationGroup<T>
{
    // Used JsonPropertyNames:
    // c e t

    /// <summary>
    /// 创建一个空的注解组。
    /// Create an empty annotation group.
    /// </summary>
    public AnnotationGroup() : this(null, null, null)
    { }

    /// <summary>
    /// 创建 <seealso cref="AnnotationEntry{T}"/> 的实例。
    /// Create a new instance of <seealso cref="AnnotationEntry{T}"/>.
    /// 一般在反序列化时使用。
    /// Only used during deserialization usually.
    /// </summary>
    /// <param name="title">
    /// 组标题。
    /// Title of the group.
    /// </param>
    /// <param name="comment">
    /// 一些关于这个组的内容。
    /// Something about this group.
    /// </param>
    /// <param name="entries">
    /// 包含的条目。
    /// Included entries.
    /// 此参数会被复制而非直接引用。
    /// This will be copied rather than directly referenced.
    /// </param>
    [JsonConstructor]
    public AnnotationGroup(string? title = null, string? comment = null, IList<AnnotationEntry<T>>? entries = null)
    {
        this.Title = title;
        this.Comment = comment;
        if (entries is null)
            this.Entries = new List<AnnotationEntry<T>>();
        else
            this.Entries = new List<AnnotationEntry<T>>(entries);
    }

    /// <summary>
    /// 组标题。
    /// Title of the group.
    /// </summary>
    [JsonPropertyName("t")]
    public string? Title { get; set; }

    /// <summary>
    /// 包含的条目。
    /// Included entries.
    /// </summary>
    [JsonPropertyName("e")]
    public IList<AnnotationEntry<T>> Entries { get; }

    /// <summary>
    /// 一些关于这个组的内容。
    /// Something about this group.
    /// </summary>
    [JsonPropertyName("c")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Comment { get; set; }

    /// <summary>
    /// 添加一个新条目。
    /// Add a new entry.
    /// </summary>
    /// <param name="target">
    /// 注解所针对的内容。
    /// Target of the annotation.
    /// </param>
    /// <param name="content">
    /// 注解内容。
    /// Content of the annotation.
    /// </param>
    /// <returns>
    /// 条目。
    /// The entry.
    /// </returns>
    public AnnotationEntry<T> AddEntry(T? target, string? content)
    {
        var e = new AnnotationEntry<T>() {
            Content = content,
            Target = target
        };
        this.Entries.Add(e);
        return e;
    }
}