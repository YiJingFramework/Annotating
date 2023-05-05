using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;
using YiJingFramework.PrimitiveTypes;
using YiJingFramework.PrimitiveTypes.Serialization;

namespace YiJingFramework.Annotating.Entities;

/// <summary>
/// 某个卦中的若干个爻。
/// Some lines of a Gua.
/// </summary>
[JsonConverter(typeof(JsonConverterOfStringConvertibleForJson<GuaLines>))]
public sealed class GuaLines :
    IStringConvertibleForJson<GuaLines>,
    IComparable<GuaLines>, IEquatable<GuaLines>,
    IEqualityOperators<GuaLines, GuaLines, bool>
{
    /// <summary>
    /// 卦。
    /// The Gua.
    /// </summary>
    public Gua Gua { get; }

    /// <summary>
    /// 表示此实例所指代的爻，应当和 <seealso cref="Gua"/> 有一样多的爻。
    /// Represent the lines indicated by the instance,
    /// which should have as many lines as <seealso cref="Gua"/>.
    /// 若 <see cref="Lines"/> 中某一爻为阳爻，
    /// 则表示此实例指代 <seealso cref="Gua"/> 中的同位爻。
    /// If a line in <see cref="Lines"/> is a Yang line,
    /// then this instance represents the line in the <seealso cref="Gua"/> at the same position.
    /// </summary>
    public Gua Lines { get; }

    /// <summary>
    /// 创建一个 <seealso cref="GuaLines"/> 的实例。
    /// Create an instance of <seealso cref="GuaLines"/>.
    /// </summary>
    /// <param name="gua">
    /// 卦。
    /// The Gua.
    /// </param>
    /// <param name="lines">
    /// 爻。
    /// The lines.
    /// 具体见 <seealso cref="Lines"/> 。
    /// For more details, see <seealso cref="Lines"/>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="gua"/> 或 <paramref name="lines"/> 为 <c>null</c> 。
    /// Either <paramref name="gua"/> or <paramref name="lines"/> is <c>null</c> 。
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="lines"/> 与 <paramref name="gua"/> 所含爻数不同。
    /// <paramref name="lines"/> and <paramref name="gua"/> have different count of lines.
    /// </exception>
    public GuaLines(Gua gua, Gua lines)
    {
        ArgumentNullException.ThrowIfNull(gua);
        ArgumentNullException.ThrowIfNull(lines);

        if (lines.Count != gua.Count)
            throw new ArgumentException(
                $"{nameof(lines)}.{nameof(lines.Count)} should be equal to {nameof(gua)}.{nameof(gua.Count)}.");
        this.Gua = gua;
        this.Lines = lines;
    }

    /// <summary>
    /// 创建一个 <seealso cref="GuaLines"/> 的实例。
    /// Create an instance of <seealso cref="GuaLines"/>.
    /// </summary>
    /// <param name="gua">
    /// 卦。
    /// The Gua.
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
    /// <paramref name="gua"/> 或 <paramref name="linesIndexes"/> 为 <c>null</c> 。
    /// Either <paramref name="gua"/> or <paramref name="linesIndexes"/> is <c>null</c> 。
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="linesIndexes"/> 超出了范围。
    /// <paramref name="linesIndexes"/> are out of range.
    /// </exception>
    public GuaLines(Gua gua, IEnumerable<int> linesIndexes, int indexBase = 0)
    {
        ArgumentNullException.ThrowIfNull(gua);
        ArgumentNullException.ThrowIfNull(linesIndexes);

        this.Gua = gua;

        var count = gua.Count;
        var linesGua = new Yinyang[count];
        foreach (var index in linesIndexes)
        {
            var line = index - indexBase;
            if (line < 0 || line >= count)
                throw new ArgumentOutOfRangeException(
                    nameof(linesIndexes),
                    $"The {nameof(index)}(base:{indexBase}) line does not exist in the Gua.");
            linesGua[line] = Yinyang.Yang;
        }
        this.Lines = new Gua(linesGua);
    }

    /// <summary>
    /// 创建一个 <seealso cref="GuaLines"/> 的实例。
    /// Create an instance of <seealso cref="GuaLines"/>.
    /// </summary>
    /// <param name="gua">
    /// 卦。
    /// The Gua.
    /// </param>
    /// <param name="linesIndexes">
    /// 爻。从零开始计。
    /// The lines. 0-based.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="gua"/> 或 <paramref name="linesIndexes"/> 为 <c>null</c> 。
    /// Either <paramref name="gua"/> or <paramref name="linesIndexes"/> is <c>null</c> 。
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="linesIndexes"/> 超出了范围。
    /// <paramref name="linesIndexes"/> are out of range.
    /// </exception>
    public GuaLines(Gua gua, params int[] linesIndexes)
        : this(gua, (IEnumerable<int>)linesIndexes)
    { }

    static bool IStringConvertibleForJson<GuaLines>.FromStringForJson(
        string s, [MaybeNullWhen(false)] out GuaLines result)
    {
        if (s.Length % 2 is not 0)
        {
            result = null;
            return false;
        }

        var half = s.Length / 2;
        if (!Gua.TryParse(s[0..half], out var gua))
        {
            result = null;
            return false;
        }
        if (!Gua.TryParse(s[half..], out var lines))
        {
            result = null;
            return false;
        }
        result = new GuaLines(gua, lines);
        return true;
    }

    string IStringConvertibleForJson<GuaLines>.ToStringForJson()
    {
        return $"{this.Gua}{this.Lines}";
    }

    int IComparable<GuaLines>.CompareTo(GuaLines? other)
    {
        var c = this.Gua.CompareTo(other?.Gua);
        if (c is 0)
            return this.Lines.CompareTo(other?.Lines);
        return c;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(GuaLines? other)
    {
        return this.Gua.Equals(other?.Gua) && this.Lines.Equals(other?.Lines);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(GuaLines? left, GuaLines? right)
    {
        return left?.Gua == right?.Gua && left?.Lines == right?.Lines;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(GuaLines? left, GuaLines? right)
    {
        return left?.Gua != right?.Gua || left?.Lines != right?.Lines;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is not GuaLines other)
            return false;
        return this.Equals(other);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(this.Gua, this.Lines);
    }
}