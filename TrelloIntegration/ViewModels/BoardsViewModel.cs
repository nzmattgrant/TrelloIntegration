﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TrelloIntegration.Models;
using TrelloIntegration.Services;

namespace TrelloIntegration.ViewModels
{
    public class BoardsViewModel
    {
        public IEnumerable<Board> Boards { get; set; }

        public async Task SetUp(ITrelloService service, User user)
        {
            Boards = await service.GetBoardsForUser(user.ID, user.TrelloToken);
        }
    }
}