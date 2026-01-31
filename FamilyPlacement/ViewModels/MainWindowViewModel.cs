using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FamilyPlacement.Abstractions;
using FamilyPlacement.Models;
using System.Collections.ObjectModel;

namespace FamilyPlacement.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly IPlacementService _placementService;
        private TreeType _selectedTreeType;
        private int _count;
        private string _statusMessage;

        public MainWindowViewModel(IPlacementService placementService) {
            PlaceCommand = new RelayCommand(PlaceTree);

            TreeTypes = new ObservableCollection<TreeType>
            {
                TreeType.Oak,
                TreeType.Pine,
                TreeType.Birch,
            };

            _placementService = placementService;
        }               

        public ObservableCollection<TreeType> TreeTypes { get; }

        public TreeType SelectedTreeType
        {
            get => _selectedTreeType;
            set => SetProperty(ref _selectedTreeType, value);
        }
        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public RelayCommand PlaceCommand { get; }

        private void PlaceTrees()
        {
            CSharpFunctionalExtensions.Result result = _placementService.Place(SelectedTreeType, Count);
            if (result.IsSuccess)
            {
                var (rows, columns) = _placementService.CalculateGridDimensions(Count);
                StatusMessage = $"Размещено {Count} экземпляров деревьев";
                TaskDialog.Show("Размещение мебели", StatusMessage);
            }
            else
            {
                StatusMessage = $"Ошибка {result.Error} экземпляров деревьев";
                TaskDialog.Show("Размещение мебели", result.Error);
            }
        }
    }
}
