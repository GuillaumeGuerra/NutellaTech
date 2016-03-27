namespace PatchManager.Models
{
    public enum GerritStatus
    {
        MissingReviews,
        MissingBuild,
        BuildFailed,
        ReadyForMerge,
        Merged,
        Unknown
    }
}