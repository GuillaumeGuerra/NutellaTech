namespace Sudoku.Business
{
    public class GameGridCell
    {
        public int? CurrentValue { get; set; }

        public override string ToString()
        {
            return CurrentValue.HasValue ? CurrentValue.Value.ToString() : "";
        }
    }
}