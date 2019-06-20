using System;

/* Made by Mohamed Abbadi for Week 6 recap
 * - Abstract classes
 * - Inheritance
 * - Override
 * - Generic Types
 * - Overloading
 * - !MISSING INTERFACE!
 */

namespace Developement4Recap
{

    public class Box
    {
        protected int value;
        public Box(int v) { this.value = v; }
        public int GetValue() { return this.value; }
    }
    public class Min : Box
    {
        public Min(int value) : base(value) { }
    }
    public class Max : Box
    {
        public Max(int value) : base(value) { }
    }
    public class Pair<a, b>
    {
        private a fst;
        private b snd;
        public Pair(a _a, b _b)
        {
            this.fst = _a;
            this.snd = _b;
        }
        public a GetFirst() { return this.fst; }
        public b GetSecond() { return this.snd; }
    }
    public class Weapon
    {
        private static Random seed = new Random();
        private string name;
        private string description;
        private bool is_one_hand;
        private Pair<Min, Max> damage;
        public Weapon(string name, string desc, bool is_one_hand, Pair<Min, Max> damage)
        {
            this.name = name;
            this.description = desc;
            this.is_one_hand = is_one_hand;
            this.damage = damage;
        }
        public string GetName() { return this.name; }
        public string GetDescription() { return this.description; }
        public bool IsOneHand() { return this.is_one_hand; }
        public Pair<Min, Max> GetDamage()
        {
            return this.damage;
        }
        public int Attack()
        {
            return Weapon.seed.Next(damage.GetFirst().GetValue(), damage.GetSecond().GetValue() + 1);
        }
    }
    public class Shield
    {
        private string name;
        private string description;
        private int defence;
        public Shield(string name, string desc, int defence)
        {
            this.name = name;
            this.description = desc;
            this.defence = defence;
        }
        public string GetName() { return this.name; }
        public string GetDescription() { return this.description; }
        public int GetDefence() { return this.defence; }
        public int AbsorbDamage(Weapon w)
        {
            int damage = w.Attack();
            return (int)((double)damage * (1.0 - ((double)defence / 100.0)));
        }
    }
    public abstract class Player
    {
        protected string name;
        protected int life;
        public Player(string name, int life)
        {
            this.name = name;
            this.life = life;
        }
        public virtual string GetName() { return this.name; }
        public abstract string GetName(string extra_symbol);
        public abstract void Attack(Player against);
        public abstract void TakeDamage(Weapon against);
        public bool IsAlive() { return this.life > 0; }
    }
    public class Paladin : Player
    {
        Weapon weapon;
        Shield shield;
        public Paladin(string name, int life) : base(name, life)
        {
            this.weapon = new Weapon("Morning Star", "...", true, new Pair<Min, Max>(new Min(1), new Max(5)));
            this.shield = new Shield("Aegis", "?", 30);
        }
        public override string GetName(string extra_symbol) { return extra_symbol + this.name + extra_symbol; }
        public override void Attack(Player against)
        {
            against.TakeDamage(this.weapon);
        }
        public override void TakeDamage(Weapon against)
        {
            int new_damage_value = this.shield.AbsorbDamage(against);
            this.life -= new_damage_value;
        }
    }
    public class Assassin : Player
    {
        Weapon weapon1;
        Weapon weapon2;
        public Assassin(string name, int life) : base(name, life)
        {
            this.weapon1 = new Weapon("Simple knife", "...", true, new Pair<Min, Max>(new Min(2), new Max(3)));
            this.weapon2 = new Weapon("Magic knife", "...", true, new Pair<Min, Max>(new Min(1), new Max(10)));
        }
        public override void Attack(Player against)
        {
            against.TakeDamage(this.weapon1);
            against.TakeDamage(this.weapon2);
        }
        public override string GetName(string extra_symbol) { return "~" + this.name + "~"; }
        public override void TakeDamage(Weapon against)
        {
            this.life -= against.Attack();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Pair<string, int>[] _players = new Pair<string, int>[] { new Pair<string, int>("Tristan", 0), new Pair<string, int>("Arrow", 0) };
            for (int x = 0; x < 20; x++)
            {

                Player[] players = new Player[] { new Paladin(_players[0].GetFirst(), 570), new Assassin(_players[1].GetFirst(), 350) };
                while (GameOver(players) == null)
                {
                    for (int i = 0; i < players.Length; i++)
                    {
                        var player1 = players[i];
                        if (player1.IsAlive())
                        {
                            for (int j = 0; j < players.Length; j++)
                            {
                                var player2 = players[j];
                                if (player1 != player2)
                                {
                                    //fight
                                    player1.Attack(player2);
                                }
                            }
                        }
                    }
                }
                Player winner = GameOver(players);
                IncreasePlayerWinCount(_players, winner);
                //Console.WriteLine("The winner is: " 
                //                    + (winner.GetName() == "Tristan" 
                //                        ? winner.GetName("~")
                //                        : winner.GetName()));
            }
            for (int i = 0; i < _players.Length; i++)
            {
                var current_player = _players[i];
                Console.WriteLine(current_player.GetFirst() + " won " + current_player.GetSecond() + " times.");
            }
        }
        static void IncreasePlayerWinCount(Pair<string, int>[] players, Player winner)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].GetFirst() == winner.GetName())
                {
                    players[i] = new Pair<string, int>(players[i].GetFirst(), players[i].GetSecond() + 1);
                }
            }
        }
        static Player GameOver(Player[] players)
        {
            Player last_alive_player = null;
            int amount_dead_players = 0;
            for (int i = 0; i < players.Length; i++)
            {
                if (!players[i].IsAlive())
                {
                    amount_dead_players++;
                }
                else
                {
                    last_alive_player = players[i];
                }
            }
            if (amount_dead_players < players.Length - 1)
            {
                return null;
            }
            return last_alive_player;
        }
    }
}


