using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain
{
    public class GameOption
    {
        public int GameOptionId { get; set; }


        [MinLength(2), MaxLength(128)]
        public string Name { get; set; } = null!;
        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }

        // enums are actually int in database
        public EBoatsCanTouch EBoatsCanTouch { get; set; }

        public ENextMoveAfterHit ENextMoveAfterHit { get; set; }

        public ICollection<Game> Games { get; set; } = null!;
    }
}