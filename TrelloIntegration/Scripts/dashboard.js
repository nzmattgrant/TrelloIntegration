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
});