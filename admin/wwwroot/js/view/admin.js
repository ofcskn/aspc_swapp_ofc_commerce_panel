// <--- Webbilir - Taylan Tüfekçi'nin kodlarından yardım alınmıştır ---->

var project = "admin";
var mediaAdminPath = "/img/admin/";
var mediaFullPath = "/admin/img/admin/";

//Page Tab
$(document).ready(function () {
    var form = $('#adminForm');
    form.validate({
        errorPlacement: function errorPlacement(error, element) {
            element.after(error);
        },
        rules: {
            Name: {
                required: true,
            },
            Surname: {
                required: true,
            },
            Email: {
                required: true,
            },
            UserName: {
                required: true,
            },
            Role: {
                required: true,
            },
            Password: {
                required: true,
            }
        },
        messages: {
            Name: {
                required: "Lütfen isim giriniz.",
            },
            Surname: {
                required: "Lütfen soyisim giriniz.",
            },
            Email: {
                required: "Lütfen mail adresi giriniz.",
            },
            UserName: {
                required: "Lütfen kullanıcı adı giriniz.",
            },
            Role: {
                required: "Lütfen rol seçiniz.",
            },
            Password: {
                required: "Lütfen şifre belirleyiniz.",
            }
        },
    });
});

/*--------------------------
       IMAGE UPLOAD
 * -----------------------*/
$(document).on('click', '.adminImageUpload', function () {
    var name = $(this).closest(".style-form").find("input[name='Name']").val();
    var surname = $(this).closest(".style-form").find("input[name='Surname']").val();
    var title = name + "-" + surname;

    if (name == "" || surname == "") {
        alert("Önce gerekli alanları doldurunuz.");
    }
    else {
        $(this).fileupload({
            url: "/admin/media/insert",
            dataType: 'json',
            formData: { filePath: mediaAdminPath, title: title, project: project },
            done: function (e, data) {
                console.log(data.result.newFileName);
                $(this).closest('.admin-image').find('input[name="Image"]').val(data.result.newFileName);

                var imgHtml = "<a href='javascript:;' data-filename='" + data.result.newFileName + "' data-project='" + project + "' data-filePath='" + mediaAdminPath + "' class='btn btn-icon btn-remove-image'><i class='fa fa-times'></i></a>"
                    + "<img class='image-box' src='" + mediaFullPath + data.result.newFileName + "' />"
                    + data.result.oldFileName;

                $(this).closest('.style-form .admin-image').find('div.file').html("");
                $(this).closest('.style-form .admin-image').find('div.file').append($('<p />').html(imgHtml));

            }
        }).prop('disabled', !$.support.fileInput)
            .parent().addClass($.support.fileInput ? undefined : 'disabled');
    }

    
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

