window.onload = function () {
    var imgs = document.querySelectorAll(".controller img");
    var mainImage = document.querySelector(".image img");
    var hs = document.querySelector("h2");

    imgs.forEach((ele) => {
        ele.onclick = function () {
            mainImage.src = ele.dataset.src; // Change the source of the main image
        };
    });
};

document.getElementById('addToCartButton').addEventListener('click', function (event) {
    event.preventDefault(); // Prevent default form submission

    var form = document.getElementById('addToCartForm');
    var formData = new FormData(form);

    // Make an AJAX request to submit the form data
    fetch(form.action, {
        method: form.method,
        body: formData
    })
        .then(response => {
            if (response.ok) {
                // Handle success response here (e.g., show a success message)
                console.log('Item added to cart successfully.');
            } else {
                // Handle error response here (e.g., show an error message)
                console.error('Error adding item to cart.');
            }
        })
        .catch(error => {
            console.error('Error adding item to cart:', error);
        });
});


$(document).ready(function () {


    var _productId = parseInt($('#productId').text());

    if (_productId !== 0) {
        $.ajax({
            url: '/Review/IndexReview',
            type: 'GET',
            data: { id: _productId },
            success: function (response) {

                $('#Review').html(response);
            },
            error: function (xhr, status, error) {
                console.error('Error:', error);
            }
        });
    }
}

);



var pages = ["1", "2", "3", "4", "5", "6", "7"];
var currentPageIndex = 0;

// Function to create pagination elements
function createPageElements() {
    var paginationList = document.getElementById("paginationList");
    paginationList.innerHTML = ""; // Clear previous elements

    // Add new page items to the pagination
    for (var i = currentPageIndex; i < currentPageIndex + 3; i++) {
        if (i >= pages.length) break; // Check if index is within array bounds
        var pageItem = document.createElement("li");
        pageItem.className = "page-item";
        var pageNumber = pages[i];
        pageItem.innerHTML = `<a class="page-link" onclick="sendPageNumber(${pageNumber})">${pageNumber}</a>`;
        paginationList.appendChild(pageItem);
    }

    // Add "Next" button
    var nextButton = document.createElement("li");
    nextButton.className = "page-item";
    nextButton.innerHTML = '<a class="page-link" >Next</a>';
    nextButton.onclick = nextPage;
    paginationList.appendChild(nextButton);

    // Add "Previous" button
    var prevButton = document.createElement("li");
    prevButton.className = "page-item";
    prevButton.innerHTML = '<a class="page-link">Previous</a>';
    prevButton.onclick = previousPage;
    paginationList.insertBefore(prevButton, paginationList.firstChild);
}

// Function to navigate to next page
function nextPage() {
    if (currentPageIndex + 3 < pages.length) {
        currentPageIndex++;
        createPageElements();
    } else {
        increasePageNumbers();
        currentPageIndex++;
        createPageElements();
    }
}

// Function to navigate to previous page
function previousPage() {
    if (currentPageIndex > 0) {
        currentPageIndex--;
        createPageElements();
    }
}

// Function to increase page numbers
function increasePageNumbers() {
    var lastPage = parseInt(pages[pages.length - 1]);
    for (var i = 1; i <= 3; i++) {
        pages.push((lastPage + i).toString());
    }
}

// Function to send selected page number to controller using AJAX
function sendPageNumber(pageNumber) {


    var _page = pageNumber - 1;
    var _productId = parseInt($('#productId').text());
    console.log(_page)
    console.log(_productId)
    $.ajax({
        type: "GET",
        url: "/Review/IndexReview?id=" + _productId + "&page=" + _page,
        // data: { id: _productId, count: _count },
        success: function (response) {

            $('#Review').html(response);
            console.log("Page number sent successfully:", _page);


        },
        error: function (xhr, status, error) {

            console.error("Error sending page number:", error);
        }
    });
}

// Initial creation of pagination elements
createPageElements();

createPageElements();
createPageElements();
createPageElements();
window.onload = function () {
    var imgs = document.querySelectorAll(".controller img");
    var mainImage = document.querySelector(".image img");
    var hs = document.querySelector("h2");

    imgs.forEach((ele) => {
        ele.onclick = function () {
            mainImage.src = ele.dataset.src; // Change the source of the main image
        };
    });
};
