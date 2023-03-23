function updateCartTotal() {
    let cartTotal = 0;
    let currentCart = JSON.parse(localStorage.getItem("cart"));
    if (localStorage.getItem("cart") == null) {
        localStorage.setItem("cart", "[]");
    }
    if (currentCart != null) {
        for (let i = 0; i < currentCart.length; i++) {
            cartTotal += currentCart[i].count;
        }
        document.getElementById("cartTotal").innerHTML = cartTotal.toString();
    }
}
updateCartTotal();

function initListeners() {
    let items = document.getElementsByClassName("AddToCart");
    for (let i = 0; i < items.length; i++) {
        items[i].addEventListener('click', onAddToCart);
    };
}
initListeners();

function onAddToCart(event) {
    event.stopImmediatePropagation();
    const id = event.target.id;
    let currentCart = JSON.parse(localStorage.getItem("cart"));
    var currentProduct = currentCart.find(x => x.id == id);
    if (currentProduct === undefined) {
        currentCart.push({ id: id, count: 1 });
    }
    else {
        currentProduct.count += 1;
    }
    localStorage.setItem("cart", JSON.stringify(currentCart));
    updateCartTotal();
}