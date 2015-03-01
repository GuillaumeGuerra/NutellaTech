using KanbanBoard.Entities;
using KanbanBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KanbanBoard.Behaviors
{
    public class BlockingStoriesManager
    {
        private UserStoriesViewModel Stories { get; set; }
        private Dictionary<Guid, StoryMove> MovesDico { get; set; }
        private Dictionary<string, IDraggableItem[]> InitialLayout { get; set; }
        public string InitialStatus { get; set; }

        public BlockingStoriesManager(UserStoriesViewModel stories, string status, int index)
        {
            MovesDico = new Dictionary<Guid, StoryMove>();
            Stories = stories;
            InitialStatus = status;

            BackupBoard(status, index);
        }

        public List<StoryMove> PrepareMoves(string newStatus, int newIndex)
        {
            var ToRollback = new List<StoryMove>();
            var ToMove = new List<StoryMove>();

            var newMoves = Stories.IdentifyBlockingStories(newStatus, newIndex, InitialLayout[newStatus]);
            if (newMoves == null) 
                return null;

            var newDico = GetDico(newMoves);
            foreach (var item in MovesDico)
            {
                StoryMove tmp = null;
                if (!newDico.TryGetValue(item.Value.Story.ItemKey, out tmp))
                {
                    ToRollback.Add(item.Value.GetReverseMove());
                    ToMove.Add(item.Value.GetReverseMove());
                }
            }

            foreach (var item in newDico)
            {
                StoryMove tmp = null;
                if (!MovesDico.TryGetValue(item.Value.Story.ItemKey, out tmp))
                {
                    ToMove.Add(item.Value);
                    MovesDico.Add(item.Key, item.Value);
                }
                else if (item.Value.TargetIndex != tmp.TargetIndex)
                {
                    throw new NotImplementedException("This case should not happen");
                }
            }

            ToRollback.ForEach(m => MovesDico.Remove(m.Story.ItemKey));

            return ToMove;
        }

        private Dictionary<Guid, StoryMove> GetDico(List<StoryMove> moves)
        {
            var result = new Dictionary<Guid, StoryMove>();
            moves.ForEach(m => result.Add(m.Story.ItemKey, m));
            return result;
        }

        private void BackupBoard(string status, int index)
        {
            InitialLayout = new Dictionary<string, IDraggableItem[]>();
            foreach (var column in Stories.BoardLayout.Columns)
            {
                IDraggableItem[] currentLayout = Stories.StoriesMatrix[column.Status];
                IDraggableItem[] copy = new IDraggableItem[currentLayout.Length];
                for (int i = 0; i < currentLayout.Length; i++)
                {
                    copy[i] = currentLayout[i];
                }

                InitialLayout.Add(column.Status, copy);
            }

            InitialLayout[status][index] = null;
        }
    }
}
