getProducts();
updateCheckoutBtn();

function getProducts() {
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/Cart");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            // receive response from server
            if (this.status !== 200) {
                return;
            }
            let data = JSON.parse(this.responseText);
            generateCartDetailsHtml(data);
            updateOrderSummary();
            updateCheckoutBtn();
        }
    }
    let data = localStorage.getItem("cart");
    xhr.send(data);
}

function checkout() {
    console.log("test");
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/Cart/Checkout");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status !== 200) {
                return;
            }
            let data = JSON.parse(this.responseText);
            if (data) {
                resetCart();
                updateCheckoutBtn();
                alert("Checkout Success!");
                window.location.href = "/Orders";
            } else {
                alert("Please login before checkout.");
                window.location.href = "/Account";
            }
        }
    }
    let data = localStorage.getItem("cart");
    xhr.send(data);
}

function generateCartDetailsHtml(data) {
    let htmlString = "";
    let currentCart = JSON.parse(localStorage.getItem("cart"));
    for (let i = 0; i < data.length; i++) {
        let count = currentCart.find(x => x.id == data[i].productId).count;

        htmlString += `
<div class="row mb-5" id="deleterow-${data[i].productName}">
    <div class="col-8 px-5 setting">
        <div class="card-deck px-5 setting" min-width="300">
            <div class="card">
                <img class="card-img-top" src="${data[i].productImg}" alt="image not found" height="200" style="object-fit: contain; padding:10px;" title="View Product Details">
                    <h5 class="card-header">${data[i].productName}</h5>
                    <div class="card-body">
                        <p class="card-text d-flex justify-content-between">
                        </p>
                        <p class="card-text font-weight-light">
                            ${data[i].productDescription}
                        </p>
                        <p></p>
                    </div>
            </div>
        </div>
    </div>
    <div class="col-4 pt-3">
        <p>Unit Price: $<span class="price-${data[i].productId}">${data[i].productPrice}</span></p>
        <p>Subtotal: $<span id="subtotal-${data[i].productId}" idx="${data[i].productId}" class="subtotal" >${data[i].productPrice * count}</span></p>
        <div style="vertical-align:middle;">
        Quantity: <input class="product-counter from-control fw-bold text-center rounded border-0 bg-light p-2" type="number" style="width: 4rem;" min="1" max="99" value="${count}" step="1" id="qty-${data[i].productId}" productId="${data[i].productId}" productName="${data[i].productName}" productPrice="${data[i].productPrice}">
        <button style="margin-left: 10px; position:relative; top:-3px;" type="button" class="btn btn-danger deletebtn" idx="${data[i].productId}" id="deletebtn-${data[i].productName}" value="${data[i].productName}"><i style="pointer-events:none;" class="fas fa-trash"></i></button>
    </div>
</div >
</div >
`;
    }
    document.getElementById("cartDetailsTable").innerHTML = htmlString;
    initCounterListeners();
}

function initCounterListeners() {
    let input = document.getElementsByClassName("product-counter");
    for (let i = 0; i < input.length; i++) {
        input[i].addEventListener('input', updateProductQty);
    };

    let deleteBtns = document.getElementsByClassName("deletebtn");
    for (let i = 0; i < deleteBtns.length; i++) {
        deleteBtns[i].addEventListener('click', removeProduct);
    }
}

function resetCart() {
    let currentCart = JSON.parse(localStorage.getItem("cart"));
    if (currentCart !== undefined || currentCart.length != 0) {
        localStorage.setItem("cart", "[]");
    }
    let deleteAllItems = document.getElementById("cartDetailsTable");
    deleteAllItems.remove();
    document.getElementById("cart-empty").innerHTML = `There are no items in your shopping cart. <a href="/Home">Continue shopping.</a>`;
    updateOrderSummary();
    updateCartTotal();
    updateCheckoutBtn();

}

function removeProduct(event) {
    let name = event.target.value;
    let elem = document.getElementById("deleterow-" + name);
    let elemForDeleteBtn = document.getElementById("deletebtn-" + name);
    removeProductRow(elem, elemForDeleteBtn);
    reinitListeners();
    updateCartTotal();
    updateOrderSummary();
    updateCheckoutBtn();
}

