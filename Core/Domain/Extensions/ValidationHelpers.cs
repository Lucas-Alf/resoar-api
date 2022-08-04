using System.ComponentModel.DataAnnotations;

namespace Domain.Extensions
{
    public static class ValidationHelpers
    {
        public static IEnumerable<ValidationResult> GetValidationErrors(this object obj)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(obj, null, null);
            Validator.TryValidateObject(obj, context, validationResults, true);
            return validationResults;
        }

        public static string GetValidationErrorMessages(this object obj)
        {
            var errors = obj.GetValidationErrors();
            var validationResults = errors as ValidationResult[] ?? errors.ToArray();
            if (!validationResults.Any())
                return null;

            var messages = validationResults.Select(x => x.ErrorMessage?.TrimEnd('.'));
            return string.Join(", ", messages);
        }
    }
}