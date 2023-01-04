using System.Diagnostics;
using System.Text.Json.Serialization;
using YiJingFramework.Core;

namespace YiJingFramework.Annotating
{
    public sealed class AnnotationGroup
    {
        public AnnotationGroup() : this(null, null)
        { }

        [JsonConstructor]
        public AnnotationGroup(string? title = null, IEnumerable<AnnotationEntry>? entries = null)
        {
            this.Title = title;
            if (entries is null)
                this.Entries = new List<AnnotationEntry>();
            else
                this.Entries = new List<AnnotationEntry>(entries);
        }
        public string? Title { get; set; }
        public IList<AnnotationEntry> Entries { get; }

        public AnnotationEntry AddEntry(AnnotationTarget target, string content)
        {
            var e = new AnnotationEntry() {
                Content = content,
                Target = target
            };
            this.Entries.Add(e);
            return e;
        }
        public AnnotationEntry AddEntryTargetsPainting(Painting painting, string content)
        {
            ArgumentNullException.ThrowIfNull(painting);
            return AddEntry(new AnnotationTarget(painting), content);
        }
        public AnnotationEntry AddEntryTargetsLine(Painting painting, int line, string content)
        {
            ArgumentNullException.ThrowIfNull(painting);
            if (line < 0 || line >= painting.Count)
                throw new ArgumentOutOfRangeException(
                    nameof(line),
                    line,
                    $"{nameof(line)} should be >= 0 and < {nameof(painting)}.{nameof(painting.Count)}.");

            return AddEntry(new AnnotationTarget(painting), content);
        }
        public AnnotationEntry AddEntryTargetsAny(string content)
        {
            return AddEntry(new AnnotationTarget(), content);
        }
    }
}