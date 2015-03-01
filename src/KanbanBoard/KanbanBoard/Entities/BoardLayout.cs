using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KanbanBoard.Entities
{
    public class BoardLayout
    {
        public int RowsCount { get; set; }
        public int ColumnsCount
        {
            get
            {
                int count = 0;
                Columns.ForEach(c => count += c.Width);
                return count;
            }
        }
        public List<BoardColumnDescription> Columns { get; set; }

        public BoardColumnDescription this[string status]
        {
            get { return Columns.Find(c => (c.Status == status)); }
        }

        public BoardLayout()
        {
            Columns = new List<BoardColumnDescription>();
        }
    }
}
