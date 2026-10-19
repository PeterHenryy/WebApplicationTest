
(function () {
    let previousFocus = null;

    function closeAddedToCartModal() {
        const overlay = document.getElementById("cartModalOverlay");

        if (!overlay || overlay.hidden) return;

        overlay.hidden = true;
        document.body.classList.remove("cart-modal-open");

        if (previousFocus && previousFocus.isConnected) {
            previousFocus.focus();
        }
    }

    window.showAddedToCartModal = function (product) {
        const overlay = document.getElementById("cartModalOverlay");

        if (!overlay) return;

        previousFocus = document.activeElement;

        document.getElementById("cartModalProductName").textContent =
            product.name || "Product";

        document.getElementById("cartModalQuantity").textContent =
            `Quantity: ${product.quantity || 1}`;

        const priceElement = document.getElementById("cartModalPrice");

        if (product.price != null) {
            priceElement.textContent =
                `$${Number(product.price).toFixed(2)}`;
            priceElement.hidden = false;
        } else {
            priceElement.hidden = true;
        }

        const imageElement =
            document.getElementById("cartModalProductImage");

        if (product.image) {
            imageElement.src = product.image;
            imageElement.hidden = false;
        } else {
            imageElement.removeAttribute("src");
            imageElement.hidden = true;
        }

        overlay.hidden = false;
        document.body.classList.add("cart-modal-open");

        document.getElementById("cartModalClose").focus();
    };

    document.addEventListener("DOMContentLoaded", function () {
        const overlay = document.getElementById("cartModalOverlay");

        if (!overlay) return;

        document.getElementById("cartModalClose")
            .addEventListener("click", closeAddedToCartModal);

        document.getElementById("cartModalContinue")
            .addEventListener("click", closeAddedToCartModal);

        overlay.addEventListener("click", function (event) {
            if (event.target === overlay) {
                closeAddedToCartModal();
            }
        });

        document.addEventListener("keydown", function (event) {
            if (overlay.hidden) return;

            if (event.key === "Escape") {
                closeAddedToCartModal();
            }

            if (event.key === "Tab") {
                const focusable = Array.from(
                    overlay.querySelectorAll(
                        'a[href], button:not([disabled]), [tabindex]:not([tabindex="-1"])'
                    )
                ).filter(el => el.getClientRects().length > 0);

                if (!focusable.length) return;

                const first = focusable[0];
                const last = focusable[focusable.length - 1];

                if (event.shiftKey && document.activeElement === first) {
                    event.preventDefault();
                    last.focus();
                } else if (!event.shiftKey &&
                    document.activeElement === last) {
                    event.preventDefault();
                    first.focus();
                }
            }
        });
    });
})();
