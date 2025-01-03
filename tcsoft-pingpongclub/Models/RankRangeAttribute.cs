using System.ComponentModel.DataAnnotations;

namespace tcsoft_pingpongclub.Models
{
    public class RankRangeAttribute : ValidationAttribute
    {
        public string RankStartProperty { get; }
        public string RankEndProperty { get; }

        public RankRangeAttribute(string rankStartProperty, string rankEndProperty)
        {
            RankStartProperty = rankStartProperty;
            RankEndProperty = rankEndProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Lấy giá trị của RankStart và RankEnd từ đối tượng hiện tại
            var rankStart = (int?)validationContext.ObjectType
                .GetProperty(RankStartProperty)
                ?.GetValue(validationContext.ObjectInstance);

            var rankEnd = (int?)validationContext.ObjectType
                .GetProperty(RankEndProperty)
                ?.GetValue(validationContext.ObjectInstance);

            if (rankStart.HasValue && rankEnd.HasValue && rankEnd > rankStart)
            {
                return new ValidationResult($"Hạng Kết Thúc Phải Lớn Hơn Hoặc Bằng Hạng Bắt Đầu");
            }

            return ValidationResult.Success;
        }
    }
}
