using CommonTestUtilities.Entities;
using CommonTestUtilities.Services;
using FCG.Api.Controllers;
using FCG.Domain.Contants;
using FCG.Tests.Fixture;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FCG.Tests.Controllers
{
    [Collection(nameof(UserFixtureCollection))]
    public class UserControllerTest
    {
        public UserBuilder _userFixture;

        public UserControllerTest(UserBuilder userFixture)
        {
            _userFixture = userFixture;
        }
        private AuthorizeAttribute? GetAuthorizeAttributeFromRoute(string methodControllerName)
        {
            var createUserMethod = typeof(UserController).GetMethod(methodControllerName);

            var authorizateAttribute = createUserMethod.GetCustomAttribute<AuthorizeAttribute>();
            return authorizateAttribute;
        }
        [Fact]
        public async Task UserControllerCreateUser_NotShrould_AuthorizationAttribute()
        {
            var authorizateAttribute = this.GetAuthorizeAttributeFromRoute(nameof(UserController.CreateUser));

            Assert.Null(authorizateAttribute);

        }
        [Fact]
        public async Task UserControllerUpdateUser_Shrould_AuthorizationAttributeWithAnyRole()
        {
            var authorizateAttribute = this.GetAuthorizeAttributeFromRoute(nameof(UserController.UpdateUser));

            Assert.Equal(FCGConstant.AdminOrDefault, authorizateAttribute.Policy);

        }

        [Fact]
        public async Task UserControllerDeleteUser_Shrould_AuthorizationAttributeWithAdmin()
        {
            var authorizateAttribute = this.GetAuthorizeAttributeFromRoute(nameof(UserController.DeleteUser));

            Assert.Equal(FCGConstant.AdminRole, authorizateAttribute.Policy);

        }

        [Fact]
        public async Task UserControllerDeleteUser_NotShrould_AuthorizationAttributeWithDefault()
        {
            var authorizateAttribute = this.GetAuthorizeAttributeFromRoute(nameof(UserController.DeleteUser));

            Assert.NotEqual(FCGConstant.UserDefault, authorizateAttribute.Policy);

        }

        [Fact]
        public async Task UserControllerCreateAdmin_Shrould_AuthorizationAttributeWithAdmin()
        {
            var authorizateAttribute = this.GetAuthorizeAttributeFromRoute(nameof(UserController.CreateAdmin));

            Assert.Equal(FCGConstant.AdminRole, authorizateAttribute.Policy);

        }


        [Fact]
        public async Task UserControllerCreateAdmin_NotShrould_AuthorizationAttributeWithDefault()
        {
            var authorizateAttribute = this.GetAuthorizeAttributeFromRoute(nameof(UserController.CreateAdmin));

            Assert.NotEqual(FCGConstant.UserDefault, authorizateAttribute.Policy);

        }

        [Fact]
        public async Task UserControllerChangePassword_Shrould_AuthorizationAttributeWithAnyRole()
        {
            var authorizateAttribute = this.GetAuthorizeAttributeFromRoute(nameof(UserController.ChangePassword));

            Assert.Equal(FCGConstant.AdminOrDefault, authorizateAttribute.Policy);

        }

    }
}
