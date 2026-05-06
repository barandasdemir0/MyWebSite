/* ============================================
   ANIMATIONS.JS - All Animation Related Functions
   ============================================ */
function startAnimations() {
    initAOS();
    initCounters();
    initProgressBars();
    setTimeout(initTypingEffect, 100); // 100ms gecikme ile çalıştır
}
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', startAnimations);
} else {
    startAnimations();
}
// Tüm CSS ve Scriptler yüklendikten sonra çalışmayı garanti eder
window.addEventListener('load', startAnimations);

/* ============================================
   AOS - Animate On Scroll
   ============================================ */
function initAOS() {
    if (typeof AOS !== 'undefined') {
        AOS.init({
            duration: 800,
            easing: 'ease-out-cubic',
            once: true,
            offset: 100,
            delay: 100
        });
    }
}

/* ============================================
   Counter Animation
   ============================================ */
function initCounters() {
    const counters = document.querySelectorAll('.counter-number');
    if (!counters.length) return;

    const animate = (counter) => {
        const target = parseInt(counter.getAttribute('data-target')) || 0;
        const step = target / 125;
        let current = 0;
        const update = () => {
            current += step;
            if (current < target) { counter.textContent = Math.floor(current); requestAnimationFrame(update); }
            else { counter.textContent = target; }
        };
        update();
    };

    const obs = new IntersectionObserver((entries) => {
        entries.forEach(e => {
            if (e.isIntersecting && !e.target.classList.contains('animated')) {
                e.target.classList.add('animated');
                animate(e.target);
            }
        });
    }, { threshold: 0.5 });

    counters.forEach(c => obs.observe(c));
}

/* ============================================
   Progress Bars
   ============================================ */
function initProgressBars() {
    const bars = document.querySelectorAll('.progress-bar-fill');
    if (!bars.length) return;

    const obs = new IntersectionObserver((entries) => {
        entries.forEach(e => {
            if (e.isIntersecting) {
                e.target.style.width = (e.target.getAttribute('data-width') || '0') + '%';
            }
        });
    }, { threshold: 0.5 });

    bars.forEach(bar => obs.observe(bar));
}

/* ============================================
   Typing Effect (Custom - No external library)
   ============================================ */
/* ============================================
   Typing Effect (Custom - No external library)
   ============================================ */
function initTypingEffect() {
    const element = document.querySelector('.typed-text');
    if (!element) return;

    // --- DEĞİŞTİRİLEN KISIM BAŞLANGICI ---
    // HTML'deki data-words niteliğini okuyoruz
    const dbWords = element.getAttribute('data-words');

    // Virgüllerden bölüp her kelimenin başındaki/sonundaki boşlukları temizliyoruz (trim)
    // Eğer veritabanından veri gelmezse yedek olarak 'Developer' yazdırıyoruz.
    const strings = dbWords ? dbWords.split(',').map(word => word.trim()) : ['Developer'];
    // --- DEĞİŞTİRİLEN KISIM BİTİŞİ ---

    let stringIndex = 0;
    let charIndex = 0;
    let isDeleting = false;

    function type() {
        const currentString = strings[stringIndex];

        if (isDeleting) {
            element.textContent = currentString.substring(0, charIndex - 1);
            charIndex--;
        } else {
            element.textContent = currentString.substring(0, charIndex + 1);
            charIndex++;
        }

        let typeSpeed = isDeleting ? 50 : 80;

        if (!isDeleting && charIndex === currentString.length) {
            typeSpeed = 2000;
            isDeleting = true;
        } else if (isDeleting && charIndex === 0) {
            isDeleting = false;
            stringIndex = (stringIndex + 1) % strings.length;
        }

        setTimeout(type, typeSpeed);
    }

    type();
}