'use strict';

(function ($) {

    /*------------------
        Preloader
    --------------------*/
    $(window).on('load', function () {
        $(".loader").fadeOut();
        $("#preloder").delay(200).fadeOut("slow");

        /*------------------
            Gallery filter
        --------------------*/
        $('.gallery-controls li').on('click', function () {
            $('.gallery-controls li').removeClass('active');
            $(this).addClass('active');
        });
        if ($('.gallery-filter').length > 0) {
            var containerEl = document.querySelector('.gallery-filter');
            var mixer = mixitup(containerEl);
        }

    });

    /*------------------
        Background Set
    --------------------*/
    $('.set-bg').each(function () {
        var bg = $(this).data('setbg');
        $(this).css('background-image', 'url(' + bg + ')');
    });

    /*------------------
        Navigation
    --------------------*/
    $(".mobile-menu").slicknav({
        prependTo: '#mobile-menu-wrap',
        allowParentLinks: true
    });

    /*------------------
        Menu Hover
    --------------------*/
    $(".header-section .nav-menu .mainmenu ul li").on('mousehover', function () {
        $(this).addClass('active');
    });
    $(".header-section .nav-menu .mainmenu ul li").on('mouseleave', function () {
        $('.header-section .nav-menu .mainmenu ul li').removeClass('active');
    });

    /*------------------------
        Class Slider
    ----------------------- */
    $(".classes-slider").owlCarousel({
        items: 3,
        dots: true,
        autoplay: true,
        loop: true,
        smartSpeed: 1200,
        responsive: {
            0: {
                items: 1,
            },
            768: {
                items: 3,
            },
            992: {
                items: 3,
            }
        }
    });

    $(".product-silder").owlCarousel({
        items: 3,
        dots: true,
        autoplay: true,
        loop: true,
        smartSpeed: 1200,
        responsive: {
            0: {
                items: 1,
            },
            768: {
                items: 3,
            },
            992: {
                items: 3,
            }
        }
    })

    /*------------------------
        Testimonial Slider
    ----------------------- */
    $(".testimonial-slider").owlCarousel({
        items: 1,
        dots: false,
        autoplay: true,
        loop: true,
        smartSpeed: 1200,
        nav: true,
        navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"]
    });

    /*------------------
        Magnific Popup
    --------------------*/
    $('.video-popup').magnificPopup({
        type: 'iframe'
    });

    /*------------------
        About Counter Up
    --------------------*/
    $('.count').each(function () {
        $(this).prop('Counter', 0).animate({
            Counter: $(this).text()
        }, {
            duration: 4000,
            easing: 'swing',
            step: function (now) {
                $(this).text(Math.ceil(now));
            }
        });
    });

      // nav click
      $('.nav.nav-tabs li a').on('click', function () {
        $('.nav.nav-tabs li a').removeClass('active-color');
        $(this).addClass('active-color');
    });


})(jQuery);



/*------------------
       Product List
    --------------------*/
    // For Filters
    document.addEventListener("DOMContentLoaded", function () {
        var filterBtn = document.getElementById('filter-btn');
        var btnTxt = document.getElementById('btn-txt');
        var filterAngle = document.getElementById('filter-angle');

        $('#filterbar').collapse(false);
        var count = 0, count2 = 0;
        function changeBtnTxt() {
            $('#filterbar').collapse(true);
            count++;
            if (count % 2 != 0) {
                filterAngle.classList.add("fa-angle-right");
                btnTxt.innerText = "show filters"
                filterBtn.style.backgroundColor = "#36a31b";
            }
            else {
                filterAngle.classList.remove("fa-angle-right")
                btnTxt.innerText = "hide filters"
                filterBtn.style.backgroundColor = "#ff935d";
            }

        }

        // For Applying Filters
        $('#inner-box').collapse(false);
        $('#inner-box2').collapse(false);

        // For changing NavBar-Toggler-Icon
        var icon = document.getElementById('icon');

        function chnageIcon() {
            count2++;
            if (count2 % 2 != 0) {
                icon.innerText = "";
                icon.innerHTML = '<span class="far fa-times-circle" style="width:100%"></span>';
                icon.style.paddingTop = "5px";
                icon.style.paddingBottom = "5px";
                icon.style.fontSize = "1.8rem";


            }
            else {
                icon.innerText = "";
                icon.innerHTML = '<span class="navbar-toggler-icon"></span>';
                icon.style.paddingTop = "5px";
                icon.style.paddingBottom = "5px";
                icon.style.fontSize = "1.2rem";
            }
        }

        // Showing tooltip for AVAILABLE COLORS
        $(function () {
            $('[data-tooltip="tooltip"]').tooltip()
        })

        // For Range Sliders
        var inputLeft = document.getElementById("input-left");
        var inputRight = document.getElementById("input-right");

        var thumbLeft = document.querySelector(".slider > .thumb.left");
        var thumbRight = document.querySelector(".slider > .thumb.right");
        var range = document.querySelector(".slider > .range");

        var amountLeft = document.getElementById('amount-left')
        var amountRight = document.getElementById('amount-right')

        function setLeftValue() {
            var _this = inputLeft,
                min = parseInt(_this.min),
                max = parseInt(_this.max);

            _this.value = Math.min(parseInt(_this.value), parseInt(inputRight.value) - 1);

            var percent = ((_this.value - min) / (max - min)) * 100;

            thumbLeft.style.left = percent + "%";
            range.style.left = percent + "%";
            amountLeft.innerText = parseInt(percent * 100);
        }
        setLeftValue();

        function setRightValue() {
            var _this = inputRight,
                min = parseInt(_this.min),
                max = parseInt(_this.max);

            _this.value = Math.max(parseInt(_this.value), parseInt(inputLeft.value) + 1);

            var percent = ((_this.value - min) / (max - min)) * 100;

            amountRight.innerText = parseInt(percent * 100);
            thumbRight.style.right = (100 - percent) + "%";
            range.style.right = (100 - percent) + "%";
        }
        setRightValue();

        inputLeft.addEventListener("input", setLeftValue);
        inputRight.addEventListener("input", setRightValue);

        inputLeft.addEventListener("mouseover", function () {
            thumbLeft.classList.add("hover");
        });
        inputLeft.addEventListener("mouseout", function () {
            thumbLeft.classList.remove("hover");
        });
        inputLeft.addEventListener("mousedown", function () {
            thumbLeft.classList.add("active");
        });
        inputLeft.addEventListener("mouseup", function () {
            thumbLeft.classList.remove("active");
        });

        inputRight.addEventListener("mouseover", function () {
            thumbRight.classList.add("hover");
        });
        inputRight.addEventListener("mouseout", function () {
            thumbRight.classList.remove("hover");
        });
        inputRight.addEventListener("mousedown", function () {
            thumbRight.classList.add("active");
        });
        inputRight.addEventListener("mouseup", function () {
            thumbRight.classList.remove("active");
        });
    });
