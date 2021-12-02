
var project = "www";

window.addEventListener('DOMContentLoaded', function () {
    var avatar = document.getElementById('avatar');
    var image = document.getElementById('image');
    var $progress = $('.progress');
    var $progressBar = $('.progress-bar');
    var $alert = $('.alert');
    var cropper;

    $progress.hide();

    //Media ---> List
    var croppedImage = $('#croppedImage');
    var croppedImageContainer = $('.cropped-img-container');
    var cropModalBtn = $(".cropModalBtn");
    var resultConfirmBtn = $("#resultConfirmBtn");
    var changeBtn = $("#changeBtn");
    var cropBtn = $('#cropBtn');
    var $modalProgress = $('.modal-progress');
    var $cropModal = $('#cropModal');

    cropModalBtn.on('click', function (e) {
        var filename_value = $(this).data('filename');
        var project = $(this).data('project');
        var filePath = $(this).data('filepath');
        var folder = $(this).data('folder');
        $("#oldFolder").val(folder);
        if (project != "www") {
            image.src = "/" + project + filePath + filename_value;
        }
        else {
            image.src = filePath + filename_value;
        }
        document.getElementById("image-name").value = filename_value;
        document.getElementById("oldFilePath").value = filePath;
        document.getElementById("fileProject").value = project;
        document.getElementById("selectFilePath").value = folder;

        croppedImageContainer.hide();
        resultConfirmBtn.hide();
        changeBtn.hide();
        $alert.hide();
        $modalProgress.hide();
        $cropModal.modal('show');

    });

    $cropModal.on('shown.bs.modal', function () {
        var minAspectRatio = 0.5;
        var maxAspectRatio = 1.5;
        cropper = new Cropper(image, {
            ready: function () {
                var cropper = this.cropper;
                var containerData = cropper.getContainerData();
                var cropBoxData = cropper.getCropBoxData();
                var aspectRatio = cropBoxData.width / cropBoxData.height;
                var newCropBoxWidth;

                if (aspectRatio < minAspectRatio || aspectRatio > maxAspectRatio) {
                    newCropBoxWidth = cropBoxData.height * ((minAspectRatio + maxAspectRatio) / 2);

                    cropper.setCropBoxData({
                        left: (containerData.width - newCropBoxWidth) / 2,
                        width: newCropBoxWidth
                    });
                }
            },

            cropmove: function () {
                var cropper = this.cropper;
                var cropBoxData = cropper.getCropBoxData();
                var aspectRatio = cropBoxData.width / cropBoxData.height;

                if (aspectRatio < minAspectRatio) {
                    cropper.setCropBoxData({
                        width: cropBoxData.height * minAspectRatio
                    });
                } else if (aspectRatio > maxAspectRatio) {
                    cropper.setCropBoxData({
                        width: cropBoxData.height * maxAspectRatio
                    });
                }
            },
        });
    }).on('hidden.bs.modal', function () {
        cropper.destroy();
        cropper = null;
    });
    cropBtn.on("click", function () {
        var initialAvatarURL;
        var canvas;

        if (cropper) {
            canvas = cropper.getCroppedCanvas({
                width: 1920,
                height: 1080,
            });

            resultConfirmBtn.show();
            changeBtn.show();

            croppedImage.attr("src", canvas.toDataURL());
            initialAvatarURL = canvas.toDataURL();
            croppedImageContainer.show();
            $alert.removeClass('alert-success alert-warning');

            canvas.toBlob(function (blob) {
                const formData = new FormData();
                const oldFileType = blob.type;
                const quality = $("#quality_degree__slider").data("value");

                //YENI FOTOGRAF EKLEME BUTONU
                resultConfirmBtn.on("click", function () {
                    $(".new-image-name-group").removeClass("d-none");
                    $("#new-image-name").css("border", "1px solid #e34e54");

                    if ($("#new-image-name").val() != "") {
                        project = $("#fileProject").val();
                        formData.append("mediaConfirm", "listAdd");
                        formData.append('title', $("#new-image-name").val());
                        formData.append('mediaFile', blob, $("#new-image-name").val());
                        formData.append('quality', quality);
                        formData.append('oldFileType', oldFileType);
                        formData.append('project', project);
                        formData.append('folderName', $('#selectFilePath').val());
                        formData.append('filePath', "\\img\\" + $('#selectFilePath').val() + "\\");

                        $modalProgress.show();
                        setTimeout(function () {
                            $.ajax({
                                url: "/admin/media/insert",
                                method: 'POST',
                                data: formData,
                                processData: false,
                                contentType: false,
                                xhr: function () {
                                    var xhr = new XMLHttpRequest();

                                    xhr.upload.onprogress = function (e) {
                                        var percent = '0';
                                        var percentage = '0%';

                                        if (e.lengthComputable) {
                                            percent = Math.round((e.loaded / e.total) * 100);
                                            percentage = percent + '%';
                                            $progressBar.width(percentage).attr('aria-valuenow', percent).text(percentage);
                                        }
                                    };

                                    return xhr;
                                },
                                success: function () {
                                    $modalProgress.hide();
                                    setTimeout(function () {
                                        $alert.show().addClass('alert-success').text('Yükleme Başarılı.');
                                    }, 200);
                                    setTimeout(function () {
                                        window.location.reload();
                                    }, 2000);

                                },
                                error: function () {
                                    $modalProgress.hide();
                                    setTimeout(function () {
                                        $alert.show().addClass('alert-warning').text('Yükleme Başarısız. Tekrar Deneyin.');
                                    }, 200);
                                    setTimeout(function () {
                                        window.location.reload();
                                    }, 2000);
                                },
                            });
                        }, 500);
                    }
                });

                //FOTOGRAFI DEGISTIRME BUTONU
                changeBtn.on("click", function () {
                    formData.append("mediaConfirm", "change");
                    project = $("#fileProject").val();
                    formData.append('mediaFile', blob, $("#image-name").val());
                    formData.append('quality', quality);
                    formData.append('oldFileType', oldFileType);
                    formData.append('project', project);
                    formData.append('folderName', $('#selectFilePath').val());
                    formData.append('filePath', "\\img\\" + $('#selectFilePath').val() + "\\");
                    setTimeout(function () {
                        $modalProgress.show();
                    }, 200);
                    setTimeout(function () {
                        $.ajax({
                            url: "/admin/media/insert",
                            method: 'POST',
                            data: formData,
                            processData: false,
                            contentType: false,

                            xhr: function () {
                                var xhr = new XMLHttpRequest();

                                xhr.upload.onprogress = function (e) {
                                    var percent = '0';
                                    var percentage = '0%';

                                    if (e.lengthComputable) {
                                        percent = Math.round((e.loaded / e.total) * 100);
                                        percentage = percent + '%';
                                        $progressBar.width(percentage).attr('aria-valuenow', percent).text(percentage);
                                    }
                                };

                                return xhr;
                            },

                            success: function () {
                                setTimeout(function () {
                                    $modalProgress.hide();
                                }, 500);
                                setTimeout(function () {
                                    $alert.show().addClass('alert-success').text('Yükleme Başarılı.');
                                }, 1000);
                                setTimeout(function () {
                                    window.location.reload();
                                }, 3000);

                            },

                            error: function () {
                                avatar.src = initialAvatarURL;
                                setTimeout(function () {
                                    $modalProgress.hide();
                                }, 1000);
                                setTimeout(function () {
                                    $alert.show().addClass('alert-warning').text('Yükleme Başarısız. Tekrar Deneyin.');
                                }, 1500);
                                setTimeout(function () {
                                    window.location.reload();
                                }, 3000);
                            },
                        });
                    }, 500);

                });

            });
        }
    });
});

