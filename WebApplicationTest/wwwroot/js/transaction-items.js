$(document).ready(function () {
    $("#reasonSelect").change(function () {
        if ($(this).val() === "Other") {
            $("#customReasonInput").show();
        } else {
            $("#customReasonInput").hide();
        }
    });
});

$(document).ready(function () {
    const refundButton = $("#refund-button");
    const refundSelect = $("#refund-reason-select");
    refundButton.on("click", () => {
        refundSelect.slideToggle("slow");
    });
});

function submitForm() {
    var selectedReason = document.getElementById('reasonSelect').value;
    var customReason = document.querySelector('#customReasonInput textarea').value;

    document.getElementById('reasonSelect').value = selectedReason === 'Other' ? customReason : selectedReason;

    document.getElementById('refundForm').submit();
}
