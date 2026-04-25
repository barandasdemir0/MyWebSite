let currentCommentRow = null;

function viewComment(btn) {
    try {
        currentCommentRow = btn.closest('tr');
        const row = currentCommentRow;

        const nameEl = row.querySelector('.author-wrapper strong');
        const name = nameEl ? nameEl.innerText : 'Kullanıcı';

        const messageEl = row.querySelector('.comment-text');
        const message = messageEl ? messageEl.innerText : '';

        let date = '';
        if (row.cells && row.cells.length > 3) {
            date = row.cells[3].innerText;
        }

        let isPending = false;
        const card = row.closest('.card');
        if (card) {
            isPending = card.dataset.tableType === 'pending';
        }

        const modalName = document.getElementById('modalUserName');
        const modalMsg = document.getElementById('modalMessage');
        const modalDate = document.getElementById('modalDate');

        if (modalName) modalName.innerText = name;
        if (modalMsg) modalMsg.value = message;
        if (modalDate) modalDate.innerText = date;

        const approveBtn = document.getElementById('approveBtn');
        const restoreBtn = document.getElementById('restoreBtn');

        if (isPending) {
            if (approveBtn) approveBtn.classList.remove('hidden');
            if (restoreBtn) restoreBtn.classList.add('hidden');
        } else {
            if (approveBtn) approveBtn.classList.add('hidden');
            if (restoreBtn) restoreBtn.classList.remove('hidden');
        }

        const modal = document.getElementById('commentModal');
        if (modal) {
            modal.classList.add('active');
        }
    } catch (err) {
        console.error('Error in viewComment:', err);
    }
}

function closeCommentModal() {
    document.getElementById('commentModal').classList.remove('active');
}

document.addEventListener('DOMContentLoaded', function () {
    const modal = document.getElementById('commentModal');

    // View/Action Handler
    document.body.addEventListener('click', function (e) {
        const viewBtn = e.target.closest('[data-action="viewComment"]');
        if (viewBtn) {
            e.stopPropagation();
            viewComment(viewBtn);
            return;
        }

        const closeBtn = e.target.closest('[data-action="closeModal"]');
        if (closeBtn) {
            e.stopPropagation();
            closeCommentModal();
            return;
        }
    }, true);

    // ==========================================
    // MODAL İÇİNDEN ONAYLAMA VE GERİ YÜKLEME (GERÇEK SUBMİT)
    // ==========================================
    const approveBtn = document.getElementById('approveBtn');
    const restoreBtn = document.getElementById('restoreBtn');

    if (approveBtn) {
        approveBtn.addEventListener('click', function () {
            if (currentCommentRow) {
                // Sadece boyama yapma, satırdaki gerçek C# formunu bul ve post et!
                const form = currentCommentRow.querySelector('form[action*="Approve"]');
                if (form) form.submit();
            }
        });
    }

    if (restoreBtn) {
        restoreBtn.addEventListener('click', function () {
            if (currentCommentRow) {
                // Sadece boyama yapma, satırdaki gerçek C# formunu bul ve post et!
                const form = currentCommentRow.querySelector('form[action*="Restore"]');
                if (form) form.submit();
            }
        });
    }

    // ==========================================
    // ARAMA VE SAYFALAMA (SENİN ESKİ KODUN AYNEN KALDI)
    // ==========================================
    const paginationBtns = document.querySelectorAll('.pagination-btn');
    paginationBtns.forEach((btn) => {
        btn.addEventListener('click', function () {
            if (this.disabled || this.classList.contains('active')) return;
            const isNumber = !this.querySelector('i');
            if (isNumber) {
                document.querySelectorAll('.pagination-btn').forEach(b => {
                    if (!b.querySelector('i')) b.classList.remove('active');
                });
                this.classList.add('active');
            } else {
                const currentActive = document.querySelector('.pagination-btn.active');
                const currentPage = parseInt(currentActive?.textContent || 1);
                const isNext = this.querySelector('.fa-chevron-right');

                if (isNext) {
                    const nextBtn = Array.from(paginationBtns).find(b => !b.querySelector('i') && parseInt(b.textContent) === currentPage + 1);
                    if (nextBtn) nextBtn.click();
                } else {
                    const prevBtn = Array.from(paginationBtns).find(b => !b.querySelector('i') && parseInt(b.textContent) === currentPage - 1);
                    if (prevBtn) prevBtn.click();
                }
            }
            updatePaginationArrows();
        });
    });

    function updatePaginationArrows() {
        const currentActive = document.querySelector('.pagination-btn.active');
        const currentPage = parseInt(currentActive?.textContent || 1);
        const allPages = Array.from(document.querySelectorAll('.pagination-btn')).filter(b => !b.querySelector('i')).map(b => parseInt(b.textContent));

        const minPage = Math.min(...allPages);
        const maxPage = Math.max(...allPages);

        const prevBtn = document.querySelector('.pagination-btn .fa-chevron-left')?.parentElement;
        const nextBtn = document.querySelector('.pagination-btn .fa-chevron-right')?.parentElement;

        if (prevBtn) prevBtn.disabled = currentPage <= minPage;
        if (nextBtn) nextBtn.disabled = currentPage >= maxPage;
    }
    updatePaginationArrows();

    const searchInput = document.querySelector('.search-box input');
    if (searchInput) {
        searchInput.addEventListener('input', function () {
            const searchTerm = this.value.toLowerCase();
            const approvedTable = document.querySelectorAll('.card')[1];
            if (approvedTable) {
                const rows = approvedTable.querySelectorAll('.admin-table tbody tr');
                rows.forEach(row => {
                    const userName = row.querySelector('strong')?.textContent.toLowerCase() || '';
                    const messageText = row.querySelector('.comment-text')?.textContent.toLowerCase() || '';
                    if (userName.includes(searchTerm) || messageText.includes(searchTerm)) {
                        row.classList.remove('hidden');
                    } else {
                        row.classList.add('hidden');
                    }
                });
            }
        });
    }
});

