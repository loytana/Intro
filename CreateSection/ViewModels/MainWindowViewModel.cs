using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CreateSection.Abstractions;
using System.Threading.Tasks;

namespace CreateSection.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly ISelectionService _selectionService;
        private readonly ISectionService _sectionService;
        private readonly RevitTask _revitTask;
        private string _sectionName = "Разрез";
        private double _widthOffsetMm = 100;
        private double _depthOffsetMm = 100;
        private double _heightOffsetMm = 100;

        public MainWindowViewModel(
            ISelectionService selectionService,
            ISectionService sectionService,
            RevitTask revitTask
            )
        {
            CreateSectionCommand = new AsyncRelayCommand(CreateThreeSectionsAsync);
            _selectionService = selectionService;
            _sectionService = sectionService;
            _revitTask = revitTask;
        }

        public string SectionName
        {
            get => _sectionName;
            set => SetProperty(ref _sectionName, value);
        }
        public double WidthOffsetMm
        {
            get => _widthOffsetMm;
            set => SetProperty(ref _widthOffsetMm, value);
        }
        public double DepthOffsetMm
        {
            get => _depthOffsetMm;
            set => SetProperty(ref _depthOffsetMm, value);
        }
        public double HeightOffsetMm
        {
            get => _heightOffsetMm;
            set => SetProperty(ref _heightOffsetMm, value);
        }

        public AsyncRelayCommand CreateSectionCommand { get; }
        public ISectionService SectionService { get; }

        private async Task CreateThreeSectionsAsync()
        {
            // Выбираем элемент
            FamilyInstance familyInstance = _selectionService.PickFamilyInstance();

            // Проверяем выбран ли элемент (исправляем условие)
            if (familyInstance == null)  // ← ИСПРАВИЛИ: если НЕ выбран
            {
                return;
            }

            // Создаем 3 разреза
            bool isCreated = await _revitTask.Run<bool>(app =>
                _sectionService.CreateThreeSections(
                    familyInstance,
                    WidthOffsetMm,
                    DepthOffsetMm,
                    HeightOffsetMm,
                    SectionName
                ));

            // Показываем результат
            if (isCreated)
            {
                TaskDialog.Show("Успех",
                    $"Создано 3 разреза:\n" +
                    $"1. {SectionName}_План (горизонтальный)\n" +
                    $"2. {SectionName}_Фронт (фронтальный)\n" +
                    $"3. {SectionName}_Профиль (профильный)");
            }
            else
            {
                TaskDialog.Show("Ошибка", "Не удалось создать один или несколько разрезов");
            }
        }

    }
}
