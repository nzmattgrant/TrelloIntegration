using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TrelloIntegration.Models;
using TrelloIntegration.ViewModels;

namespace TrelloIntegration.Services
{
    public class TrelloService : ITrelloService
    {
        private const string API_BASE = "https://api.trello.com/1/";
        private const string APPLICATION_KEY = "fed9c5de2188e9af5f1ca25c1af501ab";

        private string getAuthTokenAPIFragment(string userToken)
        {
            return "?key=" + APPLICATION_KEY + "&token=" + userToken;
        }

        public async Task<string> GetMemberIDForUserToken(string userToken)
        {
            using (var client = new HttpClient())
            {
                var uri = API_BASE + "tokens/" + userToken + "?key=" + APPLICATION_KEY;
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

        public async Task<User> GetMemberForUserToken(string userToken)
        {
            var uri = API_BASE + "tokens/" + userToken + "/member?key=" + APPLICATION_KEY;
            return await getFromAPIAsync<User>(uri);
        }

        public async Task<Board> GetBoard(string boardID, string userToken)
        {
            var url = string.Format("{0}boards/{1}{2}", API_BASE, boardID, getAuthTokenAPIFragment(userToken));
            return await getFromAPIAsync<Board>(url);
        }

        public async Task<Card> GetCard(string cardID, string userToken)
        {
            var url = string.Format("{0}Cards/{1}{2}", API_BASE, cardID, getAuthTokenAPIFragment(userToken));
            return await getFromAPIAsync<Card>(url);
        }

        public async Task<IEnumerable<Board>> GetBoardsForUser(string memberID, string userToken)
        {
            var url = string.Format("{0}members/{1}/boards{2}", API_BASE, memberID, getAuthTokenAPIFragment(userToken));
            return await getCollectionFromAPIAsync<Board>(url);
        }

        public async Task<IEnumerable<List>> GetListsForBoard(string boardID, string userToken)
        {
            var url = string.Format("{0}boards/{1}/lists{2}", API_BASE, boardID, getAuthTokenAPIFragment(userToken));
            return await getCollectionFromAPIAsync<List>(url);
        }

        public async Task<IEnumerable<Card>> GetCardsForBoard(string boardID, string userToken)
        {
            var url = string.Format("{0}boards/{1}/cards{2}", API_BASE, boardID, getAuthTokenAPIFragment(userToken));
            return await getCollectionFromAPIAsync<Card>(url);
        }

        public async Task<IEnumerable<Comment>> GetCommentsForCard(string cardID, string userToken)
        {
            var url = string.Format("{0}cards/{1}/actions{2}&filter=commentCard", API_BASE, cardID, getAuthTokenAPIFragment(userToken));
            return await getCollectionFromAPIAsync<Comment>(url);
        }

        public async Task<HttpResponseMessage> AddComment(string cardID, string comment, string userToken)
        {
            var uri = API_BASE + "cards/" + cardID + "/actions/comments" + getAuthTokenAPIFragment(userToken);

            using (var client = new HttpClient())
            {
                var commentContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("text", comment)
                });
                return await client.PostAsync(uri, commentContent);
            }
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