document.addEventListener('click', function (e) {

    /* ---------- DELETE ---------- */
    const deleteBtn = e.target.closest('[data-action="deleteRow"]');
    if (deleteBtn) {
        e.preventDefault();

        const form = deleteBtn.closest('form');
        if (!form) return;

        const row = deleteBtn.closest('tr');
        const itemName =
            row.querySelector('td:first-child strong')?.textContent || 'Bu öğe';

        const title = 'Silme İşlemi';
        const body = `"${itemName}" silinecek. <br><small>Bu işlem geri alınabilir.</small>`;

        const onConfirm = () => form.submit(); // 🔥 KRİTİK SATIR

        if (window.adminApp?.notifications?.showModal) {
            window.adminApp.notifications.showModal(title, body, onConfirm, 'danger');
        } else {
            if (confirm(`${itemName} silinsin mi?`)) onConfirm();
        }

        return;
    }

    /* ---------- RESTORE ---------- */
    const restoreBtn = e.target.closest('[data-action="restoreRow"]');
    if (restoreBtn) {
        e.preventDefault();

        const form = restoreBtn.closest('form');
        if (!form) return;

        const row = restoreBtn.closest('tr');
        const itemName =
            row.querySelector('td:first-child strong')?.textContent || 'Bu öğe';

        const title = 'Geri Yükle';
        const body = `"${itemName}" geri yüklenecek. Onaylıyor musunuz?`;

        const onConfirm = () => form.submit(); // 🔥 KRİTİK SATIR

        if (window.adminApp?.notifications?.showModal) {
            window.adminApp.notifications.showModal(title, body, onConfirm, 'warning');
        } else {
            if (confirm(body)) onConfirm();
        }
    }
});
