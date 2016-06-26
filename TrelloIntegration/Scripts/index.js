$(document).ready(function () {
    $.get(getBoardsUrl, function (result) {
        $("div.boards").html(result);
    })
});