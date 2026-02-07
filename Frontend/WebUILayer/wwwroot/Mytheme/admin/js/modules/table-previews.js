/**
 * Table Previews Module
 * Handles theme toggling and table interactions for table-previews.html
 */

// DOM Ready
document.addEventListener('DOMContentLoaded', function() {
    initThemeToggle();
    initTableActions();
    initRestoreModal();
    updateThemeIcon();
});

/**
 * Initialize theme toggle button
 */
function initThemeToggle() {
    const themeToggle = document.querySelector('.theme-toggle');
    
    if (themeToggle) {
        themeToggle.addEventListener('click', toggleTheme);
    }
}

/**
 * Toggle theme between dark and light mode
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
 * Update theme toggle icon based on current theme
 */
function updateThemeIcon() {
    const icon = document.querySelector('.theme-toggle i');
    const isLight = document.documentElement.classList.contains('light-mode');

    if (icon) {
        icon.className = isLight ? 'fas fa-sun' : 'fas fa-moon';
    }
}

/**
 * Initialize table action buttons (delete, restore, etc.)
 */
function initTableActions() {
    // Delete buttons
    document.querySelectorAll('.action-btn.delete[data-action="deleteRow"]').forEach(btn => {
        btn.addEventListener('click', function() {
            handleDeleteRow(this);
        });
    });

    // Restore buttons (icon style)
    document.querySelectorAll('.action-btn.restore[data-action="restoreRow"]').forEach(btn => {
        btn.addEventListener('click', function() {
            handleRestoreRow(this);
        });
    });

    // Restore buttons (button style)
    document.querySelectorAll('.btn-restore').forEach(btn => {
        btn.addEventListener('click', function() {
            handleRestoreRow(this);
        });
    });

    // Task checkboxes
    document.querySelectorAll('.task-checkbox').forEach(checkbox => {
        checkbox.addEventListener('change', function() {
            handleTaskToggle(this);
        });
    });
}

/**
 * Handle row deletion
 * @param {HTMLElement} button - The delete button clicked
 */
function handleDeleteRow(button) {
    const row = button.closest('tr');
    
    if (!row) return;

    // Add deleted styling
    row.classList.add('deleted-item');
    
    // Find and toggle buttons
    const deleteBtn = row.querySelector('.action-btn.delete');
    const restoreBtn = row.querySelector('.action-btn.restore');
    
    if (deleteBtn) deleteBtn.classList.add('hidden');
    if (restoreBtn) restoreBtn.classList.remove('hidden');

    // Update status badge if exists
    const statusBadge = row.querySelector('.status-badge.active');
    if (statusBadge) {
        statusBadge.classList.remove('active');
        statusBadge.classList.add('danger');
        statusBadge.textContent = 'Silindi';
    }

    // Show toast notification
    showToast('Öğe silindi', 'warning');
}

/**
 * Handle row restoration
 * @param {HTMLElement} button - The restore button clicked
 */
function handleRestoreRow(button) {
    const row = button.closest('tr');
    
    if (!row) return;

    // Remove deleted styling
    row.classList.remove('deleted-item');
    
    // Find and toggle buttons
    const deleteBtn = row.querySelector('.action-btn.delete');
    const restoreBtn = row.querySelector('.action-btn.restore');
    
    if (deleteBtn) deleteBtn.classList.remove('hidden');
    if (restoreBtn) restoreBtn.classList.add('hidden');

    // Update status badge if exists
    const statusBadge = row.querySelector('.status-badge.danger');
    if (statusBadge && statusBadge.textContent === 'Silindi') {
        statusBadge.classList.remove('danger');
        statusBadge.classList.add('active');
        statusBadge.textContent = 'Sitede Aktif';
    }

    // Show toast notification
    showToast('Öğe geri yüklendi', 'success');
}

/**
 * Handle task checkbox toggle
 * @param {HTMLElement} checkbox - The checkbox element
 */
function handleTaskToggle(checkbox) {
    const row = checkbox.closest('tr');
    const taskTitle = row.querySelector('.task-cell strong');
    
    if (checkbox.checked) {
        if (taskTitle) {
            taskTitle.classList.add('text-deleted');
        }
        showToast('Görev tamamlandı', 'success');
    } else {
        if (taskTitle) {
            taskTitle.classList.remove('text-deleted');
        }
    }
}

