/**
 * Skill Editor Module
 * Handles live preview for skill creation and editing
 */

document.addEventListener('DOMContentLoaded', function () {
    initLivePreview();
});

/**
 * Initialize live preview functionality
 */
function initLivePreview() {
    const skillName = document.getElementById('skillName');
    const skillPercent = document.getElementById('skillPercent');

    if (skillName) {
        skillName.addEventListener('input', updateSkillPreview);
    }

    if (skillPercent) {
        skillPercent.addEventListener('input', updateSkillPreview);
    }

    // İlk yüklemede preview'ı güncelle
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

    if (!skillName || !skillPercent) return;

    const name = skillName.value.trim() || 'Yetenek Adı';
    const percent = skillPercent.value;

    // Display değerlerini güncelle
    if (percentValue) percentValue.textContent = percent + '%';
    if (previewSkillName) previewSkillName.textContent = name;
    if (previewSkillPercent) previewSkillPercent.textContent = percent + '%';

    // Progress bar güncelle
    if (previewSkillProgress) {
        previewSkillProgress.style.width = percent + '%';
        previewSkillProgress.dataset.progress = percent;
    }

    // Yüzde rengini güncelle
    updatePercentColor(percent);
}

/**
 * Update percent display color based on value
 */
function updatePercentColor(percent) {
    const percentValue = document.getElementById('skillPercentValue');
    if (!percentValue) return;

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