var checkbox = $('input[name="selectCheckBox"]');
//Modal Trigger Button
var bZM = $(".zoomModalBtn");
var dMB = $(".deleteModalBtn");
var dMSBtn = $('#btnDeleteSelect');//Modal button for select all photos
var bD = $('#btnDelete');//Modal button for select all photos
var sBtn = $('#selectBtn');
var cBtn = $('#cancelBtn');
var dTBtn = $('#deleteTopBtn');
var alertTop = $('#alertTop');

dMSBtn.hide();
bD.hide();

bZM.on('click', function () {
    $('#zoomConfirmModal').modal('show');
    var filename_value = $(this).data('filename');
    var filePath = $(this).data('filepath');
    $(".confirmImg").attr("src", filePath + filename_value);
    $(".confirmImg").parent().find(".fileName").html(filename_value);
    $(".confirmImg").parent().find(".filePath").html(filePath);
});
dMB.on('click', function () {
    bD.show();
    $('#deleteConfirmModal').modal('show');
    var filename_value = $(this).data('filename');
    var filePath = $(this).data('filepath');
    project = $(this).data('project');

    $(".confirmImg").attr("src", filePath + filename_value);

    $(".deleteFilePath").val(filePath);
    bD.val(filename_value);
});
dTBtn.on('click', function () {
    dMSBtn.show();
    bD.hide();
    $('.confirmImg').hide();
    $('#deleteConfirmModal').modal('show');
});

//Delete Image With Checkbox
checkbox.hide();
dTBtn.hide();
alertTop.hide();
cBtn.hide();


sBtn.on("click", function () {
    checkbox.show();
    sBtn.hide();
    cBtn.show();
    dTBtn.show();

    dMSBtn.on("click", function () {
        $('#deleteConfirmModal').modal('hide');

        var filename = [];
        $('input[name="selectCheckBox"]:checked').each(function (i) {
            filename[i] = $(this).data("filename");

            var formData = new FormData();
            formData.append("filename", filename[i]);
            formData.append("project", project);
            formData.append("filePath", $(".deleteFilePath").val());
            $.ajax({
                url: "/admin/media/delete",
                type: "POST",
                processData: false,
                contentType: false,
                dataType: 'json',
                data: formData
            }).done(function (data) {
                setTimeout(function () { alertTop.show().addClass('alert-success').text("Silme işlemi başarıyla gerçekleşmiştir.") }, 200);
                setTimeout(function () { window.location.reload(); }, 2000);
            }).fail(function () {
                setTimeout(function () { alertTop.show().addClass('alert-danger').text("Silme işlemnizi gerçekleştiremedik. Lütfen biraz sonra tekrar deneyiniz.") }, 200);
            });
        });
    });
});
cBtn.on('click', function () {
    checkbox.hide();
    cBtn.hide();
    dTBtn.hide();
    sBtn.show();
});

//Delete Image in Modal
$(document).on('click', '#btnDelete', function (evt) {
    evt.preventDefault();
    evt.stopPropagation();

    var obj = $(this);

    var fileName = obj.val();
    var filePath = $(this).data('filepath');

    var formData = new FormData();
    formData.append("fileName", fileName);
    formData.append("project", project);
    formData.append("filePath", $(".deleteFilePath").val());
    console.log($(".deleteFilePath").val())

    $.ajax({
        url: "/admin/media/delete",
        type: "POST",
        processData: false,
        contentType: false,
        data: formData,
        success: function (data) {
            window.location.reload(); // This is not jQuery but simple plain ol' JS
        }
    }).done(function (data) {
        obj.closest('p').remove();
    });
});