$(document).ready(function () {
    // Handle change event for reason select
    $(".reason-select").change(function () {
        const customReasonInput = $(this).closest('.refund-reason-select').find(".customReasonInput");
        if ($(this).val() === "Other") {
            customReasonInput.show();
        } else {
            customReasonInput.hide();
        }
    });

    // Handle click event for refund buttons
    $(".refund-button").on("click", function () {
        const refundSelect = $(this).closest('.cart-product').find('.refund-reason-select');
        refundSelect.slideToggle("slow");
    });

});

// Submit form function
function submitForm(button) {
    const refundForm = $(button).closest('form');
    let selectedReason = refundForm.find('.reason-select').val();
    let customReason = refundForm.find('.customReasonInput textarea').val();

    // Set the selected reason to either custom or selected value
    refundForm.find('.reason-select').val(selectedReason === 'Other' ? customReason : selectedReason);

    // Submit the form
    refundForm.submit();
}

function showReviewForm(productID, button) {

    const reviewForm = document.getElementById(`review-form-${productID}`);

    if (!reviewForm)
        return;

    const isOpen = reviewForm.classList.contains("show");

    if (isOpen) {
        // Close the form
        reviewForm.classList.remove("show");

        button.innerHTML = `
            <i class="fa-solid fa-star"></i>
            Add Review
        `;
    }
    else {
        // Open the form
        reviewForm.classList.add("show");

        button.innerHTML = `
            <i class="fa-solid fa-xmark"></i>
            Close Review
        `;
    }
}