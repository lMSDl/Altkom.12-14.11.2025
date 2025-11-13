using System.ComponentModel.DataAnnotations;

namespace Models.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class MyValidationAnnotation : ValidationAttribute
    {
        public string Value { get; set; } = string.Empty;

        public override bool IsValid(object? value)
        {
            return ((value as string)?.Contains(Value)) ?? true;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must contain the value '{Value}'.";
        }
    }
}
