using System.Text;
using DomainLib.DTO;
using DomainLib.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class UserService: BaseModelService<User>
{
    private readonly ShiftService _shiftService;
    private readonly AuthInfoService _authInfoService;
    
    public UserService(IOptions<DbConfig> dbConfig, WeekService weekService, ShiftService shiftService, AuthInfoService authInfoService) : base(dbConfig)
    {
        this._shiftService = shiftService;
        this._authInfoService = authInfoService;
    }

    public async Task<User> CreateUserWithDefaults(UserDto userDto)
    {
        var filter = Builders<User>.Filter.Where(x => x.Login == userDto.Login);
        if (await this.WhereOneAsync(filter) != null) throw new BadHttpRequestException("User is already exists", 409);
        var newUser = new User
        {
            FullName = userDto.FullName,
            Login = userDto.Login,
            Role = userDto.Role,
        };
        await this.CreateAsync(newUser);
        await this._authInfoService.CreateAsync(new AuthInfo
            { UserId = newUser.Id, PasswordHash = this._authInfoService.HashWithSalt(userDto.Password) });
        var shift = await this._shiftService.CreateDefaultShift(newUser.Id);
        newUser.ShiftId = shift.Id;
        await this.UpdateAsync(newUser.Id, newUser);
        return newUser;
    }

    public async Task<Boolean> AuthenticateUser(string login, string password)
    {
        var userFilter = Builders<User>.Filter.Where(x => x.Login == login);
        var user = this.WhereOneAsync(userFilter);
        var filter = Builders<AuthInfo>.Filter.Where(x => x.PasswordHash == this._authInfoService.HashWithSalt(password));
        return await this._authInfoService.WhereOneAsync(filter) != null;
    }
}