function removeProductRow(elem, elemForDeleteBtn) {
    if (elem != null && elemForDeleteBtn != null) {

        let currentCart = JSON.parse(localStorage.getItem("cart"));

        if (currentCart !== undefined || currentCart.length != 0) {
            let idx = currentCart.findIndex(product => product.id === elemForDeleteBtn.getAttribute("idx"));

            if (idx != -1) {
                currentCart.splice(idx, 1);
                localStorage.setItem("cart", JSON.stringify(currentCart));
            }
        }
        elem.remove();
    }
    updateCheckoutBtn();
}

function updateProductQty(event) {
    let elem = document.getElementById(event.target.id);
    if (elem != null) {
        let productId = elem.getAttribute("productId");
        let currentCart = JSON.parse(localStorage.getItem("cart"));
        if (currentCart !== undefined || currentCart.length != 0) {
            let idx = currentCart.findIndex(product => product.id === productId);
            if (idx != -1) {
                currentCart[idx].count = parseInt(elem.value);
            }
            if (elem.value == "0" || elem.value == "") {
                let answer = confirm("Are you sure you want to remove this item?");
                if (answer) {
                    let name = elem.getAttribute("productName");
                    let elemRow = document.getElementById("deleterow-" + name);
                    let elemForDeleteBtn = document.getElementById("deletebtn-" + name);
                    removeProductRow(elemRow, elemForDeleteBtn);
                    currentCart[idx].splice(idx, 1);
                } else {
                    elem.value = 1;
                    currentCart[idx].count = 1;
                }
                updateOrderSummary();
            }
            localStorage.setItem("cart", JSON.stringify(currentCart));
            updateProductQtyDisplay(productId);
            let productPrice = parseInt(event.target.getAttribute("productPrice"));
            let subtotal = productPrice * currentCart[idx].count;
            document.getElementById("subtotal-" + productId).innerHTML = subtotal.toString();
            updateCartTotal();
            updateOrderSummary();
            updateCheckoutBtn();
        }

    }
}

function updateProductQtyDisplay(productId)
{
    let currentCart = JSON.parse(localStorage.getItem("cart"));

    if (currentCart !== undefined || currentCart.length != 0) {
        let idx = currentCart.findIndex(product => product.id === productId);
        if (idx != -1) {
            let qtyDisplay = document.getElementById("qty-" + productId);
            if (qtyDisplay != null) {
                qtyDisplay.innerHTML = currentCart[idx].count;
                qtyDisplay.value = currentCart[idx].count;

                updateCartTotal();
                updateCheckoutBtn();
                updateOrderSummary();
            }
        }
    }
}

function updateSubtotal(productid) {
    console.log("productid" + productid);
    let qtyElement = document.getElementById("qty-" + productid); console.log("qtyElement" + qtyElement.value);

    let priceElement = document.getElementById("price-" + productid);
    console.log("priceElement innerhtml" + priceElement.innerHTML);
    let subtotalElement = document.getElementById("subtotal-" + productid);
    console.log(subtotalElement);
}

function updateOrderSummary() {
    let cartTotalPriceElem = document.getElementById("cartTotalPrice");
    let cartTotalPrice = 0;
    let subtotal = document.getElementsByClassName("subtotal");
    for (let i = 0; i < subtotal.length; i++) {
        cartTotalPrice += parseFloat(subtotal[i].innerHTML);
    }
    cartTotalPriceElem.innerHTML = cartTotalPrice.toFixed(2).toString();
}

function cartTotal() {
    let currentCart = JSON.parse(localStorage.getItem("cart"));
    return currentCart.length;
}

function updateCheckoutBtn() {
    let checkoutBtn = document.getElementById("checkoutBtn");
    checkoutBtn.disabled = true;
    if (cartTotal() > 0) {
        checkoutBtn.disabled = false;
    }
    if (cartTotal() == 0) {
        document.getElementById("cart-empty").innerHTML = `There are no items in your shopping cart.`;
    }
}

window.onload = function () {
    let deleteBtns = document.getElementsByClassName("deletebtn");
    for (let i = 0; i < deleteBtns.length; i++) {
        deleteBtns[i].addEventListener('click', removeProduct);
    }

    let clearCartBtn = document.getElementById("clearBtn");
    clearCartBtn.addEventListener('click', () => { this.resetCart(); });
}

function reinitListeners() {
    let deleteBtns = document.getElementsByClassName("deletebtn");
    for (let i = 0; i < deleteBtns.length; i++) {
        deleteBtns[i].addEventListener('click', removeProduct);
    }
}