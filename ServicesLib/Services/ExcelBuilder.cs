using System.Reflection;
using ClosedXML.Excel;
using UtilsLib.Excel;

namespace ServicesLib.Services;

///
/// <summary>
/// Класс <c>Column</c> для формирования колонки Excel
/// </summary>
///
public class Column {
    /// <summary>
    /// Имя колонки
    /// </summary>
    public string ColumnName { get; set; } = null!;

    /// <summary>
    /// Тип данных колонки
    /// </summary>
    public string DataType { get; set; } = null!;

    /// <summary>
    /// Подпись для экселя
    /// </summary>
    public string Label { get; set; } = null!;

    /// <summary>
    /// Порядок колонки
    /// </summary>
    public int Order { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public double? ColumnWidth { get; set; }
}

///
/// <summary>
/// Класс <c>Sheet</c> для формирования листа книги Excel
/// </summary>
///
public class Sheet {
    /// <summary>
    /// Список колонок в листе книги эксель
    /// </summary>
    public IList<Column> Columns { get; set; } = null!;

    /// <summary>
    /// Наименование листа
    /// </summary>
    public string SheetName { get; set; } = null!;

    /// <summary>
    /// Данные
    /// </summary>
    public Dictionary<string, List<object>> Data { get; set; } = null!;
}

/// <summary>
/// Билдер для экселя
/// </summary>
public class ExcelBuilder {
    private XLWorkbook _workbook;

    /// <summary>
    /// его конструктор
    /// </summary>
    public ExcelBuilder() {
        _workbook = new XLWorkbook();
    }

    ///
    /// <summary> Метод для вывода книги Excel в формате Stream </summary>
    ///<returns> Книгу Excel в формате Stream </returns>
    ///
    public Stream AsStream() {
        Stream fs = new MemoryStream();
        _workbook.SaveAs(fs);
        fs.Position = 0;
        return fs;
    }

    ///
    /// <summary> Метод для добавления рабочего листа в книгу Excel </summary>
    /// <param name="sheet"> Данные для постановки типа IQueryable</param>
    ///
    public ExcelBuilder AddSheet(Sheet sheet) {
        var workSheet = _workbook.Worksheets.Add(sheet.SheetName);
        workSheet.Name = sheet.SheetName;
        workSheet.Rows().AdjustToContents();
        for (int i = 0; i < sheet.Columns.Count; i++) {
            var columnValue = sheet.Columns[i].ColumnName;
            List<object>? values;
            workSheet.Cell(1, i + 1).Value = (sheet.Columns[i].Label);

            if (sheet.Data.TryGetValue(columnValue, out values)) {
                var dataType = XLDataType.Text;
                switch (sheet.Columns[i].DataType) {
                    case "String":
                        dataType = XLDataType.Text;
                        break;
                    case "Number":
                        dataType = XLDataType.Number;
                        break;
                    case "Boolean":
                        dataType = XLDataType.Boolean;
                        break;
                    case "DateTime":
                        dataType = XLDataType.DateTime;
                        break;
                    case "Text":
                        dataType = XLDataType.Text;
                        break;
                }

                for (int j = 0; j < values.Count; ++j) {
                    workSheet.Cell(j + 2, i + 1).SetDataType(dataType);
                    workSheet.Cell(j + 2, i + 1).SetValue((values[j]));
                }
                
                if (sheet.Columns[i].ColumnWidth is not null) {
                    workSheet.Column(i + 1).Width = sheet.Columns[i].ColumnWidth!.Value;
                    workSheet.Column(i + 1).Style.Alignment.SetWrapText(true);
                } else {
                    workSheet.Column(i + 1).AdjustToContents();
                }
            }
        }
        return this;
    }

