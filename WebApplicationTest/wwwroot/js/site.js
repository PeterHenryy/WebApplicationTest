function toggleHeader() {
    let header = document.querySelector('.header');
    let printButton = document.querySelector('.print-button');
    header.style.display = 'none';
    printButton.style.display = 'none';
    window.print();
    header.style.display = 'flex';
    printButton.style.display = 'inline-block';
}

function truncateProductNames(maxLength = 35) {
    document.querySelectorAll(".product-name").forEach(product => {
        const text = product.textContent.trim();

        if (text.length > maxLength) {
            let truncated = text.substring(0, maxLength);

            const lastSpace = truncated.lastIndexOf(" ");

            if (lastSpace > 0) {
                truncated = truncated.substring(0, lastSpace);
            }

            product.textContent = truncated + "...";
        }
    });
}

document.addEventListener("DOMContentLoaded", () => {
    truncateProductNames();
});