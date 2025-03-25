using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AgendaPro.Domain.Shared;

namespace AgendaPro.Domain.ValueObjects;

[ComplexType]
public record Cnpj
{
    public string Value { get; private set; }
    protected Cnpj() {}
    private Cnpj(string value)
    {
        Value = value;
    }
    public static Result<Cnpj> Create(string value)
    {
        if (Validate(value).IsFailure)
        {
            return Result<Cnpj>.Failure(Validate(value).Error);
        }
        return Result<Cnpj>.Success(new(value));
    }
    private static Result<bool> Validate(string value)
    {
        int[] multiplicador1 = [5,4,3,2,9,8,7,6,5,4,3,2];
        int[] multiplicador2 = [6,5,4,3,2,9,8,7,6,5,4,3,2];

        var cnpj = value.Trim();
        cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

        if (cnpj.Length != 14)
            return Result<bool>.Failure(CustomerErrors.CnpjInvalid);

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

        if (!cnpj.EndsWith(digito))
        {
            return Result<bool>.Failure(CustomerErrors.CnpjInvalid);
        }
        return Result<bool>.Success(true);
    }
    public override string ToString()
    {
        return Value;
    }
}