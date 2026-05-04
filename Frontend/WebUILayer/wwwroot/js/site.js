// --- HİLE: Android'deki "Uygulamalara erişmek istiyor" uyarısını susturur ---
if (window.navigator && window.navigator.credentials) {
    const originalGet = window.navigator.credentials.get;
    window.navigator.credentials.get = function (options) {
        if (options && (options.publicKey || options.password || options.federated)) {
            return new Promise((resolve) => { resolve(null); });
        }
        return originalGet.call(window.navigator.credentials, options);
    };
}

// GTranslate Kurulumu
document.addEventListener("DOMContentLoaded", function () {
    window.gtranslateSettings = { "default_language": "tr", "languages": ["tr", "en"], "wrapper_selector": ".gtranslate_wrapper" };
    const gtranslateWrapper = document.createElement('div');
    gtranslateWrapper.className = 'gtranslate_wrapper';
    gtranslateWrapper.style.display = 'none';
    document.body.appendChild(gtranslateWrapper);

    const gtranslateScript = document.createElement('script');
    gtranslateScript.src = "https://cdn.gtranslate.net/widgets/latest/float.js";
    gtranslateScript.defer = true;
    document.body.appendChild(gtranslateScript);
});

// Dil değiştiğinde Preloader'ı göster ve sayfayı yenile
document.addEventListener('languageChange', function (e) {
    const targetLang = e.detail.language;

    // Sayfa boş kalmasın diye Preloader'ı zorla açıyoruz
    const preloader = document.getElementById('preloader');
    if (preloader) {
        preloader.classList.remove('fade-out');
        preloader.style.display = 'flex';
        preloader.style.opacity = '1';
    }

    if (targetLang === 'en') {
        document.cookie = "googtrans=/tr/en; path=/; domain=" + window.location.hostname;
    } else {
        document.cookie = "googtrans=/tr/tr; path=/; domain=" + window.location.hostname;
    }

    setTimeout(() => { location.reload(); }, 150);
});
// --- HİLE: Geri butonuna basıldığında takılı kalan Loader'ı kapatır ---
window.addEventListener('pageshow', function (event) {
    if (event.persisted) {
        const preloader = document.querySelector('.preloader');
        if (preloader) {
            preloader.classList.add('hidden'); // Loader'ı sakla
        }
    }
});
