const MAX_REPOS = 4;
const STORAGE_KEY = 'github_selected_repos';

document.addEventListener('DOMContentLoaded', function () {
    const publishBtn = document.getElementById('publishBtn');
    const reposGrid = document.getElementById('reposGrid');
    const selectedCountEl = document.getElementById('selectedCount');

    if (!reposGrid) return;

    // localStorage'dan oku
    let selectedRepos = JSON.parse(localStorage.getItem(STORAGE_KEY) || '[]');

    // Razor'ın "Kayıtlı" olarak işaretlediği kartları da ekle
    reposGrid.querySelectorAll('.repo-card.selected').forEach(card => {
        const name = card.dataset.repoName;
        if (!selectedRepos.includes(name)) {
            selectedRepos.push(name);
        }
    });

    // Tüm sayfadaki kartları array'e göre işaretle
    reposGrid.querySelectorAll('.repo-card').forEach(card => {
        card.classList.toggle('selected', selectedRepos.includes(card.dataset.repoName));
    });

    // localStorage'ı güncelle
    localStorage.setItem(STORAGE_KEY, JSON.stringify(selectedRepos));
    updateUI();

    // Kart tıklama
    reposGrid.addEventListener('click', function (e) {
        const card = e.target.closest('.repo-card');
        if (!card) return;

        const repoName = card.dataset.repoName;
        const isSelected = card.classList.contains('selected');

        if (!isSelected && selectedRepos.length >= MAX_REPOS) return;

        if (isSelected) {
            selectedRepos = selectedRepos.filter(r => r !== repoName);
            card.classList.remove('selected');
        } else {
            selectedRepos.push(repoName);
            card.classList.add('selected');
        }

        localStorage.setItem(STORAGE_KEY, JSON.stringify(selectedRepos));
        updateUI();
    });

    // Kaydet
    publishBtn.addEventListener('click', async function () {
        if (selectedRepos.length === 0) return;

        const syncUrl = publishBtn.dataset.syncUrl;
        const username = document.querySelector('input[name="username"]').value.trim();

        publishBtn.disabled = true;
        try {
            const response = await fetch(syncUrl, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ username, repoNames: selectedRepos })
            });
            const result = await response.json();
            if (result.success) {
                localStorage.removeItem(STORAGE_KEY);
                location.reload();
            }
        } catch (err) {
            console.error('Sync hatası:', err);
        } finally {
            publishBtn.disabled = false;
        }
    });

    function updateUI() {
        const count = selectedRepos.length;
        selectedCountEl.classList.toggle('hidden', count === 0);
        publishBtn.classList.toggle('hidden', count === 0);
        if (count > 0) selectedCountEl.querySelector('span').textContent = count;
    }
});
