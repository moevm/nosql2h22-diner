using DomainLib.Models;
using UtilsLib.Excel;

namespace ServicesLib.Services.Outputs;

public class ExcelResourcesOutput
{
    public ExcelResourcesOutput() {}

    public ExcelResourcesOutput(Resource resource)
    {
        Name = resource.Name;
        Amount = resource.Amount;
    }
    
    [Order(1)]
    [ExcelLabel("Resource's name")]
    public string Name { get; set; }
    
    [Order(2)]
    [ExcelLabel("Amount of resource")]
    public int Amount { get; set; }
}