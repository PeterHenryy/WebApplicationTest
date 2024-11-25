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
        var selectedPaymentMethod = $('input[name="Transaction.PaymentType"]:checked').val();
        var transactionTotalRaw = $('#transactionTotal').val();

        // Replace comma with period
        var transactionTotal = transactionTotalRaw.replace(',', '.');

        console.log('Raw Transaction Total:', transactionTotalRaw);
        console.log('Transaction Total with Period:', transactionTotal);

        // Ensure it's parsed as float
        var parsedTransactionTotal = parseFloat(transactionTotal);
        console.log('Parsed Transaction Total:', parsedTransactionTotal);

        if (selectedPaymentMethod === 'RewardPoints') {
            event.preventDefault(); // Prevent the default form submission

            $.ajax({
                url: '/Transaction/CheckRewardPoints', // URL to your controller action
                type: 'GET',
                data: { transactionTotal: parsedTransactionTotal }, // Send the TransactionTotal as a query parameter
                success: function (response) {
                    if (response.success) {
                        // Proceed with form submission
                        $('#transactionForm').off('submit').submit();
                    } else {
                        alert('Reward Points are not valid.');
                    }
                },
                error: function () {
                    alert('An error occurred while checking reward points.');
                }
            });
        }
    });
});