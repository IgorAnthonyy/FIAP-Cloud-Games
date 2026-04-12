using FCG.Domain.Entities;
using FCG.Tests.Fixture;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Tests.Entities
{
    [Collection(nameof(UserFixtureCollection))]
    public class UserTest
    {
        public UserFixture _userFixture;

        public UserTest(UserFixture userFixture)
        {
            _userFixture = userFixture;
        }

        [Fact]
        public void UserEntity_Should_AddedRole()
        {
            //Arrange
            var user = _userFixture.GenerateUserWithRoleEmpty();

            //Act
            user.AddRole(new Role
            {
                Name = "Admin",

            });

            Assert.Equal("Admin", user.Roles.ToList()[0].Name);
        }
    }
}
