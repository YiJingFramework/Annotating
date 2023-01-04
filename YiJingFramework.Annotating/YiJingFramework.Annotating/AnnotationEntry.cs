using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using YiJingFramework.Core;

namespace YiJingFramework.Annotating
{
    public sealed class AnnotationEntry
    {
        public AnnotationEntry() { }

        [SetsRequiredMembers]
        public AnnotationEntry(AnnotationTarget target, string content)
        {
            this.Target = target;
            this.Content = content;
        }

        private AnnotationTarget? target;
        public required AnnotationTarget Target
        {
            get
            {
                Debug.Assert(target is not null);
                return target;
            }
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                this.target = value;
            }
        }

        private string? content;
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
                this.content = value;
            }
        }
    }
}