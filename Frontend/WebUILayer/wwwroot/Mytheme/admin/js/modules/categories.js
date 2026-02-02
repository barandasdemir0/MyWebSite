// Categories Module - FINAL & CORRECT VERSION

let editingItem = null;

const categoryList = document.getElementById('categoryList');
const categoryForm = document.getElementById('categoryForm');
const categoryFormTitle = document.getElementById('categoryFormTitle');
const categoryName = document.getElementById('categoryName');
const addBtn = document.getElementById('addCategoryBtn');
const cancelBtn = document.getElementById('cancelCategoryBtn');
const formInner = document.getElementById('categoryFormInner');

/* ---------------- FORM HELPERS ---------------- */

function showForm(title) {
    const config = document.getElementById('categoriesConfig');
    categoryFormTitle.textContent =
        title || config?.dataset.textNew || 'Yeni Kategori Ekle';
    categoryForm.classList.add('visible');
    categoryName.focus();
}

function hideForm() {
    categoryForm.classList.remove('visible');
    categoryName.value = '';
    editingItem = null;
}

/* ---------------- EVENT DELEGATION ---------------- */

categoryList.addEventListener('click', (e) => {
    const btn = e.target.closest('.action-btn');
    if (!btn) return;

    if (btn.classList.contains('delete')) {
        handleDelete(btn);
        return;
    }

    if (btn.classList.contains('restore')) {
        handleRestore(btn);
        return;
    }

    handleEdit(btn);
});

/* ---------------- ACTIONS ---------------- */

function handleEdit(btn) {
    const item = btn.closest('.category-item');
    const nameEl = item.querySelector('.category-name');

    editingItem = item;
    categoryName.value = nameEl.textContent.trim();

    const config = document.getElementById('categoriesConfig');
    showForm(config?.dataset.textEdit || 'Kategoriyi Düzenle');
}

/* -------- DELETE -------- */

function handleDelete(btn) {
    const form = btn.closest('form');
    if (!form) return;

    const config = document.getElementById('categoriesConfig');

    if (window.adminApp && window.adminApp.notifications) {
        window.adminApp.notifications.showModal(
            config?.dataset.msgDeleteTitle || 'Silme İşlemi',
            config?.dataset.msgDeleteBody ||
            'Bu kategoriyi silmek istediğinizden emin misiniz?',
            () => {
                form.submit(); // 🔥 ASIL OLAY
            }
        );
    } else {
        if (confirm('Silmek istediğinize emin misiniz?')) {
            form.submit(); // 🔥
        }
    }
}

/* -------- RESTORE -------- */

function handleRestore(btn) {
    const form = btn.closest('form');
    if (!form) return;

    const config = document.getElementById('categoriesConfig');

    if (window.adminApp && window.adminApp.notifications) {
        window.adminApp.notifications.showModal(
            config?.dataset.textRestore || 'Geri Yükle',
            'Bu kategori geri yüklenecek. Onaylıyor musunuz?',
            () => {
                form.submit(); // 🔥
            }
        );
    } else {
        if (confirm('Bu kategori geri yüklenecek. Onaylıyor musunuz?')) {
            form.submit(); // 🔥
        }
    }
}

/* ---------------- ADD / EDIT FORM ---------------- */

addBtn?.addEventListener('click', () => {
    editingItem = null;
    showForm();
});

cancelBtn?.addEventListener('click', hideForm);

formInner?.addEventListener('submit', function () {
    // 🔴 Bilerek preventDefault YOK
    // Create / Update işlemi backend tarafından yapılacak
});
