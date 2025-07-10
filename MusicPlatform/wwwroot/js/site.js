/// <summary>
/// This function will handle the dynamic page loading
/// </summary>
const loadPage = async (url) => {
    try {
        const response = await fetch(url, {
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        });

        if (!response.ok) {
            window.location.href = url;
            return;
        }

        const newHtml = await response.text();
        const mainContent = document.getElementById('main-content');

        mainContent.innerHTML = newHtml;

        history.pushState({ path: url }, '', url);

        // <<< SELF-NOTE >>>: We may want to re-run any scripts that need to be initialized on new content
        // For example, if our new page has a carousel, we'd re-initialize it here.

    } catch (error) {
        console.error('Failed to load page: ', error);
        window.location.href = url;
    }
};

/// <summary>
/// This intercepts clicks on all internal links
/// </summary>
document.addEventListener('DOMContentLoaded', () => {
    const playerContainer = document.getElementById('global-audio-player-container');
    const player = new Plyr('#global-player');
    const playerTitleEl = document.getElementById('player-title');
    const playerArtistEl = document.getElementById('player-artist');
    const playerArtworkEl = document.getElementById('player-artwork');

    window.playTrack = function (trackData) {
        player.source = {
            type: 'audio',
            title: trackData.title,
            sources: [{ src: trackData.src, type: 'audio/mp3' }]
        };
        playerTitleEl.textContent = trackData.title;
        playerArtistEl.textContent = trackData.artist;
        playerArtworkEl.src = trackData.artwork;
        playerContainer.classList.add('visible');
        player.play();
    };

    document.body.addEventListener('click', e => {
        const link = e.target.closest('a');

        if (link && link.href && link.href.startsWith(window.location.origin) && !link.hasAttribute('data-no-ajax')) {
            e.preventDefault();
            loadPage(link.href);
        }
    });

    // SELF-NORE: We handle the browser's back/forward buttons with this one here
    window.addEventListener('popstate', e => {
        if (e.state && e.state.path) {
            loadPage(e.state.path);
        }
    });
});