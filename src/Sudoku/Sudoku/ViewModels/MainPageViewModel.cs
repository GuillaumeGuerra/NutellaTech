using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Sudoku.Business;
using UI.Framework.ViewModel;

namespace Sudoku.ViewModels
{
    public class MainPageViewModel : CommonViewModel
    {
        private GameGrid _gameGrid;

        public object NewGameCommand
        {
            get { return new RelayCommand(NewGame); }
        }

        public MainPageViewModel()
        {
            GameGrid = new GameGrid(9);
        }

        private void NewGame()
        {
            GameGrid.Reinitialize();
        }

        public GameGrid GameGrid
        {
            get { return _gameGrid; }
            set
            {
                _gameGrid = value;
                RaisePropertyChanged();
            }
        }
    }
}
