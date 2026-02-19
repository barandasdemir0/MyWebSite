const MAX_REPOS = 4;

document.addEventListener('DOMContentLoaded', function () {
    const publishBtn = document.getElementById('publishBtn');
    const reposGrid = document.getElementById('reposGrid');
    const selectedCountEl = document.getElementById('selectedCount');

    if (!reposGrid) return;

    // Kart seçimi
    reposGrid.addEventListener('click', function (e) {
        const card = e.target.closest('.repo-card');
        if (!card) return;

        const isSelected = card.classList.contains('selected');
        const currentCount = reposGrid.querySelectorAll('.repo-card.selected').length;

        if (!isSelected && currentCount >= MAX_REPOS) {
            alert('Maksimum ' + MAX_REPOS + ' repo seçebilirsiniz.');
            return;
        }

        card.classList.toggle('selected');
        updateSelectedCount();
    });

    // Seçilenleri kaydet
    publishBtn.addEventListener('click', async function () {
        const selectedCards = reposGrid.querySelectorAll('.repo-card.selected');
        if (selectedCards.length === 0) return;

        const syncUrl = publishBtn.dataset.syncUrl;
        const username = document.querySelector('input[name="username"]').value.trim();
        const repoNames = Array.from(selectedCards).map(c => c.dataset.repoName);

        publishBtn.disabled = true;

        try {
            const response = await fetch(syncUrl, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ username, repoNames })
            });
            const result = await response.json();
            if (result.success) location.reload();
        } catch (error) {
            console.error('Sync hatası:', error);
        } finally {
            publishBtn.disabled = false;
        }
    });

    function updateSelectedCount() {
        const count = reposGrid.querySelectorAll('.repo-card.selected').length;
        // Eğer en az 1 tane seçiliyse butonu göster
        if (count > 0) {
            selectedCountEl.classList.remove('hidden');
            publishBtn.classList.remove('hidden');
            selectedCountEl.querySelector('span').textContent = count;
        } else {
            selectedCountEl.classList.add('hidden');
            publishBtn.classList.add('hidden');
        }
    }
});
