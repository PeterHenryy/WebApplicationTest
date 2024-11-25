document.getElementById('product-form').addEventListener('submit', function (event) {
    let imageInput = document.getElementById('product-image');
    let categorySelect = document.getElementById('product-category');

    let fileSelected = imageInput.files.length > 0;
    let categorySelected = categorySelect.value !== "";

    if (!fileSelected) {
        alert('Please select an image for the product.');
        event.preventDefault();
        return;
    }

    let description = document.getElementById('description').value.trim();
    if (description === '') {
        alert('The description field cannot be empty.');
        event.preventDefault();
    }

    if (!categorySelected) {
        alert('Please select a category for the product.');
        event.preventDefault();
        return;
    }

});