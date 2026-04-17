using CommonTestUtilities.Entities;
using FCG.Domain.Entities;
using FCG.Tests.Fixture;

namespace FCG.Tests.Entities;

[Collection(nameof(UserFixtureCollection))]
public class UserTest
{
    public UserBuilder _userFixture;

    public UserTest(UserBuilder userFixture)
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
