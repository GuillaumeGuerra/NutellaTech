using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanBoard.Entities
{
    public class Position : IEquatable<Position>
    {
        public string Status { get; set; }
        public int Index { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public bool IsValidPosition { get; set; }

        public Position(string status, int index, int top, int left, bool isValidPosition)
        {
            Status = status;
            Index = index;
            Top = top;
            Left = left;
            IsValidPosition = isValidPosition;
        }

        public bool Equals(Position other)
        {
            return other != null && other.Status == Status && other.Index == Index;
        }
    }
}
