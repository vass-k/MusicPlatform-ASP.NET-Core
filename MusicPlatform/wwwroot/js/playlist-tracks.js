document.addEventListener('DOMContentLoaded', function () {
    const addToPlaylistBtn = document.getElementById('add-to-playlist-btn');
    if (!addToPlaylistBtn) return;

    const trackId = addToPlaylistBtn.dataset.trackId;
    const modalElement = document.getElementById('addToPlaylistModal');
    const modal = new bootstrap.Modal(modalElement);
    const playlistListContainer = document.getElementById('playlist-list-container');
    const modalErrorDisplay = document.getElementById('playlist-modal-error');

    addToPlaylistBtn.addEventListener('click', function (e) {
        e.preventDefault();
        openModal();
    });

    function openModal() {
        modal.show();
        modalErrorDisplay.textContent = '';
        playlistListContainer.innerHTML = '<div class="text-center"><div class="spinner-border" role="status"><span class="visually-hidden">Loading...</span></div></div>';

        fetch(`/api/playlist-tracks/user-playlists/${trackId}`)
            .then(response => {
                if (!response.ok) throw new Error('Failed to load playlists.');
                return response.json();
            })
            .then(playlists => {
                buildPlaylistList(playlists);
            })
            .catch(error => {
                playlistListContainer.innerHTML = `<p class="text-danger">${error.message}</p>`;
            });
    }

    function buildPlaylistList(playlists) {
        if (playlists.length === 0) {
            playlistListContainer.innerHTML = '<p>You haven\'t created any playlists yet.</p>';
            return;
        }

        const list = document.createElement('ul');
        list.className = 'list-group';

        playlists.forEach(p => {
            const item = document.createElement('li');
            item.className = 'list-group-item d-flex justify-content-between align-items-center';
            item.textContent = p.playlistName;

            if (p.isTrackAlreadyInPlaylist) {
                item.classList.add('disabled');
                item.innerHTML += '<span class="badge bg-secondary">Added</span>';
            } else {
                item.style.cursor = 'pointer';
                item.dataset.playlistId = p.playlistPublicId;
                item.addEventListener('click', addTrackToPlaylist);
            }
            list.appendChild(item);
        });

        playlistListContainer.innerHTML = '';
        playlistListContainer.appendChild(list);
    }

    function addTrackToPlaylist(e) {
        const playlistId = e.target.dataset.playlistId;
        const playlistName = e.target.textContent;
        const token = document.querySelector('form#playlist-track-form input[name="__RequestVerificationToken"]').value;

        fetch('/api/playlist-tracks/add', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
            },
            body: JSON.stringify({
                trackPublicId: trackId,
                playlistPublicId: playlistId
            })
        })
            .then(response => {
                if (!response.ok) return response.json().then(err => Promise.reject(err));
                return response.json();
            })
            .then(data => {
                modal.hide();
                alert(`Successfully added to ${playlistName}.`);
            })
            .catch(error => {
                modalErrorDisplay.textContent = error.message || 'An unexpected error occurred.';
            });
    }
});