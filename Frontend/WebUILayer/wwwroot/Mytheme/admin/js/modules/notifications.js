class NotificationSystem {
    constructor() {
        this.toastContainer = null;
        this.init();
    }

    init() {
        this.createToastContainer();
        this.setupDropdownListeners();
        this.setupNotificationPageListeners(); // Bildirim sayfası butonları
    }

    createToastContainer() {
        if (!document.querySelector('.admin-toast-container')) {
            const container = document.createElement('div');
            container.classList.add('admin-toast-container');
            document.body.appendChild(container);
            this.toastContainer = container;
        } else {
            this.toastContainer = document.querySelector('.admin-toast-container');
        }
    }

    setupDropdownListeners() {
        const notifBtnSelector = '.notification-btn';
        const dropdownSelector = '.notification-dropdown';

        document.addEventListener('click', (e) => {
            const target = e.target;
            const dropdown = document.querySelector(dropdownSelector);

            // 1. Toggle Button
            const notifBtn = target.closest(notifBtnSelector);
            if (notifBtn) {
                e.stopPropagation();

                if (window.innerWidth <= 768) {
                    window.location.href = '/Admin/Notifications/Index';
                    return;
                }

                if (dropdown) {
                    dropdown.classList.toggle('active');
                    const isActive = dropdown.classList.contains('active');

                    if (isActive) {
                        dropdown.style.opacity = "1";
                        dropdown.style.visibility = "visible";
                        dropdown.style.transform = "translateY(0) scale(1)";
                    } else {
                        dropdown.style.opacity = "";
                        dropdown.style.visibility = "";
                        dropdown.style.transform = "";
                    }
                }
                return;
            }

            // 2. View All Redirect
            if (target.closest('.view-all-btn')) {
                e.stopPropagation();
                window.location.href = '/Admin/Notifications/Index';
                return;
            }

            // 3. Mark All Read (Sadece görsel - DB'ye gitmiyor)
            if (target.closest('.mark-all-read')) {
                if (dropdown) {
                    dropdown.querySelectorAll('.notification-item.unread').forEach(item => {
                        item.classList.remove('unread');
                    });
                    this.showToast('Başarılı', 'Tüm bildirimler okundu olarak işaretlendi.', 'success');
                }
                return;
            }

            // 4. Click Outside - Kapat
            if (dropdown && dropdown.classList.contains('active')) {
                if (!dropdown.contains(target)) {
                    dropdown.classList.remove('active');
                    dropdown.style.opacity = "";
                    dropdown.style.visibility = "";
                    dropdown.style.transform = "";
                }
            }
        });

        // Hover Açma
        const notifBtn = document.querySelector(notifBtnSelector);
        const headerActions = document.querySelector('.header-actions');

        if (notifBtn) {
            notifBtn.addEventListener('mouseenter', () => {
                if (window.innerWidth <= 768) return;
                const dropdown = document.querySelector(dropdownSelector);
                if (dropdown) dropdown.classList.add('active');
            });
        }

        if (headerActions) {
            headerActions.addEventListener('mouseleave', () => {
                const dropdown = document.querySelector(dropdownSelector);
                if (dropdown) dropdown.classList.remove('active');
            });
        }
    }

    // =============================================
    // BİLDİRİM SAYFASI: Sil ve Geri Yükle Modalları
    // =============================================
    setupNotificationPageListeners() {
        let pendingDeleteForm = null;
        let pendingRestoreForm = null;

        const deleteModal = document.getElementById('notifDeleteModal');
        const restoreModal = document.getElementById('notifRestoreModal');

        // Eğer bu sayfada modal yoksa (başka sayfalarda) çık
        if (!deleteModal && !restoreModal) return;

        // --- SİLME BUTONU ---
        document.addEventListener('click', (e) => {
            const deleteBtn = e.target.closest('[data-action="deleteRow"]');
            if (deleteBtn) {
                e.preventDefault();
                pendingDeleteForm = deleteBtn.closest('.notif-delete-form');
                const name = deleteBtn.getAttribute('data-name') || 'Bu bildirim';
                const nameEl = document.getElementById('notifDeleteItemName');
                if (nameEl) nameEl.textContent = name;
                if (deleteModal) {
                    deleteModal.classList.add('active');
                }
            }
        });

        // Silme: İptal
        const cancelDelete = document.getElementById('cancelNotifDelete');
        if (cancelDelete) {
            cancelDelete.addEventListener('click', () => {
                deleteModal.classList.remove('active');
                pendingDeleteForm = null;
            });
        }

        // Silme: Onayla
        const confirmDelete = document.getElementById('confirmNotifDelete');
        if (confirmDelete) {
            confirmDelete.addEventListener('click', () => {
                if (pendingDeleteForm) {
                    pendingDeleteForm.submit();
                }
                deleteModal.classList.remove('active');
            });
        }

        // Silme: Dışarı tıkla kapat
        if (deleteModal) {
            deleteModal.addEventListener('click', (e) => {
                if (e.target === deleteModal) {
                    deleteModal.classList.remove('active');
                    pendingDeleteForm = null;
                }
            });
        }

        // --- GERİ YÜKLEME BUTONU ---
        document.addEventListener('click', (e) => {
            const restoreBtn = e.target.closest('[data-action="restoreRow"]');
            if (restoreBtn) {
                e.preventDefault();
                pendingRestoreForm = restoreBtn.closest('.notif-restore-form');
                const name = restoreBtn.getAttribute('data-name') || 'Bu bildirim';
                const nameEl = document.getElementById('notifRestoreItemName');
                if (nameEl) nameEl.textContent = name;
                if (restoreModal) {
                    restoreModal.classList.add('active');
                }
            }
        });

        // Restore: İptal
        const cancelRestore = document.getElementById('cancelNotifRestore');
        if (cancelRestore) {
            cancelRestore.addEventListener('click', () => {
                restoreModal.classList.remove('active');
                pendingRestoreForm = null;
            });
        }

        // Restore: Onayla
        const confirmRestore = document.getElementById('confirmNotifRestore');
        if (confirmRestore) {
            confirmRestore.addEventListener('click', () => {
                if (pendingRestoreForm) {
                    pendingRestoreForm.submit();
                }
                restoreModal.classList.remove('active');
            });
        }

        // Restore: Dışarı tıkla kapat
        if (restoreModal) {
            restoreModal.addEventListener('click', (e) => {
                if (e.target === restoreModal) {
                    restoreModal.classList.remove('active');
                    pendingRestoreForm = null;
                }
            });
        }
    }

    showToast(title, message, type = 'info') {
        const toast = document.createElement('div');
        toast.className = `admin-toast ${type}`;

        let icon = 'fa-info-circle';
        if (type === 'success') icon = 'fa-check-circle';
        if (type === 'error') icon = 'fa-exclamation-circle';
        if (type === 'warning') icon = 'fa-exclamation-triangle';

        toast.innerHTML = `
            <i class="fas ${icon} admin-toast-icon"></i>
            <div class="admin-toast-content">
                <div class="admin-toast-title">${title}</div>
                <div class="admin-toast-message">${message}</div>
            </div>
            <button class="admin-toast-close"><i class="fas fa-times"></i></button>
        `;

        document.querySelectorAll('.btn').forEach(btn => {
            btn.classList.add('toast-active');
            btn.blur();
        });

        toast.querySelector('.admin-toast-close').addEventListener('click', () => {
            this.closeToast(toast);
        });

        setTimeout(() => this.closeToast(toast), 4000);
        this.toastContainer.appendChild(toast);
    }

    closeToast(toast) {
        toast.classList.add('closing');
        toast.addEventListener('animationend', () => {
            toast.remove();
            document.querySelectorAll('.btn').forEach(btn => {
                btn.classList.remove('toast-active');
                btn.blur();
            });
        });
    }

    showModal(title, message, onConfirmOrType, type = 'danger') {
        let onConfirm = null;
        let modalType = type;

        if (typeof onConfirmOrType === 'function') {
            onConfirm = onConfirmOrType;
        } else if (typeof onConfirmOrType === 'string') {
            modalType = onConfirmOrType;
        }

        const existingModal = document.querySelector('.admin-confirmation-modal');
        if (existingModal) existingModal.remove();

        const overlay = document.createElement('div');
        overlay.classList.add('admin-modal-overlay', 'admin-confirmation-modal');

        let icon = modalType === 'danger' ? 'fa-trash-alt' :
            modalType === 'error' ? 'fa-exclamation-circle' :
                modalType === 'warning' ? 'fa-exclamation-triangle' : 'fa-info-circle';

        const footerHtml = onConfirm
            ? `<button class="btn btn-secondary cancel-btn">Vazgeç</button>
               <button class="btn btn-danger confirm-btn">Onayla</button>`
            : `<button class="btn btn-primary confirm-btn">Tamam</button>`;

        overlay.innerHTML = `
            <div class="admin-modal">
                <div class="modal-header">
                    <div class="modal-icon">
                        <i class="fas ${icon}"></i>
                    </div>
                    <div class="modal-title">${title}</div>
                </div>
                <div class="modal-body">${message}</div>
                <div class="modal-footer">${footerHtml}</div>
            </div>
        `;

        document.body.appendChild(overlay);
        requestAnimationFrame(() => overlay.classList.add('active'));

        const close = () => {
            overlay.classList.remove('active');
            setTimeout(() => overlay.remove(), 300);
        };

        const cancelBtn = overlay.querySelector('.cancel-btn');
        if (cancelBtn) cancelBtn.addEventListener('click', close);

        const confirmBtn = overlay.querySelector('.confirm-btn');
        if (confirmBtn) {
            confirmBtn.addEventListener('click', () => {
                close();
                if (onConfirm) onConfirm();
            });
        }

        overlay.addEventListener('click', (e) => {
            if (e.target === overlay) close();
        });
    }
}

