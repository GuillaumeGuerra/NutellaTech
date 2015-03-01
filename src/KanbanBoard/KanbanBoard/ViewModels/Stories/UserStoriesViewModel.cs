using Framework;
using Framework.Extensions;
using GalaSoft.MvvmLight.Command;
using KanbanBoard.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace KanbanBoard.ViewModels
{
    public class UserStoriesViewModel : ViewModel
    {
        #region Properties

        public static readonly DependencyProperty StoriesProperty =
            DependencyProperty.Register("Stories", typeof(ObservableCollection<IDraggableItem>), typeof(UserStoriesViewModel));
        public static readonly DependencyProperty CanvasWidthProperty =
            DependencyProperty.Register("CanvasWidth", typeof(double), typeof(UserStoriesViewModel));
        public static readonly DependencyProperty CanvasHeightProperty =
            DependencyProperty.Register("CanvasHeight", typeof(double), typeof(UserStoriesViewModel));
        public static readonly DependencyProperty GridHeightProperty =
            DependencyProperty.Register("GridHeight", typeof(double), typeof(UserStoriesViewModel), new PropertyMetadata(1000D));
        public static readonly DependencyProperty GridWidthProperty =
            DependencyProperty.Register("GridWidth", typeof(double), typeof(UserStoriesViewModel), new PropertyMetadata(1000D));
        public static readonly DependencyProperty BoardLayoutProperty =
            DependencyProperty.Register("BoardLayout", typeof(BoardLayout), typeof(UserStoriesViewModel));
        public static readonly DependencyProperty DraggableItemToEditProperty =
            DependencyProperty.Register("DraggableItemToEdit", typeof(DraggableItemToEditViewModel), typeof(UserStoriesViewModel), new PropertyMetadata(null));

        public RelayCommand AddStoryCommand
        {
            get { return new RelayCommand(AddStory); }
        }
        public RelayCommand ResetStoriesCommand
        {
            get { return new RelayCommand(ResetStories); }
        }
        public RelayCommand LoadStoriesCommand
        {
            get { return new RelayCommand(LoadStories); }
        }
        public RelayCommand SaveStoriesCommand
        {
            get { return new RelayCommand(SaveStories); }
        }
        public RelayCommand ShrinkCommand
        {
            get { return new RelayCommand(Shrink); }
        }
        public RelayCommand HighlightStopDevCommand
        {
            get { return new RelayCommand(HighlightStopDev); }
        }
        public RelayCommand ResetHighlightCommand
        {
            get { return new RelayCommand(ResetHighlight); }
        }

        public double CanvasWidth
        {
            get { return (double)GetValue(CanvasWidthProperty); }
            set { SetValue(CanvasWidthProperty, value); }
        }
        public double CanvasHeight
        {
            get { return (double)GetValue(CanvasHeightProperty); }
            set { SetValue(CanvasHeightProperty, value); }
        }
        public double GridHeight
        {
            get { return (double)GetValue(GridHeightProperty); }
            set { SetValue(GridHeightProperty, value); }
        }
        public double GridWidth
        {
            get { return (double)GetValue(GridWidthProperty); }
            set { SetValue(GridWidthProperty, value); }
        }
        public BoardLayout BoardLayout
        {
            get { return (BoardLayout)GetValue(BoardLayoutProperty); }
            set { SetValue(BoardLayoutProperty, value); }
        }
        public UserStoriesMatrix StoriesMatrix { get; set; }
        public ObservableCollection<IDraggableItem> Stories
        {
            get { return (ObservableCollection<IDraggableItem>)GetValue(StoriesProperty); }
            set { SetValue(StoriesProperty, value); }
        }
        public DraggableItemToEditViewModel DraggableItemToEdit
        {
            get { return (DraggableItemToEditViewModel)GetValue(DraggableItemToEditProperty); }
            set { SetValue(DraggableItemToEditProperty, value); }
        }

        private double FirstXPos { get; set; }
        private double FirstYPos { get; set; }
        private bool UserStoryGrabbed { get; set; }
        private int InitialStoryLeft { get; set; }
        private int InitialStoryTop { get; set; }
        private UserStoryCreator UserStoryCreator { get; set; }

        #endregion

        public UserStoriesViewModel()
        {
            UserStoryCreator = new UserStoryCreator();
            DraggableItemToEdit = new DraggableItemToEditViewModel();
        }

        #region Button Commands

        private void AddStory()
        {
            AddStory(BoardLayout.Columns[0].Status);
        }

        private void ResetStories()
        {
            StoriesMatrix = new UserStoriesMatrix(BoardLayout);
            Stories = new ObservableCollection<IDraggableItem>();

            Random rand = new Random();
            int count = BoardLayout.Columns.Count;
            for (int i = 0; i < 24; i++)
            {
                AddStory(BoardLayout.Columns[rand.Next() % count].Status);
            }
        }

        private void LoadStories()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string xml = File.ReadAllText(System.IO.Path.Combine(path, "L'appli à gg", "persisted.xml"));

            XmlSerializer serializer = new XmlSerializer(Stories.GetType());

            Stories.Clear();
            Stories.AddRange(serializer.Deserialize<ObservableCollection<UserStoryViewModel>>(xml));
        }

        private void SaveStories()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            XmlSerializer serializer = new XmlSerializer(Stories.GetType());
            string xml = serializer.Serialize(Stories);

            File.WriteAllText(System.IO.Path.Combine(path, "L'appli à gg", "persisted.xml"), xml);
        }

        private void Shrink()
        {
            var moves = new List<StoryMove>();

            foreach (var items in StoriesMatrix)
            {
                int offset = 0;
                for (int i = 0; i < items.Value.Length; i++)
                {
                    if (items.Value[i] == null)
                        offset++;
                    else if (offset > 0)
                        moves.Add(new StoryMove() { SourceIndex = i, Status = items.Key, Story = items.Value[i], TargetIndex = i - offset });
                }
            }

            if (moves.Count > 0)
                MoveStories(moves);
        }

        private void HighlightStopDev()
        {
            foreach (var story in Stories)
            {
                if (story is UserStoryViewModel && (story as UserStoryViewModel).Story.IsStopDev)
                    story.HightlightStatus = HightlightStatus.Hightlighted;
                else
                    story.HightlightStatus = HightlightStatus.Hidden;
            }
        }

        private void ResetHighlight()
        {
            foreach (var story in Stories)
            {
                story.HightlightStatus = HightlightStatus.Hightlighted;
            }
        }

        #endregion

        public void InitializeStories()
        {
            BoardLayout layout = new BoardLayout();
            layout.RowsCount = 15;
            layout.Columns.Add(new BoardColumnDescription() { Width = 4, BackgroundColor = Colors.LightBlue, Status = "Todo", Header = "TODO", DefinitionOfDone = new List<string>() { "US Created", "Priority assigned" } });
            layout.Columns.Add(new BoardColumnDescription() { Width = 3, BackgroundColor = Colors.LightCoral, Status = "Development", Header = "DEVELOPMENT", DefinitionOfDone = new List<string>() { "Automation Tests Done", "Code Reviewed", "Deployment Done In Homol" } });
            layout.Columns.Add(new BoardColumnDescription() { Width = 4, BackgroundColor = Colors.LightSteelBlue, Status = "Test", Header = "TEST", DefinitionOfDone = new List<string>() { "Tests Completed", "UAT performed" } });
            layout.Columns.Add(new BoardColumnDescription() { Width = 4, BackgroundColor = Colors.LightSalmon, Status = "Uat", Header = "UAT", DefinitionOfDone = new List<string>() { "Demonstrated to users" } });
            layout.Columns.Add(new BoardColumnDescription() { Width = 4, BackgroundColor = Colors.LightGreen, Status = "Ready", Header = "READY", DefinitionOfDone = new List<string>() });

            BoardLayout = layout;

            GridWidth = layout.ColumnsCount * 250;
            GridHeight = layout.RowsCount * 250;

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() => ResetStories()));
        }

        public void AddExistingStory(IDraggableItem story, string status, int index)
        {
            story.Width = GetHorizontalStepping();
            story.Height = GetVerticalStepping();

            Position indexes = GetPosition(status, index);
            story.Top = indexes.Top;
            story.Left = indexes.Left;
            story.Index = index;
            story.Status = status;

            story.Width = GetHorizontalStepping();
            story.Height = GetVerticalStepping();

            StoriesMatrix[story.Status][story.Index] = story;

            story.IsReadOnly = true;

            Stories.Add(story);
        }

        public Position GetPosition(string status, int index)
        {
            int startColumnIndex = 0;
            foreach (var column in BoardLayout.Columns)
            {
                if (column.Status == status)
                    break;

                startColumnIndex += column.Width;
            }

            int rowIndex = index / BoardLayout[status].Width;
            int columnIndex = startColumnIndex + MathExtensions.Remainder(index, BoardLayout[status].Width);
            return new Position(status, index, rowIndex * GetVerticalStepping(), columnIndex * GetHorizontalStepping(), IsPositionInGrid(rowIndex, columnIndex));
        }

        public Position GetPosition(int topPosition, int leftPosition)
        {
            int remainderLeft = 0;
            int remainderVertical = 0;

            int rowIndex = topPosition < 0 ? -1 : Math.DivRem(topPosition, GetVerticalStepping(), out remainderVertical);
            int columnIndex = leftPosition < 0 ? -1 : Math.DivRem(leftPosition, GetHorizontalStepping(), out remainderLeft);

            int startColumnIndex = 0;
            BoardColumnDescription statusColumn = null;
            foreach (var column in BoardLayout.Columns)
            {
                statusColumn = column;

                if (startColumnIndex + column.Width > columnIndex)
                    break;

                startColumnIndex += column.Width;
            }

            return new Position(statusColumn.Status, rowIndex * statusColumn.Width + columnIndex - startColumnIndex, rowIndex * GetVerticalStepping(), columnIndex * GetHorizontalStepping(), IsPositionInGrid(rowIndex, columnIndex));
        }

        public void MoveObject(IDraggableItem target, int sourceTop, int targetTop, int sourceLeft, int targetLeft)
        {
            Storyboard storyboard = new Storyboard();

            Int32Animation dbVertical = new Int32Animation();
            dbVertical.From = sourceTop;
            dbVertical.To = targetTop;
            dbVertical.Duration = new Duration(TimeSpan.FromSeconds(.25));

            Int32Animation dbHorizontal = new Int32Animation();
            dbHorizontal.From = sourceLeft;
            dbHorizontal.To = targetLeft;
            dbHorizontal.Duration = new Duration(TimeSpan.FromSeconds(.25));

            storyboard.Children.Add(dbHorizontal);
            Storyboard.SetTarget(dbHorizontal, target as DependencyObject);
            Storyboard.SetTargetProperty(dbHorizontal, new PropertyPath(DraggableItemViewModel.LeftProperty));

            storyboard.Children.Add(dbVertical);
            Storyboard.SetTarget(dbVertical, target as DependencyObject);
            Storyboard.SetTargetProperty(dbVertical, new PropertyPath(DraggableItemViewModel.TopProperty));

            storyboard.Begin();
        }

        public List<StoryMove> IdentifyBlockingStories(string status, int index)
        {
            return IdentifyBlockingStories(status, index, StoriesMatrix[status]);
        }

        public List<StoryMove> IdentifyBlockingStories(string status, int index, IDraggableItem[] array)
        {
            List<StoryMove> moves = new List<StoryMove>();

            int currentIndex = index;
            while (true)
            {
                if (currentIndex >= array.Length)
                    return null;
                if (array[currentIndex] == null)
                    break;

                Position currentPosition = GetPosition(status, currentIndex);
                Position targetPosition = GetPosition(status, currentIndex + 1);
                moves.Add(new StoryMove()
                {
                    Story = array[currentIndex],
                    Status = status,
                    SourceIndex = currentIndex,
                    TargetIndex = currentIndex + 1
                });

                currentIndex++;
            }

            return moves;
        }

        public void MoveStories(List<StoryMove> moves)
        {
            foreach (StoryMove move in moves)
            {
                Position sourcePosition = GetPosition(move.Status, move.SourceIndex);
                Position targetPosition = GetPosition(move.Status, move.TargetIndex);

                if (object.ReferenceEquals(StoriesMatrix[move.Status][move.SourceIndex], move.Story))
                    StoriesMatrix[move.Status][move.SourceIndex] = null;

                StoriesMatrix[move.Status][move.TargetIndex] = move.Story;

                move.Story.Status = move.Status;
                move.Story.Index = move.TargetIndex;

                MoveObject(move.Story, sourcePosition.Top, targetPosition.Top, sourcePosition.Left, targetPosition.Left);
            }
        }

        public void ShowEditor(DraggableItemViewModel itemToEdit)
        {
            DraggableItemToEdit.DraggableItem = itemToEdit;
            DraggableItemToEdit.Visibility = Visibility.Visible;
        }

        #region Internal Methods

        private int GetHorizontalStepping()
        {
            return Convert.ToInt32(CanvasWidth / BoardLayout.ColumnsCount);
        }

        private int GetVerticalStepping()
        {
            return Convert.ToInt32(CanvasHeight / BoardLayout.RowsCount);
        }

        private bool IsPositionInGrid(int rowIndex, int columnIndex)
        {
            return !(rowIndex < 0 || rowIndex > BoardLayout.RowsCount - 1 || columnIndex < 0 || columnIndex > BoardLayout.ColumnsCount - 1);
        }

        private void AddStory(string status)
        {
            int index = GetNextAvailableIndex(status);
            if (index == -1)
                return;

            IDraggableItem item = InitializeStory(status, index);
            StoriesMatrix[status][index] = item;

            Stories.Add(item);
        }

        private IDraggableItem InitializeStory(string status, int index)
        {
            IDraggableItem item = new UserStoryViewModel(this, UserStoryCreator.GetNewUserStory());

            Position indexes = GetPosition(status, index);
            item.Status = status;
            item.Index = index;
            item.Top = indexes.Top;
            item.Left = indexes.Left;
            item.Width = GetHorizontalStepping();
            item.Height = GetVerticalStepping();
            item.RotateAngleFactor = 1 - UserStoryCreator.Rand.NextDouble() * 3;
            item.IsReadOnly = true;

            SettingsViewModel settings = Application.Current.GetResource<SettingsViewModel>("Settings");
            Binding cornerRadiusBinding = new Binding(SettingsViewModel.CornerRadiusProperty.Name);
            cornerRadiusBinding.Source = settings;
            cornerRadiusBinding.Mode = BindingMode.OneWay;

            BindingOperations.SetBinding(item as DependencyObject, DraggableItemViewModel.CornerRadiusProperty, cornerRadiusBinding);

            Binding rotateFactorBinding = new Binding(SettingsViewModel.RotateAngleFactorProperty.Name);
            rotateFactorBinding.Source = settings;
            rotateFactorBinding.Mode = BindingMode.OneWay;

            BindingOperations.SetBinding(item as DependencyObject, DraggableItemViewModel.RotateAngleRangeProperty, rotateFactorBinding);

            return item;
        }

        private int GetNextAvailableIndex(string status)
        {
            int index = 0;
            IDraggableItem[] array = StoriesMatrix[status];
            while (true)
            {
                if (index > array.Length - 1)
                    return -1;
                if (array[index] == null)
                    break;

                index++;
            }

            return index;
        }

        #endregion
    }
}
