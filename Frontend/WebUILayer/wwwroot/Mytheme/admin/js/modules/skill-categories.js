/**
 * Skill Categories Module
 * Handles category and skill management for the skill-categories page
 */

document.addEventListener('DOMContentLoaded', function() {
    // Initialize
    initModals();
    initCategoryActions();
    animateProgressBars();
});

/**
 * Initialize modals
 */
function initModals() {
    // Skills View Modal - close button
    document.querySelectorAll('[data-action="closeSkillsViewModal"]').forEach(btn => {
        btn.addEventListener('click', closeSkillsViewModal);
    });

    // Close on overlay click
    document.querySelectorAll('.modal').forEach(modal => {
        modal.addEventListener('click', function(e) {
            if (e.target === this) {
                this.classList.remove('visible');
            }
        });
    });

    // Close on Escape key
    document.addEventListener('keydown', function(e) {
        if (e.key === 'Escape') {
            document.querySelectorAll('.modal.visible').forEach(modal => {
                modal.classList.remove('visible');
            });
        }
    });
}

/**
 * Initialize category table actions
 */
function initCategoryActions() {
    // Edit category - Redirect to category-editor.html
    document.querySelectorAll('[data-action="editCategory"]').forEach(btn => {
        btn.addEventListener('click', function() {
            const row = this.closest('tr');
            const categoryId = row.dataset.categoryId;
            window.location.href = `category-editor.html?edit=${categoryId}`;
        });
    });

    // Delete category
    document.querySelectorAll('[data-action="deleteCategory"]').forEach(btn => {
        btn.addEventListener('click', function() {
            handleDeleteCategory(this);
        });
    });

    // Restore category
    document.querySelectorAll('[data-action="restoreCategory"]').forEach(btn => {
        btn.addEventListener('click', function() {
            handleRestoreCategory(this);
        });
    });

    // Manage skills button in table
    document.querySelectorAll('[data-action="manageSkills"]').forEach(btn => {
        btn.addEventListener('click', function() {
            const row = this.closest('tr');
            const categoryId = row.dataset.categoryId;
            openSkillsViewModal(categoryId);
        });
    });

    // Preview card clicks - Open skills view modal
    document.querySelectorAll('.skill-category-card').forEach(card => {
        card.addEventListener('click', function() {
            const categoryId = this.dataset.categoryId;
            openSkillsViewModal(categoryId);
        });
    });
}

/**
 * Handle delete category (mark as deleted, don't remove)
 */
function handleDeleteCategory(btn) {
    const row = btn.closest('tr');
    const categoryId = row.dataset.categoryId;
    
    // Mark row as deleted
    row.classList.add('deleted-item');
    
    // Toggle buttons
    const deleteBtn = row.querySelector('[data-action="deleteCategory"]');
    const restoreBtn = row.querySelector('[data-action="restoreCategory"]');
    
    if (deleteBtn) deleteBtn.classList.add('hidden');
    if (restoreBtn) restoreBtn.classList.remove('hidden');
    
    // Update status badge
    const statusBadge = row.querySelector('.status-badge');
    if (statusBadge) {
        statusBadge.className = 'status-badge danger';
        statusBadge.textContent = 'Silindi';
    }
    
    // Mark preview card as deleted
    const card = document.querySelector(`.skill-category-card[data-category-id="${categoryId}"]`);
    if (card) {
        card.classList.add('deleted-card');
    }
    
    showToast('Kategori silindi', 'warning');
}

/**
 * Handle restore category
 */
function handleRestoreCategory(btn) {
    const row = btn.closest('tr');
    const categoryId = row.dataset.categoryId;
    
    // Remove deleted state
    row.classList.remove('deleted-item');
    
    // Toggle buttons
    const deleteBtn = row.querySelector('[data-action="deleteCategory"]');
    const restoreBtn = row.querySelector('[data-action="restoreCategory"]');
    
    if (deleteBtn) deleteBtn.classList.remove('hidden');
    if (restoreBtn) restoreBtn.classList.add('hidden');
    
    // Update status badge
    const statusBadge = row.querySelector('.status-badge');
    if (statusBadge) {
        statusBadge.className = 'status-badge active';
        statusBadge.textContent = 'Aktif';
    }
    
    // Remove deleted state from preview card
    const card = document.querySelector(`.skill-category-card[data-category-id="${categoryId}"]`);
    if (card) {
        card.classList.remove('deleted-card');
    }
    
    showToast('Kategori geri yüklendi', 'success');
}

