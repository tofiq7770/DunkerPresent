
//price

document.addEventListener('DOMContentLoaded', () => {
    const minPriceInput = document.getElementById('minPrice');
    const maxPriceInput = document.getElementById('maxPrice');
    const minPriceValue = document.getElementById('minPriceValue');
    const maxPriceValue = document.getElementById('maxPriceValue');

    function updatePriceValues() {
        minPriceValue.textContent = `$${minPriceInput.value}`;
        maxPriceValue.textContent = `$${maxPriceInput.value}`;
    }

    // Event listeners to update the price values when the sliders change
    minPriceInput.addEventListener('input', updatePriceValues);
    maxPriceInput.addEventListener('input', updatePriceValues);

    // Initial call to set the displayed price values
    updatePriceValues();
});



//sort

document.addEventListener('DOMContentLoaded', function () {
    var form = document.querySelector('#sortForm');
    var dropdownText = document.querySelector('#dropdownText');
    var dropdownMenu = document.querySelector('.dropdown-menu');

    if (!form || !dropdownMenu || !dropdownText) {
        console.error("Form or dropdown elements not found.");
        return;
    }

    // Get the sort value from the URL parameters
    var urlParams = new URLSearchParams(window.location.search);
    var sortValue = urlParams.get('sort');

    if (sortValue) {
        // Find the corresponding dropdown item and update its state
        var items = dropdownMenu.querySelectorAll('.dropdown-item');
        items.forEach(function (item) {
            if (item.getAttribute('data-value') === sortValue) {
                item.classList.add('active'); // Add 'active' class to the selected item
                dropdownText.textContent = item.textContent; // Update the button text
            } else {
                item.classList.remove('active'); // Remove 'active' class from non-selected items
            }
        });
    }

    dropdownMenu.addEventListener('click', function (e) {
        if (e.target && e.target.matches('.dropdown-item')) {
            e.preventDefault();
            e.stopPropagation();

            var sortValue = e.target.getAttribute('data-value');
            if (!sortValue) {
                console.error("Sort value not found.");
                return;
            }

            // Update hidden input with the selected sort value
            var hiddenInput = form.querySelector('input[name="sort"]');
            if (hiddenInput) {
                hiddenInput.value = sortValue;
            } else {
                hiddenInput = document.createElement('input');
                hiddenInput.type = 'hidden';
                hiddenInput.name = 'sort';
                hiddenInput.value = sortValue;
                form.appendChild(hiddenInput);
            }

            // Mark the selected item as active
            var items = dropdownMenu.querySelectorAll('.dropdown-item');
            items.forEach(function (item) {
                item.classList.remove('active'); // Remove 'active' class from all items
            });
            e.target.classList.add('active'); // Add 'active' class to the clicked item

            // Update button text
            dropdownText.textContent = e.target.textContent;

            // Submit the form
            form.submit();
        }
    });
});


//Basket and Wishlist

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
            const response = await fetch('/Basket/Add', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'X-Requested-With': 'XMLHttpRequest'
                },
                body: JSON.stringify(productId)
            });

            const result = await response.json();

            if (response.ok) {
                if (result.redirectUrl) {
                    Swal.fire({
                        title: 'Login Required',
                        text: 'You need to be logged in to add items to your basket.',
                        icon: 'warning',
                        showConfirmButton: true,
                        confirmButtonText: 'Login',
                        onClose: () => {
                            window.location.href = result.redirectUrl;
                        }
                    });
                } else {
                    Swal.fire({
                        title: 'Success!',
                        text: result.message,
                        icon: 'success',
                        timer: 1200,
                        showConfirmButton: false
                    });

                    document.querySelector('.count-basket').textContent = result.uniqueProductCount;

                    updateBasketUI();
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

function updateBasketUI() {
    // Implement this function to refresh the basket display, count items, etc.
}


const debounce = (func, delay) => {
    let timeout;
    return function (...args) {
        clearTimeout(timeout);
        timeout = setTimeout(() => func.apply(this, args), delay);
    };
};

const handleButtonClick = debounce(async function () {
}, 300);

document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('.add-to-fav-button').forEach(button => {
        button.addEventListener('click', handleButtonClick);
    });
});