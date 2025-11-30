using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace Task5
{
    [Transaction(TransactionMode.Manual)]
    public class SelectionStatisticsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            Application app = uiApp.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            try
            {
                IList<Reference> selectedReferences;
                try
                {
                    selectedReferences = uiDoc.Selection.PickObjects(
                        ObjectType.Element,
                        new FamilyInstanceSelectionFilter(),
                        "Выберите элементы для анализа статистики");
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                {                    
                    TaskDialog.Show("Отмена", "Выбор элементов отменен пользователем.");
                    return Result.Cancelled;
                }

                List<Element> selectedElements = selectedReferences
                    .Select(reference => doc.GetElement(reference))
                    .Where(e => e != null)
                    .ToList();

                if (selectedElements.Count == 0)
                {
                    TaskDialog.Show("Статистика", "Не выбрано ни одного элемента.");
                    return Result.Succeeded;
                }

                var statistics = CollectStatistics(selectedElements, doc);

                ShowStatisticsReport(statistics, selectedElements.Count);

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = $"Ошибка: {ex.Message}";
                TaskDialog.Show("Ошибка", $"Не удалось выполнить команду: {ex.Message}");
                return Result.Failed;
            }
        }

        private Dictionary<string, int> CollectStatistics(List<Element> elements, Document doc)
        {
            var categoryCount = new Dictionary<string, int>();

            foreach (Element element in elements)
            {
                string categoryName = GetElementCategoryName(element, doc);

                if (categoryCount.ContainsKey(categoryName))
                {
                    categoryCount[categoryName]++;
                }
                else
                {
                    categoryCount[categoryName] = 1;
                }
            }

            return categoryCount;
        }

        private string GetElementCategoryName(Element element, Document doc)
        {
            if (element.Category != null)
            {
                return element.Category.Name;
            }

            ElementType elementType = doc.GetElement(element.GetTypeId()) as ElementType;
            if (elementType != null && elementType.Category != null)
            {
                return elementType.Category.Name;
            }

            return "Без категории";
        }

        private void ShowStatisticsReport(Dictionary<string, int> categoryCount, int totalElements)
        {
            var sortedCategories = categoryCount
                .OrderByDescending(kv => kv.Value)
                .ThenBy(kv => kv.Key);

            // Формируем отчет
            string report = $"ОБЩАЯ СТАТИСТИКА:\n\n";
            report += $"Всего элементов: {totalElements}\n";
            report += $"Уникальных категорий: {categoryCount.Count}\n\n";
            report += "РАСПРЕДЕЛЕНИЕ ПО КАТЕГОРИЯМ:\n";

            foreach (var category in sortedCategories)
            {
                double percentage = (double)category.Value / totalElements * 100;
                report += $"{category.Key} → {category.Value} элементов ({percentage:F1}%)\n";
            }

            TaskDialog.Show("Статистика выбранных элементов", report);
        }
    }
}
