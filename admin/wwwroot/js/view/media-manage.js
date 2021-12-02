var project = "www";

var avatar = document.querySelector("#avatar");
var image = document.querySelector("#cropped-image");
var mediaImageInput = document.querySelector("#media-image-input");
var $progress = $('.progress');
var $progressBar = $('.progress-bar');
var $alert = $('.alert');
var $dropText = $('.drop-text');
var $cropForm = $('.cropForm');
var cropper;

//$progress.hide();
//$alert.hide();
//$("#crop-area").hide();

//MEDIA --> MANAGE
$('.cropForm').on({
    dragenter: function (e) {
        $(this).css('background-color', '#d5d8de');
    },
    dragleave: function (e) {
        $(this).css('background-color', '#fff');
    },
});
document.querySelector("#cancel_btn").addEventListener("click", function () {
    $("#crop-area").addClass("d-none");
    $dropText.show();
    $(".cropLabel").show();
    cropper.destroy();
    cropper = null;
}); 
window.addEventListener('DOMContentLoaded', function () {
    //File Drop Script
    $cropForm.filedrop({
        drop: function (a) {
            a.stopPropagation();
            a.preventDefault();
            var files = a.dataTransfer.files;
            var done = function (url) {
                mediaImageInput.value = '';
                image.src = url;
                $("#crop-area").removeClass("d-none");
                $dropText.hide();
                $(".cropLabel").hide();
                $cropForm.css('background-color', '#fff');
            };
            var reader;
            var file;
            var url;

            if (files && files.length > 0) {
                file = files[0];
                $("#oldFileType").val(file.type);
                if (URL) {
                    done(URL.createObjectURL(file));
                } else if (FileReader) {
                    reader = new FileReader();
                    reader.onload = function (e) {
                        done(reader.result);
                    };
                    reader.readAsDataURL(file);
                }
            }

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
        },
    });

    $('[data-toggle="tooltip"]').tooltip();
    mediaImageInput.addEventListener('change', function (e) {
        var files = e.target.files;
        var done = function (url) {
            mediaImageInput.value = '';
            image.src = url;
            $(".cropLabel").hide();
            $("#crop-area").removeClass("d-none");
            $dropText.hide();
        };
        var reader;
        var file;
        var url;

        if (files && files.length > 0) {
            file = files[0];
            $("#oldFileType").val(file.type);
            if (URL) {
                done(URL.createObjectURL(file));
            } else if (FileReader) {
                reader = new FileReader();
                reader.onload = function (e) {
                    done(reader.result);
                };
                reader.readAsDataURL(file);
            }
        }

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
    });

    $('#crop').on('click', function () {
       
        $cropForm.validate({
            errorPlacement: function errorPlacement(error, element) {
                element.after(error);
            },
            rules: {
                fileProject: {
                    required: true,
                },
                selectFilePath: {
                    required: true,
                },
                imageName: {
                    required: true,
                },
            },
            messages: {
                fileProject: {
                    required: "Lütfen dosyanın yükleneceği projeyi seçiniz.",
                },
                selectFilePath: {
                    required: "Lütfen dosya konumu seçiniz.",
                },
                imageName: {
                    required: "Lütfen fotoğrafa isim ekleyiniz.",
                },
            },
        });

        var initialAvatarURL;
        var canvas;

        if ($cropForm.valid() == true) {
            if (cropper) {
                canvas = cropper.getCroppedCanvas({
                    width: 1920,
                    height: 1080,
                });
                avatar.src = canvas.toDataURL();
                $cropForm.removeClass('text-center');
                $progress.removeClass("d-none");
                $alert.removeClass('d-none');
                canvas.toBlob(function (blob) {
                    var formData = new FormData();
                    var newFileName = $("#image-name").val();
                    const fileProject = $("#fileProject").val();
                    const folderName = $('#selectFilePath').val();
                    const filePath = "\\img\\" + folderName + "\\";
                    const oldFileType = $("#oldFileType").val();
                    const quality = $("#quality_degree__slider").data("value");
                    formData.append('mediaFile', blob, newFileName);
                    formData.append('title', newFileName);
                    formData.append('mediaConfirm', "add");
                    formData.append('project', fileProject);
                    formData.append('filePath', filePath);
                    formData.append('folderName', folderName);
                    formData.append('quality', quality);
                    formData.append('oldFileType', oldFileType);
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

                        success: function (data) {
                            if (data == "not-valid-type") {
                                setTimeout(function () { $alert.show().addClass('alert-danger').text("Desteklenmeyen dosya tipi. Lütfen farklı bir dosya tipi ile takrar deneyiniz.") }, 1000);
                            }
                            else {
                                setTimeout(function () { $alert.show().addClass('alert-success').text("Yükleme işlemi başarıyla gerçekleşmiştir.") }, 1000);
                                setTimeout(function () { window.location.href = "/admin/media/list"; }, 2500);
                            }
                        },

                        error: function (data) {
                            if (data == "not-valid-type") {
                                setTimeout(function () { $alert.show().addClass('alert-danger').text("Desteklenmeyen dosya tipi. Lütfen farklı bir dosya tipi ile takrar deneyiniz.") }, 1000);
                            }
                            else {
                                avatar.src = initialAvatarURL;
                                setTimeout(function () { $alert.show().addClass('alert-danger').text("Yükleme işlemi gerçekleştirilememiştir. Tekrar deneyiniz.") }, 1000);
                                setTimeout(function () { window.location.reload(); }, 2500);
                            }
                        },

                        complete: function () {
                            $progress.hide();
                        },
                    });
                });
            }
        }
        else {
            return $cropForm.valid();
        }
    });

});

