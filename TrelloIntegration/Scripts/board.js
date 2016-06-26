$(document).ready(function () {
    $.get(getBoardDetailUrl)
    .success(function (result) {
        $("div.boardDetail").html(result);
    })
    .error(function () {
         showErrorMessage("There was an error loading the board");
     });
});