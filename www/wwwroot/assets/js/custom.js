
$(function () {

    // MENU
    $('.nav-link').on('click', function () {
        $(".navbar-collapse").collapse('hide');
    });


    // AOS ANIMATION
    AOS.init({
        disable: 'mobile',
        duration: 800,
        anchorPlacement: 'center-bottom'
    });


    // SMOOTH SCROLL
    $(function () {
        $('.nav-link').on('click', function (event) {
            var $anchor = $(this);
            $('html, body').stop().animate({
                scrollTop: $($anchor.attr('href')).offset().top - 0
            }, 1000);
            event.preventDefault();
        });
    });

    // PROJECT SLIDE
    $('#project-slide').owlCarousel({
        loop: true,
        center: true,
        autoplayHoverPause: false,
        autoplay: true,
        lazyLoad: true,
        autoplayTimeout: 5000,
        dots: false,
        margin: 30,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                margin: 0,
            },
            768: {
                items: 2,
                dots: false,
            }
        }
    });

});



$('#sub_newsletter_btn').on('click', function () {
    const form = $('#sub_newsletter_form');
    form.validate({
        errorPlacement: function errorPlacement(error, element) {
            element.after(error);
        },
        rules: {
            Mail: {
                required: true,
                email: true,
            },
        },
        messages: {
            Mail: {
                required: "*",
                email: "*",
            },
        }
    });
    if (form.valid()) {
        var email = $("#sub_newsletter_form input[name='Mail']").val();

        var formData = new FormData();
        formData.append("Mail", email);

        $.ajax({
            url: "/home/besubscribe",
            type: "POST",
            processData: false,
            contentType: false,
            data: formData
        }).done(function (data) {
            alert(data);
        }).fail(
            function (data) {
                alert(data);
            }
        );
    }
});

$('#contact_newsletter_btn').on('click', function () {
    const form = $('#contact_newsletter_form');
    form.validate({
        errorPlacement: function errorPlacement(error, element) {
            element.after(error);
        },
        rules: {
            Mail: {
                required: true,
                email: true,
            },
        },
        messages: {
            Mail: {
                required: "*",
                email: "*",
            },
        }
    });
    if (form.valid()) {
        $(this).attr("disabled", true);

        var email = $("#contact_newsletter_form input[name='Mail']").val();

        var formData = new FormData();
        formData.append("Mail", email);

        $.ajax({
            url: "/home/besubscribe",
            type: "POST",
            processData: false,
            contentType: false,
            data: formData
        }).done(function (data) {
            $(".contact_newsletter_alert").addClass("alert-success").removeClass("d-none");
            $(".contact_newsletter_alert").text(data);
            console.log(data);
            setTimeout(function () { $(this).attr("disabled", false); }, 5000);
        }).fail(
            function (data) {
                $(".contact_newsletter_alert").addClass("alert-danger").removeClass("d-none").text(data);
                setTimeout(function () { $(this).attr("disabled", false); }, 5000);
            }
        );
    }
});