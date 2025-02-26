
using BasicDotnetTemplate.MainProject.Models.Api.Data.Auth;
using BasicDotnetTemplate.MainProject.Models.Api.Common.User;
using BasicDotnetTemplate.MainProject.Utils;

namespace BasicDotnetTemplate.MainProject.Services;

public interface IAuthService
{
    Task<AuthenticatedUser?> AuthenticateAsync(AuthenticateRequestData data);
}

public class AuthService : BaseService, IAuthService
{
    protected CryptUtils _cryptUtils;

    public AuthService(
        IConfiguration configuration
    ) : base(configuration)
    {
        _cryptUtils = new CryptUtils(_appSettings);
    }

    public async Task<AuthenticatedUser?> AuthenticateAsync(AuthenticateRequestData data)
    {
        AuthenticatedUser? authenticatedUser = null;
        var decryptedUsername = _cryptUtils.Decrypt(data.Username ?? String.Empty);
        var decryptedPassword = _cryptUtils.Decrypt(data.Password ?? String.Empty);

        if (!String.IsNullOrEmpty(decryptedUsername) && !String.IsNullOrEmpty(decryptedPassword))
        {

        }

        return authenticatedUser;
    }
}

