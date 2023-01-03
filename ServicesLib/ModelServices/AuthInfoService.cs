using System.Text;
using DomainLib.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using UtilsLib.Configurations;

namespace ServicesLib.ModelServices;

public class AuthInfoService: BaseModelService<AuthInfo>
{
    public AuthInfoService(IOptions<DbConfig> dbConfig) : base(dbConfig)
    { }
    
    public string HashWithSalt(string tmp)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(password: tmp,
            salt: Encoding.UTF8.GetBytes("A"),
            prf: KeyDerivationPrf.HMACSHA256, iterationCount: 16, numBytesRequested: 256 / 8));
    }
}