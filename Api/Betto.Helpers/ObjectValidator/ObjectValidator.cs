using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Betto.Model.Models;

namespace Betto.Helpers
{
    public class ObjectValidator : IObjectValidator
    {
        public ICollection<ErrorViewModel> ValidateObject(object validatedInstance)
        {
            var errors = new List<ErrorViewModel>();
            var context = new ValidationContext(validatedInstance, null, null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(validatedInstance, context, results, true);

            if (!isValid)
            {
                GetErrors(results, errors);
            }

            return errors;
        }

        private static void GetErrors(IEnumerable<ValidationResult> validationResults, ICollection<ErrorViewModel> errors)
        {
            foreach (var result in validationResults)
            {
                errors.Add(new ErrorViewModel {Message = result.ErrorMessage});
            }
        }
    }
}
