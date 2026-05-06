// --- 1. HİLE: Android ve PC'deki "İzin İsteği" penceresini susturur ---
if (window.navigator && window.navigator.credentials) {
    window.navigator.credentials.get = () => new Promise(resolve => resolve(null));
}

// --- 2. GTranslate Ayarları ---
window.gtranslateSettings = {
    "default_language": "tr",
    "languages": ["tr", "en"],
    "wrapper_selector": ".gtranslate_wrapper",
    "detect_browser_language": false,
    "auto_switch": false
};

// --- 3. Dil Seçimini Başlatan Kod (DOMContentLoaded içinde) ---
document.addEventListener('DOMContentLoaded', function () {
    // HTML'deki butonları dinlemeye başla
    document.querySelectorAll('.lang-option').forEach(btn => {
        btn.addEventListener('click', function () {
            const lang = this.getAttribute('data-lang');
            setLanguage(lang);
        });
    });

    // GTranslate Scriptini Yükle
    const gtranslateScript = document.createElement('script');
    gtranslateScript.src = "https://cdn.gtranslate.net/widgets/latest/float.js";
    gtranslateScript.defer = true;
    document.body.appendChild(gtranslateScript);
});

// --- 4. ASIL DİL DEĞİŞTİRME FONKSİYONU ---
// --- 1. HİLE: Android ve PC'deki "İzin İsteği" penceresini susturur ---
if (window.navigator && window.navigator.credentials) {
    window.navigator.credentials.get = () => new Promise(resolve => resolve(null));
}

// --- 2. GTranslate Ayarları ---
window.gtranslateSettings = {
    "default_language": "tr",
    "languages": ["tr", "en"],
    "wrapper_selector": ".gtranslate_wrapper",
    "detect_browser_language": false,
    "auto_switch": false
};

// --- 3. Dil Seçimini Başlatan Kod (DOMContentLoaded içinde) ---
document.addEventListener('DOMContentLoaded', function () {
    // HTML'deki butonları dinlemeye başla
    document.querySelectorAll('.lang-option').forEach(btn => {
        btn.addEventListener('click', function () {
            const lang = this.getAttribute('data-lang');
            setLanguage(lang);
        });
    });

    // GTranslate Scriptini Yükle
    const gtranslateScript = document.createElement('script');
    gtranslateScript.src = "https://cdn.gtranslate.net/widgets/latest/float.js";
    gtranslateScript.defer = true;
    document.body.appendChild(gtranslateScript);
});

// --- 4. ASIL DİL DEĞİŞTİRME FONKSİYONU ---
function setLanguage(lang) {
    // 1. ÖNCE HER ŞEYİ TEMİZLE (Eski çerezleri çöpe at)
    const domains = [null, window.location.hostname, ".localhost"];
    domains.forEach(domain => {
        let cookieStr = "googtrans=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/";
        if (domain) cookieStr += "; domain=" + domain;
        document.cookie = cookieStr;
    });

    // 2. EĞER İNGİLİZCE İSTENİYORSA YENİSİNİ YAZ
    if (lang === 'en') {
        document.cookie = "googtrans=/tr/en; path=/";
    }

    // 3. CAN ALICI NOKTA: Zorunlu Yenileme
    // Localhost'ta script bazen hafızada eski dili tutar. 
    // Yenileme yapmak her şeyi sıfırdan ve doğru yükler.
    location.reload();
}




// --- 5. HİLE: Geri butonunda takılan Loader'ı kapatır ---
window.addEventListener('pageshow', function (event) {
    if (event.persisted) {
        const preloader = document.querySelector('.preloader');
        if (preloader) { preloader.style.display = 'none'; }
    }
});
