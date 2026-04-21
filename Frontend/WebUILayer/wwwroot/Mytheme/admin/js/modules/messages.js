document.addEventListener('DOMContentLoaded', function () {
    var composeModal = document.getElementById('composeModal');

    // Mobile sidebar
    var toggle = document.getElementById('mobileSidebarToggle');
    var overlay = document.getElementById('sidebarOverlay');
    var sidebar = document.querySelector('.email-sidebar');
    if (toggle && sidebar) {
        toggle.addEventListener('click', function () {
            sidebar.classList.add('active');
            if (overlay) overlay.classList.add('active');
        });
        if (overlay) overlay.addEventListener('click', function () {
            sidebar.classList.remove('active');
            overlay.classList.remove('active');
        });
    }

    // Compose aç/kapat
    document.querySelectorAll('[data-action="openCompose"]').forEach(function (b) {
        b.addEventListener('click', function (e) { e.preventDefault(); composeModal.classList.add('active'); });
    });
    document.querySelectorAll('[data-action="closeComposeModal"]').forEach(function (b) {
        b.addEventListener('click', function () { composeModal.classList.remove('active'); });
    });

    // contenteditable → hidden input
    var form = document.getElementById('composeForm');
    if (form) form.addEventListener('submit', function () {
        var e = document.getElementById('editorContent');
        var h = document.getElementById('hiddenBody');
        if (e && h) h.value = e.innerHTML;
    });

    // Detail modal aç
    document.querySelectorAll('[data-action="openModal"]').forEach(function (b) {
        b.addEventListener('click', function () {
            var m = document.getElementById(this.dataset.target);
            if (m) m.classList.add('active');
        });
    });

    // Modal kapat
    document.querySelectorAll('[data-action="closeModal"]').forEach(function (b) {
        b.addEventListener('click', function () {
            this.closest('.admin-modal-overlay').classList.remove('active');
        });
    });

    // Overlay tıklama ile kapat
    window.addEventListener('click', function (e) {
        if (e.target.classList.contains('admin-modal-overlay')) e.target.classList.remove('active');
    });

    // Client-side arama
    var si = document.getElementById('messageSearchInput');
    if (si) si.addEventListener('input', function () {
        var t = this.value.toLowerCase();
        document.querySelectorAll('#messageTableBody tr').forEach(function (r) {
            var a = r.cells[0] ? r.cells[0].textContent.toLowerCase() : '';
            var b = r.cells[1] ? r.cells[1].textContent.toLowerCase() : '';
            r.style.display = (a.indexOf(t) > -1 || b.indexOf(t) > -1) ? '' : 'none';
        });
    });
});
