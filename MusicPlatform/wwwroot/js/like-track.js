document.addEventListener('DOMContentLoaded', function () {
    const likeButton = document.getElementById('like-button');
    if (!likeButton) return;

    likeButton.addEventListener('click', function (e) {
        e.preventDefault();

        const button = e.target.closest('a');
        const likeText = document.getElementById('like-text');
        const likeCountDisplay = document.getElementById('like-count-display');
        const likeForm = document.getElementById('like-form');
        const tokenInput = likeForm.querySelector('input[name="__RequestVerificationToken"]');

        if (!tokenInput) {
            console.error('Anti-forgery token not found.');
            return;
        }
        const token = tokenInput.value;
        let isCurrentlyLiked = button.classList.contains('liked');

        const actionUrl = isCurrentlyLiked ? button.dataset.unlikeUrl : button.dataset.likeUrl;

        // We are doing an optimistic UI Update
        let currentLikes = parseInt(likeCountDisplay.innerText.replace(/,/g, ''));
        isCurrentlyLiked = !isCurrentlyLiked;

        button.classList.toggle('liked');
        if (isCurrentlyLiked) {
            likeText.textContent = 'Liked';
            likeCountDisplay.innerText = (currentLikes + 1).toLocaleString();
        } else {
            likeText.textContent = 'Like';
            likeCountDisplay.innerText = (currentLikes - 1).toLocaleString();
        }

        fetch(actionUrl, {
            method: 'POST',
            headers: {
                'RequestVerificationToken': token
            }
        })
            .then(response => {
                if (response.status === 204) {
                    return null;
                }
                if (!response.ok) {
                    return response.json().then(err => Promise.reject(err));
                }
                return response.json();
            })
            .then(data => {
                if (data && data.newLikeCount !== undefined) {
                    likeCountDisplay.innerText = data.newLikeCount.toLocaleString();
                }
            })
            .catch(error => {
                console.error('Error toggling like status:', error.message || 'An unknown error occurred.');

                isCurrentlyLiked = !isCurrentlyLiked;
                button.classList.toggle('liked');

                if (isCurrentlyLiked) {
                    likeText.textContent = 'Liked';
                    likeCountDisplay.innerText = (currentLikes + 1).toLocaleString();
                } else {
                    likeText.textContent = 'Like';
                    likeCountDisplay.innerText = currentLikes.toLocaleString();
                }

                alert("There was an error updating the like status. Please try again.");
            });
    });
});