document.addEventListener('DOMContentLoaded', function () {
    const addToPlaylistBtn = document.getElementById('add-to-playlist-btn');
    const modalElement = document.getElementById('addToPlaylistModal');
    let modal;
    if (modalElement) {
        modal = new bootstrap.Modal(modalElement);
    }
    const playlistListContainer = document.getElementById('playlist-list-container');
    const modalErrorDisplay = document.getElementById('playlist-modal-error');

    if (addToPlaylistBtn) {
        addToPlaylistBtn.addEventListener('click', function (e) {
            e.preventDefault();
            openModalAndFetchPlaylists();
        });
    }

    document.body.addEventListener('click', function (e) {
        const removeButton = e.target.closest('.remove-from-playlist-btn');
        if (removeButton) {
            e.preventDefault();
            const trackId = removeButton.dataset.trackId;
            const playlistId = removeButton.dataset.playlistId;
            removeTrackFromPlaylist(trackId, playlistId, removeButton);
        }
    });

    function openModalAndFetchPlaylists() {
        const trackId = addToPlaylistBtn.dataset.trackId;
        modal.show();
        modalErrorDisplay.textContent = '';
        playlistListContainer.innerHTML = '<div class="text-center"><div class="spinner-border" role="status"></div></div>';

        fetch(`/api/playlist-tracks/user-playlists/${trackId}`)
            .then(handleFetchError)
            .then(playlists => buildPlaylistListInModal(playlists, trackId))
            .catch(error => {
                playlistListContainer.innerHTML = `<p class="text-danger">${error.message}</p>`;
            });
    }

    function buildPlaylistListInModal(playlists, trackId) {
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
                const removeBtn = document.createElement('button');
                removeBtn.className = 'btn btn-sm btn-outline-danger remove-from-playlist-btn';
                removeBtn.textContent = 'Remove';
                removeBtn.dataset.trackId = trackId;
                removeBtn.dataset.playlistId = p.playlistPublicId;
                item.appendChild(removeBtn);
            } else {
                item.style.cursor = 'pointer';
                item.dataset.playlistId = p.playlistPublicId;
                item.addEventListener('click', () => addTrackToPlaylist(trackId, p.playlistPublicId, p.playlistName));
            }
            list.appendChild(item);
        });

        playlistListContainer.innerHTML = '';
        playlistListContainer.appendChild(list);
    }

    function addTrackToPlaylist(trackId, playlistId, playlistName) {
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
            .then(handleFetchError)
            .then(() => {
                modal.hide();
                alert(`Successfully added to ${playlistName}.`);
            })
            .catch(error => {
                modalErrorDisplay.textContent = error.message || 'An unexpected error occurred.';
            });
    }

    function removeTrackFromPlaylist(trackId, playlistId, buttonElement) {
        if (!confirm('Are you sure you want to remove this track from the playlist?')) {
            return;
        }

        const token = document.querySelector('form#playlist-track-form input[name="__RequestVerificationToken"]').value;

        // once again - optimistic UI update
        const rowToRemove = document.getElementById(`track-row-${trackId}`);
        if (rowToRemove) {
            rowToRemove.style.opacity = '0.5';
        }

        fetch('/api/playlist-tracks/remove', {
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
            .then(handleFetchError)
            .then(() => {
                if (rowToRemove) {
                    rowToRemove.remove();
                }

                if (modalElement.classList.contains('show')) {
                    openModalAndFetchPlaylists();
                }
            })
            .catch(error => {
                if (rowToRemove) {
                    rowToRemove.style.opacity = '1';
                }
                alert(`Failed to remove track: ${error.message}`);
            });
    }

    async function handleFetchError(response) {
        if (!response.ok) {
            const errorData = await response.json().catch(() => ({ message: 'An unknown error occurred.' }));
            throw new Error(errorData.message);
        }
        return response.json();
    }
});