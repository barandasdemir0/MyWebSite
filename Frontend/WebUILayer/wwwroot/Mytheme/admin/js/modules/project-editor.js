/**
 * Project Editor Script - Düzeltilmiş
 */
document.addEventListener('DOMContentLoaded', function () {

    const form = document.getElementById('createProjectForm') || document.getElementById('updateProjectForm');
    const SUBMIT_KEY = 'projectFormLastSubmit';
    let submitBlocked = false;

    // ✅ SAYFA YÜKLENME KORUMASI - ama return yok!
    if (form) {
        const lastSubmit = parseInt(sessionStorage.getItem(SUBMIT_KEY) || '0');
        if (Date.now() - lastSubmit < 3000) {
            console.log('🛑 Auto-submit will be blocked');
            submitBlocked = true;
            setTimeout(() => {
                sessionStorage.removeItem(SUBMIT_KEY);
                submitBlocked = false;
            }, 3000);
        }
    }

    // ✅ SELECT2 KATEGORİ - Her zaman çalışır
    setTimeout(function () {
        const categorySelector = '#projectCategory';
        if (typeof $ !== 'undefined' && $(categorySelector).length > 0) {
            if ($(categorySelector).hasClass('select2-hidden-accessible')) {
                $(categorySelector).select2('destroy');
            }
            var categorySelect = $(categorySelector).select2({
                placeholder: "Kategori seçin",
                allowClear: true,
                width: '100%'
            });
            var selectedVals = [];
            $(categorySelector + ' option[selected]').each(function () {
                selectedVals.push($(this).val());
            });
            if (selectedVals.length > 0) {
                categorySelect.val(selectedVals).trigger('change');
            }
        }
    }, 100);

    // ✅ EDITOR CONTENT YÜKLEME - Her zaman çalışır
    const editorContent = document.getElementById('editorContent');
    const hiddenInput = document.getElementById('hiddenContentInput');
    if (editorContent && hiddenInput) {
        if (hiddenInput.value && hiddenInput.value.trim() !== '') {
            editorContent.innerHTML = hiddenInput.value;
        }
        editorContent.addEventListener('input', function () {
            hiddenInput.value = this.innerHTML;
        });
    }

    // ✅ TEKNOLOJİ TAG SİSTEMİ - Her zaman çalışır
    const techTagInput = document.getElementById('techTagInput');
    const techTagsContainer = document.getElementById('techTagsContainer');
    const hiddenTech = document.getElementById('hiddenTechnologies');

    function addTag(text) {
        if (!text || !techTagsContainer || !techTagInput) return;
        const tag = document.createElement('span');
        tag.className = 'tag';
        tag.innerHTML = text + ' <span class="tag-remove">×</span>';
        tag.querySelector('.tag-remove').addEventListener('click', function () {
            tag.remove();
        });
        techTagsContainer.insertBefore(tag, techTagInput);
    }

    // Mevcut tag'ları yükle
    if (hiddenTech && hiddenTech.value && hiddenTech.value.trim() !== '') {
        hiddenTech.value.split(',').forEach(tagText => {
            if (tagText.trim()) addTag(tagText.trim());
        });
    }

    if (techTagInput) {
        techTagInput.addEventListener('keydown', function (e) {
            if (e.key === 'Enter') {
                e.preventDefault();
                const value = this.value.trim();
                if (value) {
                    addTag(value);
                    this.value = '';
                }
            }
        });
    }

    // ✅ FORM SUBMIT - Koruma burada
    if (form) {
        form.addEventListener('submit', function (e) {
            // Bloklandıysa engelle
            if (submitBlocked) {
                console.log('❌ Submit blocked');
                e.preventDefault();
                return false;
            }

            e.preventDefault();
            sessionStorage.setItem(SUBMIT_KEY, Date.now().toString());

            // Sync
            const editor = document.getElementById('editorContent');
            const hidden = document.getElementById('hiddenContentInput');
            if (editor && hidden) {
                hidden.value = editor.innerHTML;
            }

            if (techTagsContainer && hiddenTech) {
                const tags = techTagsContainer.querySelectorAll('.tag');
                const values = [];
                tags.forEach(tag => {
                    values.push(tag.textContent.replace('×', '').trim());
                });
                hiddenTech.value = values.join(',');
            }

            HTMLFormElement.prototype.submit.call(this);
        });
    }
});
