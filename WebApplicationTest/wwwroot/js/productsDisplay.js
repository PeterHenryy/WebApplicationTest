document.addEventListener("DOMContentLoaded", function () {

    const sidebar = document.getElementById("filterSidebar");
    const overlay = document.getElementById("filterOverlay");

    const openButton = document.getElementById("openFilters");
    const closeButton = document.getElementById("closeFilters");

    const breakpoint = window.matchMedia("(max-width: 1600px)");

    let previousFocus = null;

    function openFilters() {
        previousFocus = document.activeElement;

        sidebar.classList.add("active");
        overlay.classList.add("active");

        document.body.classList.add("filters-open");

        openButton.setAttribute("aria-expanded", "true");

        closeButton.focus();
    }

    function closeFilters() {
        sidebar.classList.remove("active");
        overlay.classList.remove("active");

        document.body.classList.remove("filters-open");

        openButton.setAttribute("aria-expanded", "false");

        if (previousFocus && previousFocus.isConnected &&
            !previousFocus.closest(".sidebar-filter")) {
            previousFocus.focus();
        } else if (openButton.getClientRects().length > 0) {
            openButton.focus();
        }
    }

    openButton.addEventListener("click", openFilters);

    closeButton.addEventListener("click", closeFilters);

    overlay.addEventListener("click", closeFilters);

    document.addEventListener("keydown", function (event) {

        if (!sidebar.classList.contains("active")) {
            return;
        }

        if (event.key === "Escape") {
            closeFilters();
        }

        if (event.key === "Tab") {
            const focusable = Array.from(
                sidebar.querySelectorAll(
                    'a[href], button:not([disabled]), ' +
                    'input:not([disabled]), select:not([disabled]), ' +
                    'textarea:not([disabled]), [tabindex]:not([tabindex="-1"])'
                )
            ).filter(element => element.getClientRects().length > 0);

            if (!focusable.length) return;

            const first = focusable[0];
            const last = focusable[focusable.length - 1];

            if (event.shiftKey && document.activeElement === first) {
                event.preventDefault();
                last.focus();
            } else if (!event.shiftKey && document.activeElement === last) {
                event.preventDefault();
                first.focus();
            }
        }
    });

    // Return to normal desktop layout if screen gets wider.

    breakpoint.addEventListener("change", function (event) {
        if (!event.matches) {
            closeFilters();
        }
    });
});