const MAX_REPOS = 4;
const STORAGE_KEY = 'github_selected_repos';

document.addEventListener('DOMContentLoaded', function () {

    const publishBtn = document.getElementById('publishBtn');
    const reposGrid = document.getElementById('reposGrid');
    const selectedCountEl = document.getElementById('selectedCount');

    if (!reposGrid) return;

    // -----------------------------
    // 1️⃣ DB’den gelen seçili repolar
    // -----------------------------
    const dbSelected = Array.from(
        reposGrid.querySelectorAll('.repo-card.selected')
    ).map(card => card.dataset.repoName);

    // -----------------------------
    // 2️⃣ localStorage’daki seçimler
    // -----------------------------
    const storedSelected = JSON.parse(localStorage.getItem(STORAGE_KEY) || '[]');

    // -----------------------------
    // 3️⃣ DB + localStorage birleştir (duplicate yok)
    // -----------------------------
    let selectedRepos = [...new Set([...dbSelected, ...storedSelected])];

    // -----------------------------
    // 4️⃣ Güvenlik: MAX sınırını aşarsa kes
    // -----------------------------
    if (selectedRepos.length > MAX_REPOS) {
        selectedRepos = selectedRepos.slice(0, MAX_REPOS);
    }

    // -----------------------------
    // 5️⃣ Sayfadaki kartları state’e göre işaretle
    // -----------------------------
    syncCardsWithState();

    // -----------------------------
    // 6️⃣ localStorage güncelle
    // -----------------------------
    saveToStorage();

    updateUI();

    // =============================
    // Kart tıklama
    // =============================
    reposGrid.addEventListener('click', function (e) {

        const card = e.target.closest('.repo-card');
        if (!card) return;

        const repoName = card.dataset.repoName;
        const isSelected = selectedRepos.includes(repoName);

        // Eğer seçiliyse → çıkar
        if (isSelected) {
            selectedRepos = selectedRepos.filter(r => r !== repoName);
        }
        // Seçili değilse → ekle (MAX kontrol)
        else {
            if (selectedRepos.length >= MAX_REPOS) return;
            selectedRepos.push(repoName);
        }

        syncCardsWithState();
        saveToStorage();
        updateUI();
    });

    // =============================
    // Publish / Sync
    // =============================
    publishBtn?.addEventListener('click', async function () {

        if (selectedRepos.length === 0) return;

        const syncUrl = publishBtn.dataset.syncUrl;
        const username = publishBtn.dataset.username;


        publishBtn.disabled = true;

        try {
            const response = await fetch(syncUrl, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    username,
                    repoNames: selectedRepos
                })
            });

            const result = await response.json();

            if (result.success) {
                // Sync başarılıysa localStorage temizle
                localStorage.removeItem(STORAGE_KEY);
                location.reload();
            }

        } catch (err) {
            console.error('Sync hatası:', err);
        } finally {
            publishBtn.disabled = false;
        }
    });

    // =============================
    // Helper Functions
    // =============================

    function syncCardsWithState() {
        reposGrid.querySelectorAll('.repo-card').forEach(card => {
            const name = card.dataset.repoName;
            card.classList.toggle('selected', selectedRepos.includes(name));
        });
    }

    function saveToStorage() {
        localStorage.setItem(STORAGE_KEY, JSON.stringify(selectedRepos));
    }

    function updateUI() {
        const count = selectedRepos.length;

        selectedCountEl?.classList.toggle('hidden', count === 0);
        publishBtn?.classList.toggle('hidden', count === 0);

        if (count > 0 && selectedCountEl) {
            const span = selectedCountEl.querySelector('span');
            if (span) span.textContent = count;
        }
    }

});