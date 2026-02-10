/**
 * Table Previews Module
 * Handles theme toggling, table interactions, restore modal, and search
 */

// Sayfa yüklenmeden önce tema tercihini uygula (FOUC önlemek için)
(function initTheme() {
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme === 'light') {
        document.documentElement.classList.add('light-mode');
    }
})();

// DOM Yüklendiğinde
document.addEventListener('DOMContentLoaded', function () {
    updateThemeIcon();
    initTableActions();
    initRestoreModal();
    initSearch();
});

/* ============================================
   THEME
   ============================================ */

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

function updateThemeIcon() {
    const icon = document.querySelector('.theme-toggle i');
    const isLight = document.documentElement.classList.contains('light-mode');

    if (icon) {
        icon.className = isLight ? 'fas fa-sun' : 'fas fa-moon';
    }
}

/* ============================================
   TABLE ACTIONS (Delete / Restore)
   ============================================ */

function initTableActions() {
    // Sil butonları (data-action ile)
    document.querySelectorAll('[data-action="deleteRow"]').forEach(function (btn) {
        btn.addEventListener('click', function () {
            handleDeleteRow(this);
        });
    });

    // Restore butonları - ikon tipi (data-action ile)
    document.querySelectorAll('[data-action="restoreRow"]').forEach(function (btn) {
        btn.addEventListener('click', function () {
            handleRestoreRow(this);
        });
    });

    // Restore butonları - yazılı buton tipi
    document.querySelectorAll('.btn-restore').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var itemName = this.closest('tr').querySelector('strong');
            var name = itemName ? itemName.textContent : 'Bu öğe';
            showRestoreModal(name, this.closest('tr'));
        });
    });

    // Task checkboxları
    document.querySelectorAll('.task-checkbox').forEach(function (checkbox) {
        checkbox.addEventListener('change', function () {
            handleTaskToggle(this);
        });
    });
}

function handleDeleteRow(button) {
    var row = button.closest('tr');
    if (!row) return;

    row.classList.add('tr-deleted');

    var deleteBtn = row.querySelector('[data-action="deleteRow"]');
    var restoreBtn = row.querySelector('[data-action="restoreRow"]');

    if (deleteBtn) deleteBtn.classList.add('hidden');
    if (restoreBtn) restoreBtn.classList.remove('hidden');

    var statusBadge = row.querySelector('.status-badge.active');
    if (statusBadge) {
        statusBadge.classList.remove('active');
        statusBadge.classList.add('danger');
        statusBadge.textContent = 'Silindi';
    }

    showToast('Öğe silindi', 'warning');
}

function handleRestoreRow(button) {
    var row = button.closest('tr');
    if (!row) return;

    row.classList.remove('tr-deleted');

    var deleteBtn = row.querySelector('[data-action="deleteRow"]');
    var restoreBtn = row.querySelector('[data-action="restoreRow"]');

    if (deleteBtn) deleteBtn.classList.remove('hidden');
    if (restoreBtn) restoreBtn.classList.add('hidden');

    var statusBadge = row.querySelector('.status-badge.danger');
    if (statusBadge && statusBadge.textContent.trim() === 'Silindi') {
        statusBadge.classList.remove('danger');
        statusBadge.classList.add('active');
        statusBadge.textContent = 'Aktif';
    }

    showToast('Öğe geri yüklendi', 'success');
}

function handleTaskToggle(checkbox) {
    var row = checkbox.closest('tr');
    var taskTitle = row.querySelector('.task-cell strong');

    if (checkbox.checked) {
        if (taskTitle) taskTitle.classList.add('text-deleted');
        showToast('Görev tamamlandı', 'success');
    } else {
        if (taskTitle) taskTitle.classList.remove('text-deleted');
    }
}

/* ============================================
   RESTORE MODAL
   ============================================ */

var restoreTargetRow = null;

function initRestoreModal() {
    var modal = document.getElementById('restoreModal');
    var cancelBtn = document.getElementById('cancelRestore');
    var confirmBtn = document.getElementById('confirmRestore');

    if (cancelBtn) {
        cancelBtn.addEventListener('click', closeRestoreModal);
    }

    if (confirmBtn) {
        confirmBtn.addEventListener('click', confirmRestore);
    }

    // Overlay tıklama ile kapat
    if (modal) {
        modal.addEventListener('click', function (e) {
            if (e.target === modal) {
                closeRestoreModal();
            }
        });
    }

    // ESC tuşu ile kapat
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape' && modal && modal.classList.contains('active')) {
            closeRestoreModal();
        }
    });
}

function showRestoreModal(itemName, row) {
    var modal = document.getElementById('restoreModal');
    var itemNameEl = document.getElementById('restoreItemName');

    if (modal && itemNameEl) {
        restoreTargetRow = row;
        itemNameEl.textContent = itemName;
        modal.classList.add('active');
    }
}

function closeRestoreModal() {
    var modal = document.getElementById('restoreModal');
    if (modal) {
        modal.classList.remove('active');
        restoreTargetRow = null;
    }
}

function confirmRestore() {
    if (restoreTargetRow) {
        restoreTargetRow.classList.remove('tr-deleted');

        var statusBadge = restoreTargetRow.querySelector('.status-badge.danger');
        if (statusBadge) {
            statusBadge.classList.remove('danger');
            statusBadge.classList.add('active');
            statusBadge.textContent = 'Aktif';
        }

        var textDel = restoreTargetRow.querySelector('.text-deleted');
        if (textDel) textDel.classList.remove('text-deleted');
    }

    closeRestoreModal();
    showToast('Öğe başarıyla geri yüklendi', 'success');
}

/* ============================================
   SEARCH / FILTER
   ============================================ */

function initSearch() {
    var searchInput = document.getElementById('tableSearch');
    if (!searchInput) return;

    searchInput.addEventListener('input', function () {
        var query = this.value.toLowerCase().trim();
        var rows = document.querySelectorAll('.admin-table tbody tr');

        rows.forEach(function (row) {
            var text = row.textContent.toLowerCase();
            if (query === '' || text.indexOf(query) > -1) {
                row.style.display = '';
            } else {
                row.style.display = 'none';
            }
        });
    });
}


/* ============================================
   TOAST NOTIFICATION
   ============================================ */

function showToast(message, type) {
    type = type || 'info';

    // Global notification fonksiyonu varsa onu kullan
    if (typeof window.showNotification === 'function') {
        window.showNotification(message, type);
        return;
    }

    // Yoksa kendi toast'ımızı oluştur
    var existingToast = document.querySelector('.table-toast');
    if (existingToast) existingToast.remove();

    var iconMap = {
        success: 'check-circle',
        warning: 'exclamation-triangle',
        danger: 'times-circle',
        info: 'info-circle'
    };

    var toast = document.createElement('div');
    toast.className = 'table-toast toast-' + type;
    toast.innerHTML = '<i class="fas fa-' + (iconMap[type] || iconMap.info) + '"></i><span>' + message + '</span>';

    document.body.appendChild(toast);

    setTimeout(function () { toast.classList.add('show'); }, 10);
    setTimeout(function () {
        toast.classList.remove('show');
        setTimeout(function () { toast.remove(); }, 300);
    }, 3000);
}
