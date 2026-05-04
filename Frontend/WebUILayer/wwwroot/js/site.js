// 1. GTranslate Motorunu Gizlice Kur (Sayfa Yüklendiğinde)
document.addEventListener("DOMContentLoaded", function () {
    window.gtranslateSettings = {
        "default_language": "tr",
        "languages": ["tr", "en"],
        "wrapper_selector": ".gtranslate_wrapper"
    };

    const gtranslateWrapper = document.createElement('div');
    gtranslateWrapper.className = 'gtranslate_wrapper';
    gtranslateWrapper.style.display = 'none';
    document.body.appendChild(gtranslateWrapper);

    const gtranslateScript = document.createElement('script');
    gtranslateScript.src = "https://cdn.gtranslate.net/widgets/latest/float.js";
    gtranslateScript.defer = true;
    document.body.appendChild(gtranslateScript);
});

// 2. Senin language.js dosyanın fırlattığı "Dil Değişti" sinyalini yakala!
document.addEventListener('languageChange', function (e) {
    const targetLang = e.detail.language;
    if (targetLang === 'en') {
        document.cookie = "googtrans=/tr/en; path=/";
        document.cookie = "googtrans=/tr/en; path=/; domain=" + window.location.hostname;
        // Hash satırını sildik!
    } else {
        document.cookie = "googtrans=/tr/tr; path=/";
        document.cookie = "googtrans=/tr/tr; path=/; domain=" + window.location.hostname;
        // Hash satırını sildik!
    }
    // Sistemin çerezleri okuyup çevrilmiş açılması için sayfayı yenile
    location.reload();
});