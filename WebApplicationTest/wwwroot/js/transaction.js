document.addEventListener("DOMContentLoaded", function () {
    let form = document.getElementById('transactionForm');
    let creditCardRadio = document.getElementById('creditcard');
    let creditCardSelect = document.getElementById('creditCardSelect');

    form.addEventListener('submit', function (event) {
        // Check if the "Credit Card" radio button is selected
        if (creditCardRadio.checked) {
            // Check if a card is selected
            if (creditCardSelect.value === "" || creditCardSelect.value === null) {
                // Prevent form submission
                event.preventDefault();
                alert("Please select a credit card.");
            }
        }
    });
});

$(document).ready(function () {
    $('#transactionForm').on('submit', function (event) {
        let selectedPaymentMethod = $('input[name="Transaction.PaymentType"]:checked').val();
        let transactionTotalRaw = $('#transactionTotal').val();

        let transactionTotal = transactionTotalRaw.replace(',', '.');


        let parsedTransactionTotal = parseFloat(transactionTotal);

        if (selectedPaymentMethod === 'RewardPoints') {
            event.preventDefault(); 

            $.ajax({
                url: '/Transaction/CheckRewardPoints', 
                type: 'GET',
                data: { transactionTotal: parsedTransactionTotal }, 
                success: function (response) {
                    if (response.success) {
                        // Proceed with form submission
                        $('#transactionForm').off('submit').submit();
                    } else {
                        alert('You do not have enough reward points for this purchase! You can check how many reward points you have by looking to the right of your name in the website header!');
                    }
                },
                error: function () {
                    alert('An error occurred while checking reward points.');
                }
            });
        }
    });
});