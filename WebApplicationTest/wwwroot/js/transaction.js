document.addEventListener("DOMContentLoaded", function () {

    const form = document.getElementById("transactionForm");

    const creditRadio = document.getElementById("creditcard");
    const selector = document.getElementById("creditCardSelect");

    const container = document.getElementById("credit-card-container");
    const cardForm = document.querySelector(".credit-card-form");

    //----------------------------------------------------
    // Updates the credit card UI
    //----------------------------------------------------
    function updatePaymentUI() {

        // Credit Card NOT selected
        if (!creditRadio.checked) {

            container.classList.remove("show");

            if (cardForm) {
                cardForm.querySelectorAll("input").forEach(input => {
                    input.required = false;
                });
            }

            return;
        }

        // Credit Card selected
        container.classList.add("show");

        if (!selector || !cardForm)
            return;

        // User selected an existing card
        if (selector.value !== "") {

            cardForm.style.opacity = "0";
            cardForm.style.maxHeight = "0";

            cardForm.querySelectorAll("input").forEach(input => {
                input.required = false;
            });

        }
        // User wants to use a new card
        else {

            cardForm.style.opacity = "1";
            cardForm.style.maxHeight = "400px";

            cardForm.querySelectorAll("input").forEach(input => {
                input.required = true;
            });

        }

    }

    //----------------------------------------------------
    // Payment method changed
    //----------------------------------------------------
    document.querySelectorAll(".payment-method-radio")
        .forEach(radio => {
            radio.addEventListener("change", updatePaymentUI);
        });

    //----------------------------------------------------
    // Saved card changed
    //----------------------------------------------------
    if (selector) {
        selector.addEventListener("change", updatePaymentUI);
    }

    //----------------------------------------------------
    // Validate before submitting
    //----------------------------------------------------
    form.addEventListener("submit", function (event) {

        const selectedPayment =
            document.querySelector('input[name="Transaction.PaymentType"]:checked');

        if (!selectedPayment) {
            event.preventDefault();
            alert("Please choose a payment method.");
            return;
        }

        if (selectedPayment.value === "CreditCard") {

            // Saved card selected -> OK
            if (selector && selector.value !== "")
                return;

            // New card selected -> browser validates required fields
        }

    });

    //----------------------------------------------------
    // Initial page state
    //----------------------------------------------------
    updatePaymentUI();

  
});
