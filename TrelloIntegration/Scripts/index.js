$(document).ready(function () {
    var regexToken = /[&#]?token=([0-9a-f]{64})/;
    var hashFragmentToken = regexToken.exec(location.hash);
    var token = hashFragmentToken ? hashFragmentToken[0].split("=")[1] : null;
    if (token) {

        //https://api.trello.com/1/tokens/91a6408305c1e5ec1b0b306688bc2e2f8fe67abf6a2ecec38c17e5b894fcf866?key=fed9c5de2188e9af5f1ca25c1af501ab
        //Maybe use this as a check later

        $.ajax({
            type: "POST",
            url: "/Home/Index",
            data: { token: token },
            success: function (returnData) {
                if (returnData.ok)
                    window.location = returnData.newurl;
            },
            dataType: "json"
        });
    }

    $("Button.login").click(function () {
        var key = "fed9c5de2188e9af5f1ca25c1af501ab";//Developer key
        var redirectUri = window.location.href;//Get the base of the site added
        window.location = "https://trello.com/1/authorize?response_type=token&key=" + key + "&redirect_uri=" + redirectUri + "&callback_method=fragment&scope=read%2Cwrite&expiration=never&name=Trello+Integration+Demo";
    });
});