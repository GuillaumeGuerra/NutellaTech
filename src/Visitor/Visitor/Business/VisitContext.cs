namespace VisitorModel.Business
{
    public class VisitContext
    {
        public static readonly VisitContext Default = new VisitContext()
        {
            SkipShortcuts = false
        };

        public bool SkipShortcuts { get; set; }
    }
}