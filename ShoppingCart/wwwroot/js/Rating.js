function SaveFunction(button) {
    var target_product = button.getAttribute("id").substring(7);
    var orderID_productID = target_product.split("_")
    var rate_product = document.getElementById("rate_" + target_product);
    var rate_elements_children = rate_product.children;
    var holder = 0;
    for (let i = 0; i < rate_elements_children.length; i++) {
        if (rate_elements_children[i].checked) {
            holder = rate_elements_children[i].value;
        }
    }

    if (button.innerHTML == "Save") {
        button.innerHTML = "Edit Rating";
    }
    else {
        button.innerHTML = "Save";

    }

    if (!rate_product.hasAttribute('style')) {
        rate_product.setAttribute('style', 'pointer-events: none;');
        var latestRating = 0;
        for (let i = 0; i < rate_elements_children.length; i++) {
            var elem = rate_elements_children[i];
            elem.disabled = true;
            if (rate_elements_children[i].checked) {
                latestRating = rate_elements_children[i].getAttribute("value");
            }
        }
        sendProductRating(orderID_productID[0], orderID_productID[1], latestRating);
    }
    else {
        rate_product.removeAttribute('style');
        for (let i = 0; i < rate_elements_children.length; i++) {
            var elem = rate_elements_children[i];
            elem.disabled = false;
        }
    }
}

function sendProductRating(orderID, productID, rating) {
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/Orders/UpdateRating");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status !== 200) {
                return;
            }
            let data = JSON.parse(this.responseText);

            console.log(data);
        }
    }

    let data = { "orderID": orderID, "productID": productID, "rating": rating }
    xhr.send(JSON.stringify(data));
}