    ///
    /// <summary> Метод для приведения данных к формату ExcelSheet </summary>
    /// <param name="excelData"> Данные для сериализации типа IQueryable</param>
    /// <returns> Данные в формате Sheet </returns>
    ///
    public static Sheet FormatDataToExcel<T>(IQueryable<T> excelData) where T : class {
        var type = typeof(T);

        var sheet = new Sheet() {
            SheetName = type.Name,
            Columns = new List<Column>(),
            Data = new Dictionary<string, List<object>>()
        };
        var properties = type.GetProperties();
        var counter = 1;
        foreach (var propertyInfo in properties) {
            var label = GetColumnLabel(propertyInfo);
            var col = new Column() {
                ColumnName = propertyInfo.Name,
                DataType = propertyInfo.PropertyType.Name,
                Label = label,
                Order = GetColumnOrder(propertyInfo, counter),
                ColumnWidth = GetColumnWidth(propertyInfo)
            };
            sheet.Columns.Add(col);
            counter++;
            var val = new List<object>();
            foreach (var data in excelData) {
                if (data.GetType().GetProperty(propertyInfo.Name) == null) continue;
                var prop = data.GetType().GetProperty(propertyInfo.Name);
                object? dataValue = prop?.GetValue(data, null);
                if (dataValue != null)
                    val.Add(dataValue);
            }

            sheet.Data.Add(propertyInfo.Name, val);
        }

        sheet.Columns = sheet.Columns.OrderBy(x => x.Order).ToList();
        
        return sheet;
    }

    private static string GetColumnLabel(PropertyInfo propertyInfo) {
        var attributes = propertyInfo.GetCustomAttributes(true);

        var label = "";
        foreach (var attribute in attributes) {
            if (attribute.GetType() == typeof(ExcelLabel)) {
                label = ((ExcelLabel) attribute).Name;
            }
        }

        if (label.Length == 0) {
            label = propertyInfo.Name;
        }

        return label;
    }

    private static int GetColumnOrder(PropertyInfo propertyInfo, int counter) {
        if (propertyInfo.GetCustomAttributes(typeof(OrderAttribute)).Any()&& 
            propertyInfo.GetCustomAttributes(typeof(OrderAttribute)).First() is OrderAttribute order) {
            return order.Order;
        }

        return counter;
    }
    
    private static Nullable<double> GetColumnWidth(PropertyInfo propertyInfo) {
        var attributes = propertyInfo.GetCustomAttributes(true);

        Nullable<double> width = null;
        foreach (var attribute in attributes) {
            if (attribute.GetType() == typeof(ExcelColumnWidthAttribute)) {
                width = ((ExcelColumnWidthAttribute) attribute).Width;
            }
        }
        
        return width;
    }

    ///
    /// <summary> Метод для приведения данных к формату ExcelSheet </summary>
    /// <param name="excelData"> Данные для сериализации типа IEnumerable</param>
    /// <returns> Данные в формате Sheet </returns>
    ///
    public static Sheet FormatDataToExcel<T>(IEnumerable<T> excelData) where T : class {
        var type = typeof(T);
        var sheet = new Sheet() {
            SheetName = type.Name,
            Columns = new List<Column>(),
            Data = new Dictionary<string, List<object>>()
        };

        var enumerable = excelData as T[] ?? excelData.ToArray();
        if (enumerable.Any()) {
            var element = enumerable.First();
            var propertyInfos = element.GetType().GetProperties();
            var count = 1;
            foreach (var property in propertyInfos) {
                var label = GetColumnLabel(property);
                var col = new Column() {
                    ColumnName = property.Name,
                    DataType = property.PropertyType.Name,
                    Label = label,
                    Order = GetColumnOrder(property, count),
                    ColumnWidth = GetColumnWidth(property)
                };
                count++;
                sheet.Columns.Add(col);
                var data = enumerable.Select(x => x.GetType().GetProperty(property.Name)?.GetValue(x, null)).ToList();
                if (data.Count > 0)
                    sheet.Data.Add(property.Name, data!);
            }

            sheet.Columns = sheet.Columns.OrderBy(x => x.Order).ToList();
        }

        return sheet;
    }


}