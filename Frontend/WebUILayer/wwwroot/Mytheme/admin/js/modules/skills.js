document.addEventListener('DOMContentLoaded', function () {
    let selectedSkillElement = null;

    // --- MODALLAR ---
    const addModal = document.getElementById('skillModal');
    const updateModal = document.getElementById('updateSkillModal');
    const optionsModal = document.getElementById('skillOptionsModal');
    const deleteModal = document.getElementById('deleteConfirmModal');

    // --- TEXT ALANLARI ---
    const selectedSkillNameEl = document.getElementById('selectedSkillName');
    const deleteConfirmText = document.getElementById('deleteConfirmText');

    bindTechItemClicks();

    // 1. EKLEME İŞLEMLERİ
    const addBtn = document.querySelector('[data-action="addSkill"]');
    if (addBtn) {
        addBtn.addEventListener('click', () => {
            document.getElementById('addSkillForm').reset();
            addModal.classList.add('visible');
        });
    }

    // 2. KAPATMA İŞLEMLERİ (Her modal için ayrı)
    // Create kapat
    document.querySelectorAll('[data-action="closeSkillModal"]').forEach(btn =>
        btn.addEventListener('click', () => addModal.classList.remove('visible')));

    // Update kapat
    document.querySelectorAll('[data-action="closeUpdateModal"]').forEach(btn =>
        btn.addEventListener('click', () => updateModal.classList.remove('visible')));

    // Options kapat
    document.querySelectorAll('[data-action="closeSkillOptionsModal"]').forEach(btn =>
        btn.addEventListener('click', () => optionsModal.classList.remove('visible')));

    // Delete kapat
    document.querySelectorAll('[data-action="closeDeleteConfirm"]').forEach(btn =>
        btn.addEventListener('click', () => deleteModal.classList.remove('visible')));


    // 3. DÜZENLEME İŞLEMİ (Options Modal -> Update Modal)
    const editBtn = document.querySelector('[data-action="editSkill"]');
    if (editBtn) {
        editBtn.addEventListener('click', function () {
            if (!selectedSkillElement) return;

            // Options modalını kapat
            optionsModal.classList.remove('visible');

            // Verileri HTML elementinden al
            const id = selectedSkillElement.getAttribute('data-skill-id');
            const name = selectedSkillElement.querySelector('span:not(.iconify)').textContent.trim();
            const iconSpan = selectedSkillElement.querySelector('.iconify');
            const icon = iconSpan ? iconSpan.getAttribute('data-icon') : '';

            // Update Modal ID'lerini bul ve doldur
            document.getElementById('updateSkillId').value = id;
            document.getElementById('updateSkillName').value = name;
            document.getElementById('updateSkillIcon').value = icon;

            // Update modalını aç
            updateModal.classList.add('visible');
        });
    }

   
    // 4. SİLME İŞLEMİ
    const deleteBtn = document.querySelector('[data-action="deleteSkill"]');
    if (deleteBtn) {
        deleteBtn.addEventListener('click', function () {
            if (!selectedSkillElement) return;

            // Options modalını kapat
            document.getElementById('skillOptionsModal').classList.remove('visible');

            // Kutudan ID'yi ve İsmi Al
            const id = selectedSkillElement.getAttribute('data-skill-id'); // ✅

            const name = selectedSkillElement.querySelector('span:not(.iconify)').textContent.trim();

            // İsmi Yaz
            document.getElementById('deleteConfirmText').textContent = `"${name}" silinsin mi?`;

            // ID'yi Gizli Inputa Yaz (Input adı ne olursa olsun, formun içindeki İLK gizli inputa yazar)
            const hiddenInput = document.getElementById('deleteForm').querySelector('input[type="hidden"]');
            if (hiddenInput) {
                hiddenInput.value = id;
            }

            // Modalı Aç
            document.getElementById('deleteConfirmModal').classList.add('visible');
        });
    }



    // Helper: Tıklama olaylarını bağla
    function bindTechItemClicks() {
        document.querySelectorAll('.tech-item').forEach(item => {
            const newItem = item.cloneNode(true);
            item.parentNode.replaceChild(newItem, item);

            newItem.addEventListener('click', function () {
                selectedSkillElement = this;
                const name = this.querySelector('span:not(.iconify)').textContent.trim();
                selectedSkillNameEl.textContent = name;
                optionsModal.classList.add('visible');
            });
        });
    }
});
