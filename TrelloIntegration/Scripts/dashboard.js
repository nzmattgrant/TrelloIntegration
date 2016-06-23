var GetBoardsFromServer = function () {

    $.get("/dashboard/boards", function (returnData) {
        $(".boards").html(returnData)
    });
    //TODO add in fail functions
};

var GetListsFromServer = function (boardID) {

    $.get("/dashboard/lists", { boardID : boardID }, function (returnData) {
        $(".lists." + boardID).html(returnData)
    });
    //TODO add in fail functions
};

var GetCardsFromServer = function (listID) {

    $.get("/dashboard/cards", { listID: listID }, function (returnData) {
        $(".cards." + listID).html(returnData)
    });
    //TODO add in fail functions
};

var GetCommentsFromServer = function (cardID) {

    $.get("/dashboard/comments", { cardID: cardID }, function (returnData) {
        $(".comments." + cardID).html(returnData)
    });
    //TODO add in fail functions
};


angular.module('app', []);

angular
    .module('app')
    .controller('TrelloCtrl', ['$scope', '$http', TrelloCtrl])

function TrelloCtrl($scope, $http) {

    $scope.board = [];

    var displayErrorMessage = function (message) {
        $scope.hasError = true;
        $scope.errorMessage = message;
    }

    var displaySuccessMessage = function (message) {
        $scope.showSuccessMessage = true;
        $scope.successMessage = message;
    }

    $scope.logout = function () {
        $http.post(logoutAction)
            .success(function (returnData) {
                if (returnData.ok)
                    window.location = returnData.newurl;
            })
            .error(function () {
                displayErrorMessage("There was an error trying to log you out");
            });
    };

    $scope.addComment = function (card) {

        if (!card.newComment)
            return;
        card.sendingComment = true;

        $http.post(apiBase + "cards/" + card.id + "/actions/comments" + apiTokenSuffix, { text: card.newComment })
            .success(function () {
                //Unshift to add the the top of the list
                card.comments.unshift({ data: { text: card.newComment } });
                displaySuccessMessage("Comment added successfully");
                card.newComment = "";
                card.sendingComment = false;
            })
            .error(function () {
                card.sendingComment = false;
                displayErrorMessage("There was an error trying to add your comment. Please try again");
            });
    };

}