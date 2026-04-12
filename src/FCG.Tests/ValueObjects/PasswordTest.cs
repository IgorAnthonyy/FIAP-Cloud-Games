using FCG.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Tests.ValueObjects
{
    public class PasswordTest
    {
        [Fact]
        public void PasswordValue_Valid()
        {

            var password = new Password("!@Aedefsd1234");
            Assert.Equal("!@Aedefsd1234", password.Value);
        }

        [Fact]
        public void PasswordValue_InvalidLength()
        {

            Assert.Throws<ApplicationException>(() => new Password("!@Aede1"));
        }

        [Fact]
        public void PasswordValue_InvalidDigits()
        {

            Assert.Throws<ApplicationException>(() => new Password("asdasdasdasd"));
        }
    }
}
