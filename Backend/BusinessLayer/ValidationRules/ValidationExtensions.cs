using FluentValidation;
using Ganss.Xss;

namespace BusinessLayer.ValidationRules;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string> MustBeSafeHtml<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(content =>
        {
            if (string.IsNullOrEmpty(content)) return true; // Boşsa diğer kurallar baksın
            var sanitizer = new HtmlSanitizer();
            var safeHtml = sanitizer.Sanitize(content);
            return content == safeHtml;
        }).WithMessage("İçerik zararlı HTML veya Script kodu barındıramaz.");
    }
}