/**
 * Open skills view modal
 */
function openSkillsViewModal(categoryId) {
    const modal = document.getElementById('skillsViewModal');
    const card = document.querySelector(`.skill-category-card[data-category-id="${categoryId}"]`);
    
    if (!card) return;

    // Get category info
    const title = card.querySelector('.category-title').textContent;
    const desc = card.querySelector('.category-description').textContent;
    const iconEl = card.querySelector('.category-icon i');
    const iconClass = iconEl ? iconEl.className : 'fas fa-code';
    const iconWrapper = card.querySelector('.category-icon');
    
    // Update modal header
    document.getElementById('modalCategoryTitle').textContent = title;
    document.getElementById('modalCategoryDesc').textContent = desc;
    document.getElementById('modalCategoryIcon').innerHTML = `<i class="${iconClass}"></i>`;
    
    // Copy icon background style
    if (iconWrapper) {
        const computedStyle = window.getComputedStyle(iconWrapper);
        document.getElementById('modalCategoryIcon').style.background = computedStyle.background;
    }

    // Get skills from card
    const skills = card.querySelectorAll('.skill-item');
    const skillsList = document.getElementById('skillsViewList');
    
    // Build skills list
    skillsList.innerHTML = '';
    
    if (skills.length === 0) {
        skillsList.innerHTML = '<p class="no-skills-msg">Bu kategoride henüz yetenek bulunmuyor.</p>';
    } else {
        skills.forEach((skill, index) => {
            const name = skill.querySelector('.skill-name').textContent;
            const percent = skill.querySelector('.skill-percent').textContent;
            const progress = skill.querySelector('.skill-progress').dataset.progress;
            const isDeleted = skill.classList.contains('deleted-skill');
            
            const skillHtml = `
                <div class="skill-view-item ${isDeleted ? 'deleted-item' : ''}" data-skill-index="${index}" data-category-id="${categoryId}">
                    <div class="skill-view-info">
                        <div class="skill-view-header">
                            <span class="skill-view-name">${name}</span>
                            <span class="skill-view-percent">${percent}</span>
                        </div>
                        <div class="skill-view-bar">
                            <div class="skill-view-progress" style="width: ${progress}%"></div>
                        </div>
                    </div>
                    <div class="skill-view-actions">
                        <button class="action-btn edit ${isDeleted ? 'hidden' : ''}" title="Düzenle" data-action="editSkill" data-skill-id="${index}">
                            <i class="fas fa-pen"></i>
                        </button>
                        <button class="action-btn delete ${isDeleted ? 'hidden' : ''}" title="Sil" data-action="deleteSkill" data-skill-id="${index}">
                            <i class="fas fa-trash"></i>
                        </button>
                        <button class="action-btn restore ${isDeleted ? '' : 'hidden'}" title="Geri Yükle" data-action="restoreSkill" data-skill-id="${index}">
                            <i class="fas fa-undo"></i>
                        </button>
                    </div>
                </div>
            `;
            skillsList.insertAdjacentHTML('beforeend', skillHtml);
        });
        
        // Bind skill actions
        bindSkillActions(categoryId);
    }

    // Update "Yeni Yetenek Ekle" link with category ID
    const addSkillBtn = document.getElementById('addNewSkillBtn');
    if (addSkillBtn) {
        addSkillBtn.href = `skill-editor.html?category=${categoryId}`;
    }

    modal.classList.add('visible');
}

/**
 * Bind skill edit/delete/restore actions in modal
 */
