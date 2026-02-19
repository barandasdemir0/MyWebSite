const MAX_REPOS = 4;

document.addEventListener('DOMContentLoaded', function () {
    const fetchBtn = document.getElementById('fetchReposBtn');
    const publishBtn = document.getElementById('publishBtn');
    const reposGrid = document.getElementById('reposGrid');
    const selectedCountEl = document.getElementById('selectedCount');
    const template = document.getElementById('repoCardTemplate');

    // Kartı template'den oluştur (inline HTML yok)
    function createRepoCard(repo) {
        const clone = template.content.cloneNode(true);
        const card = clone.querySelector('.repo-card');

        card.dataset.repoName = repo.repoName;
        card.querySelector('.repo-name').textContent = repo.repoName;
        card.querySelector('.repo-description').textContent = repo.description || '';
        card.querySelector('.language-badge').textContent = repo.language || '';
        card.querySelector('.star-count').textContent = repo.starCount;
        card.querySelector('.fork-count').textContent = repo.forkCount;

        return clone;
    }

    // Fetch
    fetchBtn.addEventListener('click', async function () {
        const username = document.getElementById('githubUsername').value.trim();
        if (!username) return;

        fetchBtn.disabled = true;
        fetchBtn.querySelector('i').classList.add('fa-spin');

        try {
            const response = await fetch(`${GITHUB_FETCH_URL}?username=${username}`);
            const repos = await response.json();

            reposGrid.innerHTML = '';
            repos.forEach(repo => reposGrid.appendChild(createRepoCard(repo)));
            updateSelectedCount();
        } catch (error) {
            console.error('Fetch hatası:', error);
        } finally {
            fetchBtn.disabled = false;
            fetchBtn.querySelector('i').classList.remove('fa-spin');
        }
    });

    // Kart seçimi
    reposGrid.addEventListener('click', function (e) {
        const card = e.target.closest('.repo-card');
        if (!card) return;

        const isSelected = card.classList.contains('selected');
        const currentCount = reposGrid.querySelectorAll('.repo-card.selected').length;

        if (!isSelected && currentCount >= MAX_REPOS) {
            alert(`Maksimum ${MAX_REPOS} repo seçebilirsiniz.`);
            return;
        }

        card.classList.toggle('selected');
        updateSelectedCount();
    });

    // Publish
    publishBtn.addEventListener('click', async function () {
        const selectedCards = reposGrid.querySelectorAll('.repo-card.selected');
        if (selectedCards.length === 0) return;

        const username = document.getElementById('githubUsername').value.trim();
        const repoNames = Array.from(selectedCards).map(c => c.dataset.repoName);

        publishBtn.disabled = true;

        try {
            const response = await fetch(GITHUB_SYNC_URL, {
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
        selectedCountEl.classList.toggle('hidden', count === 0);
        publishBtn.classList.toggle('hidden', count === 0);
        if (count > 0) selectedCountEl.querySelector('span').textContent = count;
    }
});
