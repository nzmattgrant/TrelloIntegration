var showErrorMessage = function (message) {
    $("span.errorMessageContent").html(message);
    $("div.errorMessage").removeClass("hidden");
}

$(document).ready(function () {
    var anitforgeryToken = $("input[name=__RequestVerificationToken]").val();
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
                showErrorMessage("There was a problem logging out")
            },
            dataType: "json"
        });
    });
});