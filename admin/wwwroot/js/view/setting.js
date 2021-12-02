$(document).ready(function () {
    $('.summernote').summernote(summernoteSettings);
});
var project = "www";
var mediaSettingPath = "/img/setting/";
//Page Tab
$(document).ready(function () {
    // $('textarea[name="shortDescription"]').summernote(summernoteSettings);
    var form = $('#settingForm');
    var $progress = $(".progress");
    var $progressBar = $(".progress-bar");

    var formData = new FormData();
    $.ajax({
        url: "/admin/setting/get",
        type: "POST",
        processData: false,
        contentType: false,
        data: formData
    }).done(function (data) {
        populateForm(data);
    });
    form.validate({
        errorPlacement: function errorPlacement(error, element) {
            element.after(error);
        },
        rules: {
            paramImage1: {
                required: true,
            },
            paramImage3: {
                required: true,
            },
            sidebarTitle: {
                required: true,
                maxlength: 64
            },
            sidebarDescription: {
                required: true,
                maxlength: 350
            },
        },
        messages: {
            paramImage1: {
                required: "Lütfen fotoğraf ekleyiniz",
            },
            paramImage3: {
                required: "Lütfen fotoğraf ekleyiniz",
            },
            sidebarTitle: {
                required: "Lütfen kısa başlık ekleyiniz",
                maxlength: "Lütfen daha kısa açıklama yazın."
            },
            sidebarDescription: {
                required: "Lütfen kısa açıklama ekleyiniz",
                maxlength: "Lütfen daha kısa açıklama yazın."
            },
        },
    });
    $("#wizard").steps({
        headerTag: "h4",
        bodyTag: "section",
        transitionEffect: "fade",
        enableAllSteps: true,
        transitionEffectSpeed: 500,
        labels: {
            finish: "Onayla",
            next: "Kaydet",
            previous: "Geri Dön"
        },
        onStepChanging: function (event, currentIndex, newIndex) {
            if (currentIndex > newIndex) {
                return true;
            }
            if (newIndex == currentIndex + 1) {
                if (form.valid() == true) {
                    addUpdate();
                    return true;
                }
                return form.valid();
            }
            if (newIndex > currentIndex + 1) {
                return false;
            }
        },
        onStepChanged: function (event, currentIndex, priorIndex) {
            var total = 4;
            var current = currentIndex + 1;
            var $percent = (current / total) * 100;
            $progressBar.css("width", $percent + "%");
            $progressBar.attr("aria-valuenow", $percent + "%");

        },
        onFinishing: function (event, currentIndex) {

            if (form.valid() == true) {
                addUpdate();
                window.location.href = "/ge/setting/index";
            }
            return false;
        },
        onFinished: function (event, currentIndex) {
        }
    });
});

