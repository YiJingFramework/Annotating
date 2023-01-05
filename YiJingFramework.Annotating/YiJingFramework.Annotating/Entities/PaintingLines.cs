using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;
using YiJingFramework.Core;
using YiJingFramework.Serialization;

namespace YiJingFramework.Annotating.Entities
{
    /// <summary>
    /// 某个卦画中的若干个爻。
    /// Some lines of a painting.
    /// </summary>
    [JsonConverter(typeof(JsonConverterOfStringConvertibleForJson<PaintingLines>))]
    public sealed class PaintingLines :
        IStringConvertibleForJson<PaintingLines>,
        IComparable<PaintingLines>, IEquatable<PaintingLines>,
        IEqualityOperators<PaintingLines, PaintingLines, bool>
    {
        /// <summary>
        /// 卦画。
        /// The painting.
        /// </summary>
        public Painting Painting { get; }

        /// <summary>
        /// 表示此实例所指代的爻，应当和 <seealso cref="Painting"/> 有一样多的爻。
        /// Represent the lines indicated by the instance,
        /// which should have as many lines as <seealso cref="Painting"/>.
        /// 若 <see cref="Lines"/> 中某一爻为阳爻，
        /// 则表示此实例指代 <seealso cref="Painting"/> 中的同位爻。
        /// If a line in <see cref="Lines"/> is a Yang line,
        /// then this instance represents the line in the <seealso cref="Painting"/> at the same position.
        /// </summary>
        public Painting Lines { get; }

        /// <summary>
        /// 创建一个 <seealso cref="PaintingLines"/> 的实例。
        /// Create an instance of <seealso cref="PaintingLines"/>.
        /// </summary>
        /// <param name="painting">
        /// 卦画。
        /// The painting.
        /// </param>
        /// <param name="lines">
        /// 爻。
        /// The lines.
        /// 具体见 <seealso cref="Lines"/> 。
        /// For more details, see <seealso cref="Lines"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="painting"/> 或 <paramref name="lines"/> 为 <c>null</c> 。
        /// Either <paramref name="painting"/> or <paramref name="lines"/> is <c>null</c> 。
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="lines"/> 与 <paramref name="painting"/> 所含爻数不同。
        /// <paramref name="lines"/> and <paramref name="painting"/> have different count of lines.
        /// </exception>
        public PaintingLines(Painting painting, Painting lines)
        {
            ArgumentNullException.ThrowIfNull(painting);
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Count != painting.Count)
                throw new ArgumentException(
                    $"{nameof(lines)}.{nameof(lines.Count)} should be equal to {nameof(painting)}.{nameof(painting.Count)}.");
            Painting = painting;
            Lines = lines;
        }

        /// <summary>
        /// 创建一个 <seealso cref="PaintingLines"/> 的实例。
        /// Create an instance of <seealso cref="PaintingLines"/>.
        /// </summary>
        /// <param name="painting">
        /// 卦画。
        /// The painting.
        /// </param>
        /// <param name="linesIndexes">
        /// 爻。
        /// The lines.
        /// </param>
        /// <param name="indexBase">
        /// <paramref name="linesIndexes"/> 的起始数。
        /// The base of <paramref name="linesIndexes"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="painting"/> 或 <paramref name="linesIndexes"/> 为 <c>null</c> 。
        /// Either <paramref name="painting"/> or <paramref name="linesIndexes"/> is <c>null</c> 。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="linesIndexes"/> 超出了范围。
        /// <paramref name="linesIndexes"/> are out of range.
        /// </exception>
        public PaintingLines(Painting painting, IEnumerable<int> linesIndexes, int indexBase = 0)
        {
            ArgumentNullException.ThrowIfNull(painting);
            ArgumentNullException.ThrowIfNull(linesIndexes);

            Painting = painting;

            var count = painting.Count;
            var linesPainting = new YinYang[count];
            foreach (var index in linesIndexes)
            {
                var line = index - indexBase;
                if (line < 0 || line >= count)
                    throw new ArgumentOutOfRangeException(
                        nameof(linesIndexes),
                        $"The {nameof(index)}(base:{indexBase}) line does not exist in the painting.");
                linesPainting[line] = YinYang.Yang;
            }
            Lines = new Painting(linesPainting);
        }

        /// <summary>
        /// 创建一个 <seealso cref="PaintingLines"/> 的实例。
        /// Create an instance of <seealso cref="PaintingLines"/>.
        /// </summary>
        /// <param name="painting">
        /// 卦画。
        /// The painting.
        /// </param>
        /// <param name="linesIndexes">
        /// 爻。从零开始计。
        /// The lines. 0-based.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="painting"/> 或 <paramref name="linesIndexes"/> 为 <c>null</c> 。
        /// Either <paramref name="painting"/> or <paramref name="linesIndexes"/> is <c>null</c> 。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="linesIndexes"/> 超出了范围。
        /// <paramref name="linesIndexes"/> are out of range.
        /// </exception>
        public PaintingLines(Painting painting, params int[] linesIndexes)
            : this(painting, (IEnumerable<int>)linesIndexes)
        { }

        static bool IStringConvertibleForJson<PaintingLines>.FromStringForJson(
            string s, [MaybeNullWhen(false)] out PaintingLines result)
        {
            if (s.Length % 2 is not 0)
            {
                result = null;
                return false;
            }

            var half = s.Length / 2;
            if (!Painting.TryParse(s[0..half], out var painting))
            {
                result = null;
                return false;
            }
            if (!Painting.TryParse(s[half..], out var lines))
            {
                result = null;
                return false;
            }
            result = new PaintingLines(painting, lines);
            return true;
        }

        string IStringConvertibleForJson<PaintingLines>.ToStringForJson()
        {
            return $"{Painting}{Lines}";
        }

        int IComparable<PaintingLines>.CompareTo(PaintingLines? other)
        {
            var c = Painting.CompareTo(other?.Painting);
            if (c is 0)
                return Lines.CompareTo(other?.Lines);
            return c;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(PaintingLines? other)
        {
            return Painting.Equals(other?.Painting) && Lines.Equals(other?.Lines);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(PaintingLines? left, PaintingLines? right)
        {
            return left?.Painting == right?.Painting && left?.Lines == right?.Lines;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(PaintingLines? left, PaintingLines? right)
        {
            return left?.Painting != right?.Painting || left?.Lines != right?.Lines;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj is not PaintingLines other)
                return false;
            return Equals(other);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Painting, Lines);
        }
    }
}