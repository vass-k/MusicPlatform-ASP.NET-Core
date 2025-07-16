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
        const token = formData.get('__RequestVerificationToken');

        // Clear previous errors
        if (errorDisplay) errorDisplay.textContent = '';

        fetch('/Comment/Add', {
            method: 'POST',
            body: formData,
        })
            .then(response => {
                if (response.ok) {
                    return response.text();
                }
                return response.json().then(errorData => {
                    let errorMessage = 'An error occurred.';
                    if (errorData.Content && errorData.Content.errors) {
                        errorMessage = errorData.Content.errors[0].errorMessage;
                    } else if (errorData.message) {
                        errorMessage = errorData.message;
                    }
                    throw new Error(errorMessage);
                });
            })
            .then(html => {
                commentList.insertAdjacentHTML('afterbegin', html);
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
});