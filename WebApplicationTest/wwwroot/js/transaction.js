document.addEventListener("DOMContentLoaded", function () {

    const form = document.getElementById("transactionForm");

    const creditRadio = document.getElementById("creditcard");
    const selector = document.getElementById("creditCardSelect");

    const container = document.getElementById("credit-card-container");
    const cardForm = document.querySelector(".credit-card-form");

    function updatePaymentUI() {

        if (!creditRadio.checked) {

            container.classList.remove("show");

            if (cardForm) {
                cardForm.querySelectorAll("input").forEach(input => {
                    input.required = false;
                });
            }

            return;
        }

        container.classList.add("show");

        if (!selector || !cardForm)
            return;

        if (selector.value !== "") {

            cardForm.style.opacity = "0";
            cardForm.style.maxHeight = "0";

            cardForm.querySelectorAll("input").forEach(input => {
                input.required = false;
            });

        }
        else {

            cardForm.style.opacity = "1";
            cardForm.style.maxHeight = "400px";

            cardForm.querySelectorAll("input").forEach(input => {
                input.required = true;
            });

        }

    }

    document.querySelectorAll(".payment-method-radio")
        .forEach(radio => {
            radio.addEventListener("change", updatePaymentUI);
        });

    if (selector) {
        selector.addEventListener("change", updatePaymentUI);
    }

    form.addEventListener("submit", function (event) {

        const selectedPayment =
            document.querySelector('input[name="Transaction.PaymentType"]:checked');


        if (selectedPayment.value === "CreditCard") {

            if (selector && selector.value !== "")
                return;
        }

    });

    updatePaymentUI();

});


const cardNumberInput = document.querySelector("#UserNewCard_CardNumber");
const cvvInput = document.querySelector("#UserNewCard_CVV");
const expiryInput = document.querySelector("#UserNewCard_Expiry");


cardNumberInput.addEventListener("input", function () {
    this.value = this.value.replace(/\D/g, "");
});



cvvInput.addEventListener("input", function () {
    this.value = this.value.replace(/\D/g, "");

    if (this.value.length > 3) {
        this.value = this.value.substring(0, 3);
    }
});


function validateExpiry() {

    const selectedDate = new Date(expiryInput.value + "-01");
    const today = new Date();

    today.setHours(0,0,0,0);

    if (selectedDate <= today) {
        return false;
    }

    return true;
}



function validateCardNumber(cardNumber) {

    cardNumber = cardNumber.replace(/\s/g, "");

    if (!/^\d{13,19}$/.test(cardNumber)) {
        return false;
    }

    let sum = 0;
    let shouldDouble = false;


    for (let i = cardNumber.length - 1; i >= 0; i--) {

        let digit = parseInt(cardNumber[i]);

        if (shouldDouble) {
            digit *= 2;

            if (digit > 9) {
                digit -= 9;
            }
        }

        sum += digit;

        shouldDouble = !shouldDouble;
    }


    return sum % 10 === 0;
}


document.querySelector("form").addEventListener("submit", function(e){

    const cardNumber = cardNumberInput.value;
    const cvv = cvvInput.value;


    if (!validateCardNumber(cardNumber)) {
        e.preventDefault();
        alert("Invalid credit card number format.");
        return;
    }


    if (!validateExpiry()) {
        e.preventDefault();
        alert("Your card has expired.");
        return;
    }


    if (cvv.length !== 3) {
        e.preventDefault();
        alert("CVV must contain exactly 3 digits.");
        return;
    }

});

cardNumberInput.addEventListener("input", function () {

    let value = this.value.replace(/\D/g, "");


    if (value.length > 16) {
        value = value.substring(0, 16);
    }


    value = value.replace(/(.{4})/g, "$1 ").trim();


    this.value = value;
});
