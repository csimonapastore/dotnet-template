
using BasicDotnetTemplate.MainProject.Models.Api.Data.Auth;
using BasicDotnetTemplate.MainProject.Models.Api.Common.User;
using BasicDotnetTemplate.MainProject.Core.Database;
using BasicDotnetTemplate.MainProject.Utils;

namespace BasicDotnetTemplate.MainProject.Services;

public interface IAuthService
{
    Task<AuthenticatedUser?> AuthenticateAsync(AuthenticateRequestData data);
}

public class AuthService : BaseService, IAuthService
{
    protected CryptUtils _cryptUtils;
    protected readonly IUserService _userService;

    public AuthService(
        IConfiguration configuration,
        SqlServerContext sqlServerContext,
        IUserService userService
    ) : base(configuration, sqlServerContext)
    {
        _cryptUtils = new CryptUtils(_appSettings);
        _userService = userService;
    }

    public async Task<AuthenticatedUser?> AuthenticateAsync(AuthenticateRequestData data)
    {
        AuthenticatedUser? authenticatedUser = null;
        var decryptedUsername = _cryptUtils.Decrypt(data.Username ?? String.Empty);
        var decryptedPassword = _cryptUtils.Decrypt(data.Password ?? String.Empty);

        if (!String.IsNullOrEmpty(decryptedUsername) && !String.IsNullOrEmpty(decryptedPassword))
        {
            var user = await this._userService.GetUserByUsernameAndPassword(decryptedUsername, decryptedPassword);
            if (user != null)
            {
                authenticatedUser = new AuthenticatedUser(user);
            }
        }

        return authenticatedUser;
    }
}

