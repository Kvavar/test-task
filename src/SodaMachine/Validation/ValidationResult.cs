namespace SodaMachine.Core.Validation
{
    public class ValidationResult
    {
        public ValidationResult(bool isValid)
        {
            IsValid = isValid;
        }
        public ValidationResult(bool isValid, string message) : this(isValid)
        {
            Message = message;
        }

        public bool IsValid { get; }
        public string Message { get; }

        public static ValidationResult Valid()
        {
            return new ValidationResult(true);
        }

        public static ValidationResult Invalid(string message)
        {
            return new ValidationResult(false, message);
        }
    }
}