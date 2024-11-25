function addItemToCart(itemID, quantity, productName) {
    const data = {
        itemID,
        quantity
    };
    $.ajax({
        url: "/ShoppingCart/AddItemToCart",
        type: "POST",
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data
    });
    toastr.success(`Added ${productName} to Cart! (x${quantity})`);
}

let couponDiscount = 0;
calculateOrderSubtotal();
displayOrderItems();
calculateShipping();
calculateTax();
calculateOrderTotal();

function updateItemsAndPrices(productID, quantity, price) {
    updateItemQuantityInCheckout(productID, quantity);
    updateItemQuantityHTML(productID);
    updateProductTotalPrice(productID, quantity, price);
    calculateOrderSubtotal(couponDiscount);
    displayOrderItems();
    calculateShipping();
    calculateOrderTotal();
    subtotalPriceBeforeCoupon = document.querySelector('.js-subtotal-price').innerHTML;
    totalBeforeCoupon = document.querySelector('.order-total-price').innerHTML;
}

function updateCartItemQuantity(quantity) {
    const cartItemQuantityElement = document.querySelector('.cart-item-quantity');
    let itemQuantity = Number(cartItemQuantityElement.innerHTML);
    itemQuantity += Number(quantity);
    cartItemQuantityElement.innerHTML = itemQuantity;
}

function updateItemQuantityHTML(productID) {
    const cartItemQuantityElement = document.querySelector('.cart-item-quantity');
    const quantitySelectors = document.querySelectorAll('.quantity-selector');
    let quantity = 0;
    quantitySelectors.forEach(x => quantity += Number(x.value));
    cartItemQuantityElement.innerHTML = quantity;
}

function updateProductTotalPrice(productID, itemQuantity, price) {
    const productTotalElement = document.querySelector(`.js-product-total-${productID}`);
    const productTotal = itemQuantity * parseFloat(price);
    productTotalElement.innerHTML = productTotal;
}

function resetDropdown(selectElement) {
    selectElement.selectedIndex = 0;
}

function updateItemQuantityInCheckout(itemID, quantity) {
    const data = {
        itemID,
        quantity
    };
    $.ajax({
        url: "/ShoppingCart/UpdateCartItemQuantity",
        type: "POST",
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data
    })

}
function clearCart() {
    const orderSummaryContainer = document.querySelector('.order-summary-container');
    const clearCartButton = document.querySelector('.clear-cart');
    orderSummaryContainer.remove();
    clearCartButton.innerHTML = '<h1>There are no items in your cart!</h1>';
}
function removeFromCart(itemID, productName) {
    const data = {
        itemID
    };
    $.ajax({
        url: "/ShoppingCart/RemoveFromCart",
        type: "POST",
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data
    })
    toastr.warning(`Removed ${productName} from cart!`);

    const cartProductElement = document.querySelector(`.js-cart-product-${itemID}`)
    cartProductElement.remove();
    const cartProducts = document.querySelectorAll('.cart-product');
    if (cartProducts.length === 0) {
        clearCart();
        updateItemQuantityHTML(itemID);
    }
    else {
        updateItemQuantityHTML(itemID);
        displayOrderItems();
        calculateOrderSubtotal(couponDiscount);
        calculateShipping(itemID);
        calculateOrderTotal();
    }
}
function calculateProductTotalPrices() {
    const productPrices = document.querySelectorAll('.product-total');
    let productsTotal = 0;
    productPrices.forEach(price => {
        productsTotal += Number(price.innerHTML);
    });
    return productsTotal;
}

function calculateOrderSubtotal(couponDiscount = 0) {
    const subtotalPrice = document.querySelector('.js-subtotal-price');
    let orderTotalPrice = calculateProductTotalPrices();
    subtotalPrice.innerHTML = ((orderTotalPrice * 100) / 100).toFixed(2);
    if (couponDiscount !== 0) {
        calculateTax();
        const summaryPricesBeforeCoupon = calculateSummaryPrices();
        const orderTotalBeforeCoupon = document.querySelector('.js-order-total-before-coupon');
        orderTotalBeforeCoupon.innerHTML = `<p>Was: <span style="position: absolute; right: 0;"> <del>$${summaryPricesBeforeCoupon.toFixed(2)}</del></span></p>`;
        orderTotalPrice -= (couponDiscount / 100) * orderTotalPrice;
        subtotalPrice.innerHTML = ((orderTotalPrice * 100) / 100).toFixed(2);
    }
    else {
        calculateTax();
    }
}

function displayOrderItems() {
    const subtotalItemsElement = document.querySelector('.js-subtotal-items');
    const cartItemQuantityElement = document.querySelector('.cart-item-quantity');
    const subtotalItems = cartItemQuantityElement.innerHTML;
    subtotalItemsElement.innerHTML = subtotalItems !== "1" ? `Subtotal (${subtotalItems} items)` : `Subtotal (${subtotalItems} item)`;
}

function calculateTax() {
    const taxPercentage = 8;
    const subtotal = document.querySelector('.js-subtotal-price');
    const formatedTotal = (Number(subtotal.innerHTML) * 100) / 100;
    const tax = Math.round((formatedTotal * (taxPercentage / 100)) * 100) / 100;
    const taxElement = document.querySelector('.tax-cost');
    taxElement.innerHTML = tax.toFixed(2);
}

