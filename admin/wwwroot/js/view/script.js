
//Navbar Toggle Icon
$(document).ready(function () {


    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
        $(this).toggleClass('active');
    });

    var formData = new FormData();

    $.ajax({
        url: "/admin/home/getlangcookie",
        type: "POST",
        processData: false,
        contentType: false,
        dataType: 'json',
        data: formData
    }).done(function (data) {
        $("#LangCookie").val(data);
    });

    $("#LangCookie").on("change", function () {
        var formData = new FormData();
        formData.append("lang", $(this).val());

        $.ajax({
            url: "/admin/home/setlangcookie",
            type: "POST",
            processData: false,
            contentType: false,
            dataType: 'json',
            data: formData
        }).done(function (data) {
            swal(data + " diline geçiş yapıldı");
            setTimeout(function () { window.location.reload(); }, 1500);
        });
    });
});

//Is Admin ---> Admin/List
$(document).on('click', 'input[name="IsAdmin"]', function () {
    $('input:checkbox').not(this).prop('checked', false);

    var formData = new FormData();
    formData.append("Id", $(this).val());

    $.ajax({
        url: "/admin/root",
        type: "POST",
        processData: false,
        contentType: false,
        dataType:'json',
        data: formData
    }).done(function (data) {
    });
});

/*-----------------------------------------
 TAB CONTENT SCRIPT
 ----------------------------------------*/
$(document).ready(function () {

    $('.tab-link').click(function () {
        var tab_id = $(this).attr('data-tab');
        console.log(tab_id);
        $('.tabs li').removeClass('current');
        $('.tab-content').removeClass('current');

        $(this).addClass('current');
        $("#" + tab_id).addClass('current');
    });
});



//SIGNALR NOTIFICATIONS
