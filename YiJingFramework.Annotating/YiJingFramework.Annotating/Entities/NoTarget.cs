namespace YiJingFramework.Annotating.Entities
{
    /// <summary>
    /// 应用于 <seealso cref="AnnotationStore.NoTargetGroups"/> 。
    /// Used for <seealso cref="AnnotationStore.NoTargetGroups"/>.
    /// </summary>
    public record struct NoTarget
    {
        /// <summary>
        /// 唯一可能的值。
        /// The only possible value.
        /// </summary>
        public static NoTarget Default => default;
    }
}