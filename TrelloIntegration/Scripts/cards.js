
angular.module('app', []);

angular
    .module('app')
    .controller('TrelloCtrl', ['$scope', '$http', TrelloCtrl])

function TrelloCtrl($scope, $http) {

    //Closed over variables
    var apiBase = "https://api.trello.com/1/";
    var applicationKey = "fed9c5de2188e9af5f1ca25c1af501ab";
    //var apiTokenSuffix = "tokens/" + token + "?key=" + applicationKey;
    var apiTokenSuffix = "?key=" + applicationKey + "&token=" + token;

    $scope.logout = function () {
        //TODO update the url to be the action
        $http.post("/Home/Logout")
            .success(function (returnData) {
                if (returnData.ok)
                    window.location = returnData.newurl;
            })
        .error(function () {
            //TODO
        });
    };

    $scope.addComment = function (card) {

        if (!card.newComment)
            return;

        $http
            .post(apiBase + "cards/" + card.id + "/actions/comments?key=" + applicationKey + "&token=" + token, { text: card.newComment })
            .success(function () {
                //Unshift to add the the top of the list
                card.comments.unshift({ data: { text: card.newComment } });
                card.newComment = "";
            })
        .error(function () {
            //TODO add error handling
        });

    };

    //Use the token to get the member id then set up the boards
    $http.get(apiBase + "tokens/" + token + "?key=" + applicationKey)
        .success(function (response) {
            getBoardsForUser(response.idMember);
        })
        .error(function () {

        });


    var dsiplayErrorMessage = function (message) {

    };

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
            //TODO
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
            //TODO
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
             //TODO
         });
    };

    var getCommentsForCard = function (card) {
        $http.get(apiBase + "cards/" + card.id + "/actions" + apiTokenSuffix)
            .success(function (response) {
                card.comments = response.filter(function (action) { return action.type == "commentCard" });
            })
        .error(function () {
            //TODO
        });

    };

}