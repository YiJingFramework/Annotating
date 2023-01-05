using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace YiJingFramework.Annotating
{
    /// <summary>
    /// 注解条目。
    /// An annotation entry.
    /// </summary>
    /// <typeparam name="T">
    /// 注解所针对内容的类型。
    /// Type of target of the entry.
    /// 应该使用可以（被 <seealso cref="System.Text.Json"/> ）（反）序列化的类型。
    /// It should be (de)serializable (with <seealso cref="System.Text.Json"/>).
    /// </typeparam>
    public sealed class AnnotationEntry<T>
    {
        // Used JsonPropertyNames:
        // c t

        /// <summary>
        /// 创建 <seealso cref="AnnotationEntry{T}"/> 的实例。
        /// Create a new instance of <seealso cref="AnnotationEntry{T}"/>.
        /// </summary>
        public AnnotationEntry() { }

        /// <summary>
        /// 创建 <seealso cref="AnnotationEntry{T}"/> 的实例。
        /// Create a new instance of <seealso cref="AnnotationEntry{T}"/>.
        /// </summary>
        /// <param name="target">
        /// 注解所针对的内容。
        /// Target of the annotation.
        /// </param>
        /// <param name="content">
        /// 注解内容。
        /// Content of the annotation.
        /// </param>
        [SetsRequiredMembers]
        public AnnotationEntry(T target, string content)
        {
            Target = target;
            Content = content;
        }

        [JsonIgnore]
        private T? target;

        /// <summary>
        /// 注解所针对的内容。
        /// Target of the annotation.
        /// </summary>
        [JsonPropertyName("t")]
        public required T Target
        {
            get
            {
                Debug.Assert(target is not null);
                return target;
            }
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                target = value;
            }
        }

        [JsonIgnore]
        private string? content;

        /// <summary>
        /// 注解内容。
        /// Content of the annotation.
        /// </summary>
        [JsonPropertyName("c")]
        public required string Content
        {
            get
            {
                Debug.Assert(content is not null);
                return content;
            }
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                content = value;
            }
        }
    }
}