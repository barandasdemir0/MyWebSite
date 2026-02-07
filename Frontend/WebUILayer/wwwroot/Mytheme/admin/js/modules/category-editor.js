/**
 * Category Editor Module
 * Handles category creation and editing
 */

document.addEventListener('DOMContentLoaded', function() {
    initFormHandlers();
    checkEditMode();
});

/**
 * Initialize form handlers
 */
function initFormHandlers() {
    // Form submission
    const form = document.getElementById('categoryEditorForm');
    if (form) {
        form.addEventListener('submit', handleFormSubmit);
    }
}


/**
 * Check if we're in edit mode (URL parameter)
 */
function checkEditMode() {
    const urlParams = new URLSearchParams(window.location.search);
    const editId = urlParams.get('edit');

    if (editId) {
        // Edit mode
        document.querySelector('.page-title h1').textContent = 'Kategori Düzenle';
        document.querySelector('.page-title p').textContent = 'Mevcut kategoriyi düzenleyin';
        
        // Load category data
        loadCategoryData(editId);
    }
}

/**
 * Load category data for editing
 * Data is loaded from hidden inputs in HTML (set by Razor)
 */
function loadCategoryData(categoryId) {
    // Get data from hidden input in HTML
    const categoryDataEl = document.getElementById('categoryData');
    
    if (categoryDataEl) {
        const name = categoryDataEl.dataset.name || '';
        const description = categoryDataEl.dataset.description || '';
        const icon = categoryDataEl.dataset.icon || '';

        if (name) document.getElementById('categoryName').value = name;
        if (description) document.getElementById('categoryDescription').value = description;
        if (icon) document.getElementById('categoryIcon').value = icon;
    }
}

/**
 * Handle form submission
 */
function handleFormSubmit(e) {
    e.preventDefault();

    const name = document.getElementById('categoryName').value.trim();
    const description = document.getElementById('categoryDescription').value.trim();
    const icon = document.getElementById('categoryIcon').value.trim() || 'fas fa-code';

    // Validation
    if (!name) {
        showToast('Lütfen kategori adı girin', 'warning');
        return;
    }

    // Save category (would normally send to API)
    const categoryData = {
        name,
        description,
        icon,
        status: 'active' // Always active by default
    };

    console.log('Saving category:', categoryData);

    // Show success message
    showToast('Kategori başarıyla kaydedildi', 'success');

    // Redirect back to categories page after delay
    setTimeout(() => {
        window.location.href = 'skill-categories.html';
    }, 1500);
}

/**
 * Show toast notification
 */
function showToast(message, type = 'info') {
    // Check for global toast function
    if (typeof window.showNotification === 'function') {
        window.showNotification(message, type);
        return;
    }

    // Fallback toast
    const existingToast = document.querySelector('.ce-toast');
    if (existingToast) existingToast.remove();

    const toast = document.createElement('div');
    toast.className = `ce-toast ce-toast-${type}`;
    
    const icons = {
        success: 'check-circle',
        warning: 'exclamation-triangle',
        danger: 'times-circle',
        info: 'info-circle'
    };

    toast.innerHTML = `
        <i class="fas fa-${icons[type] || icons.info}"></i>
        <span>${message}</span>
    `;

    // Add styles
    if (!document.getElementById('ce-toast-styles')) {
        const style = document.createElement('style');
        style.id = 'ce-toast-styles';
        style.textContent = `
            .ce-toast {
                position: fixed;
                bottom: 30px;
                right: 30px;
                padding: 15px 25px;
                border-radius: 10px;
                display: flex;
                align-items: center;
                gap: 10px;
                font-weight: 500;
                z-index: 10001;
                transform: translateY(100px);
                opacity: 0;
                transition: all 0.3s ease;
                box-shadow: 0 5px 20px rgba(0, 0, 0, 0.2);
            }
            .ce-toast.show { transform: translateY(0); opacity: 1; }
            .ce-toast-success { background: var(--admin-success); color: #fff; }
            .ce-toast-warning { background: var(--admin-warning); color: #000; }
            .ce-toast-danger { background: var(--admin-danger); color: #fff; }
            .ce-toast-info { background: var(--admin-info); color: #fff; }
        `;
        document.head.appendChild(style);
    }

    document.body.appendChild(toast);
    setTimeout(() => toast.classList.add('show'), 10);
    setTimeout(() => {
        toast.classList.remove('show');
        setTimeout(() => toast.remove(), 300);
    }, 3000);
}
