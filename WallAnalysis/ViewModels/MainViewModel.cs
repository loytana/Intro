using Autodesk.Revit.UI;
using System;
using System.ComponentModel;
using System.Windows.Input;
using WallAnalysis.Abstractions;
using WallAnalysis.Models;

namespace WallAnalysis.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IRevitService _revitService;
        private readonly UIDocument _uiDocument;

        private WallData _wallData;
        public WallData WallData
        {
            get => _wallData;
            set
            {
                _wallData = value;
                OnPropertyChanged(nameof(WallData));
                OnPropertyChanged(nameof(HasWallData));
            }
        }

        private ValidationResult _validationResult;
        public ValidationResult ValidationResult
        {
            get => _validationResult;
            set
            {
                _validationResult = value;
                OnPropertyChanged(nameof(ValidationResult));
            }
        }

        public bool HasWallData => WallData != null;

        public ICommand SelectWallCommand { get; }
        public ICommand CloseCommand { get; set; }

        public MainViewModel(IRevitService revitService, UIDocument uiDocument)
        {
            _revitService = revitService;
            _uiDocument = uiDocument;

            SelectWallCommand = new RelayCommand(ExecuteSelectWall);
            CloseCommand = new RelayCommand(ExecuteClose);
        }

        private void ExecuteSelectWall()
        {
            try
            {
                var wall = _revitService.GetSelectedWall(_uiDocument);
                if (wall != null)
                {
                    WallData = _revitService.GetWallData(wall);
                    ValidationResult = _revitService.ValidateWallThickness(WallData);
                }
            }
            catch (Exception ex)
            {
                ValidationResult = new ValidationResult
                {
                    IsValid = false,
                    Message = $"Ошибка: {ex.Message}",
                    Status = ValidationStatus.Error
                };
            }
        }

        private void ExecuteClose()
        {
            // Закрытие окна будет обработано в View
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
