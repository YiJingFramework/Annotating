using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using YiJingFramework.Core;
using YiJingFramework.Serialization;

namespace YiJingFramework.Annotating
{
    [JsonConverter(typeof(JsonConverterOfStringConvertibleForJson<AnnotationTarget>))]
    public sealed record AnnotationTarget(Painting? Painting = null, int Line = -1)
        : IStringConvertibleForJson<AnnotationTarget>
    {
        static bool IStringConvertibleForJson<AnnotationTarget>.FromStringForJson(
            string s, [MaybeNullWhen(false)] out AnnotationTarget result)
        {
            var sp = s.Split();
            switch (sp.Length)
            {
                case 0:
                {
                    result = new AnnotationTarget();
                    return true;
                }
                case 1:
                {
                    if (!Painting.TryParse(sp[0], out var painting))
                    {
                        result = null;
                        return false;
                    }
                    result = new(painting);
                    return true;
                }
                case 2:
                {
                    if (!int.TryParse(sp[1], out var line))
                    {
                        result = null;
                        return false;
                    }
                    if (!Painting.TryParse(sp[0], out var painting))
                    {
                        result = null;
                        return false;
                    }
                    result = new(painting, line);
                    return true;
                }
                default:
                {
                    result = null;
                    return false;
                }
            }
        }

        string IStringConvertibleForJson<AnnotationTarget>.ToStringForJson()
        {
            if (Painting is null)
                return " ";

            if (Line < 0 || Line >= Painting.Count)
                return $"{Painting}";

            return $"{Painting} {Line}";
        }
    }
}