using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using TrelloIntegration.ViewModels;

namespace TrelloIntegration.Services
{
    public class TrelloService : ITrelloService
    {
        public string GetMemberIDForUserToken(string userToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Board>> GetBoardsForUser(string memberID)
        {
            throw new NotImplementedException();
        }

        private async Task<IEnumerable<List>> GetListsForUser(string memberID)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Card>> GetCardsForUser(string memberID)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Comment>> GetCommentsForUser(string memberID)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Board>> GetBoardsWithNestedCollectionsForUser(string memberID)
        {
            //Async calls to the trello api
            //We are using the member id to retrieve the structures to that we have to do as few calls to the api as possible
            var comments = await GetCommentsForUser(memberID);
            var cards = await GetCardsForUser(memberID);
            var lists = await GetListsForUser(memberID);
            var boards = await GetBoardsForUser(memberID);

            foreach (var card in cards)
            {
                card.Comments = comments.Where(c => c.card.id == card.id);
            }

            foreach (var list in lists)
            {
                list.Cards = cards.Where(c => c.idList == list.id);
            }

            foreach (var board in boards)
            {
                board.Lists = lists.Where(l => l.idBoard == board.id);
            }

            return boards;
        }



        private async Task<IEnumerable<T>> getFromAPIAsync<T>(string uri)
        {
            using (var client = new HttpClient())
            {
                var trelloAPIBase = "";
                client.BaseAddress = new Uri(trelloAPIBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                IEnumerable <T> collection = null;

                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    //IEnumerable <T> collection = await response.Content. .<IEnumerable<T>>();
                    //Console.WriteLine("{0}\t${1}\t{2}", product.Name, product.Price, product.Category);
                }

                return collection;
            }
        }
        //        = function(memberID) {
        //        $http.get(apiBase + "members/" + memberID + "/boards" + apiTokenSuffix)
        //            .success(function (response)
        //        {
        //                $scope.boards = response;
        //            //Get all the lists
        //            angular.forEach($scope.boards, function(board, key) {
        //                board.isCollapsed = true;
        //                getListsForBoard(board);
        //            });
        //        })
        //            .error(function ()
        //        {
        //            displayErrorMessage("There was an error retrieving your boards from Trello!");
        //        });
        //    };

        //    var getListsForBoard = function(board) {
        //        $http.get(apiBase + "boards/" + board.id + apiTokenSuffix + "&lists=open")
        //            .success(function (response)
        //    {
        //        board.lists = response.lists;
        //        //Get all the cards
        //        angular.forEach(board.lists, function(list, key) {
        //            list.isCollapsed = true;
        //            getCardsForList(list);
        //        });
        //    })
        //            .error(function ()
        //    {
        //        displayErrorMessage("There was an error retrieving your card lists from Trello!");
        //    });
        //    };

        //var getCardsForList = function(list) {
        //        $http.get(apiBase + "lists/" + list.id + apiTokenSuffix + "&cards=open")
        //            .success(function (response)
        //{
        //    list.cards = response.cards;
        //    //Get all the comments
        //    angular.forEach(list.cards, function(card, key) {
        //        card.isCollapsed = true;
        //        getCommentsForCard(card);
        //    });
        //})
        //             .error(function ()
        //{
        //    displayErrorMessage("There was an error retrieving your cards from Trello!");
        //});
        //    };

        //    var getCommentsForCard = function(card) {
        //        $http.get(apiBase + "cards/" + card.id + "/actions" + apiTokenSuffix)
        //            .success(function (response)
        //{
        //    card.comments = response.filter(function(action) { return action.type == "commentCard" });
        //})
        //            .error(function ()
        //{
        //    displayErrorMessage("There was an error retrieving your card comments from Trello!");
        //});

        //    };
        //    }
    }