/**
 * Initialize restore modal functionality
 */
function initRestoreModal() {
    const modal = document.getElementById('restoreModal');
    const cancelBtn = document.getElementById('cancelRestore');
    const confirmBtn = document.getElementById('confirmRestore');

    if (cancelBtn) {
        cancelBtn.addEventListener('click', closeRestoreModal);
    }

    if (confirmBtn) {
        confirmBtn.addEventListener('click', confirmRestore);
    }

    // Close on overlay click
    if (modal) {
        modal.addEventListener('click', function(e) {
            if (e.target === modal) {
                closeRestoreModal();
            }
        });
    }

    // Close on escape key
    document.addEventListener('keydown', function(e) {
        if (e.key === 'Escape' && modal && modal.classList.contains('active')) {
            closeRestoreModal();
        }
    });
}

/**
 * Show restore modal
 * @param {string} itemName - Name of the item to restore
 * @param {HTMLElement} row - The table row element
 */
function showRestoreModal(itemName, row) {
    const modal = document.getElementById('restoreModal');
    const itemNameEl = document.getElementById('restoreItemName');

    if (modal && itemNameEl) {
        itemNameEl.textContent = itemName;
        modal.classList.add('active');
        modal.dataset.targetRow = row;
    }
}

/**
 * Close restore modal
 */
function closeRestoreModal() {
    const modal = document.getElementById('restoreModal');
    
    if (modal) {
        modal.classList.remove('active');
    }
}

/**
 * Confirm restore action
 */
function confirmRestore() {
    // Implementation for modal-based restore
    closeRestoreModal();
    showToast('Öğe başarıyla geri yüklendi', 'success');
}

/**
 * Show toast notification
 * @param {string} message - The message to display
 * @param {string} type - Type of toast (success, warning, danger, info)
 */
function showToast(message, type = 'info') {
    // Check if there's a global toast function
    if (typeof window.showNotification === 'function') {
        window.showNotification(message, type);
        return;
    }

    // Fallback: Create simple toast
    const existingToast = document.querySelector('.table-toast');
    if (existingToast) {
        existingToast.remove();
    }

    const toast = document.createElement('div');
    toast.className = `table-toast toast-${type}`;
    toast.innerHTML = `
        <i class="fas fa-${getToastIcon(type)}"></i>
        <span>${message}</span>
    `;

    // Add toast styles if not exists
    addToastStyles();

    document.body.appendChild(toast);

    // Trigger animation
    setTimeout(() => toast.classList.add('show'), 10);

    // Auto remove
    setTimeout(() => {
        toast.classList.remove('show');
        setTimeout(() => toast.remove(), 300);
    }, 3000);
}

/**
 * Get icon for toast type
 * @param {string} type - Toast type
 * @returns {string} Icon class name
 */
function getToastIcon(type) {
    const icons = {
        success: 'check-circle',
        warning: 'exclamation-triangle',
        danger: 'times-circle',
        info: 'info-circle'
    };
    return icons[type] || icons.info;
}

/**
 * Add toast styles dynamically
 */
function addToastStyles() {
    if (document.getElementById('table-toast-styles')) return;

    const styles = document.createElement('style');
    styles.id = 'table-toast-styles';
    styles.textContent = `
        .table-toast {
            position: fixed;
            bottom: 30px;
            right: 30px;
            padding: 15px 25px;
            border-radius: 10px;
            display: flex;
            align-items: center;
            gap: 10px;
            font-weight: 500;
            z-index: 10000;
            transform: translateY(100px);
            opacity: 0;
            transition: all 0.3s ease;
            box-shadow: 0 5px 20px rgba(0, 0, 0, 0.2);
        }
        
        .table-toast.show {
            transform: translateY(0);
            opacity: 1;
        }
        
        .table-toast.toast-success {
            background: var(--admin-success);
            color: #fff;
        }
        
        .table-toast.toast-warning {
            background: var(--admin-warning);
            color: #000;
        }
        
        .table-toast.toast-danger {
            background: var(--admin-danger);
            color: #fff;
        }
        
        .table-toast.toast-info {
            background: var(--admin-info);
            color: #fff;
        }
    `;
    document.head.appendChild(styles);
}