function calculateShipping(productID) {
    const shippingElement = document.querySelector('.shipping-cost');
    const selectedShipping = document.querySelectorAll('.js-delivery-option');
    let shippingCost = 0;
    selectedShipping.forEach(element => {
        if (element.checked) {
            shippingCost += Number(element.value);
        }
    });
    shippingElement.innerHTML = (shippingCost === 0) ? "FREE" : `$${shippingCost}`;
    let wasTotal = calculateOrderTotal();
    if (couponDiscount !== 0) {
        const orderProductPrices = calculateProductTotalPrices()
        wasTotal += Math.round((orderProductPrices * (couponDiscount / 100)) * 100) / 100;
        const orderTotalBeforeCoupon = document.querySelector('.js-order-total-before-coupon');
        orderTotalBeforeCoupon.innerHTML = `<p>Was: <span style="position: absolute; right: 0;"> <del>$${wasTotal.toFixed(2)}</del></span></p>`;
    }
}

function calculateSummaryPrices() {
    const summaryPrices = document.querySelectorAll('.js-summary-price');
    let finalPrice = 0;
    summaryPrices.forEach(price => {
        if (price.innerHTML !== "FREE") {
            let fixedPrice = price.innerHTML.replace('$', '');
            finalPrice += Number(fixedPrice);
        }
    });
    return (finalPrice * 100) / 100;
}
function calculateOrderTotal() {
    const orderFinalPriceElement = document.querySelector('.order-total-price');
    const finalPrice = calculateSummaryPrices();
    const formattedFinalPrice = (finalPrice * 100) / 100;
    orderFinalPriceElement.innerHTML = `${formattedFinalPrice.toFixed(2)}`;
    return formattedFinalPrice;
}

$(document).ready(function () {
    const couponButton = $(".coupon-button");
    const couponInput = $(".coupon-input");
    const submitCoupon = $("#submit-coupon");
    couponButton.on("click", () => {
        couponInput.slideToggle("slow");
        submitCoupon.slideToggle("slow");
    });
    submitCoupon.on("click", () => {
        couponInput.slideToggle("slow");
        submitCoupon.slideToggle("slow");
    });
});

const submitCouponButton = document.getElementById('submit-coupon');
const couponCodeInput = document.getElementById('coupon-code');
let subtotalPriceBeforeCoupon = document.querySelector('.js-subtotal-price').innerHTML;
let totalBeforeCoupon = document.querySelector('.order-total-price').innerHTML;
let submitedCouponCode = "";

submitCouponButton.addEventListener('click', () => {
    var url = '/Transaction/Create?couponCode=' + encodeURIComponent(couponCodeInput.value);

    document.getElementById('checkout-link').setAttribute('href', url);
    if (submitedCouponCode !== couponCodeInput.value) {
        submitedCouponCode = couponCodeInput.value;
        validateCoupon(submitedCouponCode);
    }

});
function validateCoupon(couponCode) {
    $.ajax({
        type: 'GET',
        url: '/Transaction/ValidateCoupon',
        data: { couponCode },
        success: (result) => {
            if (result.couponValid) {
                console.log(result);
                giveTransactionDiscount(result, couponCode);
            }
            else {
                calculateOrderSubtotal();
                calculateOrderTotal();
                const orderTotalBeforeCoupon = document.querySelector('.js-order-total-before-coupon');
                const orderTotalElement = document.querySelector('.order-total');
                orderTotalBeforeCoupon.innerHTML = '';
                orderTotalElement.innerHTML = "Order Total:";
                toastr.error(`Coupon "${couponCode}" is not valid!`);
                couponDiscount = 0;
            }
        },
        error: function (error) {
            console.error(error);
        }
    });
}


function giveTransactionDiscount(result, couponCode) {
    const summaryPriceElement = document.querySelector('.js-summary-price');
    summaryPriceElement.innerHTML = Number(result.total).toFixed(2);
    const orderFinalPriceElement = document.querySelector('.order-total-price');
    const orderTotalBeforeCoupon = document.querySelector('.js-order-total-before-coupon');
    orderTotalBeforeCoupon.innerHTML = `<p>Was: <span style="position: absolute; right: 0;"> <del>$${orderFinalPriceElement.innerHTML}</del></span></p>`;
    calculateOrderTotal();
    const newOrderTotal = document.querySelector('.order-total');
    newOrderTotal.innerHTML = "New Order Total:";
    toastr.success(`Coupon "${couponCode}" applied!`);
    couponDiscount = result.couponPercentage;
}

function updateCartItemShippingOption(itemID, newShippingCost, newShippingOption) {
    $.ajax({
        type: 'POST',
        url: '/ShoppingCart/UpdateCartItemShippingOption',
        data: { itemID, newShippingCost, newShippingOption }
    });
}

function redirectToLogin(loginUrl, productId, quantity) {
    var returnUrl = 'ShoppingCart/DisplayCartItems';
    var url = loginUrl + '?returnUrl=' + encodeURIComponent(returnUrl) +
        '&cartItemQuantity=' + encodeURIComponent(quantity) +
        '&cartItemProductID=' + encodeURIComponent(productId);

    window.location.href = url;
}