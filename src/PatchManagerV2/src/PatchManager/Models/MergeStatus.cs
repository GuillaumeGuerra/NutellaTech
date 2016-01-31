namespace PatchManager.Models
{
    public enum MergeStatus
    {
        MissingReviews,
        MissingBuild,
        BuildFailed,
        ReadyForMerge,
        Merged
    }
}