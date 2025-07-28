function initializeDynamicComponents() {
    const dropdownElementList = [].slice.call(document.querySelectorAll('[data-toggle="dropdown"]'));
    dropdownElementList.map(function (dropdownToggleEl) {
        return new bootstrap.Dropdown(dropdownToggleEl);
    });
}

document.addEventListener('turbo:load', function () {
    console.log('Turbo loaded a new page. Initializing dynamic components...');
    initializeDynamicComponents();
});