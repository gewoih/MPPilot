﻿using System.Security.Claims;

namespace MPPilot.Domain.Services.Token
{
    public interface ITokenService
    {
        public string GetToken();
        public string GenerateToken(ClaimsIdentity identity);
        public bool ValidateToken(string token);
    }
}
