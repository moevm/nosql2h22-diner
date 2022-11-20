namespace UtilsLib.Excel; 

public class ExcelColumnWidthAttribute: Attribute {
    public double Width { get; set; }
    
    public ExcelColumnWidthAttribute(double width) {
        Width = width;
    }
}