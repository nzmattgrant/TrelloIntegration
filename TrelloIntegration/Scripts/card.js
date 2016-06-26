$(document).ready(function () {
    $.get(getCardDetailUrl, function (result) {
        $("div.cardDetail").html(result);
    })

    $("button.submitComment").click(function (e) {
        var button = $(this);

        if (!button.prev("input.comment").val())
            e.preventDefault();
        else
            button.addClass("disabled");
    })
});