var populateForm = function (data) {
    console.log(data);
    var settingJsonData = jQuery.parseJSON("[" + data.settingJsonData + "]");
    if (settingJsonData.length !== 0) {
        $.each(settingJsonData, function (index, value) {
            var lastAppendedObj = $("#settingForm");
            if (value === null) {
                return false;
            }
            else {
                console.log(value);
                console.log("fastlink", value.fastLink);
                $("input[name='paramImage1']").val(value.paramImage1);
                $("input[name='paramImage2']").val(value.paramImage2);
                $("input[name='paramImage3']").val(value.paramImage3);
                $("input[name='paramImage4']").val(value.paramImage4);
                $("input[name='paramImage5']").val(value.paramImage5);
                $("input[name='paramImage6']").val(value.paramImage6);

                $("input[name='paramTitle1']").val(value.paramTitle1);
                $("input[name='paramTitle2']").val(value.paramTitle2);
                $("input[name='paramTitle3']").val(value.paramTitle3);
                $("input[name='paramTitle4']").val(value.paramTitle4);

                $("input[name='noResultTitle']").val(value.noResultTitle);
                $("textarea[name='noResultDescription']").val(value.noResultDescription);

                //Error
                $("input[name='errorTitle']").val(value.errorTitle);
                $("textarea[name='errorDescription']").val(value.errorDescription);
                $("input[name='errorImageSite']").val(value.errorImageSite);
                $("input[name='errorImageCopyright']").val(value.errorImageCopyright);

                $("input[name='noteBtnTitle']").val(value.noteBtnTitle);
                $("input[name='noteTitle']").val(value.noteTitle);
                $("textarea[name='noteDescription']").val(value.noteDescription);
                $("input[name='noteLink']").val(value.noteLink);

                $("input[name='logoTitle']").val(value.logoTitle);

                $("textarea[name='shortDescription']").summernote("code", value.shortDescription);
                $("input[name='sidebarTitle']").val(value.sidebarTitle);
                $("textarea[name='sidebarDescription']").val(value.sidebarDescription);

                $("input[name='phone']").val(value.phone);
                $("input[name='phone2']").val(value.phone2);
                $("input[name='mail']").val(value.mail);
                $("input[name='mail2']").val(value.mail2);

                $("textarea[name='adress']").val(value.adress);
                $("textarea[name='adress2']").val(value.adress2);
                $("input[name='facebook']").val(value.facebook);
                $("input[name='instagram']").val(value.instagram);
                $("input[name='twitter']").val(value.twitter);
                $("input[name='linkedIn']").val(value.linkedIn);
                $("input[name='whatshapp']").val(value.whatshapp);
                $("input[name='youtube']").val(value.youtube);

                $("input[name='errorSeoTitle']").val(value.errorSeoTitle);
                $("textarea[name='errorSeoDescription']").val(value.errorSeoDescription);
                $("input[name='errorSeoKeyword']").val(value.errorSeoKeyword);


                $("input[name='footerTitle1']").val(value.footerTitle1);
                $("input[name='footerTitle2']").val(value.footerTitle2);
                $("input[name='footerTitle3']").val(value.footerTitle3);
                $("input[name='footerTitle4']").val(value.footerTitle4);
                $("input[name='footerCopyright']").val(value.footerCopyright);
                $("input[name='footerDesigned']").val(value.footerDesigned);
                $("input[name='footerCopyrightYear']").val(value.footerCopyrightYear);

                $("input[name='pageColor']").val(value.pageColor);
                $("input[name='pageColorHover']").val(value.pageColorHover);
                $("input[name='pageColorActive']").val(value.pageColorActive);
                $("input[name='pageColor2']").val(value.pageColor2);

                if (value.paramImage1 != "") {
                    if ($.isArray(splitImages(value.paramImage1, ','))) {
                        $.each(splitImages(value.paramImage1, ','), function (i, v) {
                            lastAppendedObj.find("div.file1").append($('<p />').html(logoImgFileHtml(v, 1)));
                        });
                    }
                    else {

                        lastAppendedObj.find("div.file1").append($('<p />').html(logoImgFileHtml(removeLastComma(value.paramImage1), 1)));
                    }
                }
                if (value.paramImage2 != "") {

                    if ($.isArray(splitImages(value.paramImage2, ','))) {
                        $.each(splitImages(value.paramImage2, ','), function (i, v) {
                            lastAppendedObj.find("div.file2").append($('<p />').html(logoImgFileHtml(v, 2)));
                        });
                    }
                    else {

                        lastAppendedObj.find("div.file2").append($('<p />').html(logoImgFileHtml(removeLastComma(value.paramImage2), 2)));
                    }
                }
                if (value.paramImage3 != "") {

                    if ($.isArray(splitImages(value.paramImage3, ','))) {
                        $.each(splitImages(value.paramImage3, ','), function (i, v) {
                            lastAppendedObj.find("div.file3").append($('<p />').html(logoImgFileHtml(v, 3)));
                        });
                    }
                    else {

                        lastAppendedObj.find("div.file3").append($('<p />').html(logoImgFileHtml(removeLastComma(value.paramImage3), 3)));
                    }
                }
                if (value.paramImage4 != "") {

                    if ($.isArray(splitImages(value.paramImage4, ','))) {
                        $.each(splitImages(value.paramImage4, ','), function (i, v) {
                            lastAppendedObj.find("div.file4").append($('<p />').html(logoImgFileHtml(v, 4)));
                        });
                    }
                    else {

                        lastAppendedObj.find("div.file4").append($('<p />').html(logoImgFileHtml(removeLastComma(value.paramImage4), 4)));
                    }
                }
                if (value.paramImage5 != "") {

                    if ($.isArray(splitImages(value.paramImage5, ','))) {
                        $.each(splitImages(value.paramImage5, ','), function (i, v) {
                            lastAppendedObj.find("div.file5").append($('<p />').html(logoImgFileHtml(v, 5)));
                        });
                    }
                    else {

                        lastAppendedObj.find("div.file5").append($('<p />').html(logoImgFileHtml(removeLastComma(value.paramImage5), 5)));
                    }
                }
                if (value.paramImage6 != "") {

                    if ($.isArray(splitImages(value.paramImage6, ','))) {
                        $.each(splitImages(value.paramImage6, ','), function (i, v) {
                            lastAppendedObj.find("div.file6").append($('<p />').html(logoImgFileHtml(v, 6)));
                        });
                    }
                    else {

                        lastAppendedObj.find("div.file6").append($('<p />').html(logoImgFileHtml(removeLastComma(value.paramImage6), 6)));
                    }
                }
            }
        });
    }
};

