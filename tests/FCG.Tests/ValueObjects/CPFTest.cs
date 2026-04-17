using FCG.Domain.ValueObjects;

namespace FCG.Tests.ValueObjects;

public class CPFTest
{
    [Fact]
    public void CPFValue_Valid()
    {

        var cpf = new CPF("27966231577");
        Assert.Equal("27966231577", cpf.Code);
    }

    [Fact]
    public void CPFValue_InvalidLength()
    {

        Assert.Throws<ApplicationException>(() => new CPF("2796623157"));
    }

    [Fact]
    public void CPFValue_InvalidDigits()
    {

        Assert.Throws<ApplicationException>(() => new CPF("27966231572"));
    }
}
