using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace RPGGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }

    public class Game
    {
        private Player player;
        private Shop shop;
        private bool isRunning;



        public void Start()
        {
            Console.WriteLine("Welcome to the world of Spira! Get ready to embark on an epic adventure.\nBut first we need to know a few things.");

            // Initialize player and shop and call to choose class
            Console.Write("\nEnter your character's name: ");
            string name = Console.ReadLine();

            Console.WriteLine("\nChoose your class:");
            Console.WriteLine("Warrior");
            Console.WriteLine("Mage");
            Console.WriteLine("Druid");
            Console.WriteLine("Paladin");
            Console.WriteLine("\nEnter your choice:");
            string classChoice = Console.ReadLine();
            //should prevent any wrong input for class choice by keeping it in the loop until a class is entered
            while (classChoice != "warrior" || classChoice != "mage" || classChoice != "druid" || classChoice != "paladin")
            {
                if (classChoice.Equals("warrior", StringComparison.OrdinalIgnoreCase))
                { classChoice = "warrior"; break; }
                if (classChoice.Equals("mage", StringComparison.OrdinalIgnoreCase))
                { classChoice = "mage"; break; }
                if (classChoice.Equals("druid", StringComparison.OrdinalIgnoreCase))
                { classChoice = "druid"; break; }
                if (classChoice.Equals("paladin", StringComparison.OrdinalIgnoreCase))
                { classChoice = "paladin"; break; }
                else
                {
                    Console.WriteLine("Invalid Input. Try again");
                    Console.WriteLine("\nEnter your choice:");
                    classChoice = Console.ReadLine();
                }
            }
            CharacterClass chosenClass = classChoice switch
            {
                "warrior" => new Warrior(),
                "mage" => new Mage(),
                "druid" => new Druid(),
                "paladin" => new Paladin(),
                _ => throw new Exception("Invalid Class choice.") // throws an exception if any other input makes its way out of the loop

            };

            //generate a new player
            player = new Player(name, chosenClass);
            shop = new Shop();
            //sets the game running condition to true
            isRunning = true;


            //The game loop
            while (isRunning)
            {
                DisplayMenu();
                HandleInput();
            }

            Console.WriteLine("Thank you for playing! Goodbye!");
        }

        private void DisplayMenu()
        {
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("1. View Character");
            Console.WriteLine("2. Explore");
            Console.WriteLine("3. Use Item");
            Console.WriteLine("4. Visit Shop");
            Console.WriteLine("5. Unequip Item");
            Console.WriteLine("6. Exit Game");
        }

        private void HandleInput()
        {
            Console.Write("Enter your choice: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    player.DisplayStats();
                    break;
                case "2":
                    Explore();
                    break;
                case "3":
                    UseItem();
                    break;
                case "4":
                    VisitShop();
                    break;
                case "5":
                    Unequip();
                    break;
                case "6":
                    isRunning = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        private void Explore()
        {
            List<Enemy> commonEnemies = new List<Enemy>
            {   new Enemy("Goblin", 15, 6, 2, 2),
                new Enemy("Spider", 10, 6, 1, 2),
                new Enemy("Wolf", 10, 6, 1, 2),
                new Enemy("Bandit", 20, 10, 10, 5)
            };
            List<Enemy> uncommonEnemies = new List<Enemy>
            {   new Enemy("Giant", 25, 10, 5, 10),
                new Enemy("Bomb", 15, 7, 2, 5),
                new Enemy("Skeleton", 20, 10, 5, 5),
                new Enemy("Harpy", 20, 10, 5, 5)
            };
            List<Enemy> rareEnemies = new List<Enemy>
            {
                new Enemy("Troll", 35, 10, 5, 10),
                new Enemy("Werewolf", 35, 15, 5, 15),
                new Enemy("Demon", 35, 10, 10, 15),
                new Enemy("Chimera", 40, 10, 5, 15)
            };
            List<Enemy> onePercentEnemies = new List<Enemy>
            {
                new Enemy("Minotaur", 55, 25, 20, 50),
                new Enemy("Basilisk", 55, 25, 15, 50),
                new Enemy("Lich", 60, 30, 25, 70)
            };
            List<Item> randomLoot = new List<Item>
            {
                new Weapon("Beast Sword", 20, 40),
                new Potion("Minor Health Potion", 20, 20),
                new Potion("Health Potion", 50, 40),
                new Armor("Beast Armor", 5, 40)
            };
            Console.WriteLine("You venture into the unknown...");
            Random random = new Random();
            int encounterChance = random.Next(1, 151);
            int whichEnemy;
            int whichLoot;
            if (encounterChance <= 50)
            {
                whichEnemy = random.Next(0, 3);
                Enemy enemy = commonEnemies[whichEnemy];
                Console.WriteLine($"A wild {enemy.Name} appears!");
                Battle(enemy);
            }
            if (encounterChance > 50 && encounterChance <= 80)
            {
                whichEnemy = random.Next(0, 3);
                Enemy enemy = uncommonEnemies[whichEnemy];
                Console.WriteLine($"A fierce {enemy.Name} appears!");
                Battle(enemy);
            }
            if (encounterChance > 80 && encounterChance <= 99)
            {
                whichEnemy = random.Next(0, 3);
                Enemy enemy = rareEnemies[whichEnemy];
                Console.WriteLine($"A deadly {enemy.Name} approaches!");
                Battle(enemy);
            }
            if (encounterChance == 100)
            {
                whichEnemy = random.Next(0, 2);
                Enemy enemy = onePercentEnemies[whichEnemy];
                Console.WriteLine($"A Legendary {enemy.Name} approaches!");
                Battle(enemy);
            }
            if (encounterChance > 100 && encounterChance <= 110)
            {
                whichLoot = random.Next(0, 3);
                player.AddItem(randomLoot[whichLoot]);
            }
            else
            {
                Console.WriteLine("You find nothing of interest.");
            }
        }

        private void UseItem()
        {
            Console.WriteLine("\nYour Inventory:");
            player.DisplayInventory();
            Console.Write("Choose an item to use: ");
            string itemName = Console.ReadLine();
            player.UseItem(itemName);
        }

        private void Unequip()
        {
            Console.WriteLine("\nYour Equipped Items:");
            player.DisplayEquipped();
            if (player.EquippedContainsAny() == false)
            {
                return;
            }
            Console.WriteLine("Choose an item to unequip:");
            string itemName = Console.ReadLine();
            player.Unequip(itemName);
        }

        private void VisitShop()
        {
            Console.WriteLine("\nWelcome to the Shop!");
            shop.DisplayItems();

            Console.Write("Enter the name of the item you want to buy (or type 'exit' to leave): ");
            string itemName = Console.ReadLine();

            if (itemName.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("You leave the shop.");
                return;
            }

            shop.SellItem(itemName, player);
        }

        private void Battle(Enemy enemy)
        {
            Item item = new Potion("Minor Health Potion", 20, 15);
            int specialAbilityCounter = 0;
            Random random = new Random();
            int escapeDice;
            int lootGold;
            int lootItems;
            int amountGold = 0;

            while (enemy.Health > 0 && player.Health > 0)
            {
                Console.WriteLine("\nChoose an action:");
                Console.WriteLine("1. Attack");
                Console.WriteLine("2. Run");
                Console.WriteLine("3. Use an Item");
                Console.WriteLine("4. Special Ability");
                string choice = Console.ReadLine();

                if (choice == "1" || choice.Equals("attack", StringComparison.OrdinalIgnoreCase))
                {//add a roll to determine damage with the maximum being set at your attack power, maybe apply weapon boost after, and maybe apply a critical chance
                    player.Attack(enemy);

                    if (enemy.Health > 0)
                    {
                        enemy.Attack(player);
                        //if (enemy.LoseTurn() == false) { enemy.Attack(player); }
                    }
                }
                else if (choice == "2" || choice.Equals("Run", StringComparison.OrdinalIgnoreCase))
                {
                    escapeDice = random.Next(1, 21);
                    if (escapeDice >= 10)
                    {
                        Console.WriteLine("You successfully escape the battle.");
                        return;
                    }
                    else Console.WriteLine("You failed to escape the battle.");
                    enemy.Attack(player);
                }
                else if (choice == "3" || choice.Equals("Use an Item", StringComparison.OrdinalIgnoreCase) || choice.Equals("Item", StringComparison.OrdinalIgnoreCase))
                {
                    UseItem();
                    enemy.Attack(player);

                }
                else if (choice == "4" || choice.Equals("Special Ability", StringComparison.OrdinalIgnoreCase) || choice.Equals("Special", StringComparison.OrdinalIgnoreCase) || choice.Equals("Ability", StringComparison.OrdinalIgnoreCase))
                {
                    if (specialAbilityCounter == 0)
                    {
                        player.UseSpecialAbility(enemy);

                        specialAbilityCounter++;
                        if (enemy.Health > 0)
                        {
                            enemy.Attack(player);
                        }
                    }
                    else if (specialAbilityCounter == 1)
                    {
                        Console.WriteLine("\nYou have already used your Special Ability this battle.");
                        enemy.Attack(player);
                    }

                }
                else
                {
                    Console.WriteLine("Invalid choice. The enemy attacks!");
                    enemy.Attack(player);
                }
            }

            if (player.Health <= 0)
            {
                Console.WriteLine("You have been defeated. Game over.");
                isRunning = false;
            }
            else if (enemy.Health <= 0)
            {
                lootGold = random.Next(1, 101);
                lootItems = random.Next(1, 121);

                if (lootGold > 90) { amountGold = random.Next(1, 10); }

                if (enemy.Name == "Lich")
                {
                    Weapon weapon = new Weapon("Lich Killer Sword", 30, 300);
                    Console.WriteLine($"You have defeated the {enemy.Name}!");
                    player.GainExperience(enemy.Experience);
                    player.GainGold(enemy.Gold + amountGold);
                    player.AddItem(weapon);
                    if (lootItems > 110) { player.AddItem(item); }
                    player.LevelUp();
                    return;

                }
                if (enemy.Name == "Basilisk")
                {
                    Weapon weapon = new Weapon("Sword of the Basilisk's Fang", 35, 350);
                    Console.WriteLine($"You have defeated the {enemy.Name}!");
                    player.GainExperience(enemy.Experience);
                    player.GainGold(enemy.Gold + amountGold);
                    player.AddItem(weapon);
                    if (lootItems > 110) { player.AddItem(item); }
                    player.LevelUp();
                    return;
                }
                if (enemy.Name == "Minotaur")
                {
                    Weapon weapon = new Weapon("Minotaur's Axe", 30, 300);
                    Console.WriteLine($"You have defeated the {enemy.Name}!");
                    player.GainExperience(enemy.Experience);
                    player.GainGold(enemy.Gold + amountGold);
                    player.AddItem(weapon);
                    if (lootItems > 110) { player.AddItem(item); }
                    player.LevelUp();
                    return;
                }
                else
                    Console.WriteLine($"You have defeated the {enemy.Name}!");
                player.GainExperience(enemy.Experience);
                player.GainGold(enemy.Gold + amountGold);
                if (lootItems > 110) { player.AddItem(item); }
                player.LevelUp();

            }
        }
    }

    public class Player
    {
        public string Name { get; private set; }
        public int Level { get; private set; }
        public int Health { get; private set; }
        public int AttackPower { get; private set; }
        public int MagicPower { get; private set; }
        public int HiddenExperience { get; private set; }
        public int Experience { get; private set; }
        public int Gold { get; private set; }
        public int ArmorBoost { get; private set; }
        public int BlockPower { get; private set; }
        public CharacterClass Class { get; private set; }
        private List<Item> inventory;
        private List<Item> equipped;
        Random random = new Random();

        public Player(string name, CharacterClass characterClass)
        {
            Name = name;
            Level = 1;
            Health = characterClass.BaseHealth;
            AttackPower = characterClass.BaseAttackPower;
            MagicPower = characterClass.BaseMagicPower;
            HiddenExperience = 0;
            Experience = 0;
            Gold = 0;
            inventory = new List<Item>();
            equipped = new List<Item>(2);
            Class = characterClass;
            ArmorBoost = 0;
            BlockPower = characterClass.BaseBlock;

            // Add starter items
            inventory.Add(new Weapon("Basic Sword", 5, 5));
            inventory.Add(new Potion("Minor Health Potion", 20, 15));
        }
        public void UseSpecialAbility(Enemy enemy)
        {
            Class.UseSpecialAbility(this, enemy);
        }

        public void LevelUp()
        {
            if (HiddenExperience == 100)
            {
                Level++;
                Health += 10;
                AttackPower += 5;
                MagicPower += 5;
                BlockPower += 2;
                HiddenExperience = 0;

                Console.WriteLine($"You have leveled up. \nYou feel your powers growing.\n You are now {Level}");
                DisplayStats();
            }
        }

        public void DisplayStats()
        {
            Console.WriteLine($"\n{Name}'s Stats:");
            Console.WriteLine($"Health: {Health}");
            Console.WriteLine($"Attack Power: {AttackPower}");
            if (MagicPower > 0) { Console.WriteLine($"Magic Power: {MagicPower}"); }
            if (ArmorBoost > 0) { Console.WriteLine($"Defense: +{ArmorBoost}"); }
            Console.WriteLine($"Experience: {Experience}");
            Console.WriteLine($"Gold: {Gold}");
        }

        public void DisplayInventory()
        {
            if (inventory.Count == 0) { Console.WriteLine("Your inventory is empty."); return; }
            foreach (var item in inventory) { Console.WriteLine(item.Name); }
        }

        public void DisplayEquipped()
        {
            if (equipped.Count == 0) { Console.WriteLine("You have no items equipped"); return; }
            foreach (var item in equipped) { Console.WriteLine(item.Name); }
        }

        public bool EquippedContainsAny()
        {
            if (equipped.Count == 0) return false;
            else return true;
        }

        public void UseItem(string itemName)
        {
            Item item = inventory.Find(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));

            if (item == null)
            {
                Console.WriteLine("Item not found.");
                return;
            }

            item.Use(this);
            if (item is Potion)
            {
                inventory.Remove(item);
            }

        }

        public void EquipArmor(Armor armor)
        {
            if (equipped.Contains(armor)) { Console.WriteLine("You already have armor equipped. Unequip it first"); return; }
            else
                ArmorBoost += armor.DefenseBoost;
            equipped.Add(armor);
            inventory.Remove(armor);
            Console.WriteLine($"You equipped {armor.Name}. Defense increased by {armor.DefenseBoost}");
        }

        public void EquipWeapon(Weapon weapon)
        {

            //if (equipped.Find("Sword"))
            //{
            //    Console.WriteLine("You already have a weapon equipped. Unequip it first.");
            //    return;
            //}
            //else
            AttackPower += weapon.AttackBoost;
            equipped.Add(weapon);
            inventory.Remove(weapon);
            Console.WriteLine($"You equipped {weapon.Name}. Attack Power increased by {weapon.AttackBoost}.");
        }
        public void Unequip(string itemName)
        {
            Item item = equipped.Find(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
            if (equipped.Count == 0)
            {
                Console.WriteLine("You don't have any items equipped.");
                return;
            }
            else
                inventory.Add(item);
            equipped.Remove(item);
            Console.WriteLine($"You have successfully unequipped {item.Name}.");

        }

        public void Heal(int amount)
        {
            Health += amount;
            Console.WriteLine($"You healed {amount} health points. Current health: {Health}.");
        }

        public void Attack(Enemy enemy)
        {
            int attackPower = random.Next(1, AttackPower);//rolls for damage between 1 and your maximum attack power
            Console.WriteLine($"You attack the {enemy.Name} for {attackPower} damage!");
            enemy.TakeDamage(attackPower);
        }

        public void TakeDamage(int damage)
        {
            int blockDamage = random.Next(0, BlockPower);
            if (blockDamage >= damage) { Console.WriteLine("You successfully blocked the attack!"); }
            if (blockDamage == 0) { Console.WriteLine($"{Name} takes {damage} damage. Remaining health: {Health}"); Health -= damage; }
            if (blockDamage < damage && blockDamage > 0)
            {
                Console.WriteLine($"\n{Name} partially blocked the attack.");
                Health -= (damage - blockDamage);
                Console.WriteLine($"{Name} takes {damage - blockDamage} damage. Remaining health: {Health}");
            }


        }

        public void GainExperience(int amount)
        {
            Experience += amount;
            HiddenExperience += amount;
            Console.WriteLine($"You gained {amount} experience points!");
        }

        public void GainGold(int amount)
        {
            Gold += amount;
            Console.WriteLine($"You found {amount} gold!");
        }

        public void SpendGold(int amount)
        {
            if (Gold >= amount)
            {
                Gold -= amount;
                Console.WriteLine($"You spent {amount} gold. Remaining gold: {Gold}");
            }
            else
            {
                Console.WriteLine("You don't have enough gold.");
            }
        }

        public void AddItem(Item item)
        {
            inventory.Add(item);
            Console.WriteLine($"You received {item.Name}.");
        }
    }

    public abstract class Item
    {
        public string Name { get; private set; }
        public int Value { get; set; }

        protected Item(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public abstract void Use(Player player);
    }

    public class Weapon : Item
    {
        public int AttackBoost { get; private set; }

        public Weapon(string name, int attackBoost, int value) : base(name, value)
        {
            AttackBoost = attackBoost;
        }

        public override void Use(Player player)
        {
            player.EquipWeapon(this);
        }
    }

    public class Potion : Item
    {
        public int HealAmount { get; private set; }

        public Potion(string name, int healAmount, int value) : base(name, value)
        {
            HealAmount = healAmount;
        }

        public override void Use(Player player)
        {
            player.Heal(HealAmount);
        }

    }

    public class Armor : Item
    {
        public int DefenseBoost { get; private set; }

        public Armor(string name, int defenseBoost, int value) : base(name, value)
        {
            DefenseBoost = defenseBoost;
        }
        public override void Use(Player player)
        {
            player.EquipArmor(this);
        }


    }

    public class Shop
    {
        private List<Item> itemsForSale;

        public Shop()
        {
            itemsForSale = new List<Item>
            {
                new Weapon("Steel Sword", 10, 20),
                new Potion("Health Potion", 20, 15),
                new Armor("Leather Armor", 1, 30)
            };
        }

        public void DisplayItems()
        {
            Console.WriteLine("Items available for purchase:");
            foreach (var item in itemsForSale)
            {
                if (item is Weapon weapon)
                {
                    Weapon w = item as Weapon;
                    Console.WriteLine($"{w.Name} - Attack Boost: {w.AttackBoost} (Price: {weapon.Value} Gold)");
                }
                else if (item is Potion potion)
                {
                    Console.WriteLine($"{potion.Name} - Heal Amount: {potion.HealAmount} (Price: {potion.Value} Gold)");
                }
                else if (item is Armor armor)
                {
                    Console.WriteLine($"{armor.Name} - Defense Boost: {armor.DefenseBoost} (Price: {armor.Value} Gold");
                }
            }
        }

        public void SellItem(string itemName, Player player)
        {
            Item item = itemsForSale.Find(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));

            if (item == null)
            {
                Console.WriteLine("Item not found in the shop.");
                return;
            }

            int itemCost = item.Value;

            if (player.Gold >= itemCost)
            {
                player.SpendGold(itemCost);
                player.AddItem(item);
                //Console.WriteLine($"You purchased {item.Name}!");
            }
            else
            {
                Console.WriteLine("You don't have enough gold to buy this item.");
            }
        }
    }

    public class Enemy
    {
        public string Name { get; private set; }
        public int Health { get; private set; }
        public int AttackPower { get; private set; }
        public int Gold { get; private set; }
        public int Experience { get; private set; }
        public int BlockPower { get; private set; }
        Random random = new Random();

        public Enemy(string name, int health, int attackPower, int gold, int experience)
        {
            Name = name;
            Health = health;
            AttackPower = attackPower;
            Gold = gold;
            Experience = experience;
        }

        public void Attack(Player player)
        {
            int attackPower = random.Next(1, AttackPower);
            AttackPower = attackPower;
            if (player.ArmorBoost >= attackPower) { Console.WriteLine($"\n{Name} couldn't get through {player.Name}'s armor."); }
            else
                Console.WriteLine($"{Name} attacks for {AttackPower - player.ArmorBoost} damage!");
            player.TakeDamage(AttackPower - player.ArmorBoost);
        }

        public void TakeDamage(int damage)
        {
            int blockPower = random.Next(0, BlockPower);
            if (blockPower >= damage) { Console.WriteLine($"\n{Name} successfully blocked the attack."); }
            if (blockPower == 0) { Health -= damage; Console.WriteLine($"\n{Name} takes {damage} damage. Remaining health: {Health}"); }
            if (blockPower < damage && blockPower > 0)
            {
                Health -= (damage - blockPower);
                Console.WriteLine($"\n{Name} partially blocked the attack\n{Name} takes {damage - blockPower} damage. Remaining health: {Health}");
            }
        }

        public bool LoseTurn()
        {
            return false;
        }
    }
    public abstract class CharacterClass
    {
        public abstract string ClassName { get; }
        public abstract int BaseHealth { get; }
        public abstract int BaseAttackPower { get; }
        public abstract int BaseMagicPower { get; }
        public abstract int BaseBlock { get; }
        public abstract string SpecialAbilityDescription { get; }

        public virtual void UseSpecialAbility(Player player, Enemy enemy)
        {
            Console.WriteLine($"Special ability for {ClassName} not implemented yet.");
        }
    }

    public class Warrior : CharacterClass
    {
        public override string ClassName => "Warrior";
        public override int BaseHealth => 150;
        public override int BaseAttackPower => 10;
        public override int BaseMagicPower => -100;
        public override int BaseBlock => 10;
        public override string SpecialAbilityDescription => "Shield Bash: Deal massive damage to the enemy.";

        public override void UseSpecialAbility(Player player, Enemy enemy)
        {
            int bashDamage = player.Level * 10;
            Console.WriteLine($"You used Shield Bash! The enemy takes {bashDamage} damage.");
            enemy.TakeDamage(bashDamage);
        }
    }

    public class Mage : CharacterClass
    {
        public override string ClassName => "Mage";
        public override int BaseHealth => 80;
        public override int BaseAttackPower => 5;
        public override int BaseMagicPower => 15;
        public override int BaseBlock => 5;
        public override string SpecialAbilityDescription => "Fireball: Deal high magic damage to the enemy.";

        public override void UseSpecialAbility(Player player, Enemy enemy)
        {
            int fireballDamage = 25;
            Console.WriteLine($"You cast Fireball! It deals {fireballDamage} damage to the enemy.");
            enemy.TakeDamage(fireballDamage);
        }
    }
    public class Druid : CharacterClass
    {
        public override string ClassName => "Druid";
        public override int BaseHealth => 90;
        public override int BaseAttackPower => 10;
        public override int BaseMagicPower => 10;
        public override int BaseBlock => 5;
        public override string SpecialAbilityDescription => "One with Nature: Heal damage from player";

        public override void UseSpecialAbility(Player player, Enemy enemy)
        {
            int healAmount = 25;
            Console.WriteLine($"You become One with Nature! You can feel life force flowing into you through your feet.\n Heals you for {healAmount} health.");
            player.Heal(healAmount);
        }
    }
    public class Paladin : CharacterClass
    {
        public override string ClassName => "Paladin";
        public override int BaseHealth => 120;
        public override int BaseAttackPower => 10;
        public override int BaseMagicPower => 10;
        public override int BaseBlock => 10;
        public override string SpecialAbilityDescription => "Divine Justice: Damages enemies and heals player for that same amount";

        public override void UseSpecialAbility(Player player, Enemy enemy)
        {
            Random random = new Random();
            int damageAmount = random.Next(10, 51);
            damageAmount = 5;
            int healAmount = damageAmount;
            Console.WriteLine($"You strike your enemy with divine light! You feel your lifeforce increasing!\n {enemy.Name} takes {damageAmount} of damage, and you receive {healAmount} health.");
            enemy.TakeDamage(damageAmount);
            player.Heal(healAmount);
        }
    }
}

