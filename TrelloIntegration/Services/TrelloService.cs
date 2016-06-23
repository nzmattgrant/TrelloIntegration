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

        public async Task<IEnumerable<Board>> GetBoardsForUser(string memberID)
        {
            //TODO where open?
            var url = string.Format("{0}members/{1}/boards{2}", API_BASE, memberID, getAuthTokenAPIFragment());
            return await getCollectionFromAPIAsync<Board>(url);
        }

        private async Task<IEnumerable<List>> GetListsForBoard(string boardID)
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
        //public async Task<IEnumerable<Card>> GetCardsForBoard(string boardID)
        //{
        //    var url = string.Format("{0}boards/{1}/cards{2}", API_BASE, boardID, getAuthTokenAPIFragment());
        //    return await getCollectionFromAPIAsync<Card>(url);
        //}

        //public async Task<IEnumerable<Comment>> GetCommentsForCard(string cardID)
        //{
        //    var url = string.Format("{0}cards/{1}/actions{2}&filter=commentCard", API_BASE, cardID, getAuthTokenAPIFragment());
        //    return await getCollectionFromAPIAsync<Comment>(url);
        //}

        //TODO I think we can still do better and get everything by the board id
        //Ideally there would be a way to get all the items associated with a users boards in one hit.
        //I can't see a way to do this so we have to nest calls
        public async Task<IEnumerable<Board>> GetBoardsWithNestedCollectionsForUser(string memberID)
        {
            var boards = await GetBoardsForUser(memberID);
            foreach (var board in boards)
            {
                board.Lists = await GetListsForBoard(board.id);
                foreach (var list in board.Lists)
                {
                    list.Cards = await GetCardsForList(list.id);
                    foreach (var card in list.Cards)
                    {
                        card.Comments = await GetCommentsForCard(card.id);
                    }
                }
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
    }
}