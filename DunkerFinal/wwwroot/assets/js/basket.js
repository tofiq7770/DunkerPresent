 

    $(document).ready(function() {
        $('.increment-button').on('click', function () {
            const button = $(this);
            const id = parseInt(button.data('id')); // Ensure id is an integer

            $.ajax({
                url: '/Basket/Increase',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ id }), // Send as an object with 'id' key
                dataType: 'json',
                success: function (result) {
                    console.log('Response JSON:', result);

                    // Use the button's closest parent to find the correct elements
                    const container = button.closest('.counter-container');
                    const quantityElement = container.find('#value');
                    const totalPriceElement = button.closest('.wishlist-item').find('.total-price span');

                    // Update quantity and total price for the specific item
                    const newQuantity = parseInt(quantityElement.text()) + 1;
                    quantityElement.text(newQuantity);
                    totalPriceElement.text('$' + result.totalPrice.toFixed(2));

                    // Update the overall cart totals
                    updateCartTotals(result.total);

                    // SweetAlert for success
                    Swal.fire({
                        icon: 'success',
                        title: 'Quantity Increased',
                        text: `The quantity has been increased to ${newQuantity}.`,

                        timer: 1200,
                        showConfirmButton: false
                    });
                },
                error: function (xhr) {
                    console.log('Response Status:', xhr.status);
                    console.log('Response Headers:', xhr.getAllResponseHeaders());

                    // SweetAlert for failure
                    Swal.fire({
                        icon: 'error',
                        title: 'Failed to Increase Quantity',
                        text: `Server responded with status ${xhr.status}. ${xhr.responseText}`,

                        timer: 1200,
                        showConfirmButton: false
                    });
                }
            });
        });

    function updateCartTotals(totalItems) {
        // Update the total number of items
        const cartTotalElement = $('#cart-total'); // Ensure this ID matches your HTML
    if (cartTotalElement.length) {
        cartTotalElement.text('Total Items: ' + totalItems);
        }

    // Calculate and update subtotal and total price
    let subtotal = 0;
    $('.wishlist-item').each(function() {
            const itemTotalPrice = parseFloat($(this).find('.total-price span').text().replace('$', '')) || 0;
    subtotal += itemTotalPrice;
        });

    const subtotalElement = $('.cart-totals .totals-item .value').eq(0);
    const totalElement = $('.cart-totals .totals-item .value').eq(2);

    // Update subtotal and total price in the cart totals section
    subtotalElement.text('$' + subtotal.toFixed(2));
    totalElement.html('<strong>$' + subtotal.toFixed(2) + '</strong>');
    }
});




    $(document).ready(function() {
        $(document).on('click', '.minus-btn', function () {
            const button = $(this);
            const productId = button.data('id');
            console.log('Product ID:', productId);

            if (!productId || productId <= 0) {
                Swal.fire({
                    icon: 'error',
                    title: 'Invalid Request',
                    text: 'The product ID is not valid.',
                    timer: 1200,
                    showConfirmButton: false
                });
                return;
            }

            $.ajax({
                url: '/Basket/Decrease',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ id: productId }), // Ensure data is sent as an object
                dataType: 'json',
                success: function (response) {
                    if (response.success) {
                        console.log('Response JSON:', response);

                        const container = button.closest('.counter-container');
                        const quantityElement = container.find('#value');
                        const totalPriceElement = button.closest('.wishlist-item').find('.total-price span');

                        // Update the quantity and total price for this item
                        const oldQuantity = parseInt(quantityElement.text());
                        const newQuantity = oldQuantity - 1;

                        if (newQuantity > 0) {
                            // Update the quantity and total price
                            quantityElement.text(newQuantity);

                            // Calculate new total price for this item
                            const itemPrice = parseFloat(totalPriceElement.text().replace('$', '')) / oldQuantity;
                            const newTotalPrice = itemPrice * newQuantity;

                            totalPriceElement.text('$' + newTotalPrice.toFixed(2));
                        } else {
                            // Remove the product's container if the quantity is 0
                            button.closest('.wishlist-item').remove();
                        }

                        // Update the cart totals
                        updateCartTotals(response.totalQuantity);

                        // Update the basket count
                        $('.count-basket').text(response.basketCount);

                        Swal.fire({
                            icon: 'success',
                            title: 'Quantity Decreased',
                            text: newQuantity > 0
                                ? `The quantity has been decreased to ${newQuantity}.`
                                : 'The item has been removed from your cart.',
                            timer: 1200,
                            showConfirmButton: false
                        });
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Failed to Decrease Quantity',
                            text: response.message,
                            timer: 1200,
                            showConfirmButton: false
                        });
                    }
                },
                error: function (xhr) {
                    console.log('Response Status:', xhr.status);
                    console.log('Response Headers:', xhr.getAllResponseHeaders());
                    Swal.fire({
                        icon: 'error',
                        title: 'Failed to Decrease Quantity',
                        text: `Server responded with status ${xhr.status}. ${xhr.responseText}`,
                        timer: 1200,
                        showConfirmButton: false
                    });
                }
            });
        });

    function updateCartTotals(totalQuantity) {
        let subtotal = 0;
    let hasItems = false;

    // Calculate subtotal from visible items
    $('.wishlist-item').each(function() {
        hasItems = true; // There are items in the cart
    const itemTotalPrice = parseFloat($(this).find('.total-price span').text().replace('$', '')) || 0;
    subtotal += itemTotalPrice;
        });

    if (hasItems) {
        // Update subtotal and total price in the cart totals section
        $('.cart-totals .totals-item .value').eq(0).text('$' + subtotal.toFixed(2)); // Subtotal
    $('.cart-totals .totals-item .value').eq(2).html('<strong>$' + subtotal.toFixed(2) + '</strong>'); // Total

    // Update the total items count
    const cartTotalElement = $('#cart-total');
    if (cartTotalElement.length) {
        cartTotalElement.text('Total Items: ' + totalQuantity);
            }

    // Show the cart totals section and hide the "No Products Added" message
    $('.cart-totals').show();
    $('#no-products-message').hide();
        } else {
        // Hide the cart totals section and show the "No Products Added" message
        $('.cart-totals').hide();
    $('#no-products-message').show();
        }
    }
});





    $(document).ready(function() {
        $(document).on('click', '.delete-btn', function (e) {
            e.preventDefault();

            const button = $(this);
            const productId = button.data('id');

            if (!productId) {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Product ID is missing.',
                    timer: 1200,
                    showConfirmButton: false
                });
                return;
            }

            Swal.fire({
                title: 'Are you sure?',
                text: "This action will remove the product from your cart.",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Yes, delete it!',
                cancelButtonText: 'No, cancel!',
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        url: `/Basket/Delete/${productId}`,
                        type: 'POST',
                        success: function (response) {
                            if (response.success) {
                                button.closest('.wishlist-item').remove();

                                // Update cart totals
                                $('#cart-total').text('Total Items: ' + response.totalQuantity);
                                $('#total-price').text('$' + response.totalPrice.toFixed(2));

                                // Update totals section
                                $('.totals-item .value').each(function () {
                                    const label = $(this).prev().text().trim();
                                    if (label === 'Subtotal' || label === 'Total') {
                                        $(this).text('$' + response.totalPrice.toFixed(2));
                                    }
                                });

                                // Update basket count
                                $('.count-basket').text(response.basketCount);

                                // Show or hide "No Products Added" message and cart totals section
                                if (response.isEmpty) {
                                    $('#wishlist-items').hide();
                                    $('#no-products-message').show();
                                    $('.cart-totals').hide(); // Hide cart totals if empty
                                } else {
                                    $('#wishlist-items').show();
                                    $('#no-products-message').hide();
                                    $('.cart-totals').show(); // Show cart totals if not empty
                                }

                                Swal.fire({
                                    icon: 'success',
                                    title: 'Deleted!',
                                    text: 'The product has been removed from your cart.',
                                    timer: 1200,
                                    showConfirmButton: false
                                });
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Failed',
                                    text: response.message,
                                    timer: 1200,
                                    showConfirmButton: false
                                });
                            }
                        },
                        error: function (xhr) {
                            Swal.fire({
                                icon: 'error',
                                title: 'Error',
                                text: `Server responded with status ${xhr.status}. ${xhr.responseText}`,
                                timer: 1200,
                                showConfirmButton: false
                            });
                        }
                    });
                }
            });
        });
    });
