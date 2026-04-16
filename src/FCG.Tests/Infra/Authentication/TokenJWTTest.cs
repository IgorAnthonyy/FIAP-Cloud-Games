using FCG.Application.Interfaces;
using FCG.Domain.Contants;
using FCG.Domain.Entities;
using FCG.Infrastructure.Authentication;
using FCG.Tests.Fixture;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FCG.Tests.Infra.Authentication
{
    [Collection(nameof(UserFixtureCollection))]
    public class TokenJWTTest
    {
        public UserFixture _userFixture;
        public TokenJWTTest(UserFixture userFixture)
        {
            _userFixture = userFixture;
        }

        [Fact]
        public async Task TokenService_Should_GeneratedTokenWithoutRole()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "Jwt:Key", "93323a7c6db8b59b1f9f2ac55704fb3701c12be607aeff516051f917b8cd5535994323ae" },
                { "Jwt:Issuer", "meu-issuer" }
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            //Arrange
            User userLogged = _userFixture.GenerateUserWithRoleEmpty();
            
            var tokenService = new TokenService(configuration);
            //Act
            string token = tokenService.GenerateTokenJWT(userLogged);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var containRoleEmpty = !jwt.Claims.Any(c => c.Type == ClaimTypes.Role);
            Assert.True(containRoleEmpty);


        }


        [Fact]
        public async Task TokenService_Should_GeneratedTokenWithRoleAdmin()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "Jwt:Key", "93323a7c6db8b59b1f9f2ac55704fb3701c12be607aeff516051f917b8cd5535994323ae" },
                { "Jwt:Issuer", "meu-issuer" }
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            //Arrange
            User userLogged = _userFixture.GenerateUserWithRoleAdmin();

            var tokenService = new TokenService(configuration);
            //Act
            string token = tokenService.GenerateTokenJWT(userLogged);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var containRoleEmpty = jwt.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == FCGConstant.AdminRole);
            Assert.True(containRoleEmpty);


        }


        [Fact]
        public async Task TokenService_Should_GeneratedTokenWithRoleDefault()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "Jwt:Key", "93323a7c6db8b59b1f9f2ac55704fb3701c12be607aeff516051f917b8cd5535994323ae" },
                { "Jwt:Issuer", "meu-issuer" }
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            //Arrange
            User userLogged = _userFixture.GenerateUserWithRolesDefault();

            var tokenService = new TokenService(configuration);
            //Act
            string token = tokenService.GenerateTokenJWT(userLogged);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var containRoleEmpty = jwt.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == FCGConstant.UserDefault);
            Assert.True(containRoleEmpty);


        }

    }
}
