// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function navigateTo(url) {
    // Replace the current URL with the new URL
    window.location.replace(url);
}







function toggleButtons(buttonIds) {
    buttonIds.forEach(function (buttonId) {
        var button = document.getElementById(buttonId);
        button.addEventListener('click', function () {
            var hiddenButtonId, shownButtonId;
            if (buttonId.includes('button3')) {
                hiddenButtonId = 'button3';
                shownButtonId = 'button4';
            } else if (buttonId.includes('button4')) {
                hiddenButtonId = 'button4';
                shownButtonId = 'button3';
            } else if (buttonId.includes('button5')) {
                hiddenButtonId = 'button5';
                shownButtonId = 'button6';
            } else if (buttonId.includes('button6')) {
                hiddenButtonId = 'button6';
                shownButtonId = 'button5';
            } else if (buttonId.includes('button7')) {
                hiddenButtonId = 'button7';
                shownButtonId = 'button8';
            } else if (buttonId.includes('button8')) {
                hiddenButtonId = 'button8';
                shownButtonId = 'button7';
            } else if (buttonId.includes('button9')) {
                hiddenButtonId = 'button9';
                shownButtonId = 'button10';
            } else if (buttonId.includes('button10')) {
                hiddenButtonId = 'button10';
                shownButtonId = 'button9';
            } else if (buttonId.includes('button11')) {
                hiddenButtonId = 'button11';
                shownButtonId = 'button12';
            } else if (buttonId.includes('button12')) {
                hiddenButtonId = 'button12';
                shownButtonId = 'button11';
            } else if (buttonId.includes('button13')) {
                hiddenButtonId = 'button13';
                shownButtonId = 'button14';
            } else if (buttonId.includes('button14')) {
                hiddenButtonId = 'button14';
                shownButtonId = 'button13';
            }

            document.getElementById(hiddenButtonId).style.display = 'none';
            document.getElementById(shownButtonId).style.display = 'block';
        });

        // Disable automatic carousel sliding
        button.addEventListener('click', function (e) {
            e.preventDefault();
        });
    });
}

function reloadPage() {

    location.reload();
}

toggleButtons(['button3', 'button4',
    'button5', 'button6',
    'button7', 'button8',
    'button9', 'button10',
    'button11', 'button12',
    'button13', 'button14']);



    //const swiper = new Swiper('.swiper', {

    //    loop: true,

    //pagination: {
    //    el: '.swiper-pagination',
    //        },


    //navigation: {
    //    nextEl: '.swiper-button-next',
    //prevEl: '.swiper-button-prev',
    //        },

    //slidesPerView: 1,
    //spaceBetween: 10,
    //// Responsive breakpoints
    //breakpoints: {
    //    // when window width is >= 320px
    //    320: {
    //    slidesPerView: 2,
    //spaceBetween: 20
    //            },
    //// when window width is >= 480px
    //480: {
    //    slidesPerView: 3,
    //spaceBetween: 30
    //            },
    //// when window width is >= 640px
    //640: {
    //    slidesPerView: 5,
    //spaceBetween: 40
    //            }
    //        }
    //    });
