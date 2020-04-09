using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Betto.Helpers
{
    public class ObjectValidator : IObjectValidator
    {
        public void ValidateObject(object validatedInstance)
        {
            var context = new ValidationContext(validatedInstance, null, null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(validatedInstance, context, results, true);

            if (!isValid)
            {
                var errorMessage = PrepareErrorMessage(results);
                throw new Exception(errorMessage);
            }
        }

        private string PrepareErrorMessage(IEnumerable<ValidationResult> validationResults)
        {
            var builder = new StringBuilder();

            foreach (var result in validationResults)
            {
                builder.Append(result.ErrorMessage + Environment.NewLine);
            }

            return builder.ToString();
        }
    }
}
