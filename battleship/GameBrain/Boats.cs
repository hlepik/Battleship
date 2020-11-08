
namespace GameBrain
{
    public class Patrol : Ship
    {
        public Patrol()
        {
            Name = "Patrol";
            Width = 1;

        }
    }
    public class Cruiser : Ship
    {
        public Cruiser()
        {
            Name = "Cruiser";
            Width = 2;
        }

    }
    public class Submarine : Ship
    {
        public Submarine()
        {
            Name = "Submarine";
            Width = 3;

        }

    }
    public class Battleship : Ship
    {
        public Battleship()
        {
            Name = "Battleship";
            Width = 4;

        }

    }
    public class Carrier : Ship
    {
        public Carrier()
        {
            Name = "Carrier";
            Width = 5;

        }
    }
    public class  MegaSubmarine : Ship
    {
        public  MegaSubmarine()
        {
            Name = "MegaSubmarine";
            Width = 6;

        }
    }

    public class MegaBattleship : Ship
    {
        public MegaBattleship()
        {
            Name = "MegaBattleship";
            Width = 7;

        }
    }

    public class MegaCarrier : Ship
    {
        public MegaCarrier()
        {
            Name = "MegaCarrier";
            Width = 8;

        }
    }
}

