// ============================================
// GUESTBOOK PAGE - (INLINE JS OLMADAN, TEMİZ HALİ)
// ============================================

document.addEventListener('DOMContentLoaded', function () {
    const successModal = document.getElementById('guestbookSuccessModal');
    const closeModalBtn = document.getElementById('closeSuccessModalBtn');

    // 1. Kapatma Butonu (Inline onclick yerine EventListener kullanıldı)
    if (closeModalBtn && successModal) {
        closeModalBtn.addEventListener('click', function () {
            successModal.classList.remove('visible');
        });
    }

    // 2. Modal dışına tıklayınca modalı kapatma
    if (successModal) {
        successModal.addEventListener('click', function (e) {
            if (e.target === this) {
                successModal.classList.remove('visible');
            }
        });
    }

    // 3. C#'tan Gelen Başarı Durumunu Kontrol Et (data-success attribute üzerinden)
    if (successModal && successModal.getAttribute('data-success') === 'true') {
        successModal.classList.add('visible'); // Modalı Aç
        fireGuestbookConfetti(); // Konfetiyi patlat
    }

    // Form input validation (Textarea boşken "Gönder" butonunu kapatma)
    const guestbookForm = document.querySelector('.guestbook-form form');
    const submitBtn = guestbookForm ? guestbookForm.querySelector('button[type="submit"]') : null;
    const textarea = guestbookForm ? guestbookForm.querySelector('textarea') : null;

    if (textarea && submitBtn) {
        textarea.addEventListener('input', function () {
            if (textarea.value.trim().length > 0) {
                submitBtn.disabled = false;
                submitBtn.title = 'Gönder';
            } else {
                submitBtn.disabled = true;
                submitBtn.title = 'Mesaj yazmalısınız';
            }
        });
    }

    // Confetti Fonksiyonu (Sadece içeriden çağrılacak şekilde encapsüle edildi)
    function fireGuestbookConfetti() {
        if (typeof confetti === 'undefined') {
            console.warn('Confetti library not loaded');
            return;
        }

        const count = 200;
        const defaults = { origin: { y: 0.7 } };

        function fire(particleRatio, opts) {
            confetti(Object.assign({}, defaults, opts, {
                particleCount: Math.floor(count * particleRatio)
            }));
        }

        fire(0.25, { spread: 26, startVelocity: 55 });
        fire(0.2, { spread: 60 });
        fire(0.35, { spread: 100, decay: 0.91, scalar: 0.8 });
        fire(0.1, { spread: 120, startVelocity: 25, decay: 0.92, scalar: 1.2 });
        fire(0.1, { spread: 120, startVelocity: 45 });
    }
});

// Pagination functionality for guestbook page
document.addEventListener('DOMContentLoaded', function () {
    const paginationBtns = document.querySelectorAll('.pagination-btn');
    if (paginationBtns.length === 0) return;

    paginationBtns.forEach((btn) => {
        btn.addEventListener('click', function () {
            if (this.disabled || this.classList.contains('active')) return;
            const isNumber = !this.querySelector('i');
            if (isNumber) {
                document.querySelectorAll('.pagination-btn').forEach(b => {
                    if (!b.querySelector('i')) b.classList.remove('active');
                });
                this.classList.add('active');
            } else {
                const currentActive = document.querySelector('.pagination-btn.active');
                const currentPage = parseInt(currentActive?.textContent || 1);
                const isNext = this.querySelector('.fa-chevron-right');

                if (isNext) {
                    const nextBtn = Array.from(paginationBtns).find(b => !b.querySelector('i') && parseInt(b.textContent) === currentPage + 1);
                    if (nextBtn) nextBtn.click();
                } else {
                    const prevBtn = Array.from(paginationBtns).find(b => !b.querySelector('i') && parseInt(b.textContent) === currentPage - 1);
                    if (prevBtn) prevBtn.click();
                }
            }
            updateGuestbookPaginationArrows();
        });
    });

    function updateGuestbookPaginationArrows() {
        const currentActive = document.querySelector('.pagination-btn.active');
        const currentPage = parseInt(currentActive?.textContent || 1);
        const allPages = Array.from(document.querySelectorAll('.pagination-btn')).filter(b => !b.querySelector('i')).map(b => parseInt(b.textContent));

        const minPage = Math.min(...allPages);
        const maxPage = Math.max(...allPages);

        const prevBtn = document.getElementById('prevPage');
        const nextBtn = document.getElementById('nextPage');

        if (prevBtn) prevBtn.disabled = currentPage <= minPage;
        if (nextBtn) nextBtn.disabled = currentPage >= maxPage;
    }
    updateGuestbookPaginationArrows();
});
