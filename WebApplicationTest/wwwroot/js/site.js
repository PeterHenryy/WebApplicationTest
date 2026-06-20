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

function scrollCarousel(button, direction) {
    const container = button.parentElement.querySelector('.carousel-track');
    const cards = Array.from(container.querySelectorAll('.product-card'));

    if (!cards.length) return;

    const scrollLeft = container.scrollLeft;

    let currentIndex = 0;

    for (let i = 0; i < cards.length; i++) {
        if (cards[i].offsetLeft >= scrollLeft) {
            currentIndex = i;
            break;
        }
    }

    const targetIndex = Math.max(0, Math.min(cards.length - 1, currentIndex + direction));

    cards[targetIndex].scrollIntoView({
        behavior: "smooth",
        inline: "start",
        block: "nearest"
    });
}