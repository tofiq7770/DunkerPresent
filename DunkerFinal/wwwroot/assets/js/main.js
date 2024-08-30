

document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('.add-to-fav-button').forEach(button => {
        button.addEventListener('click', async function () {
            const productId = this.getAttribute('data-product-id');
            const buttonElement = this;
            const svgElement = buttonElement.querySelector('svg');

            try {
                const response = await fetch('/Wishlist/Add', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'X-Requested-With': 'XMLHttpRequest'
                    },
                    body: JSON.stringify({ ProductId: parseInt(productId) })
                });

                const result = await response.json();

                if (response.ok) {
                    if (result.success) {
                        Swal.fire({
                            title: 'Success!',
                            text: result.message,
                            icon: 'success',
                            timer: 1200,
                            showConfirmButton: false
                        });

                        if (result.alreadyInWishlist) {
                            buttonElement.classList.add('already-in-wishlist');
                            svgElement.style.fill = 'white';
                            Swal.fire({
                                title: 'Already in Wishlist!',
                                text: 'This product is already in your wishlist.',
                                icon: 'info',
                                timer: 1200,
                                showConfirmButton: false
                            });
                        } else {
                            buttonElement.classList.add('already-in-wishlist');
                            svgElement.style.fill = 'white';
                        }
                    } else {
                        // Check for a redirect URL
                        if (result.redirectUrl) {
                            window.location.href = result.redirectUrl; // Redirect to login page
                        } else {
                            Swal.fire({
                                title: 'Error!',
                                text: result.message,
                                icon: 'error',
                                timer: 1200,
                                showConfirmButton: false
                            });
                        }
                    }
                } else {
                    Swal.fire({
                        title: 'Error!',
                        text: 'Failed to add product to wishlist. Please try again later.',
                        icon: 'error',
                        timer: 1200,
                        showConfirmButton: false
                    });
                }
            } catch (error) {
                console.error('Network error:', error);
                Swal.fire({
                    title: 'Network Error!',
                    text: 'An error occurred. Please try again.',
                    icon: 'error',
                    timer: 1200,
                    showConfirmButton: false
                });
            }
        });
    });
});

document.querySelectorAll('.add-to-basket-button').forEach(button => {
    button.addEventListener('click', async function () {
        const productId = this.getAttribute('data-product-id');

        try {
            const response = await fetch(`/Basket/Add?productId=${productId}`, {
                method: 'POST',
                headers: {
                    'X-Requested-With': 'XMLHttpRequest'
                }
            });

            const result = await response.json();

            if (response.ok) {
                if (result.redirectUrl) {
                    Swal.fire({
                        title: 'Login Required',
                        text: 'You need to be logged in to add items to your basket.',
                        icon: 'warning',
                        showConfirmButton: true,
                        confirmButtonText: 'Login'
                    }).then(() => {
                        window.location.href = result.redirectUrl;
                    });
                } else {
                    Swal.fire({
                        title: 'Success!',
                        text: result.message,
                        icon: 'success',
                        timer: 1200,
                        showConfirmButton: false
                    });

                    // Update basket count
                    document.querySelector('.count-basket').textContent = result.uniqueProductCount;
                    document.querySelector('.amount').textContent = result.uniqueProductCount;

                    if (!result.isUpdate) {
                        // Only add new HTML for new products
                        document.querySelector(".basket-products").innerHTML += result.partialView;
                    }
                }
            } else {
                Swal.fire({
                    title: 'Error!',
                    text: 'Failed to add product to basket. ' + result.message,
                    icon: 'error',
                    timer: 1200,
                    showConfirmButton: false
                });
            }
        } catch (error) {
            console.error('Network error:', error);
            Swal.fire({
                title: 'Network Error!',
                text: 'An error occurred. Please try again.',
                icon: 'error',
                timer: 1200,
                showConfirmButton: false
            });
        }
    });
});


//document.querySelectorAll('.add-to-basket-button').forEach(button => {
//    button.addEventListener('click', async function () {
//        const productId = this.getAttribute('data-product-id');

//        try {
//            const response = await fetch('/Basket/Add', {
//                method: 'POST',
//                headers: {
//                    'Content-Type': 'application/json',
//                    'X-Requested-With': 'XMLHttpRequest'
//                },
//                body: JSON.stringify(productId)
//            });

//            const result = await response.json();

//            if (response.ok) {
//                if (result.redirectUrl) {
//                    // Handle redirection if provided
//                    Swal.fire({
//                        title: 'Login Required',
//                        text: 'You need to be logged in to add items to your basket.',
//                        icon: 'warning',
//                        showConfirmButton: true,
//                        confirmButtonText: 'Login',
//                        onClose: () => {
//                            window.location.href = result.redirectUrl; // Redirect to login
//                        }
//                    });
//                } else {
//                    Swal.fire({
//                        title: 'Success!',
//                        text: result.message,
//                        icon: 'success',
//                        timer: 1200,
//                        showConfirmButton: false
//                    });

//                    document.querySelector('.count-basket').textContent = result.uniqueProductCount;

//                    updateBasketUI();
//                }
//            } else {
//                Swal.fire({
//                    title: 'Error!',
//                    text: 'Failed to add product to basket. ' + result.message,
//                    icon: 'error',
//                    timer: 1200,
//                    showConfirmButton: false
//                });
//            }
//        } catch (error) {
//            console.error('Network error:', error);
//            Swal.fire({
//                title: 'Network Error!',
//                text: 'An error occurred. Please try again.',
//                icon: 'error',
//                timer: 1200,
//                showConfirmButton: false
//            });
//        }
//    });
//});

//function updateBasketUI() {
//    // Implement this function to refresh the basket display, count items, etc.
//}


const debounce = (func, delay) => {
    let timeout;
    return function (...args) {
        clearTimeout(timeout);
        timeout = setTimeout(() => func.apply(this, args), delay);
    };
};

const handleButtonClick = debounce(async function () {
    // Your fetch and update code here
}, 300);

document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('.add-to-fav-button').forEach(button => {
        button.addEventListener('click', handleButtonClick);
    });
});


document.addEventListener('DOMContentLoaded', function () {
    // Get all filter buttons
    const filterButtons = document.querySelectorAll('.filter-btn');
    const showAllButton = document.querySelector('.show-all-btn');
    const productCards = document.querySelectorAll('.featured-products-card');

    function filterProducts(brandName) {
        productCards.forEach(card => {
            if (card.getAttribute('data-brand') === brandName || brandName === 'all') {
                card.style.display = 'block';
            } else {
                card.style.display = 'none';
            }
        });
    }

    filterButtons.forEach(button => {
        button.addEventListener('click', function () {
            const brandName = this.getAttribute('data-brand');
            filterProducts(brandName);
        });
    });

    showAllButton.addEventListener('click', function () {
        filterProducts('all');
    });
});