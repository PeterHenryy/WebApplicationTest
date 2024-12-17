$(document).ready(function () {
    // Handle change event for reason select
    $(".reasonSelect").change(function () {
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
    let selectedReason = refundForm.find('.reasonSelect').val();
    let customReason = refundForm.find('.customReasonInput textarea').val();

    // Set the selected reason to either custom or selected value
    refundForm.find('.reasonSelect').val(selectedReason === 'Other' ? customReason : selectedReason);

    // Submit the form
    refundForm.submit();
}