document.addEventListener('DOMContentLoaded', function () {
    const commentForm = document.getElementById('comment-form');
    if (!commentForm) {
        return;
    }

    commentForm.addEventListener('submit', function (e) {
        e.preventDefault();

        const formData = new FormData(commentForm);
        const commentList = document.querySelector('.comment-list');
        const textarea = commentForm.querySelector('textarea');
        const errorDisplay = document.getElementById('comment-error');
        
        // Clear previous errors
        if (errorDisplay) errorDisplay.textContent = '';

        fetch('/api/commentsapi', {
            method: 'POST',
            body: formData,
        })
        .then(response => {
            if (response.ok) {
                return response.json(); 
            }
            return response.json().then(errorData => {
                let errorMessage = 'An error occurred while posting your comment.';
                if (errorData.errors && errorData.errors.Content) {
                    errorMessage = errorData.errors.Content[0];
                } else if (errorData.message) {
                    errorMessage = errorData.message;
                }
                throw new Error(errorMessage);
            });
        })
        .then(newComment => {
            const commentHtml = createCommentHtml(newComment);

            commentList.insertAdjacentHTML('afterbegin', commentHtml);
            
            textarea.value = '';
        })
        .catch(error => {
            console.error('Error posting comment:', error);
            if (errorDisplay) {
                errorDisplay.textContent = error.message;
            } else {
                alert(error.message);
            }
        });
    });

    /**
     * Creates the HTML for a single comment item from a comment view model object.
     * @param {object} comment - The comment view model from the API.
     * @returns {string} The HTML string for the new comment.
     */
    function createCommentHtml(comment) {
        return `
            <div class="comment-item">
                <img src="/${comment.authorAvatarUrl}" alt="User Avatar" class="comment-avatar">
                <div class="comment-body">
                    <span class="comment-user">${escapeHtml(comment.authorUsername)}</span>
                    <p class="comment-text">${escapeHtml(comment.content)}</p>
                </div>
            </div>
        `;
    }

    /**
     * A simple utility to prevent XSS attacks by escaping HTML content.
     * @param {string} unsafe - The raw string from the API.
     * @returns {string} The HTML-escaped string.
     */
    function escapeHtml(unsafe) {
        return unsafe
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }
});