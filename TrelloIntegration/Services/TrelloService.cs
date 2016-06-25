using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TrelloIntegration.Models;
using TrelloIntegration.ViewModels;

namespace TrelloIntegration.Services
{
    //TODO add in tests and error handling
    public class TrelloService : ITrelloService
    {
        private const string API_BASE = "https://api.trello.com/1/";
        private const string APPLICATION_KEY = "fed9c5de2188e9af5f1ca25c1af501ab";
        private string UserToken = "";//Needs to be set on creation

        private string getAuthTokenAPIFragment()
        {
            return "?key=" + APPLICATION_KEY + "&token=" + UserToken;
        }

        public TrelloService(string userToken)
        {
            UserToken = userToken;
        }

        public async Task AddComment(string cardID, string comment)
        {
            var uri = API_BASE + "cards/" + cardID + "/actions/comments" + getAuthTokenAPIFragment();

            using (var client = new HttpClient())
            {
                var commentContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("text", comment)
                });
                var response = await client.PostAsync(uri, commentContent);
            }
        }

        public async Task<string> GetMemberIDForUserToken()
        {
            using (var client = new HttpClient())
            {
                var uri = API_BASE + "tokens/" + UserToken + "?key=" + APPLICATION_KEY;
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

        public async Task<User> GetMemberForUserToken()
        {
            var uri = API_BASE + "tokens/" + UserToken + "/member?key=" + APPLICATION_KEY;
            return await getFromAPIAsync<User>(uri);
        }

        public async Task<Board> GetBoard(string boardID)
        {
            var url = string.Format("{0}boards/{1}{2}", API_BASE, boardID, getAuthTokenAPIFragment());
            return await getFromAPIAsync<Board>(url);
        }

        public async Task<Card> GetCard(string cardID)
        {
            var url = string.Format("{0}Cards/{1}{2}", API_BASE, cardID, getAuthTokenAPIFragment());
            return await getFromAPIAsync<Card>(url);
        }

        public async Task<IEnumerable<Board>> GetBoardsForUser(string memberID)
        {
            //TODO where open?
            var url = string.Format("{0}members/{1}/boards{2}", API_BASE, memberID, getAuthTokenAPIFragment());
            return await getCollectionFromAPIAsync<Board>(url);
        }

        public async Task<IEnumerable<List>> GetListsForBoard(string boardID)
        {
            var url = string.Format("{0}boards/{1}/lists{2}", API_BASE, boardID, getAuthTokenAPIFragment());
            return await getCollectionFromAPIAsync<List>(url);
        }

        public async Task<IEnumerable<Card>> GetCardsForList(string listID)
        {
            var url = string.Format("{0}lists/{1}/cards{2}", API_BASE, listID, getAuthTokenAPIFragment());
            return await getCollectionFromAPIAsync<Card>(url);
        }

        public async Task<IEnumerable<Comment>> GetCommentsForCard(string cardID)
        {
            var url = string.Format("{0}cards/{1}/actions{2}&filter=commentCard", API_BASE, cardID, getAuthTokenAPIFragment());
            return await getCollectionFromAPIAsync<Comment>(url);
        }
        //YOu can do this get the basic implementation working thoush since this one requires extracting the data object from the card 
        public async Task<IEnumerable<Card>> GetCardsForBoard(string boardID)
        {
            var url = string.Format("{0}boards/{1}/cards{2}", API_BASE, boardID, getAuthTokenAPIFragment());
            return await getCollectionFromAPIAsync<Card>(url);
        }

        public async Task<IEnumerable<Comment>> GetCommentsForBoard(string boardID)
        {
            var url = string.Format("{0}boards/{1}/actions{2}&filter=commentCard", API_BASE, boardID, getAuthTokenAPIFragment());
            return await getCollectionFromAPIAsync<Comment>(url);
        }

        //TODO I think we can still do better and get everything by the board id
        //Ideally there would be a way to get all the items associated with a users boards in one hit.
        //I can't see a way to do this so we have to nest calls
        public async Task<IEnumerable<Board>> GetBoardsWithNestedCollectionsForUser(string memberID)
        {
            var boards = await GetBoardsForUser(memberID);
            foreach (var board in boards)
            {
                //Get lists cards and comments at the same time to cut down time
                //It seems like you can't get everything for 
                board.Lists = await GetListsForBoard(board.id);
                var cards = await GetCardsForBoard(board.id);
                //TODO maybe make the comments a partial view and get them via json to speed things up
                var comments = await GetCommentsForBoard(board.id);

                foreach (var card in cards)
                {
                    card.Comments = comments.Where(c => c.data.card.ID == card.ID);
                }

                foreach (var list in board.Lists)
                {
                    list.Cards = cards.Where(c => c.IDList == list.id);
                }
            }
            return boards;
        }

        private async Task<T> getFromAPIAsync<T>(string uri) where T : new()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                var json = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }

                return new T();
            }
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
    }
}