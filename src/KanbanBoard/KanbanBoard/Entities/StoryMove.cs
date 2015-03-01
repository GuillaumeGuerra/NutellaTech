using KanbanBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanBoard.Entities
{
    public class StoryMove
    {
        public IDraggableItem Story { get; set; }

        public string Status { get; set; }

        public int SourceIndex { get; set; }
        public int TargetIndex { get; set; }

        public StoryMove GetReverseMove()
        {
            return new StoryMove() { Story = Story, Status = Status, SourceIndex = TargetIndex, TargetIndex = SourceIndex };
        }
    }
}
