document.addEventListener('DOMContentLoaded', function () {
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

    // TempData Success varsa modalı göster
    const successAlert = document.querySelector('.alert-success, [data-success="true"]');
    if (successAlert || document.querySelector('[data-contact-success]')) {
        if (successModal) {
            successModal.classList.add('visible');
            successModal.classList.add('success-animation');
            setTimeout(() => successModal.classList.remove('success-animation'), 1000);
        }
    }
});
