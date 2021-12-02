$(document).ready(function () {
    /*
     CLICK DATA
 */

    window.addEventListener('beforeunload', function (e) {
        e.preventDefault();
        //e.returnValue = '';
        /*CLICK END DATE DATA*/
        var formData = new FormData();
        formData.append("leavinginpage", "true");
        $.ajax({
            url: "/home/clickdata",
            type: "POST",
            processData: false,
            contentType: false,
            dataType: 'json',
            data: formData
        }).done(function (data) {
        });
    });

    const permalink = window.location.pathname;
    const formData = new FormData();
    formData.append("permalink", permalink);

    $.ajax({
        url: "/home/ClickData",
        type: "POST",
        processData: false,
        contentType: false,
        dataType: 'json',
        data: formData
    }).done(function (data) {
    });

    //LAZY LOADING JS
    $('.lazy').Lazy({
        lazyLoader: function (element) {
            element.load();
        },
        afterLoad: function (element) {
            element.closest("div").children(".lazy-loader").hide();
        },
        //element.html("<div class='lazy-loader'><div class='loader'></div></div>");
        scrollDirection: 'vertical',
        visibleOnly: true,
        effect: "fadeIn",
        effectTime: 1000,
        threshold: 0,
    });

    //LAZY LOADING BACKGROUND
    $('.lazy-bg').Lazy({
        lazyLoader: function (element) {
            element.load();
        },
        afterLoad: function (element) {
            element.children(".lazy-loader").hide();
        },
        //element.html("<div class='lazy-loader'><div class='loader'></div></div>");
        scrollDirection: 'vertical',
        visibleOnly: true,
        effect: "fadeIn",
        effectTime: 1000,
        threshold: 0,
    });

});