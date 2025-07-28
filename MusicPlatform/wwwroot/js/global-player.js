function initializeGlobalPlayerAndButtons() {
    let currentTrackIdForPlayback = null;
    let playHasBeenRecorded = false;
    const PLAY_RECORD_THRESHOLD_SECONDS = 15;

    if (!window.plyrInstance) {
        const playerContainer = document.getElementById('global-audio-player-container');
        if (playerContainer) {
            window.plyrInstance = new Plyr('#global-player');
            const playerTitleEl = document.getElementById('player-title');
            const playerArtistEl = document.getElementById('player-artist');
            const playerArtworkEl = document.getElementById('player-artwork');

            window.plyrInstance.on('timeupdate', () => {
                if (window.plyrInstance.currentTime > PLAY_RECORD_THRESHOLD_SECONDS && !playHasBeenRecorded) {
                    playHasBeenRecorded = true;
                    recordPlay(currentTrackIdForPlayback);
                }
            });

            window.playTrack = function (trackData) {
                currentTrackIdForPlayback = trackData.id;
                playHasBeenRecorded = false;

                window.plyrInstance.source = {
                    type: 'audio',
                    title: trackData.title,
                    sources: [{ src: trackData.src, type: 'audio/mp3' }]
                };
                playerTitleEl.textContent = trackData.title;
                playerArtistEl.textContent = trackData.artist;
                playerArtworkEl.src = trackData.artwork;
                playerContainer.classList.add('visible');
                window.plyrInstance.play();
            };
        }
    }

    const playButton = document.getElementById('play-track-button');
    if (playButton) {
        if (playButton.dataset.listenerAttached !== 'true') {
            playButton.addEventListener('click', function () {
                const trackData = {
                    id: this.dataset.trackId,
                    src: this.dataset.src,
                    title: this.dataset.title,
                    artist: this.dataset.artist,
                    artwork: this.dataset.artwork
                };
                if (window.playTrack) {
                    window.playTrack(trackData);
                }
            });

            playButton.dataset.listenerAttached = 'true';
        }
    }
}

/**
 * Sends a "fire and forget" POST request to record a track play.
 * @param {string} trackId - The public GUID of the track.
 */
function recordPlay(trackId) {
    if (!trackId) {
        return;
    }

    const tokenInput = document.querySelector('form input[name="__RequestVerificationToken"]');
    if (!tokenInput) {
        console.error('Anti-forgery token not found on the page.');
        return;
    }
    const token = tokenInput.value;

    fetch('/api/trackplayapi/increment-play', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        },
        body: JSON.stringify({ trackId: trackId })
    }).catch(error => {
        console.error('Failed to record play count:', error);
    });
}

document.addEventListener('turbo:load', initializeGlobalPlayerAndButtons);