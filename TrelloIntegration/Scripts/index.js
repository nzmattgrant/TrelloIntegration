$(document).ready(function () {
    $.get(getBoardsUrl)
    .success(function (result) {
        $("div.boards").html(result);
    })
    .error(function () {
        showErrorMessage("There was an error loading your boards");
    });
});