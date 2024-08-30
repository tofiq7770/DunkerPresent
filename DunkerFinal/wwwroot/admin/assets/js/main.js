const dltColorBtns = document.querySelectorAll(".delete-color");

dltColorBtns.forEach(dltBtn => {
    dltBtn.addEventListener("click", function () {
        Swal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, delete it!"
        }).then((result) => {
            if (result.isConfirmed) {
                const btn = this;
                const id = btn.getAttribute("color-id");
                $.ajax({
                    type: "POST",
                    url: `/Admin/Product/DeleteColor/${id}`,
                    success: function (result) {
                        btn.parentNode.parentNode.remove();

                        const colors = document.querySelector(".colors");
                        colors.innerHTML += result;
                    }
                });
            }
        });
    });
});

const dltImgBtns = document.querySelectorAll(".delete-img");

dltImgBtns.forEach(dltBtn => {
    dltBtn.addEventListener("click", function () {
        Swal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, delete it!"
        }).then((result) => {
            if (result.isConfirmed) {
                const btn = this;
                const imgId = btn.getAttribute("img-id");
                const productId = btn.getAttribute("product-id");

                $.ajax({
                    type: "POST",
                    url: `/Admin/Product/DeleteImgByProductId?imgId=${imgId}&productId=${productId}`,
                    success: function (result) {
                        if (result == "CountError") {
                            alert("Image min 1!");
                            return;
                        }

                        if (result == "IsMainError") {
                            alert("Main image min 1!");
                            return;
                        }

                        btn.parentNode.parentNode.remove();
                    }
                });
            }
        });
    });
});

const dltMainBtns = document.querySelectorAll(".main-img");

dltMainBtns.forEach(dltBtn => {
    dltBtn.addEventListener("click", function () {

        const btn = this;
        const imgId = btn.getAttribute("img-id");
        const productId = btn.getAttribute("product-id");

        $.ajax({
            type: "POST",
            url: `/Admin/Product/MakeMain?imgId=${imgId}&productId=${productId}`,
            success: function () {
                const images = document.querySelectorAll(".images");

                images.forEach(image => {
                    image.style.border = "none";
                })

                btn.parentNode.previousElementSibling.style.border = "5px solid red";
            }
        });
    });
});