function bindSkillActions(categoryId) {
    // Edit skill - redirect to skill-editor.html
    document.querySelectorAll('[data-action="editSkill"]').forEach(btn => {
        btn.addEventListener('click', function(e) {
            e.stopPropagation();
            const skillId = this.dataset.skillId;
            // Redirect to skill editor with edit parameter
            window.location.href = `skill-editor.html?category=${categoryId}&edit=${skillId}`;
        });
    });

    // Delete skill (mark as deleted, don't remove)
    document.querySelectorAll('[data-action="deleteSkill"]').forEach(btn => {
        btn.addEventListener('click', function(e) {
            e.stopPropagation();
            handleDeleteSkill(this, categoryId);
        });
    });

    // Restore skill
    document.querySelectorAll('[data-action="restoreSkill"]').forEach(btn => {
        btn.addEventListener('click', function(e) {
            e.stopPropagation();
            handleRestoreSkill(this, categoryId);
        });
    });
}

/**
 * Handle delete skill (mark as deleted)
 */
function handleDeleteSkill(btn, categoryId) {
    const item = btn.closest('.skill-view-item');
    const skillIndex = item.dataset.skillIndex;
    
    // Mark item as deleted
    item.classList.add('deleted-item');
    
    // Toggle buttons
    const editBtn = item.querySelector('[data-action="editSkill"]');
    const deleteBtn = item.querySelector('[data-action="deleteSkill"]');
    const restoreBtn = item.querySelector('[data-action="restoreSkill"]');
    
    if (editBtn) editBtn.classList.add('hidden');
    if (deleteBtn) deleteBtn.classList.add('hidden');
    if (restoreBtn) restoreBtn.classList.remove('hidden');
    
    // Mark skill in preview card as deleted
    const card = document.querySelector(`.skill-category-card[data-category-id="${categoryId}"]`);
    if (card) {
        const skills = card.querySelectorAll('.skill-item');
        if (skills[skillIndex]) {
            skills[skillIndex].classList.add('deleted-skill');
        }
    }
    
    showToast('Yetenek silindi', 'warning');
}

/**
 * Handle restore skill
 */
function handleRestoreSkill(btn, categoryId) {
    const item = btn.closest('.skill-view-item');
    const skillIndex = item.dataset.skillIndex;
    
    // Remove deleted state
    item.classList.remove('deleted-item');
    
    // Toggle buttons
    const editBtn = item.querySelector('[data-action="editSkill"]');
    const deleteBtn = item.querySelector('[data-action="deleteSkill"]');
    const restoreBtn = item.querySelector('[data-action="restoreSkill"]');
    
    if (editBtn) editBtn.classList.remove('hidden');
    if (deleteBtn) deleteBtn.classList.remove('hidden');
    if (restoreBtn) restoreBtn.classList.add('hidden');
    
    // Remove deleted state from skill in preview card
    const card = document.querySelector(`.skill-category-card[data-category-id="${categoryId}"]`);
    if (card) {
        const skills = card.querySelectorAll('.skill-item');
        if (skills[skillIndex]) {
            skills[skillIndex].classList.remove('deleted-skill');
        }
    }
    
    showToast('Yetenek geri yüklendi', 'success');
}

/**
 * Close skills view modal
 */
function closeSkillsViewModal() {
    const modal = document.getElementById('skillsViewModal');
    if (modal) {
        modal.classList.remove('visible');
    }
}

/**
 * Animate progress bars on load
 */
function animateProgressBars() {
    const progressBars = document.querySelectorAll('.skill-progress');
    
    progressBars.forEach(bar => {
        const progress = bar.dataset.progress || 0;
        bar.style.width = '0%';
        
        setTimeout(() => {
            bar.style.width = progress + '%';
        }, 100);
    });
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
    const existingToast = document.querySelector('.sc-toast');
    if (existingToast) existingToast.remove();
    
    const toast = document.createElement('div');
    toast.className = `sc-toast sc-toast-${type}`;
    
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
    if (!document.getElementById('sc-toast-styles')) {
        const style = document.createElement('style');
        style.id = 'sc-toast-styles';
        style.textContent = `
            .sc-toast {
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
            .sc-toast.show { transform: translateY(0); opacity: 1; }
            .sc-toast-success { background: var(--admin-success); color: #fff; }
            .sc-toast-warning { background: var(--admin-warning); color: #000; }
            .sc-toast-danger { background: var(--admin-danger); color: #fff; }
            .sc-toast-info { background: var(--admin-info); color: #fff; }
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
