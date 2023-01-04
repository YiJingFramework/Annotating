using System.Text.Json;
using System.Text.Json.Serialization;

namespace YiJingFramework.Annotating
{
    public sealed class AnnotationStore
    {
        public AnnotationStore() : this(null, null, null)
        { }
        
        [JsonConstructor]
        public AnnotationStore(string? title,
            IEnumerable<AnnotationGroup>? groups,
            IEnumerable<string>? tags)
        {
            this.Title = title;

            if (groups is null)
                this.Groups = new List<AnnotationGroup>();
            else
                this.Groups = new List<AnnotationGroup>(groups);

            if (tags is null)
                this.Tags = new List<string>();
            else
                this.Tags = new List<string>(tags);
        }
        public string? Title { get; set; }
        public IList<AnnotationGroup> Groups { get; }
        public IList<string> Tags { get; }

        public AnnotationGroup AddGroup(string? name = null)
        {
            var g = new AnnotationGroup(name);
            this.Groups.Add(g);
            return g;
        }
        public void AddTag(string s)
        {
            this.Tags.Add(s);
        }
    }
}