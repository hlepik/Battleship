using System;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages.ViewGame
{
    public class Index : PageModel
    {

        private readonly DAL.AppDbContext _context;
        private readonly ILogger<Index> _logger;

        public Index(ILogger<Index> logger, DAL.AppDbContext context)
        {
            _context = context;
            _logger = logger;

        }
        public Game? Game { get; set; }
        public int AllCount { get; set; }
        public BattleShip BattleShip { get; set; } = new BattleShip();
        [BindProperty]public int Count { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id, int? index, int? forward)
        {

            Game = await _context.Games!
                .Include(g => g.GameOption)
                .Include(g => g.PlayerA)
                .Include(g => g.PlayerB)
                .ThenInclude(x => x.PlayerBoardStates)
                .FirstOrDefaultAsync(m => m.GameId == id);

            BattleShip.Height = Game.GameOption.BoardHeight;
            BattleShip.Width = Game.GameOption.BoardWidth;
            BattleShip.Player1 = Game.PlayerA.Name;
            BattleShip.Player2 = Game.PlayerB.Name;
            AllCount = Game.PlayerB.PlayerBoardStates.Count;
            if (index == null && forward == null)
            {
                Count = Game.PlayerB.PlayerBoardStates.Count;

            }
            else if(index != null && index > 1)
            {
                Count = index.Value - 1;
            }
            else if (forward != null && forward < AllCount + 2)
            {
                Count = forward.Value + 1;

            }
            var boardCount = 0;
            var board1 = "";
            foreach (var each in _context.PlayerBoardStates.Where(p =>p.PlayerId == Game.PlayerAId)
                .OrderBy(x =>x.CreatedAt))
            {
                boardCount++;
                if (boardCount == Count)
                {
                    board1 = each.BoardState;
                }

            }

            boardCount = 0;
            var board2 = "";
            foreach (var each in _context.PlayerBoardStates.Where(p =>p.PlayerId == Game.PlayerBId)
                .OrderBy(x =>x.CreatedAt))
            {
                boardCount++;
                if (boardCount == Count)
                {
                    board2 = each.BoardState;
                }

            }
            BattleShip.SetGameStateFromJsonString(board1, BattleShip.Player1);
            BattleShip.SetGameStateFromJsonString(board2, BattleShip.Player2);


            return Page();
        }

    }



}