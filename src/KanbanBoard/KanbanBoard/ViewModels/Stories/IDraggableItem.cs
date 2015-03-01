using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanBoard.ViewModels
{
    public interface IDraggableItem
    {
        double Width { get; set; }
        double Height { get; set; }
        int Top { get; set; }
        int Left { get; set; }
        int ZIndex { get; set; }
        bool IsReadOnly { get; set; }
        bool QuickActionsVisible { get; set; }
        string Status { get; set; }
        int Index { get; set; }
        double RotateAngleFactor { get; set; }
        int CornerRadius { get; set; }
        Guid ItemKey { get; set; }
        HightlightStatus HightlightStatus { get; set; }
    }
}
