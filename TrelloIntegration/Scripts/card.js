$(document).ready(function () {

    $.get(getCardDetailUrl)
    .success(function (result) {
        $("div.cardDetail").html(result);
    })
    .error(function () {
        showErrorMessage("There was an error loading the card");
    });

    $("button.submitComment").click(function (e) {
        var button = $(this);

        if (!button.prev("input.comment").val())
            e.preventDefault();
        else
            button.addClass("disabled");
    });
});