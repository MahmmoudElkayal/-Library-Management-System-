using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LibraryManagementSystem.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AllowedFileExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;
        private readonly long _maxFileSizeInBytes;

        public AllowedFileExtensionsAttribute(string allowedExtensions = ".jpg,.jpeg,.png,.gif", long maxFileSizeInBytes = 2 * 1024 * 1024)
        {
            _allowedExtensions = allowedExtensions.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(e => e.Trim().ToLowerInvariant()).ToArray();
            _maxFileSizeInBytes = maxFileSizeInBytes;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_allowedExtensions.Contains(ext))
                {
                    return new ValidationResult($"Only the following file types are allowed: {string.Join(", ", _allowedExtensions)}");
                }

                if (file.Length > _maxFileSizeInBytes)
                {
                    var maxMB = _maxFileSizeInBytes / (1024 * 1024);
                    return new ValidationResult($"File size must not exceed {maxMB}MB.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
