using System.ComponentModel.DataAnnotations;

namespace HiHoHuBlog.Utils;

public class BaseEntity
{

    public Result<Unit, Err> Validate()
    {
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(this, new ValidationContext(this), validationResults, true);
            
        if (!isValid)
        {
            var errorMessages = validationResults.Select(vr => vr.ErrorMessage).ToList();
            return Result<Unit, Err>.Err(UtilErrors.ErrValidatorError(errorMessages));
        }
        return Result<Unit,Err>.Ok(new Unit());
    }
}