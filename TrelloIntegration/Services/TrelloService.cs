using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TrelloIntegration.ViewModels;

namespace TrelloIntegration.Services
{
    public class Member
    {

    }

    public class TrelloService : ITrelloService
    {
        private const string API_BASE = "";
        private const string APPLICATION_KEY = "";
        private string USER_TOKEN = "";

        private string getAuthTokenAPIFragment()
        {
            return "";
        }

        public void SetUserToken(string userToken)
        {
            USER_TOKEN = userToken;
        }

        public async Task<string> GetMemberIDForUserToken()
        {
            using (var client = new HttpClient())
            {
                var uri = API_BASE + "tokens/" + USER_TOKEN + "?key=" + APPLICATION_KEY;
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    JObject returnedObject = JObject.Parse(json);
                    return returnedObject["idMember"].ToString();
                }

                return "";
            }
        }

        public async Task<IEnumerable<Board>> GetBoardsForUser(string memberID)
        {
            var url = string.Format("{0}members/{1}/boards{2}", API_BASE, memberID, getAuthTokenAPIFragment());
            return await getCollectionFromAPIAsync<Board>(url);
        }

        /
        private async Task<IEnumerable<List>> GetListsForUser(string memberID)
        {
            var url = string.Format("{0}members/{1}/boards{2}&lists=all", API_BASE, memberID, getAuthTokenAPIFragment());
            return await getCollectionFromAPIAsync<List>(url);
        }

        //You can't get cards available for a user like this
        public async Task<IEnumerable<Card>> GetCardsForUser(string memberID)
        {
            var url = "";
            return await getCollectionFromAPIAsync<Card>(url);
        }

        public async Task<IEnumerable<Comment>> GetCommentsForUser(string memberID)
        {
            var url = "";
            return await getCollectionFromAPIAsync<Comment>(url);
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



        private async Task<IEnumerable<T>> getCollectionFromAPIAsync<T>(string uri)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                var json = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
                }

                return new List<T>();
            }
        }

        //        = function(memberID) {
        //        $http.get()
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
        }
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
    //})