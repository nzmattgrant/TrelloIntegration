using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TrelloIntegration.Services;

namespace TrelloIntegration.ViewModels
{
    public class DashboardViewModel
    {
        public string UserToken { get; set; }
        public IEnumerable<Board> Boards { get; set; }

        public async Task SetUp(string userToken)
        {
            UserToken = userToken;
            var service = new TrelloService(userToken);
            var memberID = await service.GetMemberIDForUserToken();
            Boards = await service.GetBoardsWithNestedCollectionsForUser(memberID);
        }
    }
}