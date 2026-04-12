using FCG.Domain.Shared;

namespace FCG.Domain.ValueObjects;

public class CPF
{
    public string Code { get; set; }

    public CPF(string code)
    {
        string cpf = code.Trim().Replace("-", "").Replace(".", "");
        AssertionConcern.AssertLength(cpf, 11, 11, "CPF precisa ter 11 dígitos");
        AssertionConcern.AssertTrue(IsValid(cpf), "CPF ínvalido");
        Code = cpf;
    }

    private int GetVerifyCode(string cpf, int endCode, int factor)
    {
        int sumDigits = 0;
        for (int i = 0; i <= endCode; i++)
        {
            sumDigits += int.Parse(cpf[i].ToString()) * factor;
            factor--;
        }
        int modDigit = sumDigits % 11;
        if (modDigit >= 2) return 11 - modDigit;
        else return 0;
    }


    private bool IsValid(string cpf)
    {
        int primaryVerifyDigit = GetVerifyCode(cpf, 8, 10);
        if (primaryVerifyDigit.ToString() != cpf[9].ToString()) return false;
        int secondaryVerifyDigit = GetVerifyCode(cpf, 9, 11);
        if (secondaryVerifyDigit.ToString() != cpf[10].ToString()) return false;
        return true;
    }

}