using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Enums;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace WebApp.Pages.GamePlay
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
        public BattleShip BattleShip { get; set; } = new BattleShip();

        public string Message { get; set; } = "";


        public async Task<IActionResult> OnGetAsync(int id, int? x, int? y)
        {

            Game = await _context.Games
                .Where(p => p.GameId == id)
                .Include(p => p.GameOption)
                .Include(p => p.PlayerA)
                .ThenInclude(p =>p.PlayerBoardStates).Take(1)
                .Include(p => p.PlayerB)
                .ThenInclude(p =>p.PlayerBoardStates).Take(1)
                .FirstOrDefaultAsync();


            var boardState1 = Game.PlayerA.PlayerBoardStates.Select(x => x.BoardState).Last();
            var boardState2 = Game.PlayerB.PlayerBoardStates.Select(x => x.BoardState).Last();

            BattleShip.Width = Game.GameOption.BoardWidth;
            BattleShip.Height = Game.GameOption.BoardHeight;
            BattleShip.Player1 = Game.PlayerA.Name;
            BattleShip.Player2 = Game.PlayerB.Name;
            BattleShip.GameRule = Game.GameOption.EBoatsCanTouch;

            BattleShip.SetGameStateFromJsonString(boardState1, boardState2);

            var player = BattleShip.Player1;
            var playerBoard = BattleShip.Board2;
            if (!BattleShip.NextMoveByX)
            {
                player = BattleShip.Player2;
                playerBoard = BattleShip.Board1;
            }

            if (x != null && y != null)
            {

                BattleShip.MakeAMove(x.Value, y.Value, playerBoard);

                var boardState = new PlayerBoardState()
                {
                    PlayerId = Game.PlayerAId,
                    BoardState = BattleShip.GetSerializedGameState(BattleShip.Board1)
                };
                _context.PlayerBoardStates.Add(boardState);

                boardState = new PlayerBoardState()
                {
                    PlayerId = Game.PlayerBId,
                    BoardState = BattleShip.GetSerializedGameState(BattleShip.Board2)
                };
                _context.PlayerBoardStates.Add(boardState);


                if (BattleShip.Player1 == player)
                {
                    //see on t[hi juuuuu
                    foreach (var each in BattleShip.Player2Ships)
                    {
                        var gameBoat = new GameBoat()
                        {
                            Name = each.Name,
                            Size = each.Width,
                            IsSunken = each.IsSunken,
                            Direction = each.Direction,
                            LifeCount = each.LifeCount,
                            ShipId = each.ShipId
                        };
                        gameBoat.Player = Game.PlayerA;
                        _context.GameBoats!.Add(gameBoat);
                    }
                }
                else
                {
                    foreach (var each in BattleShip.Player1Ships)
                    {
                        var gameBoat = new GameBoat()
                        {
                            Name = each.Name,
                            Size = each.Width,
                            IsSunken = each.IsSunken,
                            Direction = each.Direction,
                            LifeCount = each.LifeCount,
                            ShipId = each.ShipId
                        };
                        gameBoat.Player = Game.PlayerB;
                        _context.GameBoats!.Add(gameBoat);
                    }
                }
                await _context.SaveChangesAsync();

                if (BattleShip.TextWhenMiss)
                {
                    Message = "You missed!";
                    return RedirectToPage("/NextMove/Index", new {id = Game.GameId, message = Message});
                }

                BattleShip.BoardAfterHit(x.Value, y.Value, BattleShip);
                Message = BattleShip.TextWhenHit ? "Hit!" : "Ship has been destroyed!";

                if(Message.Length > 1 && Game.GameOption.ENextMoveAfterHit == ENextMoveAfterHit.SamePlayer)
                {
                    return Page();
                }

            }
            return Page();
        }

    }
}