function initializeCommentSection() {
    const commentForm = document.getElementById('comment-form');
    const commentList = document.querySelector('.comment-list');
    const errorDisplay = document.getElementById('comment-error');

    function getAntiForgeryToken() {
        const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
        return tokenInput ? tokenInput.value : '';
    }

    function escapeHtml(unsafe) {
        return unsafe
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }

    function createCommentHtml(comment) {
        const username = escapeHtml(comment.authorUsername);
        const avatarUrl = escapeHtml(comment.authorAvatarUrl);
        const content = escapeHtml(comment.content);
        const owned = comment.isOwnedByCurrentUser;

        return `
            <div class="comment-item" data-comment-id="${comment.id}">
              <a href="/Profile/Index?username=${username}">
                <img src="/${avatarUrl}" alt="Avatar for ${username}" class="comment-avatar">
              </a>
              <div class="comment-body">
                <div class="comment-header">
                  <a href="/Profile/Index?username=${username}" class="comment-user">${username}</a>
                  ${owned
                            ? `<button class="comment-delete-btn" title="Delete Comment"><i class="fa fa-trash"></i></button>`
                            : ''}
                </div>
                <p class="comment-text">${content}</p>
              </div>
            </div>`;
    }

    if (commentForm) {
        if (commentForm.dataset.listenerAttached !== 'true') {
            commentForm.addEventListener('submit', function (e) {
                e.preventDefault();

                if (errorDisplay) errorDisplay.textContent = '';

                const formData = new FormData(commentForm);

                fetch('/api/commentsapi', {
                    method: 'POST',
                    body: formData,
                    headers: {
                        'RequestVerificationToken': getAntiForgeryToken()
                    }
                })
                    .then(res => {
                        if (res.ok) return res.json();
                        return res.json().then(err => {
                            let msg = err.message || 'Failed to post comment.';
                            if (err.errors && err.errors.Content) {
                                msg = err.errors.Content[0];
                            }
                            throw new Error(msg);
                        });
                    })
                    .then(newComment => {
                        commentList.insertAdjacentHTML('afterbegin', createCommentHtml(newComment));
                        commentForm.querySelector('textarea').value = '';
                    })
                    .catch(err => {
                        console.error('Error posting comment:', err);
                        if (errorDisplay) {
                            errorDisplay.textContent = err.message;
                        } else {
                            alert(err.message);
                        }
                    });
            });

            commentForm.dataset.listenerAttached = 'true';
        }
    }

    if (commentList) {
        if (commentList.dataset.listenerAttached !== 'true') {
            commentList.addEventListener('click', function (e) {
                const btn = e.target.closest('.comment-delete-btn');
                if (!btn) return;

                const item = btn.closest('.comment-item');
                const id = item?.getAttribute('data-comment-id');
                if (!id) {
                    console.error('No comment-id found on .comment-item');
                    return;
                }

                if (!confirm('Are you sure you want to delete this comment?')) {
                    return;
                }

                fetch(`/api/commentsapi/${id}`, {
                    method: 'DELETE',
                    headers: {
                        'RequestVerificationToken': getAntiForgeryToken(),
                        'Content-Type': 'application/json'
                    }
                })
                    .then(res => {
                        if (res.ok) {
                            item.remove();
                        } else {
                            return res.json()
                                .then(err => { throw new Error(err.message || 'Could not delete.'); });
                        }
                    })
                    .catch(err => {
                        console.error('Error deleting comment:', err);
                        alert(err.message || 'Failed to delete comment.');
                    });
            });

            commentList.dataset.listenerAttached = 'true';
        }
    }
}

document.addEventListener('turbo:load', initializeCommentSection);

