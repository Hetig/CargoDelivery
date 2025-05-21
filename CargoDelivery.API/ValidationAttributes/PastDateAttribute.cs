using System.ComponentModel.DataAnnotations;

namespace CargoDelivery.API.ValidationAttributes;

public class PastDateAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        return value is DateTime date && date < DateTime.Now;
    }
}