
var project = "www";
var mediaTeamPath = "/img/team/";

/*--------------------------
       POPULATE IMAGE
 * -----------------------*/
$(document).ready(function () {


    var id = $('input[name="Id"]').val();
    var formData = new FormData();
    formData.append("id", id);

    $.ajax({
        url: "/admin/team/get",
        type: "POST",
        processData: false,
        contentType: false,
        data: formData
    }).done(function (data) {
        var lastAppendedObj = $('.team-image');
        console.log(data);
        if (data.image != null) {
            lastAppendedObj.find("div.file").append($('<p />').html(pageFileHtml(data.image)));
            lastAppendedObj.find("input[name='Image']").val(data.image);
        }
    });
    /*--------------------------
           IMAGE UPLOAD
     * -----------------------*/

    $(document).on('click', '.teamImageUpload', function () {
        var name = $(this).closest(".style-form").find("input[name='Name']").val();
        var surname = $(this).closest(".style-form").find("input[name='Surname']").val();
        var title = name + surname;

        $(this).fileupload({
            url: "/admin/media/insert",
            dataType: 'json',
            formData: { filePath: mediaTeamPath, title: title, project: project },
            done: function (e, data) {
                $(this).closest('.team-image').find('input[name="Image"]').val(data.result);

                var imgHtml = "<a href='javascript:;' data-filename='" + data.result + "' data-project='" + project + "' data-filePath='" + mediaTeamPath + "' class='btn btn-icon btn-remove-image'><i class='fa fa-times'></i></a>"
                    + "<img class='image-box' src='" + mediaTeamPath + data.result + "' />"
                    + data.result;

                $(this).closest('.style-form').find('div.file').html("");
                $(this).closest('.style-form').find('div.file').append($('<p />').html(imgHtml));


            }
        }).prop('disabled', !$.support.fileInput)
            .parent().addClass($.support.fileInput ? undefined : 'disabled');
    });

    /*--------------------------
           REMOVE IMAGE
     * -----------------------*/
    $(document).on('click', '.btn-remove-image', function (evt) {
        evt.preventDefault();
        evt.stopPropagation();

        var obj = $(this);

        var fileName = obj.data('filename');
        var filePath = obj.data('filepath');
        var project = obj.data('project');

        var formData = new FormData();
        formData.append("fileName", fileName);
        formData.append("filePath", filePath);
        formData.append("project", project);

        $.ajax({
            url: "/admin/media/delete",
            type: "POST",
            processData: false,
            contentType: false,
            data: formData
        }).done(function (data) {

            var currentValue = obj.closest('.style-form').find('input[name="Image"]').val();

            obj.closest('.style-form').find('input[name="Image"]').val("");

            obj.closest('p').remove();
        });
    });

    /*--------------------------
           HTML FUNCTION
     * -----------------------*/
    var pageFileHtml = function (newFileName) {
        return "<a href='javascript:;' data-filename='" + newFileName + "' data-project='" + project + "' data-filePath='" + mediaTeamPath + "' class='btn btn-remove-image'><i class='fa fa-times'></i></a>"
            + "<img class='image-box' src='" + mediaTeamPath + newFileName + "' />"
            + newFileName;
    }
    /*--------------------------
           FUNCTIONS
     * -----------------------*/

});
//date picker start

if (top.location != location) {
    top.location.href = document.location.href;
}
$(function () {
    window.prettyPrint && prettyPrint();
    $('.default-date-picker').datepicker({
        format: 'mm-dd-yyyy'
    });
    $('.dpYears').datepicker();
    $('.dpMonths').datepicker();


    var startDate = new Date(2012, 1, 20);
    var endDate = new Date(2012, 1, 25);
    $('.dp4').datepicker()
        .on('changeDate', function (ev) {
            if (ev.date.valueOf() > endDate.valueOf()) {
                $('.alert').show().find('strong').text('The start date can not be greater then the end date');
            } else {
                $('.alert').hide();
                startDate = new Date(ev.date);
                $('#startDate').text($('.dp4').data('date'));
            }
            $('.dp4').datepicker('hide');
        });
    $('.dp5').datepicker()
        .on('changeDate', function (ev) {
            if (ev.date.valueOf() < startDate.valueOf()) {
                $('.alert').show().find('strong').text('The end date can not be less then the start date');
            } else {
                $('.alert').hide();
                endDate = new Date(ev.date);
                $('.endDate').text($('.dp5').data('date'));
            }
            $('.dp5').datepicker('hide');
        });

    // disabling dates
    var nowTemp = new Date();
    var now = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate(), 0, 0, 0, 0);

    var checkin = $('.dpd1').datepicker({
        onRender: function (date) {
            return date.valueOf() < now.valueOf() ? 'disabled' : '';
        }
    }).on('changeDate', function (ev) {
        if (ev.date.valueOf() > checkout.date.valueOf()) {
            var newDate = new Date(ev.date)
            newDate.setDate(newDate.getDate() + 1);
            checkout.setValue(newDate);
        }
        checkin.hide();
        $('.dpd2')[0].focus();
    }).data('datepicker');
    var checkout = $('.dpd2').datepicker({
        onRender: function (date) {
            return date.valueOf() <= checkin.date.valueOf() ? 'disabled' : '';
        }
    }).on('changeDate', function (ev) {
        checkout.hide();
    }).data('datepicker');
});

//date picker end


//datetime picker start

$(".form_datetime").datetimepicker({ format: 'yyyy-mm-dd hh:ii' });

$(".form_datetime-component").datetimepicker({
    format: "dd MM yyyy - hh:ii"
});

$(".form_datetime-adv").datetimepicker({
    format: "dd MM yyyy - hh:ii",
    autoclose: true,
    todayBtn: true,
    startDate: "2013-02-14 10:00",
    minuteStep: 10
});

$(".form_datetime-meridian").datetimepicker({
    format: "dd MM yyyy - HH:ii P",
    showMeridian: true,
    autoclose: true,
    todayBtn: true
});

//datetime picker end

//timepicker start
$('.timepicker-default').timepicker();


$('.timepicker-24').timepicker({
    autoclose: true,
    minuteStep: 1,
    showSeconds: true,
    showMeridian: false
});

//timepicker end


