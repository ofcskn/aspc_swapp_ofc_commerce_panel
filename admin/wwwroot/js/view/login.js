$(document).ready(function () {
    const form = $('#rememberPassword .remember-form');
    form.validate({
        errorPlacement: function errorPlacement(error, element) {
            element.after(error);
        },
        rules: {
            username: {
                required: true,
            },
            email: {
                required: true,
                email: true,
            },
            phone: {
                required: true,
            },
            birthday: {
                required: true,
            },
            question: {
                required: true,
            }
        },
        messages: {
            username: {
                required: "Lütfen kullanıcı adınızı giriniz.",
            },
            email: {
                required: "Lütfen mail adresinizi giriniz.",
                email: "Lütfen mail adresi giriniz.",
            },
            phone: {
                required: "Lütfen telefon numaranızı giriniz.",
            },
            birthday: {
                required: "Lütfen doğum gününüzü giriniz.",
            },
            question: {
                required: "Lütfen favori numaranızı giriniz.",
            }
        },
    });

    const form = $('#changePassword .change-form');
    form.validate({
        errorPlacement: function errorPlacement(error, element) {
            element.after(error);
        },
        rules: {
            password: {
                required: true,
            },
            passwordAgain: {
                required: true,
            },
        },
        messages: {
            password: {
                required: "Lütfen şifrenizi giriniz.",
            },
            passwordAgain: {
                required: "Lütfen şifrenizi tekrardan giriniz.",
            },
        },
    });

});



$(document).ready(function () {
    var passwordEye = $('#passwordEye');
    var input = $('input[type=password]');
    input.on("keyup", function () {
        if (this.value.length > 3) {
            $(passwordEye).fadeIn(150);
        }
    });
    passwordEye.on('click', function () {
        if (input.attr("type") === "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password");
        }
    });

    const form = $('#login-form');
    form.validate({
        errorPlacement: function errorPlacement(error, element) {
            element.after(error);
        },
        rules: {
            password: {
                required: true,
            },
            username: {
                required: true,
            },
        },
        messages: {
            password: {
                required: "Lütfen şifrenizi giriniz.",
            },
            username: {
                required: "Lütfen kullanıcı adınızı giriniz.",
            },
        },
    });

    const loginButton = $("#login-form .login-button");
    loginButton.on("click", function () {
        
    });
});


