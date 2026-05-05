// --- 1. HİLE: İzin Penceresini Sustur (En Üstte) ---
if (window.navigator && window.navigator.credentials) {
    window.navigator.credentials.get = () => new Promise(resolve => resolve(null));
}

// --- 2. GTranslate Ayarları ---
window.gtranslateSettings = {
    "default_language": "tr",
    "languages": ["tr", "en"],
    "wrapper_selector": ".gtranslate_wrapper"
    // detect_browser_language ve auto_switch'i şimdilik kaldırdık, çalışınca ekleriz.
};

// --- 3. DİL DEĞİŞTİRME FONKSİYONU ---
function setLanguage(lang) {
    // Çerez değerini hazırla
    const cookieValue = (lang === 'en') ? "/tr/en" : "/tr/tr";

    // Çerezi "expires" (Bitiş tarihi) ile beraber yazıyoruz (Bu çok önemli!)
    const d = new Date();
    d.setTime(d.getTime() + (365 * 24 * 60 * 60 * 1000)); // 1 yıllık çerez
    const expires = "expires=" + d.toUTCString();

    // Çerezi hem genel hem de domainli olarak en garanti haliyle yazıyoruz
    document.cookie = "googtrans=" + cookieValue + ";" + expires + ";path=/";

    // Sayfayı yenilemeden önce çerezin yazıldığından emin olmak için süreyi artırdık
    setTimeout(() => { location.reload(); }, 300);
}

// --- 4. GTranslate Scriptini Yükle ---
document.addEventListener("DOMContentLoaded", function () {
    const gtranslateScript = document.createElement('script');
    gtranslateScript.src = "https://cdn.gtranslate.net/widgets/latest/float.js";
    gtranslateScript.defer = true;
    document.body.appendChild(gtranslateScript);
});

// --- 5. Event Dinleyicin (Butonuna basınca bunu tetiklediğinden emin ol) ---
document.addEventListener('languageChange', function (e) {
    setLanguage(e.detail.language);
});
