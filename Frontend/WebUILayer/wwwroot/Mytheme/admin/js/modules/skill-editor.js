/**
 * Skill Editor Module
 * Handles skill creation and editing with live preview
 */

document.addEventListener('DOMContentLoaded', function() {
    initFormHandlers();
    initLivePreview();
    checkEditMode();
});

/**
 * Initialize form handlers
 */
function initFormHandlers() {
    // Form submission
    const form = document.getElementById('skillEditorForm');
    if (form) {
        form.addEventListener('submit', handleFormSubmit);
    }
}

/**
 * Initialize live preview functionality
 */
function initLivePreview() {
    // Category select
    const categorySelect = document.getElementById('skillCategory');
    if (categorySelect) {
        categorySelect.addEventListener('change', updateCategoryPreview);
    }

    // Skill name input
    const skillName = document.getElementById('skillName');
    if (skillName) {
        skillName.addEventListener('input', updateSkillPreview);
    }

    // Skill percent range
    const skillPercent = document.getElementById('skillPercent');
    if (skillPercent) {
        skillPercent.addEventListener('input', updateSkillPreview);
    }

    // Initialize preview
    updateSkillPreview();
}

/**
 * Update category preview based on selection
 */
function updateCategoryPreview() {
    const categorySelect = document.getElementById('skillCategory');
    const selectedOption = categorySelect.options[categorySelect.selectedIndex];
    
    if (!categorySelect.value) {
        return;
    }

    // Get data from HTML data attributes
    const name = selectedOption.dataset.name || selectedOption.text;
    const description = selectedOption.dataset.description || '';
    const icon = selectedOption.dataset.icon || 'fas fa-code';

    const previewCard = document.getElementById('previewCard');
    const categoryIcon = document.getElementById('previewCategoryIcon');
    const categoryTitle = document.getElementById('previewCategoryTitle');
    const categoryDesc = document.getElementById('previewCategoryDesc');

    // Update icon
    categoryIcon.innerHTML = `<i class="${icon}"></i>`;

    // Update title and description
    categoryTitle.textContent = name;
    categoryDesc.textContent = description;

    // Update progress bar color
    updateSkillPreview();
}

/**
 * Update skill preview (name and percentage)
 */
function updateSkillPreview() {
    const skillName = document.getElementById('skillName');
    const skillPercent = document.getElementById('skillPercent');
    const percentValue = document.getElementById('skillPercentValue');
    const previewSkillName = document.getElementById('previewSkillName');
    const previewSkillPercent = document.getElementById('previewSkillPercent');
    const previewSkillProgress = document.getElementById('previewSkillProgress');

    const name = skillName.value.trim() || 'Yetenek Adı';
    const percent = skillPercent.value;

    // Update display values
    percentValue.textContent = percent + '%';
    previewSkillName.textContent = name;
    previewSkillPercent.textContent = percent + '%';
    
    // Update progress bar
    previewSkillProgress.style.width = percent + '%';
    previewSkillProgress.dataset.progress = percent;

    // Update range value color based on percentage
    updatePercentColor(percent);
}

/**
 * Update percent display color based on value
 */
function updatePercentColor(percent) {
    const percentValue = document.getElementById('skillPercentValue');
    
    if (percent >= 80) {
        percentValue.style.color = 'var(--admin-success)';
    } else if (percent >= 50) {
        percentValue.style.color = 'var(--admin-primary)';
    } else if (percent >= 25) {
        percentValue.style.color = 'var(--admin-warning)';
    } else {
        percentValue.style.color = 'var(--admin-danger)';
    }
}

/**
 * Check if we're in edit mode (URL parameter)
 */
function checkEditMode() {
    const urlParams = new URLSearchParams(window.location.search);
    const editId = urlParams.get('edit');
    const categoryId = urlParams.get('category');

    if (editId) {
        // Edit mode
        document.querySelector('.page-title h1').textContent = 'Yetenek Düzenle';
        document.querySelector('.page-title p').textContent = 'Mevcut yeteneği düzenleyin';
        
        // Load skill data (would come from API/database)
        loadSkillData(editId);
    }

    if (categoryId) {
        // Pre-select category
        const categorySelect = document.getElementById('skillCategory');
        if (categorySelect) {
            categorySelect.value = categoryId;
            updateCategoryPreview();
        }
    }
}

/**
 * Load skill data for editing
 * Data is loaded from hidden inputs in HTML (set by Razor)
 */
function loadSkillData(skillId) {
    // Get data from hidden inputs or data attributes in HTML
    const skillDataEl = document.getElementById('skillData');
    
    if (skillDataEl) {
        const name = skillDataEl.dataset.name || '';
        const percent = skillDataEl.dataset.percent || 80;
        const category = skillDataEl.dataset.category || '';

        if (name) document.getElementById('skillName').value = name;
        if (percent) document.getElementById('skillPercent').value = percent;
        if (category) document.getElementById('skillCategory').value = category;

        updateCategoryPreview();
        updateSkillPreview();
    }
}

/**
 * Handle form submission
 */
function handleFormSubmit(e) {
    e.preventDefault();

    const category = document.getElementById('skillCategory').value;
    const name = document.getElementById('skillName').value.trim();
    const percent = document.getElementById('skillPercent').value;

    // Validation
    if (!category) {
        showToast('Lütfen bir kategori seçin', 'warning');
        return;
    }

    if (!name) {
        showToast('Lütfen yetenek adı girin', 'warning');
        return;
    }

    // Save skill (would normally send to API)
    const skillData = {
        category,
        name,
        percent: parseInt(percent),
        status: 'active' // Always active by default
    };

    console.log('Saving skill:', skillData);

    // Show success message
    showToast('Yetenek başarıyla kaydedildi', 'success');

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
    const existingToast = document.querySelector('.se-toast');
    if (existingToast) existingToast.remove();

    const toast = document.createElement('div');
    toast.className = `se-toast se-toast-${type}`;
    
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
    if (!document.getElementById('se-toast-styles')) {
        const style = document.createElement('style');
        style.id = 'se-toast-styles';
        style.textContent = `
            .se-toast {
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
            .se-toast.show { transform: translateY(0); opacity: 1; }
            .se-toast-success { background: var(--admin-success); color: #fff; }
            .se-toast-warning { background: var(--admin-warning); color: #000; }
            .se-toast-danger { background: var(--admin-danger); color: #fff; }
            .se-toast-info { background: var(--admin-info); color: #fff; }
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
