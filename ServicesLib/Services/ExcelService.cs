using Microsoft.Extensions.Options;

namespace ServicesLib.Services;

///
/// <summary>
/// Класс <c>ExcelService</c> для работы с Excel
/// </summary>
///
public class ExcelService {

    /// <summary>
    /// 
    /// </summary>
    public ExcelService() {}

    /// <summary>
    /// Метод для создания инстанса билдера
    /// </summary>
    /// <returns></returns>
    public ExcelBuilder CreateNewExcelBuilder() {
        return new ExcelBuilder();
    }
}