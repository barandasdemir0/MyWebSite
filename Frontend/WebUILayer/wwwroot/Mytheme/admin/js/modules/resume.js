/**
 * Resume Page Script - ASP.NET Core Integration
 * Handles Delete and Restore actions via Form Submission
 */

document.addEventListener('DOMContentLoaded', function () {

    // Tüm sayfa tıklamalarını dinle (ViewComponent ile sonradan yüklenenler dahil çalışır)
    document.addEventListener('click', function (e) {

        // ---------------------------------------------------------
        // DELETE (SİLME) İŞLEMİ
        // ---------------------------------------------------------
        const deleteBtn = e.target.closest('[data-action="deleteRow"]');
        if (deleteBtn) {
            e.preventDefault(); // Varsayılan tıklamayı durdur

            const form = deleteBtn.closest('form');
            if (!form) return; // Form yoksa işlem yapma

            const row = deleteBtn.closest('tr');
            // İsim bilgisini satırdan al (ilk sütundaki strong etiketi)
            const itemName = row.querySelector('td strong')?.textContent
                || row.querySelector('td')?.textContent
                || 'Bu öğe';

            // Onaylanınca çalışacak fonksiyon
            const onConfirm = () => {
                form.submit(); // 🔥 Formu sunucuya gönder
            };

            const title = 'Silme Onayı';
            const body = `"${itemName}" silinecek.<br><small>Bu işlem geri alınabilir (soft delete).</small>`;

            // Tema Modalı varsa onu kullan, yoksa tarayıcı onay kutusu
            if (window.adminApp && window.adminApp.notifications && window.adminApp.notifications.showModal) {
                window.adminApp.notifications.showModal(title, body, onConfirm, 'danger');
            } else {
                if (confirm(`${itemName} silinsin mi?`)) {
                    onConfirm();
                }
            }
            return;
        }

        // ---------------------------------------------------------
        // RESTORE (GERİ YÜKLEME) İŞLEMİ
        // ---------------------------------------------------------
        const restoreBtn = e.target.closest('[data-action="restoreRow"]');
        if (restoreBtn) {
            e.preventDefault();

            const form = restoreBtn.closest('form');
            if (!form) return;

            const row = restoreBtn.closest('tr');
            const itemName = row.querySelector('td strong')?.textContent
                || row.querySelector('td')?.textContent
                || 'Bu öğe';

            // Onaylanınca çalışacak fonksiyon
            const onConfirm = () => {
                form.submit(); // 🔥 Formu sunucuya gönder
            };

            const title = 'Geri Yükle';
            const body = `"${itemName}" geri yüklenecek. Onaylıyor musunuz?`;

            // Tema Modalı varsa onu kullan
            if (window.adminApp && window.adminApp.notifications && window.adminApp.notifications.showModal) {
                window.adminApp.notifications.showModal(title, body, onConfirm, 'warning');
            } else {
                if (confirm(`"${itemName}" geri yüklensin mi?`)) {
                    onConfirm();
                }
            }
        }
    });

});
