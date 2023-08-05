using Microsoft.AspNetCore.Components.Authorization;
using MPBoom.Domain.Models.Account;
using MPBoom.Domain.Services.Security.Token;
using System.Security.Claims;

namespace MPPilot.App.Services
{
    public class JWTAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ITokenService _tokenService;
        private readonly AccountsService _accountsService;
        private ClaimsPrincipal _user;

        public JWTAuthenticationStateProvider(ITokenService tokenService, AccountsService accountsService)
        {
            _tokenService = tokenService;
            _accountsService = accountsService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _tokenService.GetTokenAsync();
            var isValidToken = await _tokenService.ValidateToken(token);

            if (isValidToken)
            {
                var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, token) }, "Token");
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            else
            {
                var anonymousIdentity = new ClaimsIdentity();
                var anonymousUser = new ClaimsPrincipal(anonymousIdentity);
                return new AuthenticationState(anonymousUser);
            }
        }

        public async Task LogInUser(Account account)
        {
            var userIdentity = await _accountsService.GetIdentityAsync(account);
            _user = new ClaimsPrincipal(userIdentity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));
        }

        public void LogOutUser()
        {
            _user = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));
        }
    }
}
