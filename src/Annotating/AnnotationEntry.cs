using System.Text.Json.Serialization;

namespace YiJingFramework.Annotating;

/// <summary>
/// 注解条目。
/// An annotation entry.
/// </summary>
public sealed class AnnotationEntry
{
    // Used JsonPropertyNames:
    // c t

    /// <summary>
    /// 创建 <seealso cref="AnnotationEntry"/> 的实例。
    /// Create a new instance of <seealso cref="AnnotationEntry"/>.
    /// </summary>
    public AnnotationEntry() { }

    /// <summary>
    /// 创建 <seealso cref="AnnotationEntry"/> 的实例。
    /// Create a new instance of <seealso cref="AnnotationEntry"/>.
    /// </summary>
    /// <param name="target">
    /// 注解所针对的内容。
    /// Target of the annotation.
    /// </param>
    /// <param name="content">
    /// 注解内容。
    /// Content of the annotation.
    /// </param>
    [JsonConstructor]
    public AnnotationEntry(string? target, string? content)
    {
        this.Target = target;
        this.Content = content;
    }

    /// <summary>
    /// 注解所针对的内容。
    /// Target of the annotation.
    /// </summary>
    [JsonPropertyName("t")]
    public string? Target { get; set; }

    /// <summary>
    /// 注解内容。
    /// Content of the annotation.
    /// </summary>
    [JsonPropertyName("c")]
    public string? Content { get; set; }
}