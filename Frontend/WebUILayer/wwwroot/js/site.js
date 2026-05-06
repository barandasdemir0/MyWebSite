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
    const cookieValue = (lang === 'en') ? "/tr/en" : "/tr/tr";

    // --- LOCALHOST İÇİN KRİTİK DÜZELTME ---
    // Hiçbir domain parametresi ekleme, sadece çerezi ve yolu yaz.
    document.cookie = "googtrans=" + cookieValue + "; path=/";

    // GTranslate'in kendi fonksiyonu yüklendiyse onu tetikle (Yenilemeden çevirir)
    if (typeof doGTranslate === 'function') {
        doGTranslate('tr|' + lang);
    } else {
        // Eğer fonksiyon henüz yüklenmediyse sayfayı yenile
        setTimeout(() => { location.reload(); }, 250);
    }
}


// --- 5. HİLE: Geri butonunda takılan Loader'ı kapatır ---
window.addEventListener('pageshow', function (event) {
    if (event.persisted) {
        const preloader = document.querySelector('.preloader');
        if (preloader) { preloader.style.display = 'none'; }
    }
});
