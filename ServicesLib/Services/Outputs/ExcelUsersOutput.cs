using DomainLib.Models;
using UtilsLib.Excel;

namespace ServicesLib.Services.Outputs; 

public class ExcelUsersOutput {
    public  ExcelUsersOutput() {}
    
    public ExcelUsersOutput(User user) {
        Login = user.Login;
        Name = user.FullName;
        Role = user.Role;
        Status = user.Status;
    }

    [Order(1)]
    [ExcelLabel("User's login")]
    public string Login { get; set; }
    
    [Order(2)]
    [ExcelLabel("User's name")]
    public string Name { get; set; }
    
    [Order(3)]
    [ExcelLabel("User's role")]
    public UserRole Role { get; set; }
    
    [Order(4)]
    [ExcelLabel("User's Status")]
    public UserStatus Status { get; set; }
}