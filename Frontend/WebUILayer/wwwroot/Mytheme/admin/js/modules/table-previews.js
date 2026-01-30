/**
 * Table Previews Module
 * Handle theme toggling and other interaction logic for table-previews.html
 */

// Sayfa yüklenmeden önce tema tercihini uygula (FOUC önlemek için)
(function initTheme() {
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme === 'light') {
        document.documentElement.classList.add('light-mode');
    }
})();

// DOM Yüklendiğinde
document.addEventListener('DOMContentLoaded', () => {
    updateThemeIcon();
});


/**
 * Temayı değiştirir (Dark/Light)
 */
function toggleTheme() {
    const html = document.documentElement;

    if (html.classList.contains('light-mode')) {
        html.classList.remove('light-mode');
        localStorage.setItem('theme', 'dark');
    } else {
        html.classList.add('light-mode');
        localStorage.setItem('theme', 'light');
    }

    updateThemeIcon();
}

/**
 * Tema ikonunu günceller
 */
function updateThemeIcon() {
    const icon = document.querySelector('.theme-toggle i');
    const isLight = document.documentElement.classList.contains('light-mode');

    if (icon) {
        // Light mode ise Güneş, Dark mode ise Ay
        icon.className = isLight ? 'fas fa-sun' : 'fas fa-moon';
    }
}
