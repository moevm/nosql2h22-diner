namespace UtilsLib.Excel; 

public class ExcelLabel: Attribute {
    public string Name { get; }

    public ExcelLabel(string name) {  
        Name = name;
    }  
}  
    
    
