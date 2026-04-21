document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('contactForm');
    const successModal = document.getElementById('successModal');
    const closeModalBtn = document.querySelector('.close-modal');
    const modalCloseBtn = document.getElementById('modalCloseBtn');

    // Modal Kapatma İşlemleri
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

    // Form Gönderme İşlemi
    if (form) {
        form.addEventListener('submit', function (e) {
            // 1. SAYFANIN YENİLENMESİNİ KESİNLİKLE DURDUR
          /*  e.preventDefault();*/

            // Form kurallara uyuyor mu kontrol et (Boş alan var mı?)
            if (!form.checkValidity()) {
                e.stopPropagation();
                form.classList.add('was-validated');
                return;
            }

            // 2. BUTON ANİMASYONUNU BAŞLAT
            const submitBtn = form.querySelector('button[type="submit"]');
            const originalText = submitBtn.innerHTML;
            submitBtn.disabled = true;
            submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Gönderiliyor...';

            // 3. İŞTE O SİHİRLİ SATIR: Verileri arka planda C# Controller'a gönderir
            fetch(form.action, {
                method: form.method,
                body: new FormData(form)
            });

            // 4. BEKLEME ANİMASYONU VE MODALI AÇMA (1.5 Saniye sonra)
            setTimeout(() => {
                submitBtn.disabled = false;
                submitBtn.innerHTML = originalText;
                form.reset();
                form.classList.remove('was-validated');

                // Modalı Göster ve Efekti Çalıştır
                if (successModal) {
                    successModal.classList.add('visible');
                    successModal.classList.add('success-animation');
                    setTimeout(() => {
                        successModal.classList.remove('success-animation');
                    }, 1000);
                }
            }, 1500);
        });
    }
});