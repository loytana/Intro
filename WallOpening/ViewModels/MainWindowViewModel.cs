using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WallOpening.Abstractions;
using WallOpening.Models;

namespace WallOpening.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly ISelectionService _selectionService;
        private readonly IGeometryService _geometryService;

        public MainWindowViewModel(ISelectionService selectionService, IGeometryService geometryService)
        {
            CalcOpening = new RelayCommand(OnCalsOpenongExecute);
            _selectionService = selectionService;
            _geometryService = geometryService;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string propertyName=null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private OpeningInfo _openingInfo;
        public OpeningInfo OpeningInfo
        {
            get => _openingInfo; 
            set
            {
                _openingInfo = value;
                OnPropertyChanged();
            }
        }
        private string _statusMessage = "Выберите проём";
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }
        private double _limit = 1000;

        public double Limit
        {
            get => _limit;
            set
            {
                _limit = value;
                OnPropertyChanged();
            }
        }

        public ICommand CalcOpening {  get; }
        private void OnCalsOpenongExecute(object parameter)
        {
            FamilyInstance opening = _selectionService.PickOpening();
            if (opening != null)
            {
                return;
            }

            GeometryService geometryService = new GeometryService();
            OpeningInfo = geometryService.GetOpeningInfo(opening, Limit);

            StatusMessage = $"Обработан {OpeningInfo.Name}";
        }

        
    }
}
