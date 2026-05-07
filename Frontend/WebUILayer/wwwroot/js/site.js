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

// --- 3. Dil Seçimini Başlatan Kod ---
document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.lang-option').forEach(btn => {
        btn.addEventListener('click', function () {
            const lang = this.getAttribute('data-lang');
            setLanguage(lang);
        });
    });

    const gtranslateScript = document.createElement('script');
    gtranslateScript.src = "https://cdn.gtranslate.net/widgets/latest/float.js";
    gtranslateScript.defer = true;
    document.body.appendChild(gtranslateScript);
});

// --- 4. ASIL DİL DEĞİŞTİRME FONKSİYONU ---
function setLanguage(lang) {
    // Tüm olası domainler için çerezi temizle
    var hostname = window.location.hostname;
    var parts = hostname.split('.');
    var rootDomain = parts.length >= 2 ? parts.slice(-2).join('.') : hostname;
    var domains = [null, hostname, "." + hostname, "." + rootDomain];

    domains.forEach(function (domain) {
        var cookieStr = "googtrans=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/";
        if (domain) cookieStr += "; domain=" + domain;
        document.cookie = cookieStr;
    });

    if (lang === 'en') {
        document.cookie = "googtrans=/tr/en; path=/";
        document.cookie = "googtrans=/tr/en; path=/; domain=." + rootDomain;
    }

    location.reload();
}

// --- 5. HİLE: Geri butonunda takılan Loader'ı kapatır ---
window.addEventListener('pageshow', function (event) {
    if (event.persisted) {
        var preloader = document.querySelector('.preloader');
        if (preloader) { preloader.style.display = 'none'; }
    }
});
// === Analytics Loader (Minimum JS) ===
(function () {
    var el = document.getElementById("analytics-data");
    if (!el) return;

    var ga = el.dataset.gaId;
    var cl = el.dataset.clarityId;

    if (ga) {
        var s = document.createElement("script");
        s.async = true;
        s.src = "https://www.googletagmanager.com/gtag/js?id=" + ga;
        document.head.appendChild(s);
        s.onload = function () { window.dataLayer = window.dataLayer || []; function gtag() { dataLayer.push(arguments); } gtag("js", new Date()); gtag("config", ga); };
    }

    if (cl) {
        var c = document.createElement("script");
        c.textContent = '(function(c,l,a,r,i,t,y){c[a]=c[a]||function(){(c[a].q=c[a].q||[]).push(arguments)};t=l.createElement(r);t.async=1;t.src="https://www.clarity.ms/tag/"+i;y=l.getElementsByTagName(r)[0];y.parentNode.insertBefore(t,y)})(window,document,"clarity","script","' + cl + '")';
        document.head.appendChild(c);
    }
})();
