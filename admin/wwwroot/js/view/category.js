var project = "www";
var mediaCategoryPath = "/img/category/";

$(document).ready(function () {

    var id = $('input[name="Id"]').val();
    var formData = new FormData();
    formData.append("id", id);
    formData.append("get", "ok");

    $.ajax({
        url: "/admin/category/get",
        type: "POST",
        processData: false,
        contentType: false,
        data: formData,
    }).done(function (data) {
        populateForm(data);
        console.log(data);
    });

    var populateForm = function (data) {
        if (data == null) {
            return false;
        }
        else {
            $("input[name='Title']").val(data.title);
            $("textarea[name='SeoTitle']").val(data.seoTitle);
            $("textarea[name='SeoDescription']").val(data.seoDescription);
            $("textarea[name='SeoKeyword']").val(data.seoKeyword);
            $("textarea[name='Image']").val(data.image);
            $("input[name='ListImage']").val(data.listImage);

            console.log(data);

            if (data.image != null) {
                var lastAppendedObj = $("#categoryForm");
                if ($.isArray(splitImages(data.image, ','))) {
                    $.each(splitImages(data.image, ','), function (i, v) {
                        lastAppendedObj.find("div.file").append("<p>" + categoryImageHtml(v) + "</p>");
                    });
                }
                else {
                    lastAppendedObj.find("div.file").append("<p>" + categoryImageHtml(data.image) + "</p>");
                }
            }

            if (data.listImage != null) {
                var lastAppendedObj = $("#categoryForm");
                if ($.isArray(splitImages(data.listImage, ','))) {
                    $.each(splitImages(data.listImage, ','), function (i, v) {
                        lastAppendedObj.find("div.list-file").append("<p>" + categoryListImageHtml(v) + "</p>");
                    });
                }
                else {
                    lastAppendedObj.find("div.list-file").append("<p>" + categoryListImageHtml(data.listImage) + "</p>");
                }
            }
        }
    };


    //ORDER CATEGORY //
    $("#order-category").sortable({
        handle: ".order-category-handle",
        connectWith: ".order-category-element",
        items: ".order-category-element",
        opacity: 0.8,
        coneHelperSize: true,
        placeholder: 'portlet-sortable-placeholder',
        forcePlaceholderSize: true,
        tolerance: "pointer",
        helper: "clone",
        tolerance: "pointer",
        forcePlaceholderSize: !0,
        helper: "clone",
        cancel: ".portlet-sortable-empty, .portlet-fullscreen", // cancel dragging if portlet is in fullscreen mode
        revert: 250, // animation in milliseconds
        update: function (b, c) {
            if (c.item.prev().hasClass("portlet-sortable-empty")) {
                c.item.prev().before(c.item);
            }

            orderCategory();
        }
    });

    var orderCategory = function () {
        $('#order-category').children('tr').each(function () {
            $(this).find("input[name='order']").val($(this).index());
            $(this).attr("data-order", $(this).index());

            var id = $(this).data("id");
            var order = $(this).data("order");

            var formData = new FormData();
            formData.append("id", id);
            formData.append("priority", order);
            $.ajax({
                url: "/admin/category/changePriority",
                type: "POST",
                dataType: "json",
                processData: false,
                contentType: false,
                data: formData
            }).done(function () {
                window.location.reload();
            });

        });
    }

    $(".btn-publish").on("click", function () {
        var categoryId = $(this).data("id");

        swal("Bu kategoriyi aktifleştireceksiniz. Emin misiniz?", {
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
                        formData.append("categoryId", categoryId);
                        formData.append("enabled", false);

                        $.ajax({
                            url: "/admin/category/publishstatus",
                            method: 'POST',
                            data: formData,
                            dataType: "json",
                            processData: false,
                            contentType: false,
                            success: function (data) {
                                if (data == "no") {
                                    swal("Owww!", "Kategori duraklatılamadı. Lütfen daha sonra tekrar dene.!", "error");
                                    setTimeout(
                                        function () {
                                            location.reload();
                                        }, 3500);
                                }
                                else {
                                    swal("Heey", "Kategori duraklatıldı.", "success");
                                    setTimeout(
                                        function () {
                                            location.reload();
                                        }, 3500);
                                }
                            },
                            error: function () {
                                swal("Owww!", "Kategori duraklatılamadı. Lütfen daha sonra tekrar dene.!", "error");
                                setTimeout(
                                    function () {
                                        location.reload();
                                    }, 3500);
                            }
                        });
                        break;

                    case "catch":

                        //Ajax
                        var formData = new FormData();
                        formData.append("categoryId", categoryId);
                        formData.append("enabled", true);

                        $.ajax({
                            url: "/admin/category/publishstatus",
                            method: 'POST',
                            data: formData,
                            dataType: "json",
                            processData: false,
                            contentType: false,
                            success: function (data) {
                                if (data == "no") {
                                    swal("Owww!", "Kategori aktifleştirilemedi. Lütfen daha sonra tekrar dene.!", "error");
                                    setTimeout(
                                        function () {
                                            location.reload();
                                        }, 3500);
                                }
                                else {
                                    swal("Harika!", "Kategori aktifleştirildi!", "success");
                                    setTimeout(
                                        function () {
                                            location.reload();
                                        }, 3500);
                                }
                            },
                            error: function () {
                                swal("Owww!", "Kategori aktifleştirilemedi. Lütfen daha sonra tekrar dene.!", "error");
                                setTimeout(
                                    function () {
                                        location.reload();
                                    }, 3500);
                            }
                        });

                        break;

                    default:
                        swal("Görüşürüz!");
                        setTimeout(
                            function () {
                                location.reload();
                            }, 1000);
                }
            });

    });

});

