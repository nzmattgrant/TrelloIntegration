using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TrelloIntegration.Models;
using TrelloIntegration.ViewModels;

namespace TrelloIntegration.Services
{
    //TODO add in tests and error handling
    //TODO figure out how to get the parent objects returned from the api calls for the breadcrumb
    public class TrelloService : ITrelloService
    {
        private const string API_BASE = "https://api.trello.com/1/";
        private const string APPLICATION_KEY = "fed9c5de2188e9af5f1ca25c1af501ab";
        private string UserToken = "";//Needs to be set on creation

        public TrelloService(string userToken)
        {
            UserToken = userToken;
        }

        private string getAuthTokenAPIFragment()
        {
            return "?key=" + APPLICATION_KEY + "&token=" + UserToken;
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

        public async Task<BoardViewModel> GetBoard(string boardID)
        {
            var url = string.Format("{0}boards/{1}{2}", API_BASE, boardID, getAuthTokenAPIFragment());
            return await getFromAPIAsync<BoardViewModel>(url);
        }

        public async Task<CardViewModel> GetCard(string cardID)
        {
            var url = string.Format("{0}Cards/{1}{2}", API_BASE, cardID, getAuthTokenAPIFragment());
            return await getFromAPIAsync<CardViewModel>(url);
        }

        public async Task<IEnumerable<BoardViewModel>> GetBoardsForUser(string memberID)
        {
            var url = string.Format("{0}members/{1}/boards{2}", API_BASE, memberID, getAuthTokenAPIFragment());
            return await getCollectionFromAPIAsync<BoardViewModel>(url);
        }

        public async Task<IEnumerable<ListViewModel>> GetListsForBoard(string boardID)
        {
            var url = string.Format("{0}boards/{1}/lists{2}", API_BASE, boardID, getAuthTokenAPIFragment());
            return await getCollectionFromAPIAsync<ListViewModel>(url);
        }

        public async Task<IEnumerable<CardViewModel>> GetCardsForBoard(string boardID)
        {
            var url = string.Format("{0}boards/{1}/cards{2}", API_BASE, boardID, getAuthTokenAPIFragment());
            return await getCollectionFromAPIAsync<CardViewModel>(url);
        }

        public async Task<IEnumerable<CommentViewModel>> GetCommentsForCard(string cardID)
        {
            var url = string.Format("{0}cards/{1}/actions{2}&filter=commentCard", API_BASE, cardID, getAuthTokenAPIFragment());
            return await getCollectionFromAPIAsync<CommentViewModel>(url);
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