$(document).ready(function () {
    var regexToken = /[&#]?token=([0-9a-f]{64})/;
    var hashFragmentToken = regexToken.exec(location.hash);
    var userToken = hashFragmentToken ? hashFragmentToken[0].split("=")[1] : null;
    if (userToken) {

        $("button.login").toggleClass("disabled");
        $("span.loggingInMessage, span.loginMessage").toggleClass("hidden");
        var anitforgeryToken = $("input[name=__RequestVerificationToken]").val()

        $.ajax({
            type: "POST",
            url: loginAction,
            data: {
                token: userToken,
                __RequestVerificationToken: anitforgeryToken
            },
            success: function (returnData) {
                if (returnData.ok)
                    window.location = returnData.newurl;
            },
            error: function() { 
                $(".errorMessage").removeClass("hidden");
                $("button.login").toggleClass("disabled");
                $("span.loggingInMessage, span.loginMessage").toggleClass("hidden");
            },
            dataType: "json"
        });
    }

    $("Button.login").click(function () {
        var key = "fed9c5de2188e9af5f1ca25c1af501ab";//App key
        var redirectUri = window.location.href;//Get the base of the site added
        window.location = "https://trello.com/1/authorize?response_type=token&key=" + key + "&redirect_uri=" + redirectUri + "&callback_method=fragment&scope=read%2Cwrite&expiration=never&name=Trello+Integration+Demo";
    });
});