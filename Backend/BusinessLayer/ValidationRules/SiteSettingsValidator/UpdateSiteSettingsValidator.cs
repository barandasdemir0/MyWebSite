using DtoLayer.SiteSettingDtos;
using FluentValidation;

namespace BusinessLayer.ValidationRules.SiteSettingsValidator;

public class UpdateSiteSettingsValidator:AbstractValidator<UpdateSiteSettingDto>
{
    public UpdateSiteSettingsValidator()
    {
        // WorkStatus zorunlu değil ama maximum değer koyalım
        RuleFor(x => x.WorkStatus)
            .MaximumLength(100).WithMessage("Çalışma durumu en fazla 100 karakter olabilir.");
        // SEO Alanları
        RuleFor(x => x.SiteTitle)
            .MaximumLength(200).WithMessage("Site başlığı en fazla 200 karakter olabilir.");
        RuleFor(x => x.MetaDescription)
            .MaximumLength(300).WithMessage("Meta açıklaması en fazla 300 karakter olması önerilir (SEO için).");
        RuleFor(x => x.SiteKeywords)
            .MaximumLength(300).WithMessage("Anahtar kelimeler çok uzun olamaz.");
        RuleFor(x => x.GoogleAnalyticsId)
            .MaximumLength(50).WithMessage("Analytics ID çok uzun.");

        // URL Kontrolleri (Doluysa geçerli URL olmalı)
        RuleFor(x => x.CvFileUrlTr)
             .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).When(x => !string.IsNullOrEmpty(x.CvFileUrlTr))
             .WithMessage("Türkçe CV linki geçerli bir URL olmalıdır.");
        RuleFor(x => x.CvFileUrlEn)
             .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).When(x => !string.IsNullOrEmpty(x.CvFileUrlEn))
             .WithMessage("İngilizce CV linki geçerli bir URL olmalıdır.");
    }
}
