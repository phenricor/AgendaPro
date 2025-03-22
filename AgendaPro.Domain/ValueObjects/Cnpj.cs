using AgendaPro.Domain.Shared;

namespace AgendaPro.Domain.ValueObjects;

public record Cnpj
{
    public string Value { get; set; }
    private Cnpj(string value)
    {
        Value = value;
    }

    public static Result<Cnpj> Create(string value)
    {
        if (!IsValid(value))
        {
            return Result<Cnpj>.Failure(CustomerErrors.CnpjInvalid);
        }
        return Result<Cnpj>.Success(new Cnpj(value));
    }

    private static bool IsValid(string value)
    {
        int[] multiplicador1 = [5,4,3,2,9,8,7,6,5,4,3,2];
        int[] multiplicador2 = [6,5,4,3,2,9,8,7,6,5,4,3,2];

        var cnpj = value.Trim();
        cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

        if (cnpj.Length != 14)
            return false;

        var tempCnpj = cnpj.Substring(0, 12);

        var soma = 0;
        for(var i=0; i<12; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

        var resto = (soma % 11);
        if ( resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        var digito = resto.ToString();

        tempCnpj = tempCnpj + digito;
        soma = 0;
        for (int i = 0; i < 13; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

        resto = (soma % 11);
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digito = digito + resto.ToString();

        return cnpj.EndsWith(digito);
    }
    public override string ToString()
    {
        return Value;
    }
}