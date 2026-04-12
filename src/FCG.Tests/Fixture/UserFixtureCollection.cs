using Bogus;
using Bogus.Extensions.Brazil;
using FCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Tests.Fixture
{
    [CollectionDefinition("UserFixtureCollection")]
    public class UserFixtureCollection : ICollectionFixture<UserFixture>
    {
        
        
    }
}
