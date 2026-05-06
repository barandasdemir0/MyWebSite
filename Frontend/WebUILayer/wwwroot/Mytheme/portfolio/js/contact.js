document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('contactForm');
    const successModal = document.getElementById('successModal');
    const closeModalBtn = document.querySelector('.close-modal');
    const modalCloseBtn = document.getElementById('modalCloseBtn');

    function closeSuccessModal() {
        if (successModal) successModal.classList.remove('visible');
    }
    if (closeModalBtn) closeModalBtn.addEventListener('click', closeSuccessModal);
    if (modalCloseBtn) modalCloseBtn.addEventListener('click', closeSuccessModal);
    if (successModal) {
        successModal.addEventListener('click', function (e) {
            if (e.target === this) closeSuccessModal();
        });
    }

    // Hata mesajlarını temizle
    function clearErrors() {
        form.querySelectorAll('.server-error').forEach(el => el.remove());
        form.querySelectorAll('.is-invalid').forEach(el => el.classList.remove('is-invalid'));
    }

    // Sunucu hata mesajlarını input'ların altına yaz
    function showErrors(errors) {
        clearErrors();
        for (const [key, messages] of Object.entries(errors)) {
            // "createMessageDto.SenderName" → input'u bul
            const input = form.querySelector(`[name="${key}"]`);
            if (input) {
                input.classList.add('is-invalid');
                const span = document.createElement('span');
                span.className = 'text-danger server-error';
                span.textContent = messages[0]; // İlk mesajı göster
                input.insertAdjacentElement('afterend', span);
            }
        }
    }

    if (form) {
        form.addEventListener('submit', async function (e) {
            e.preventDefault();

            // Client-side boş alan kontrolü
            if (!form.checkValidity()) {
                e.stopPropagation();
                form.classList.add('was-validated');
                return;
            }

            clearErrors();

            const submitBtn = form.querySelector('button[type="submit"]');
            const originalText = submitBtn.innerHTML;
            submitBtn.disabled = true;
            submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Gönderiliyor...';

            try {
                const response = await fetch(form.action, {
                    method: form.method,
                    body: new FormData(form)
                });

                if (response.ok) {
                    // Başarılı
                    form.reset();
                    form.classList.remove('was-validated');
                    if (successModal) {
                        successModal.classList.add('visible');
                        successModal.classList.add('success-animation');
                        setTimeout(() => successModal.classList.remove('success-animation'), 1000);
                    }
                } else if (response.status === 400) {
                    // Validasyon hataları
                    const errors = await response.json();
                    if (errors && typeof errors === 'object') {
                        showErrors(errors);
                    }
                } else {
                    alert('Sistemsel bir hata oluştu. Lütfen tekrar deneyin.');
                }
            } catch (err) {
                alert('Bağlantı hatası. Lütfen tekrar deneyin.');
            } finally {
                submitBtn.disabled = false;
                submitBtn.innerHTML = originalText;
            }
        });
    }
});