// Pagination
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
                    const nextBtn = Array.from(paginationBtns).find(b =>
                        !b.querySelector('i') && parseInt(b.textContent) === currentPage + 1
                    );
                    if (nextBtn) nextBtn.click();
                } else {
                    const prevBtn = Array.from(paginationBtns).find(b =>
                        !b.querySelector('i') && parseInt(b.textContent) === currentPage - 1
                    );
                    if (prevBtn) prevBtn.click();
                }
            }

            updateNotificationsPaginationArrows();
        });
    });

    function updateNotificationsPaginationArrows() {
        const currentActive = document.querySelector('.pagination-btn.active');
        const currentPage = parseInt(currentActive?.textContent || 1);
        const allPages = Array.from(document.querySelectorAll('.pagination-btn'))
            .filter(b => !b.querySelector('i'))
            .map(b => parseInt(b.textContent));

        const minPage = Math.min(...allPages);
        const maxPage = Math.max(...allPages);

        const prevBtn = document.getElementById('prevPage');
        const nextBtn = document.getElementById('nextPage');

        if (prevBtn) prevBtn.disabled = currentPage <= minPage;
        if (nextBtn) nextBtn.disabled = currentPage >= maxPage;
    }

    updateNotificationsPaginationArrows();
});
const notificationSystem = new NotificationSystem();
