using DomainLib.Models;
using UtilsLib.Excel;

namespace ServicesLib.Services.Outputs; 

public class ExcelDishesOutput {
    
    public ExcelDishesOutput() {}

    public ExcelDishesOutput(Dish dish)
    {
        Name = dish.Name;
        Price = dish.Price;
        Description = dish.Description;
    }
    
    [Order(1)]
    [ExcelLabel("Dish's name")]
    public string Name { get; set; }
    
    [Order(2)]
    [ExcelLabel("Price of dish")]
    public int Price { get; set; }
    
    [Order(3)]
    [ExcelLabel("Description of dish")]
    public string Description { get; set; }
}