// ==========================================
// SATIRDAKİ ÇÖP KUTUSU (SİLME) BUTONU (GERÇEK SUBMİT)
// ==========================================
document.body.addEventListener('click', function (e) {
    const btn = e.target.closest('.action-btn.delete');
    if (btn) {
        e.stopPropagation();

        const form = btn.closest('form'); // Butonun bağlı olduğu C# formunu al
        const row = btn.closest('tr');
        const itemName = row.querySelector('.author-wrapper strong')?.textContent || 'Bu kullanıcı';

        const modal = document.getElementById('deleteConfirmModal');
        const itemNameSpan = document.getElementById('deleteItemName');
        const confirmBtn = document.getElementById('confirmDelete');
        const cancelBtn = document.getElementById('cancelDelete');

        if (modal && itemNameSpan) {
            itemNameSpan.textContent = itemName;
            modal.classList.add('active');

            const closeModal = () => modal.classList.remove('active');
            cancelBtn.onclick = closeModal;
            modal.onclick = (evt) => { if (evt.target === modal) closeModal(); };

            confirmBtn.onclick = () => {
                // Sadece arayüzü soldurma, C# postunu tetikle!
                if (form) form.submit();
            };
        }
    }
}, true);

// ==========================================
// SATIRDAKİ GERİ YÜKLE BUTONU (GERÇEK SUBMİT)
// ==========================================
document.body.addEventListener('click', function (e) {
    if (e.target.closest('[data-action="restoreItem"]')) {
        const btn = e.target.closest('[data-action="restoreItem"]');
        e.stopPropagation();

        const form = btn.closest('form'); // Butonun bağlı olduğu C# formunu al
        const row = btn.closest('tr');
        const itemName = row.querySelector('.author-wrapper strong')?.textContent || 'Bu kullanıcı';

        const modal = document.getElementById('restoreModal');
        const itemNameSpan = document.getElementById('restoreItemName');
        const confirmBtn = document.getElementById('confirmRestore');
        const cancelBtn = document.getElementById('cancelRestore');

        if (modal && itemNameSpan) {
            itemNameSpan.textContent = itemName;
            modal.classList.add('active');

            const closeModal = () => modal.classList.remove('active');
            cancelBtn.onclick = closeModal;
            modal.onclick = (evt) => { if (evt.target === modal) closeModal(); };

            confirmBtn.onclick = function () {
                // Sadece arayüzü soldurma, C# postunu tetikle!
                if (form) form.submit();
            };
        }
    }
}, true);
