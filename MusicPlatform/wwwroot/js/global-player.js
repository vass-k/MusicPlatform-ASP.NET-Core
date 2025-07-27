function initializeGlobalPlayerAndButtons() {
    if (!window.plyrInstance) {
        const playerContainer = document.getElementById('global-audio-player-container');
        if (playerContainer) {
            window.plyrInstance = new Plyr('#global-player');
            const playerTitleEl = document.getElementById('player-title');
            const playerArtistEl = document.getElementById('player-artist');
            const playerArtworkEl = document.getElementById('player-artwork');

            window.playTrack = function (trackData) {
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
        if (playButton.dataset.listenerAttached === 'true') {
            return;
        }

        playButton.addEventListener('click', function () {
            const trackData = {
                src: this.dataset.src,
                title: this.dataset.title,
                artist: this.dataset.artist,
                artwork: this.dataset.artwork
            };

            window.playTrack(trackData);
        });

        playButton.dataset.listenerAttached = 'true';
    }
}

document.addEventListener('turbo:load', initializeGlobalPlayerAndButtons);