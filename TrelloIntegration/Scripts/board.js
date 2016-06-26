$(document).ready(function () {
    $.get(getBoardDetailUrl, function (result) {
        $("div.boardDetail").html(result);
    })
});