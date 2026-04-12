using FCG.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Tests.ValueObjects
{
    public class EmailTest
    {
        [Fact]
        public void EmailValue_Valid()
        {

            var email = new Email("teste@email.com");
            Assert.Equal("teste@email.com", email.Value);
        }

        [Fact]
        public void EmailValue_Required()
        {

            Assert.Throws<ApplicationException>(() => new Email(""));
        }

        [Fact]
        public void EmailValue_InvalidFormat()
        {

            Assert.Throws<ApplicationException>(() => new Email("teste@asas"));
        }
    }
}
