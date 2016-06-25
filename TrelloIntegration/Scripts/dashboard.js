$(document).ready(function () {
    var anitforgeryToken = $("input[name=__RequestVerificationToken]").val()
    $("button.logout").click(function () {
        $.ajax({
            type: "POST",
            url: logoutAction,
            data: {
                __RequestVerificationToken: anitforgeryToken
            },
            success: function (returnData) {
                if (returnData.ok)
                    window.location = returnData.newurl;
            },
            error: function () {
                $(".errorMessage").removeClass("hidden");
            },
            dataType: "json"
        });
    });

    $("button.submitComment").click(function (e) {
        var button = $(this);

        if (!button.prev("input.comment").val())
           e.preventDefault();
        else
            button.addClass("disabled");
    })

});