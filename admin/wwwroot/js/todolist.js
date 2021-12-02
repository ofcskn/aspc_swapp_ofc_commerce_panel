$(document).ready(function () {
    var addToDo = $('#addToDo');
    var checkbox = $('.checkToDo');
    var deleteToDoBtn = $('.deleteToDoBtn');

    var toDoAlert = $('.toDoAlert');
    toDoAlert.hide();

    checkbox.on('click', function () {
        var id = $(this).data("id");
        var toDoText = $('.toDoText').filter('[data-id="' + id + '"]');
        var toDoItem = $('.to-do-item').filter('[data-id="' + id + '"]');

        var formData = new FormData();
        formData.append("id", id);
        $.ajax({
            url: "/admin/ToDoList/Check",
            type: "POST",
            data: formData,
            dataType: "json",
            contentType: false,
            processData: false,
            success: function (data) {
                if (checkbox.filter('[data-id="' + id + '"]').prop("checked") == true) {
                    setTimeout(function () { toDoText.addClass("checkedText").fadeIn(200); }, 0);
                    setTimeout(function () { toDoItem.hide().fadeOut(500); }, 1000);

                }
                else {
                    toDoText.removeClass("checkedText").fadeIn(200);
                }
            }
        });
    });

    var add = function () {
        var toDo = $('#toDoText').val();

        var formData = new FormData();
        console.log(toDo);
        formData.append("todo", toDo);
        $.ajax({
            url: "/admin/ToDoList/Add",
            type: "POST",
            data: formData,
            dataType: "json",
            contentType: false,
            processData: false,
            success: function (data) {
                setTimeout(function () { toDoAlert.addClass("alert-success"); toDoAlert.append("<p>Hedefiniz başarıyla eklendi.</p>"); toDoAlert.show().fadeIn(200); }, 300);
                setTimeout(function () { window.location.reload(); }, 1000);
            },
            error: function (data) {
                setTimeout(function () { toDoAlert.addClass("alert-danger"); toDoAlert.append("<p>Hedefiniz eklenirken bir sorun oluştu. Lütfen sonra tekrar deneyiniz.</p>"); toDoAlert.show().fadeIn(200); }, 300);
                setTimeout(function () { window.location.reload(); }, 1000);
            }
        });
    }
    addToDo.on('click', function () {
        add();
    });
    $('#toDoText').on('keyup', function (e) {
        if (e.keyCode == 13) {
            add();
        }
    });


    deleteToDoBtn.on('click', function () {
        if (confirm('Seçtiğin hedef silinecektir. Emin misin?')) {
            var id = $(this).data("id");
            var formData = new FormData();
            formData.append("id", id);
            $.ajax({
                url: "/admin/ToDoList/Delete",
                type: "POST",
                data: formData,
                dataType: "json",
                contentType: false,
                processData: false,
                success: function (data) {
                    console.log(data);
                    setTimeout(function () { toDoAlert.addClass("alert-success"); toDoAlert.append("<p>Hedefiniz başarıyla silindi.</p>"); toDoAlert.show().fadeIn(200); }, 300);
                    setTimeout(function () { window.location.reload(); }, 1500);
                },
                error: function (data) {
                    setTimeout(function () { toDoAlert.addClass("alert-danger"); toDoAlert.append("<p>Hedefiniz slinirken bir sorun oluştu. Lütfen sonra tekrar deneyiniz.</p>"); toDoAlert.show().fadeIn(200); }, 300);
                    setTimeout(function () { window.location.reload(); }, 1500);
                }
            });
        }
    });

    $('body').on('click', '[data-editable]', function () {

        var $el = $(this);

        var $input = $('<input class="updateInput form-control" />').val($el.text());
        $el.closest(".icheck-primary").removeClass("d-inline");
        $el.closest(".icheck-primary").addClass("d-flex align-items-center");
        $el.replaceWith($input);

        var save = function () {
            var $p = $('<td data-editable />').text($input.val());
            $input.replaceWith($p);

            var id = $el.data("id");
            var formData = new FormData();
            formData.append("id", id);
            formData.append("goal", $input.val());

            $.ajax({
                url: "/admin/ToDoList/Update",
                type: "POST",
                data: formData,
                dataType: "json",
                contentType: false,
                processData: false,
                success: function () {
                    setTimeout(function () { toDoAlert.addClass("alert-success"); toDoAlert.append("<p>Hedefiniz başarıyla güncellendi.</p>"); toDoAlert.show().fadeIn(200); }, 300);
                    setTimeout(function () { window.location.reload(); }, 1500);
                },
                error: function () {
                    setTimeout(function () { toDoAlert.addClass("alert-danger"); toDoAlert.append("<p>Hedefiniz güncellenirken bir sorun oluştu. Lütfen sonra tekrar deneyiniz.</p>"); toDoAlert.show().fadeIn(200); }, 300);
                    setTimeout(function () { window.location.reload(); }, 1500);
                }
            });
        };
        /**
          We're defining the callback with `one`, because we know that
          the element will be gone just after that, and we don't want
          any callbacks leftovers take memory.
          Next time `p` turns into `input` this single callback
          will be applied again.
        */
        $input.on("keyup", function (e) {
            if (e.keyCode == 13) {
                save();
            }
        });
    });
});
