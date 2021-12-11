using System.Text;
using System.Security.Cryptography;
namespace UserOrchestration.Utilities;

sealed class UserPasswordHashingService : IUserPasswordHashingService
{
    readonly ILogger<UserPasswordHashingService> _logger;

    public UserPasswordHashingService(ILogger<UserPasswordHashingService> logger)
    {
        _logger = logger;
    }

    public string ComputeHash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            _logger.LogTrace("Password is either null or white-space");
            return "";
        }

        _logger.LogTrace("Computing password hash");
        using var algo = SHA512.Create();
        var data = Encoding.UTF8.GetBytes(password);
        var hashData = algo.ComputeHash(data);
        return new StringBuilder(BitConverter.ToString(hashData))
            .Replace("-", "")
            .ToString();
    }
}