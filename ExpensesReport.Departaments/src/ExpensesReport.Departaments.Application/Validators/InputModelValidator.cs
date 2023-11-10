using System.ComponentModel.DataAnnotations;

namespace ExpensesReport.Departaments.Application.Validators
{
    internal class InputModelValidator
    {
        public InputModelValidator() { }

        public static string?[]? Validate<T>(T inputModel)
        {
            if (inputModel is null)
                return Array.Empty<string>();

            var context = new ValidationContext(inputModel, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(inputModel, context, results, true);
            var errors = results.Select(r => r.ErrorMessage).ToArray();

            if (!isValid)
            {
                foreach (var property in inputModel.GetType().GetProperties())
                {
                    var propertyValue = property.GetValue(inputModel);

                    if (propertyValue is null)
                        continue;

                    var nestedErrors = Validate(propertyValue);

                    if (nestedErrors?.Length > 0)
                        errors = errors.Concat(nestedErrors).ToArray();
                }
            }

            return errors;
        }
    }
}
