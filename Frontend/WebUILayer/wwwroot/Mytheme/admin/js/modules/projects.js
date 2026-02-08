// Projects page functionality
document.addEventListener('DOMContentLoaded', function () {
    const projectForm = document.getElementById('projectForm');
    const projectFontSizeSelect = document.getElementById('projectFontSizeSelect');

    // Show form button
    const showFormBtns = document.querySelectorAll('[data-action="showForm"]');
    showFormBtns.forEach(btn => {
        btn.addEventListener('click', function () {
            if (projectForm) projectForm.classList.add('visible');
        });
    });

    // Hide form button
    const hideFormBtns = document.querySelectorAll('[data-action="hideForm"]');
    hideFormBtns.forEach(btn => {
        btn.addEventListener('click', function () {
            if (projectForm) projectForm.classList.remove('visible');
        });
    });

    // Font size selection for editor
    if (projectFontSizeSelect) {
        projectFontSizeSelect.addEventListener('change', function () {
            const editor = document.getElementById('projectEditor');
            if (editor) {
                editor.focus();
                document.execCommand('fontSize', false, this.value);
            }
        });
    }

    // Text color picker
    const textColorPicker = document.getElementById('projectTextColorPicker');
    if (textColorPicker) {
        textColorPicker.addEventListener('change', function () {
            document.getElementById('projectTextColorIndicator').style.background = this.value;
            const editor = document.getElementById('projectEditor');
            if (editor) editor.focus();
            document.execCommand('foreColor', false, this.value);
        });
    }

    // Background color picker
    const bgColorPicker = document.getElementById('projectBgColorPicker');
    if (bgColorPicker) {
        bgColorPicker.addEventListener('change', function () {
            const editor = document.getElementById('projectEditor');
            if (editor) editor.focus();
            document.execCommand('backColor', false, this.value);
        });
    }

    // Add tag input handler
    const tagInput = document.querySelector('[data-action="addTag"]');
    if (tagInput) {
        tagInput.addEventListener('keydown', function (event) {
            if (event.key === 'Enter') {
                event.preventDefault();
                addProjectTag(event);
            }
        });
    }

    // ====================================
    // RESTORE MODAL
    // ====================================
    let restoreForm = null;
    const restoreModal = document.getElementById('restoreModal');
    const cancelRestore = document.getElementById('cancelRestore');
    const confirmRestore = document.getElementById('confirmRestore');

    document.querySelectorAll('[data-action="restoreItem"]').forEach(btn => {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            e.stopPropagation();

            restoreForm = this.closest('form');
            const row = this.closest('tr');
            const itemName = row.querySelector('td[data-label="Proje Adı"] strong')?.textContent || 'Bu öğe';

            if (restoreModal) {
                document.getElementById('restoreItemName').textContent = itemName;
                restoreModal.classList.add('active');
            }
        });
    });

    if (cancelRestore) {
        cancelRestore.addEventListener('click', function () {
            restoreModal.classList.remove('active');
            restoreForm = null;
        });
    }

    if (confirmRestore) {
        confirmRestore.addEventListener('click', function () {
            restoreModal.classList.remove('active');
            if (restoreForm) {
                restoreForm.submit();
            }
        });
    }

    if (restoreModal) {
        restoreModal.addEventListener('click', function (e) {
            if (e.target === this) {
                this.classList.remove('active');
                restoreForm = null;
            }
        });
    }

    // ====================================
    // DELETE MODAL
    // ====================================
    let deleteForm = null;
    const deleteModal = document.getElementById('deleteModal');
    const cancelDelete = document.getElementById('cancelDelete');
    const confirmDelete = document.getElementById('confirmDelete');

    document.querySelectorAll('[data-action="deleteItem"]').forEach(btn => {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            e.stopPropagation();

            deleteForm = this.closest('form');
            const row = this.closest('tr');
            const itemName = row.querySelector('td[data-label="Proje Adı"] strong')?.textContent || 'Bu öğe';

            if (deleteModal) {
                document.getElementById('deleteItemName').textContent = itemName;
                deleteModal.classList.add('active');
            }
        });
    });

    if (cancelDelete) {
        cancelDelete.addEventListener('click', function () {
            deleteModal.classList.remove('active');
            deleteForm = null;
        });
    }

    if (confirmDelete) {
        confirmDelete.addEventListener('click', function () {
            deleteModal.classList.remove('active');
            if (deleteForm) {
                deleteForm.submit();
            }
        });
    }

    if (deleteModal) {
        deleteModal.addEventListener('click', function (e) {
            if (e.target === this) {
                this.classList.remove('active');
                deleteForm = null;
            }
        });
    }
});

// Add project tag from input
function addProjectTag(event) {
    const input = event.target;
    const value = input.value.trim();

    if (value) {
        const container = input.parentElement;
        const tag = document.createElement('span');
        tag.className = 'tag';
        tag.textContent = value + ' ×';

        tag.addEventListener('click', function () {
            tag.remove();
        });

        container.insertBefore(tag, input);
        input.value = '';
    }
}