$("#categoryForm").validate({
    errorPlacement: function errorPlacement(error, element) {
        element.after(error);
    },
    rules: {
        Title: {
            required: true,
            maxlength: 256,
        },
        SeoTitle: {
            required: true,
            maxlength: 75,
        },
        SeoDescription: {
            required: true,
            maxlength: 155,
        },
        SeoKeyword: {
            required: true,
            maxlength: 150,
        }
    },
    messages: {
        Title: {
            required: "Lütfen başlık ekleyiniz",
            maxlength: "Lütfen daha az kelime giriniz.",
        },
        SeoTitle: {
            required: "Lütfen seo başlığı ekleyiniz.",
            maxlength: "Lütfen daha az kelime giriniz.",
        },
        SeoDescription: {
            required: "Lütfen seo açıklaması ekleyiniz.",
            maxlength: "Lütfen daha az kelime giriniz.",
        },
        SeoKeyword: {
            required: "Lütfen seo açıklaması ekleyiniz.",
            maxlength: "Lütfen daha az kelime giriniz.",
        }
    },
});


/*--------------------------
       IMAGE UPLOAD
 * -----------------------*/
$(document).on('click', '.categoryImageUpload', function () {
    var title = $(this).closest(".style-form").find("input[name='Title']").val();

    $(this).fileupload({
        url: "/admin/media/insert",
        dataType: 'json',
        formData: { filePath: mediaCategoryPath, title: title, project: project },
        done: function (e, data) {
            console.log(data.result.newFileName);
            $(this).closest('.category-image').find('input[name="Image"]').val(data.result.newFileName);

            var imgHtml = "<a href='javascript:;' data-filename='" + data.result.newFileName + "' data-project='" + project + "' data-filePath='" + mediaCategoryPath + "' class='btn btn-icon btn-remove-image bg-danger text-white'><i class='fa fa-times'></i></a>"
                + "<img class='image-box' src='" + mediaCategoryPath + data.result.newFileName + "' />"
                + data.result.oldFileName;

            $(this).closest('.style-form .category-image').find('div.file').html("");
            $(this).closest('.style-form .category-image').find('div.file').append($('<p />').html(imgHtml));

        }
    }).prop('disabled', !$.support.fileInput)
        .parent().addClass($.support.fileInput ? undefined : 'disabled');
});

/*--------------------------
       LIST IMAGE UPLOAD
 * -----------------------*/
$(document).on('click', '.categoryListImageUpload', function () {
    var title = $(this).closest(".style-form").find("input[name='Title']").val() + "-l";

    $(this).fileupload({
        url: "/admin/media/insert",
        dataType: 'json',
        formData: { filePath: mediaCategoryPath, title: title, project: project },
        done: function (e, data) {
            console.log(data.result.newFileName);
            $(this).closest('.category-list-image').find('input[name="ListImage"]').val(data.result.newFileName);

            var imgHtml = "<a href='javascript:;' data-filename='" + data.result.newFileName + "' data-project='" + project + "' data-filePath='" + mediaCategoryPath + "' class='btn btn-icon btn-remove-list-image bg-danger text-white'><i class='fa fa-times'></i></a>"
                + "<img class='image-box' src='" + mediaCategoryPath + data.result.newFileName + "' />"
                + data.result.oldFileName;

            $(this).closest('.style-form .category-list-image').find('div.list-file').html("");
            $(this).closest('.style-form .category-list-image').find('div.list-file').append($('<p />').html(imgHtml));

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
       REMOVE LIST IMAGE
 * -----------------------*/
$(document).on('click', '.btn-remove-list-image', function (evt) {
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
        obj.closest('.style-form').find('input[name="ListImage"]').val("");
        obj.closest('p').remove();
    });
});

/*--------------------------
       HTML FUNCTION
 * -----------------------*/
var categoryImageHtml = function (newFileName) {
    return "<a href='javascript:;' data-filename='" + newFileName + "' data-project='" + project + "' data-filePath='" + mediaCategoryPath + "' class='btn btn-remove-image bg-danger text-white'><i class='fa fa-times'></i></a>"
        + "<img class='image-box' src='" + mediaCategoryPath + newFileName + "' />"
        + newFileName;
};

/*--------------------------
       HTML LIST FUNCTION
 * -----------------------*/
var categoryListImageHtml = function (newFileName) {
    return "<a href='javascript:;' data-filename='" + newFileName + "' data-project='" + project + "' data-filePath='" + mediaCategoryPath + "' class='btn btn-remove-list-image bg-danger text-white'><i class='fa fa-times'></i></a>"
        + "<img class='image-box' src='" + mediaCategoryPath + newFileName + "' />"
        + newFileName;
};

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
