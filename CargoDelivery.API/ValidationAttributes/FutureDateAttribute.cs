using System.ComponentModel.DataAnnotations;

namespace CargoDelivery.API.ValidationAttributes;

public class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        return value is DateTime date && date >= DateTime.Now;
    }
}