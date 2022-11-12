using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class UserService : BaseModelService<User> {
    private readonly ShiftService _shiftService;
    private readonly AuthInfoService _authInfoService;

    public UserService(IOptions<DbConfig> dbConfig, WeekService weekService, ShiftService shiftService,
        AuthInfoService authInfoService) : base(dbConfig) {
        _shiftService = shiftService;
        _authInfoService = authInfoService;
    }

    public async Task<User> CreateUserWithDefaults(UserDto userDto) {
        var filter = Builders<User>.Filter.Where(x => x.Login == userDto.Login);
        if (await WhereOneAsync(filter) != null) throw new Exception("User is already exists");
        var newUser = new User {
            FullName = userDto.FullName,
            Login = userDto.Login,
            Role = userDto.Role,
        };
        await CreateAsync(newUser);
        await _authInfoService.CreateAsync(new AuthInfo
            { UserId = newUser.Id, PasswordHash = _authInfoService.HashWithSalt(userDto.Password) });
        var shift = await _shiftService.CreateDefaultShift(newUser.Id);
        newUser.ShiftId = shift.Id;
        await UpdateAsync(newUser.Id, newUser);
        return newUser;
    }

    public async Task<AuthInfo?> AuthenticateUser(string login, string password) {
        var userFilter = Builders<User>.Filter.Where(x => x.Login == login);
        var user = await WhereOneAsync(userFilter);
        
        if (user is null) return null;
        
        var filter =
            Builders<AuthInfo>.Filter.Where(x => x.PasswordHash == _authInfoService.HashWithSalt(password) && x.UserId == user.Id);
        return await _authInfoService.WhereOneAsync(filter);
    }
}