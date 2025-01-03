using System.ComponentModel.DataAnnotations;

namespace tcsoft_pingpongclub.Models
{
    public class DateRangeAttribute : ValidationAttribute
    {
        public string StartDateProperty { get; }
        public string EndDateProperty { get; }

        public DateRangeAttribute(string startDateProperty, string endDateProperty)
        {
            StartDateProperty = startDateProperty;
            EndDateProperty = endDateProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var startDate = (DateTime?)validationContext.ObjectType
                .GetProperty(StartDateProperty)
                ?.GetValue(validationContext.ObjectInstance);

            var endDate = (DateTime?)validationContext.ObjectType
                .GetProperty(EndDateProperty)
                ?.GetValue(validationContext.ObjectInstance);

            if (startDate.HasValue && endDate.HasValue && startDate >= endDate)
            {
                return new ValidationResult($"Thời gian kết thúc phải lớn hơn thời gian bắt đầu.");
            }

            return ValidationResult.Success;
        }
    }
}
