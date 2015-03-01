using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace KanbanBoard.Entities
{
    public class BoardColumnDescription
    {
        public int Width { get; set; }
        public Color BackgroundColor { get; set; }

        public string Status { get; set; }
        public string Header { get; set; }
        public List<string> DefinitionOfDone { get; set; }
    }
}
