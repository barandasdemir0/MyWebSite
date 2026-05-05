// --- 1. HİLE: İzin Penceresini Blokla (Sayfanın en başında kalsın) ---
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

// --- 3. DİL DEĞİŞTİRME FONKSİYONU (Butonuna bunu bağlayabilirsin) ---
function setLanguage(lang) {
    const domain = window.location.hostname;

    // Preloader'ı aç
    const preloader = document.getElementById('preloader');
    if (preloader) {
        preloader.style.display = 'flex';
        preloader.style.opacity = '1';
    }

    // Çerezleri hem domainli hem domainsiz yazıyoruz (Garanti olsun)
    const cookieValue = (lang === 'en') ? "/tr/en" : "/tr/tr";

    document.cookie = "googtrans=" + cookieValue + "; path=/;";
    document.cookie = "googtrans=" + cookieValue + "; path=/; domain=." + domain;
    document.cookie = "googtrans=" + cookieValue + "; path=/; domain=" + domain;

    // Sayfayı yenile
    setTimeout(() => { location.reload(); }, 250);
}

// --- 4. Senin Mevcut Event Dinleyicin (Eğer bunu kullanıyorsan kalsın) ---
document.addEventListener('languageChange', function (e) {
    setLanguage(e.detail.language);
});

// --- 5. GTranslate Scriptini Yükle ---
document.addEventListener("DOMContentLoaded", function () {
    const gtranslateScript = document.createElement('script');
    gtranslateScript.src = "https://cdn.gtranslate.net/widgets/latest/float.js";
    gtranslateScript.defer = true;
    document.body.appendChild(gtranslateScript);
});
