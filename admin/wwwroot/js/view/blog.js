var editor;
$(document).ready(function () {
    editor = CKEDITOR.replace('Description');
});


var project = "www";
var mediaBlogPath = "/img/blog/";

var form = $('#blogForm');
var $progress = $(".progress");
var $progressBar = $(".progress-bar");

var id = $('input[name="Id"]').val();
var formData = new FormData();
formData.append("id", id);



$.ajax({
    url: "/admin/blog/get",
    type: "POST",
    processData: false,
    contentType: false,
    data: formData
}).done(function (data) {
    console.log(data);
    populateForm(data);
});
form.validate({
    errorPlacement: function errorPlacement(error, element) {
        element.after(error);
    },
    rules: {
        Title: {
            required: true,
            maxlength: 256,
        },
        AuthorId: {
            required: true,
        },
        SubTitle: {
            required: true,
            maxlength: 128,
        },
        ShortDescription: {
            required: true,
            maxlength: 512,
        },
        Image: {
            required: true,
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
        AuthorId: {
            required: "Lütfen yazar seçiniz",
        },
        SubTitle: {
            required: "Lütfen kısa başlık ekleyiniz",
            maxlength: "Lütfen daha az kelime giriniz.",
        },
        ShortDescription: {
            required: "Lütfen kısa açıklama ekleyiniz",
            maxlength: "Lütfen daha az kelime giriniz.",
        },

        Image: {
            required: "Lütfen en az 1 fotoğraf ekleyiniz.",
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
        var total = 3;
        var current = currentIndex + 1;
        var $percent = (current / total) * 100;
        $progressBar.css("width", $percent + "%");
        $progressBar.attr("aria-valuenow", $percent + "%");

    },
    onFinishing: function (event, currentIndex) {

        if (form.valid() == true) {
            addUpdate();
            window.location.href = "/admin/blog/list";
        }
        return false;
    },
    onFinished: function (event, currentIndex) {
    }
});


var populateForm = function (data) {
    if (data == null) {
        return false;
    }
    else {
        console.log(data.blog);
        $("input[name='Title']").val(data.blog.title);
        $("input[name='SubTitle']").val(data.blog.subTitle);
        $("textarea[name='ShortDescription']").val(data.blog.shortDescription);

        $("select[name='AuthorId']").val(data.blog.authorId);
        $("input[name='Image']").val(data.blog.image);
        $("input[name='ListImage']").val(data.blog.listImage);
        $("input[name='Date']").val(data.blog.date);
        $("input[name='EmbedVideoLink']").val(data.blog.embedVideoLink);

        $("input[name='Enabled']").val(data.blog.enabled);

        $("input[name='SeoTitle']").val(data.blog.seoTitle);
        $("input[name='SeoDescription']").val(data.blog.seoDescription);
        $("input[name='SeoKeyword']").val(data.blog.seoKeyword);

        if (data.bg != null) {
            $.each(data.bg, function (i, v) {
                var formData = new FormData();
                formData.append("Id", v.categoryId);

                $.ajax({
                    url: "/admin/blog/GetTagTitle",
                    method: 'POST',
                    data: formData,
                    dataType: "json",
                    processData: false,
                    contentType: false,
                    success: function (data) {
                        $("#tagHtml").append("<div class='btn btn-admin mr-2 mb-2'>" + data + " <a class='removeTag' data-bgid='" + v.id + "' href='#'><i class='fa fa-remove'></i></a></div>");
                    }
                });
            });
        }


        if (data.blog.image != null) {
            var lastAppendedObj = $("#blogForm");
            if ($.isArray(splitImages(data.blog.image, ','))) {
                $.each(splitImages(data.blog.image, ','), function (i, v) {
                    lastAppendedObj.find("div.file").append("<p>" + blogImageHtml(v) + "</p>");
                });
            }
            else {
                lastAppendedObj.find("div.file").append("<p>" + blogImageHtml(data.blog.image) + "</p>");
            }
        }
        if (data.blog.listImage != null) {
            var lastAppendedObj = $("#blogForm");
            if ($.isArray(splitImages(data.blog.listImage, ','))) {
                $.each(splitImages(data.blog.listImage, ','), function (i, v) {
                    lastAppendedObj.find("div.list-file").append("<p>" + blogListImageHtml(v) + "</p>");
                });
            }
            else {
                lastAppendedObj.find("div.list-file").append("<p>" + blogListImageHtml(data.blog.listImage) + "</p>");
            }
        }
    }

};

var addUpdate = function () {

    var formData = new FormData();

    formData.append("Id", $('input[name="Id"]').val());

    formData.append("Title", $("input[name='Title']").val());
    formData.append("SubTitle", $("input[name='SubTitle']").val());

    const description = editor.getData();
    console.log(description);
    formData.append("Description", description);
    formData.append("ShortDescription", $("textarea[name='ShortDescription']").val());

    formData.append("AuthorId", $("select[name='AuthorId']").val());

    formData.append("Date", $("input[name='Date']").val());
    formData.append("Image", $("input[name='Image']").val());
    formData.append("ListImage", $("input[name='ListImage']").val());
    formData.append("Enabled", $("input[name='Enabled']").val());
    formData.append("EmbedVideoLink", $("input[name='EmbedVideoLink']").val());

    formData.append("SeoTitle", $("input[name='SeoTitle']").val());
    formData.append("SeoDescription", $("input[name='SeoDescription']").val());
    formData.append("SeoKeyword", $("input[name='SeoKeyword']").val());

    $.ajax({
        url: "/admin/blog/manage",
        method: 'POST',
        data: formData,
        dataType: "json",
        processData: false,
        contentType: false,
        success: function (data) {
            $('input[name="Id"]').val(data.blogId);
            $('input[name="Date"]').val(data.date);
        }
    });
}

/*--------------------------
       IMAGE UPLOAD
 * -----------------------*/
$(document).on('click', '.blogImageUpload', function () {
    var title = $(this).closest(".style-form").find("input[name='Title']").val();

    $(this).fileupload({
        url: "/admin/media/insert",
        dataType: 'json',
        formData: { filePath: mediaBlogPath, title: title, project: project },
        done: function (e, data) {
            console.log(data.result.newFileName);
            $(this).closest('.blog-image').find('input[name="Image"]').val(data.result.newFileName);

            var imgHtml = "<a href='javascript:;' data-filename='" + data.result.newFileName + "' data-project='" + project + "' data-filePath='" + mediaBlogPath + "' class='btn btn-icon btn-remove-image bg-danger text-white'><i class='fa fa-times'></i></a>"
                + "<img class='image-box' src='" + mediaBlogPath + data.result.newFileName + "' />"
                + data.result.oldFileName;

            $(this).closest('.style-form .blog-image').find('div.file').html("");
            $(this).closest('.style-form .blog-image').find('div.file').append($('<p />').html(imgHtml));

        }
    }).prop('disabled', !$.support.fileInput)
        .parent().addClass($.support.fileInput ? undefined : 'disabled');
});


/*--------------------------
       LIST IMAGE UPLOAD
 * -----------------------*/
$(document).on('click', '.blogListImageUpload', function () {
    var title = $(this).closest(".style-form").find("input[name='Title']").val() + "-l";

    $(this).fileupload({
        url: "/admin/media/insert",
        dataType: 'json',
        formData: { filePath: mediaBlogPath, title: title, project: project },
        done: function (e, data) {
            console.log(data.result.newFileName);
            $(this).closest('.blog-list-image').find('input[name="ListImage"]').val(data.result.newFileName);

            var imgHtml = "<a href='javascript:;' data-filename='" + data.result.newFileName + "' data-project='" + project + "' data-filePath='" + mediaBlogPath + "' class='btn btn-icon btn-remove-list-image bg-danger text-white'><i class='fa fa-times'></i></a>"
                + "<img class='image-box' src='" + mediaBlogPath + data.result.newFileName + "' />"
                + data.result.oldFileName;

            $(this).closest('.style-form .blog-list-image').find('div.list-file').html("");
            $(this).closest('.style-form .blog-list-image').find('div.list-file').append($('<p />').html(imgHtml));

        }
    }).prop('disabled', !$.support.fileInput)
        .parent().addClass($.support.fileInput ? undefined : 'disabled');
});

/*--------------------------
       ADD CATEGORY
 * -----------------------*/

$(document).on('click', '#addTag', function () {
    var tagTitle = $("select[name='tagTitle'] option:selected").data("title");
    var tagId = $("select[name='tagTitle']").val();
    var blogId = $("input[name='Id']").val();
    console.log(blogId);
    var formData = new FormData();
    formData.append("BlogId", blogId);
    formData.append("CategoryId", tagId);
    console.log(tagId);
    $.ajax({
        url: "/admin/blog/addBlogTag",
        method: 'POST',
        data: formData,
        dataType: "json",
        processData: false,
        contentType: false,
        success: function (data) {
            console.log(data);
            var bgId = data.id;
            $("#tagHtml").append("<div class='btn btn-admin mr-2 mb-2'>" + tagTitle + " <a class='removeTag' data-bgid='" + bgId + "' href='#'><i class='fa fa-remove'></i></a></div>");
        }
    });
});

$(document).on('click', '.removeTag', function () {
    var obj = $(this);
    var bgId = $(this).data("bgid");
    var formData = new FormData();
    formData.append("Id", bgId);

    $.ajax({
        url: "/admin/blog/deleteBlogTag",
        method: 'POST',
        data: formData,
        dataType: "json",
        processData: false,
        contentType: false,
        success: function (data) {
            obj.parent().hide();
        }
    });
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
var blogImageHtml = function (newFileName) {
    return "<a href='javascript:;' data-filename='" + newFileName + "' data-project='" + project + "' data-filePath='" + mediaBlogPath + "' class='btn btn-remove-image bg-danger text-white'><i class='fa fa-times'></i></a>"
        + "<img class='image-box' src='" + mediaBlogPath + newFileName + "' />"
        + newFileName;
};
/*--------------------------
       HTML LIST FUNCTION
 * -----------------------*/
var blogListImageHtml = function (newFileName) {
    return "<a href='javascript:;' data-filename='" + newFileName + "' data-project='" + project + "' data-filePath='" + mediaBlogPath + "' class='btn btn-remove-list-image bg-danger text-white'><i class='fa fa-times'></i></a>"
        + "<img class='image-box' src='" + mediaBlogPath + newFileName + "' />"
        + newFileName;
};
/*--------------------------
       FUNCTIONS
 * -----------------------*/

// Section top tools & actions
$(document).on('click', '.section-bar > .section-bar-top  > .tools > a.remove', function (evt) {
    orderProperty();
});
//Section Bar Functions
function changeTitle(obj) {
    $(obj).closest('.section-bar').find(".property-title-span").html($(obj).val());
}
function collapePanel(obj) {
    obj.find(".tools .action i").removeClass("fa-caret-up").addClass("fa-caret-down").addClass("expand");
    obj.children(".section-bar-bottom").hide();
}
function orderProperty() {
    $('#propertyHtml').children('div').each(function () {
        $(this).find("input[name='order']").val($(this).index());
        $(this).attr("data-order", $(this).index());
    });
}

//Collapse and Expand Action
$('body').on('click', '.page-tab .tab-content .form .section-manage .section-bar .section-bar-top .tools .action .collapse, .page-tab .tab-content .form .section-manage .section-bar .section-bar-top .tools .action .expand', function (e) {
    e.preventDefault();
    var el = $(this).closest(".section-bar").children(".section-bar-bottom");
    if ($(this).hasClass("collapse")) {
        $(this).removeClass("collapse").addClass("expand");
        el.slideUp(200);
    } else {
        $(this).removeClass("expand").addClass("collapse");
        el.slideDown(200);
    }
});

//Remove Action
$('body').on('click', '#propertyHtml .section-bar > .section-bar-top  > .tools > a.remove', function (e) {
    e.preventDefault();
    var slider = $(this).closest(".section-bar");
    slider.remove();
});
function expandPanel(obj) {
    obj.find(".tools .action i ").removeClass("fa-caret-down").addClass("fa-caret-up").addClass("collapse");
    obj.children(".section-bar-bottom").show();
}

Array.prototype.remove = function () {
    var what, a = arguments, L = a.length, ax;
    while (L && this.length) {
        what = a[--L];
        while ((ax = this.indexOf(what)) != -1) {
            this.splice(ax, 1);
        }
    }
    return this;
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
