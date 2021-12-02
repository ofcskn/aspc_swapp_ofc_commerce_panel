$(document).ready(function () {
    const basketSpan = $("#basket_count_span");
    var formData = new FormData();
    formData.append("filter", "get");
    $.ajax({
        url: "/admin/product/shoppingbasket",
        method: 'POST',
        data: formData,
        dataType: "json",
        processData: false,
        contentType: false,
        success: function (data) {
            basketSpan.html(data);
        },
    });
});

$(document).ready(function () {
    $.each($(".main-sidebar .nav-item .nav-link"), function (item, value) {
        if ($(this).attr("href") == window.location.pathname + window.location.search) {
            $(this).addClass("active");
            $(this).closest(".has-treeview").addClass("menu-open");
            $(this).closest(".has-treeview").find(".nav-link").first().addClass("active");
        }
    })

    $(".textarea").summernote();

    const permalink = window.location.pathname;
    const formData2 = new FormData();
    formData2.append("permalink", permalink);

    $.ajax({
        url: "/admin/home/ClickData",
        type: "POST",
        processData: false,
        contentType: false,
        dataType: 'json',
        data: formData2
    }).done(function (data) {
    });


    window.addEventListener('beforeunload', function (e) {
        e.preventDefault();
        /*CLICK END DATE DATA*/
        var formData = new FormData();
        formData.append("leavinginpage", "true");
        $.ajax({
            url: "/admin/home/clickdata",
            type: "POST",
            processData: false,
            contentType: false,
            dataType: 'json',
            data: formData
        }).done(function (data) {
        });
    });

    //Timeout
    var minute = 20;

    $(function () {
        var idleTimer;
        function resetTimer() {
            clearTimeout(idleTimer);
            idleTimer = setTimeout(whenUserIdle, 60000 * minute);
        }
        //$(document.body).bind('mousemove keydown click', resetTimer); //space separated events list that we want to monitor
        resetTimer(); // Start the timer when the page loads
    });

    function whenUserIdle() {
        window.location.href = "/admin/home/lockscreen";
    }

    $("#backdown_timer").countdowntimer({
        hours: 0,
        minutes: 20,
        seconds: 0,
        size: "lg"
    });

    $(".list_delete__btn").on("click", function () {
        const itemId = $(this).data("id");
        var ajaxUrl = $(this).data("url");

        const formData = new FormData();
        formData.append("id", itemId);

        swal({
            title: 'Silmek istediğine emin misin?',
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((willDelete) => {
                if (willDelete) {
                    $.ajax({
                        url: ajaxUrl,
                        type: "POST",
                        processData: false,
                        contentType: false,
                        data: formData,
                        success: function (data) {
                            if (data == "success") {
                                swal("Silme İşlemi Başarılı");
                                setTimeout(function () { window.location.reload() }, 1500);
                            }
                            else {
                                swal("Silme işlemi başarılı olamadı. Tekrar Deneyiniz.");
                            }

                        },
                        error: function () {
                            swal("Silme işlemi başarılı olamadı. Tekrar Deneyiniz.");
                        }
                    });

                    swal("Seçtiğin nesne veritabanından silindi!", {
                        icon: "success",
                    });
                } else {
                    swal("Silme işlemi iptal edildi!");
                }
            });
    });

    $(".list_publish__btn").on("click", function () {
        var itemId = $(this).data("id");
        var ajaxUrl = $(this).data("url");

        swal("Aktiflik durumunu değiştireceksiniz. Emin misiniz?", {
            buttons: {
                cancel: "Geri Dön",
                catch: {
                    text: "Yayınla",
                    value: "catch",
                    closeModal: false,
                },
                pause: {
                    text: "Duraklat",
                    value: "pause",
                    closeModal: false,
                },
            },
        })
            .then((value) => {
                switch (value) {
                    case "pause":
                        var formData = new FormData();
                        formData.append("itemId", itemId);
                        formData.append("enabled", false);

                        $.ajax({
                            url: ajaxUrl,
                            method: 'POST',
                            data: formData,
                            dataType: "json",
                            processData: false,
                            contentType: false,
                            success: function (data) {
                                if (data == "no") {
                                    swal("Owww!", "Durum pasifleştirilemedi. Lütfen daha sonra tekrar deneyin!", "error");
                                }
                                else {
                                    swal("Heey", "Durum pasifleştirildi.", "success");
                                    setTimeout(
                                        function () {
                                            location.reload();
                                        }, 1000);
                                }
                            },
                            error: function () {
                                swal("Owww!", "Durum pasifleştirildi. Lütfen daha sonra tekrar deneyin!", "error");
                            }
                        });
                        break;

                    case "catch":

                        //Ajax
                        var formData = new FormData();
                        formData.append("itemId", itemId);
                        formData.append("enabled", true);

                        $.ajax({
                            url: ajaxUrl,
                            method: 'POST',
                            data: formData,
                            dataType: "json",
                            processData: false,
                            contentType: false,
                            success: function (data) {
                                if (data == "no") {
                                    swal("Owww!", "Durum aktifleştirilemedi. Lütfen daha sonra tekrar deneyin!", "error");
                                }
                                else {
                                    swal("Harika!", "Durum aktifleştirildi!", "success");
                                    setTimeout(
                                        function () {
                                            location.reload();
                                        }, 1000);
                                }
                            },
                            error: function () {
                                swal("Owww!", "Durum aktifleştirilemedi. Lütfen daha sonra tekrar deneyin!", "error");
                            }
                        });

                        break;

                    default:
                        swal("Görüşürüz!");
                }
            });

    });
    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    });
    $(".swal_text").on("click", function () {
        var msg = $(this).data("msg");
        swal(msg);
    });
});

$(".btn_submit").on("click", function () {
    $(this).addClass("disabled");
    $(this).prop("disabled", true);
    $(this).closest("form").submit();
});
