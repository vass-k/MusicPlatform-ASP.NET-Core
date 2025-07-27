let audioPondInstance = null;
let imagePondInstance = null;

function initializeUploadForm() {
    const uploadForm = document.getElementById('upload-form');
    if (!uploadForm) {
        return;
    }

    FilePond.registerPlugin(
        FilePondPluginFileValidateType,
        FilePondPluginImagePreview
    );

    const audioInputElement = document.querySelector('input[name="AudioFile"]');
    const imageInputElement = document.querySelector('input[name="ImageFile"]');

    audioPondInstance = FilePond.create(audioInputElement, {
        labelIdle: `Drag & Drop your Audio File or <span class="filepond--label-action">Browse</span>`,
        acceptedFileTypes: ['audio/mpeg', 'audio/mp3', 'audio/wav'],
        storeAsFile: true
    });

    imagePondInstance = FilePond.create(imageInputElement, {
        labelIdle: `Drag & Drop your Artwork or <span class="filepond--label-action">Browse</span>`,
        acceptedFileTypes: ['image/jpeg', 'image/png'],
        storeAsFile: true
    });

    const submitButton = document.getElementById('submit-button');
    const progressContainer = document.getElementById('progress-container');
    const progressBarInner = document.getElementById('progress-bar-inner');
    const uploadActions = document.querySelector('.upload-actions');

    if (uploadForm.dataset.listenerAttached !== 'true') {
        uploadForm.addEventListener('submit', function (e) {
            e.preventDefault();

            submitButton.disabled = true;
            uploadActions.style.display = 'none';
            progressContainer.style.display = 'block';

            setTimeout(() => { progressBarInner.style.width = '95%'; }, 100);
            setTimeout(() => { uploadForm.submit(); }, 500);
        });
        uploadForm.dataset.listenerAttached = 'true';
    }
}

function destroyUploadForm() {
    if (audioPondInstance) {
        FilePond.destroy(audioPondInstance.element);
        audioPondInstance = null;
    }
    if (imagePondInstance) {
        FilePond.destroy(imagePondInstance.element);
        imagePondInstance = null;
    }
}

document.addEventListener('turbo:load', initializeUploadForm);
document.addEventListener('turbo:before-cache', destroyUploadForm);