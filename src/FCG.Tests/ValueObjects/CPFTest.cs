using FCG.Domain.Entities;
using FCG.Domain.ValueObjects;
using FCG.Tests.Fixture;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Tests.ValueObjects
{
    public class CPFTest
    {
        [Fact]
        public void CPFValue_Valid()
        {
            
            var cpf = new CPF("06515537506");
            Assert.Equal("06515537506", cpf.Code);
        }

        [Fact]
        public void CPFValue_InvalidLength()
        {

            Assert.Throws<ApplicationException>(() => new CPF("0651553750"));
        }

        [Fact]
        public void CPFValue_InvalidDigits()
        {

            Assert.Throws<ApplicationException>(() => new CPF("06515537507"));
        }
    }
}