var addUpdate = function () {
    var settingJsonData = "";

    var formData = new FormData();
    formData.append("Id", $('input[name="Id"]').val());
    $('#settingForm').each(function () {
        settingJsonData += $(this).find("input,textarea,select").serializeJSON();
    });
    formData.append("SettingJsonData", settingJsonData);
    $.ajax({
        url: "/admin/setting/manage",
        method: 'POST',
        data: formData,
        dataType: "json",
        processData: false,
        contentType: false,
    });
}

//Image Uplaod Button
$(document).on('click', '.settingFileUpload', function () {
    var id = $(this).data("id");
    var title = $(this).data("filename");
    if (title == "" || title == null) {
        title = $(this).closest(".image-upload").find("input[name='imageName']").val();
    }
    console.log(title);
    $(this).fileupload({
        url: "/admin/media/insert",
        dataType: 'json',
        formData: { filePath: mediaSettingPath, title: title, project: project },
        done: function (e, data) {
            console.log(data);
            $(this).closest('#settingForm').find('input[name="paramImage' + id + '"]').val(data.result.newFileName);

            var imgHtml = "<a href='javascript:;' data-id='" + id + "' data-filename='" + data.result.newFileName + "' data-project='" + project + "' data-filePath='" + mediaSettingPath + "' class='btn btn-icon btn-remove-image'><i class='fa fa-times'></i></a>"
                + "<img class='settingFile' src='" + mediaSettingPath + data.result.newFileName + "' />"
                + data.result.newFileName;

            $(this).closest('.image-upload').find('div.file' + id).html("");
            $(this).closest('.image-upload').find('div.file' + id).append($('<p />').html(imgHtml));


        }
    }).prop('disabled', !$.support.fileInput)
        .parent().addClass($.support.fileInput ? undefined : 'disabled');
});



$(document).on('click', '.btn-remove-image', function (evt) {
    evt.preventDefault();
    evt.stopPropagation();

    var obj = $(this);

    var fileName = obj.data('filename');
    var id = obj.data('id');
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

        var currentValue = obj.closest('.image-upload').find('input[name="paramImage' + id + '"]').val();
        var newValue = currentValue.replace(fileName + ",", '');


        obj.closest('.image-upload').find('input[name="paramImage' + id + '"]').val("");

        obj.closest('p').remove();
    });
});
var logoImgFileHtml = function (newFileName, id) {
    return "<a href='javascript:;' data-filename='" + newFileName + "' data-id='" + id + "' data-project='" + project + "' data-filePath='" + mediaSettingPath + "' class='btn btn-remove-image'><i class='fa fa-times'></i></a>"
        + "<img class='settingFile' src='" + mediaSettingPath + newFileName + "' />"
        + newFileName;
}

jQuery.fn.sortDivs = function sortDivs() {
    $("> div", this[0]).sort(dec_sort).appendTo(this[0]);
    function dec_sort(a, b) { return ($(b).data("order")) < ($(a).data("order")) ? 1 : -1; }
}
function sortJSON(data, key, way) {
    return data.sort(function (a, b) {
        var x = a[key]; var y = b[key];
        if (way === 'asc') { return ((x < y) ? -1 : ((x > y) ? 1 : 0)); }
        if (way === 'desc') { return ((x > y) ? -1 : ((x < y) ? 1 : 0)); }
    });
}
var removeLastChar = function (value, char) {

    if (!value || !value.length) { return; }

    var lastChar = value.slice(-1);
    if (lastChar == char) {
        value = value.slice(0, -1);
    }
    return value;
}

var removeLastComma = function (value) {
    return value.replace(/,\s*$/, "");
}

var splitImages = function (value, char) {

    if (!value || !value.length) { return; }

    if (value.indexOf(char) > 0)
        return removeLastChar(value, char).split(',');
    else
        return removeLastChar(value, char);
}
