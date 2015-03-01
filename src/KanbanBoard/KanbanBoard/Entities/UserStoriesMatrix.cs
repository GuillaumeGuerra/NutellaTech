using KanbanBoard.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KanbanBoard.Entities
{
    public class UserStoriesMatrix : IEnumerable<KeyValuePair<string, IDraggableItem[]>>
    {
        private Dictionary<string, IDraggableItem[]> MatrixDico { get; set; }
        private List<IDraggableItem[]> MatrixList { get; set; }

        public IDraggableItem[] this[string status] { get { return MatrixDico[status]; } }
        public IDraggableItem[] this[int index] { get { return MatrixList[index]; } }

        public UserStoriesMatrix(BoardLayout layout)
        {
            MatrixDico = new Dictionary<string, IDraggableItem[]>();
            MatrixList = new List<IDraggableItem[]>();

            foreach (var column in layout.Columns)
            {
                IDraggableItem[] array = new IDraggableItem[column.Width * layout.RowsCount];
                MatrixDico.Add(column.Status, array);
                MatrixList.Add(array);
            }
        }

        public IEnumerator<KeyValuePair<string, IDraggableItem[]>> GetEnumerator()
        {
            return MatrixDico.GetEnumerator();
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return MatrixDico.GetEnumerator();
        }
    }
}
