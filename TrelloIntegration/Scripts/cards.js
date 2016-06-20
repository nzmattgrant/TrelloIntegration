
angular.module('app', []);

angular
    .module('app')
    .controller('TrelloCtrl', ['$scope', '$http', TrelloCtrl])

function TrelloCtrl($scope, $http) {

    //Closed over variables
    var apiBase = "https://api.trello.com/1/";
    var applicationKey = "fed9c5de2188e9af5f1ca25c1af501ab";
    var apiTokenSuffix = "?key=" + applicationKey + "&token=" + token;

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

        $http.post(apiBase + "cards/" + card.id + "/actions/comments?key=" + applicationKey + "&token=" + token, { text: card.newComment })
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

    //Use the token to get the member id then set up the boards
    $http.get(apiBase + "tokens/" + token + "?key=" + applicationKey)
        .success(function (response) {
            getBoardsForUser(response.idMember);
        })
        .error(function () {
            displayErrorMessage("There was an error accessing the trello user!");
        });

    //Functions that cascade calls to the trello API to retrieve the nested Board->List->Card->Comment structure from Trello
    var getBoardsForUser = function (memberID) {
        $http.get(apiBase + "members/" + memberID + "/boards" + apiTokenSuffix)
            .success(function (response) {
                $scope.boards = response;
                angular.forEach($scope.boards, function (board, key) {
                    board.isCollapsed = true;
                    getListsForBoard(board);
                });
            })
            .error(function () {
                displayErrorMessage("There was an error retrieving your boards from Trello!");
            });
    };

    var getListsForBoard = function (board) {
        $http.get(apiBase + "boards/" + board.id + apiTokenSuffix + "&lists=open&list_fields=name&fields=name")
            .success(function (response) {
                board.lists = response.lists;
                //Get all the cards
                angular.forEach(board.lists, function (list, key) {
                    list.isCollapsed = true;
                    getCardsForList(list);
                });
            })
            .error(function () {
                displayErrorMessage("There was an error retrieving your card lists from Trello!");
            });
    };

    var getCardsForList = function (list) {
        $http.get(apiBase + "lists/" + list.id + apiTokenSuffix + "&cards=open&card_fields=name")
            .success(function (response) {
                list.cards = response.cards;
                //Get all the comments
                angular.forEach(list.cards, function (card, key) {
                    card.isCollapsed = true;
                    getCommentsForCard(card);
                });
            })
             .error(function () {
                 displayErrorMessage("There was an error retrieving your cards from Trello!");
             });
    };

    var getCommentsForCard = function (card) {
        $http.get(apiBase + "cards/" + card.id + "/actions" + apiTokenSuffix)
            .success(function (response) {
                card.comments = response.filter(function (action) { return action.type == "commentCard" });
            })
            .error(function () {
                displayErrorMessage("There was an error retrieving your card comments from Trello!");
            });

    };

}