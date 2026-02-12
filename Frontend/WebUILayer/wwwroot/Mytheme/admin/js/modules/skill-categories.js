/**
 * Skill Categories Module (MVC Version)
 * JS sadece modal açma/kapama ve animasyon yapıyor
 */

document.addEventListener('DOMContentLoaded', function () {
    initModals();
    initCategoryActions();
    animateProgressBars();
});

/**
 * Initialize modals
 */
function initModals() {
    document.querySelectorAll('[data-action="closeSkillsViewModal"]').forEach(function (btn) {
        btn.addEventListener('click', closeSkillsViewModal);
    });

    document.querySelectorAll('.modal').forEach(function (modal) {
        modal.addEventListener('click', function (e) {
            if (e.target === this) {
                this.classList.remove('visible');
            }
        });
    });

    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') {
            document.querySelectorAll('.modal.visible').forEach(function (modal) {
                modal.classList.remove('visible');
            });
        }
    });
}

/**
 * Initialize category table actions
 */
function initCategoryActions() {
    // Manage skills — modal aç
    document.querySelectorAll('[data-action="manageSkills"]').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var row = this.closest('tr');
            var categoryId = row.dataset.categoryId;
            openSkillsViewModal(categoryId);
        });
    });

    // Preview card click — modal aç
    document.querySelectorAll('.skill-category-card').forEach(function (card) {
        card.addEventListener('click', function () {
            var categoryId = this.dataset.categoryId;
            openSkillsViewModal(categoryId);
        });
    });
}

/**
 * Open skills view modal — sadece panel göster/gizle
 */
function openSkillsViewModal(categoryId) {
    var modal = document.getElementById('skillsViewModal');

    // Tüm panelleri gizle
    modal.querySelectorAll('.skills-panel').forEach(function (p) {
        p.classList.add('hidden');
    });

    // İlgili paneli göster
    var panel = modal.querySelector('.skills-panel[data-panel-id="' + categoryId + '"]');
    if (panel) {
        panel.classList.remove('hidden');

        // Progress bar animasyonları
        panel.querySelectorAll('.skill-view-progress').forEach(function (bar) {
            var width = bar.style.width;
            bar.style.width = '0%';
            setTimeout(function () {
                bar.style.width = width;
            }, 100);
        });
    }

    modal.classList.add('visible');
}

/**
 * Close skills view modal
 */
function closeSkillsViewModal() {
    var modal = document.getElementById('skillsViewModal');
    if (modal) {
        modal.classList.remove('visible');
    }
}

/**
 * Animate progress bars on load
 */
function animateProgressBars() {
    document.querySelectorAll('.skill-progress').forEach(function (bar) {
        var progress = bar.dataset.progress || 0;
        bar.style.width = '0%';
        setTimeout(function () {
            bar.style.width = progress + '%';
        }, 100);
    });
}
