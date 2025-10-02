using System;
using static System.Net.Mime.MediaTypeNames;

namespace TextRpg
{
    public enum WEAPON_TYPE
    {
        ARMOR = 0,
        ATTACKER = 1
    }

    public enum GameState
    {
        LOBBY,
        STATUS,
        INVENTORY,
        EQUIPMENT,
        SHOP,
        BUY,
        SELL,
        HEALING,
        DUNGEON,
        PLAYDUNGEON,
        END
    }
    public class Player
    {
        public int level;
        public string name;
        public string job;
        public int atk;
        public int def;
        public int hp;
        public int gold;
        public int stamina;
        public float exp;
        public float[] needExp = { 50, 80, 150, 500 };
        public List<Item> haveItemList;
        public List<Item> equipItems;
        public int Level
        { get { return level; } set { level = value; } }

        public int Hp {
            get { return hp; }
            set { 
                hp = value;
                if (hp >= 500)
                    hp = 500;
                else if(hp < 0)
                    hp = 0;
            }
        }

        public int Stamina
        {
            get { return stamina; }
            set
            {
                stamina = value;
                if (stamina >= 100)
                    stamina = 100;
                else if (stamina < 0)
                    stamina = 0;
            }
        }

        public float Exp
        {
            get {  return exp; }
            set 
            { 
                exp = value;
                if (exp >= needExp[level-1] && level <=needExp.Length)
                {
                    exp = exp - needExp[level - 1];
                    level++;
                    Console.WriteLine("레벨 업!!\n");
                }
            }
        }

        public Player(int level, string name, string job, int atk, int def, int hp, int gold, int stamina = 100, float exp = 0f)
        {
            this.Level = level;
            this.name = name;
            this.job = job;
            this.atk = atk;
            this.def = def;
            this.Hp = hp;
            this.gold = gold;
            this.Stamina = stamina;
            this.Exp = exp;
            haveItemList = new List<Item>();
            equipItems = new List<Item>();
        }
        public void CharacterInfo()
        {
            Console.Clear();
            Console.WriteLine($"Lv. {Level}\n{name} ( {job} )\n공격력 : {atk}\n방어력 : " +
                $"{def}\n체력 : {Hp}\nGold : {gold} G\nStamina : {Stamina}\nExp : {Exp} (%)\n");
            Console.WriteLine("0. 나가기\n");
            Console.Write("원하시는 행동을 입력해주세요.\n>>");
        }
        public void EquipOrUnEquip(Item item, bool isEquip)
        {
            if (item.type == WEAPON_TYPE.ARMOR)
                this.def += isEquip ? item.weaponStrength : -item.weaponStrength;
            else if(item.type == WEAPON_TYPE.ATTACKER)
                this.atk += isEquip ? item.weaponStrength : -item.weaponStrength;
        }
        public void PlusMinusGold(int gold, bool isPlus)
        {
            this.gold += isPlus ? gold : -gold;
        }

        public void PlusExp(float exp)
        {
            this.Exp += exp;
        }

        public void HealHp(int hp)
        {
            this.Hp += hp;
        }

        public void HealStamina(int stamina)
        {
            this.Stamina += stamina;
        }

        public void RemoveHp(int hp)
        {
            this.Hp -= hp;
        }
    }

    public class Item
    {
        public string name;
        public WEAPON_TYPE type; //타입에 따라 공격력, 방어력 나누기 위함
        public int weaponStrength;
        public string description;
        public int gold;
        public bool isPurchase = false;

        public Item (string name, WEAPON_TYPE type, int weaponStrength, string description, int gold = 0, bool isPurchase = false)
        {
            this.name = name;
            this.type = type;
            this.weaponStrength = weaponStrength;
            this.description = description;
            this.gold = gold;
            this.isPurchase = isPurchase;
        }
    }

    //ItemManager 하나 만들어서 통합 아이템 관리 매니저를 구현해보면 좋을 것 같다.
    public class ItemManager
    {
        /*List<Item> itemList = new List<Item>();
        List<Item> shopItemList = new List<Item>();

        public List<Item> ItemList { get  { return itemList; } set { itemList = value; } }
        public List<Item> ShopItemList { get { return shopItemList; } set { shopItemList = value; } }

        public ItemManager(List<Item> itemList, List<Item> shopItemList)
        {
            this.ItemList = itemList;
            this.ShopItemList = shopItemList;
        }*/
        public void InitializePlayerItem(Player player)
        {
            Item steelArmor = new Item("무쇠갑옷", WEAPON_TYPE.ARMOR, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.");
            Item oldSword = new Item("낡은 검", WEAPON_TYPE.ATTACKER, 2, "쉽게 볼 수 있는 낡은 검 입니다.");
            Item practiceSpear = new Item("연습용 창", WEAPON_TYPE.ATTACKER, 3, "검보다는 그래도 창이 다루기 쉽죠.");
            Item woodShield = new Item("나무 방패", WEAPON_TYPE.ARMOR, 1, "많이 갈라진 나무 방패입니다.");
            player.haveItemList.Add(steelArmor);
            player.haveItemList.Add(oldSword);
            player.haveItemList.Add(practiceSpear);
            player.haveItemList.Add(woodShield);
        }
        public void InitializeShop(Player player, List<Item> items)
        {
            Item traineeArmor = new Item("수련자 갑옷", WEAPON_TYPE.ARMOR, 5, "수련에 도움을 주는 갑옷입니다.", 1000);
            Item steelArmor = new Item("무쇠갑옷", WEAPON_TYPE.ARMOR, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 1800);
            Item spartaArmor = new Item("스파르타의 갑옷", WEAPON_TYPE.ARMOR, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500);
            Item oldSword = new Item("낡은 검", WEAPON_TYPE.ATTACKER, 2, "쉽게 볼 수 있는 낡은 검 입니다.", 600);
            Item bronzeAxe = new Item("청동 도끼", WEAPON_TYPE.ATTACKER, 5, "어디선가 사용됐던거 같은 도끼입니다.", 1500);
            Item spartaSpear = new Item("스파르타의 창", WEAPON_TYPE.ATTACKER, 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 2700);

            items.Add(traineeArmor);
            items.Add(steelArmor);
            items.Add(spartaArmor);
            items.Add(oldSword);
            items.Add(bronzeAxe);
            items.Add(spartaSpear);

            for (int i = 0; i < items.Count; i++)
            {
                for (int j = 0; j < player.haveItemList.Count; j++) 
                {
                    if (player.haveItemList[j].name == items[i].name)
                    {
                        items[i].isPurchase = true;
                        break;
                    }
                }
            }
        }
    }

    public class Dungeon
    {
        public string name;
        public int minDef;
        public int rewardGold;
        public float rewardExp;
        public Dungeon(string name, int minDef, int rewardGold, float rewardExp)
        {
            this.name = name;
            this.minDef = minDef;
            this.rewardGold = rewardGold;
            this.rewardExp = rewardExp;
        }
    }

    public class DungeonManager
    {
        public List<Dungeon> dungeons = new List<Dungeon>();
        public Dungeon selectedDungeon;
        public void InitializeDungeon()
        {
            Dungeon easyDungeon = new Dungeon("쉬운 던전", 5, 1000, 50);
            Dungeon normalDungeon = new Dungeon("일반 던전", 11, 1700, 100);
            Dungeon hardDungeon = new Dungeon("어려운 던전", 17, 2500, 200);

            dungeons.Add(easyDungeon);
            dungeons.Add(normalDungeon);
            dungeons.Add(hardDungeon);
        }

        public void DungeonClear(Player player, Dungeon dungeon)
        {
            Random rndNum = new Random();
            int plusRemoveHp = player.def - dungeon.minDef;
            int removeHp = rndNum.Next(20+ plusRemoveHp, 36+ plusRemoveHp);
            Random rndPercent = new Random();
            int percent = rndPercent.Next(player.atk, (player.atk * 2) + 1);
            float totalExp = dungeon.rewardExp * (1 + (percent / 100));
            Console.WriteLine($"축하합니다!!\n{dungeon.name}을 클리어 하였습니다.\n");
            Console.WriteLine($"[탐험 결과]\n체력 {player.Hp} -> {player.Hp - removeHp}\n" +
                $"Gold {player.gold} -> {player.gold + dungeon.rewardGold} G\n");
            Console.WriteLine("0. 나가기");
            player.PlusMinusGold(dungeon.rewardGold, true);
            player.RemoveHp(removeHp);
            player.PlusExp(totalExp);
            Console.Write("\n원하시는 행동을 입력해주세요.\n>>");
        }

        public void DungeonFail(Player player, Dungeon dungeon)
        {
            Console.WriteLine($"{dungeon.name}을 실패하였습니다.\n");
            Console.WriteLine("0. 나가기");
            Console.Write("\n원하시는 행동을 입력해주세요.\n>>");
            player.Hp /= 2;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            //bool isStart = false;
            InitializeConsole();

            GameState state = GameState.LOBBY;
            List<Item> shopItemList = new List<Item>();
            Player player = new Player(1, "스파르타", "전사", 15, 5, 400, 2000);
            ItemManager itemManager = new ItemManager();
            itemManager.InitializePlayerItem(player);
            itemManager.InitializeShop(player, shopItemList);
            DungeonManager dungeonManager = new DungeonManager();
            dungeonManager.InitializeDungeon();
            /*GameStart();

            while(!isStart)
            {
                string input = Console.ReadLine();
                switch(input)
                {
                    case "1":
                        Console.WriteLine("상태보기를 선택했습니다");
                        isStart = true;
                        player.CharacterInfo();
                        bool isStatus = true;
                        while(isStatus)
                        {
                            input = Console.ReadLine();
                            if (input == "0")
                            {
                                GameStart();
                                isStart = false;
                                isStatus = false;
                            }
                            else
                            {
                                player.CharacterInfo();
                            }
                        }
                        GameStart();
                        isStart = false;
                        break;
                    case "2":
                        Console.WriteLine("인벤토리를 선택했습니다");
                        isStart = true;
                        ItemListShow(itemList);
                        bool isItemShow = true;
                        while(isItemShow)
                        {
                            input = Console.ReadLine();
                            if(input == "0")
                            {
                                GameStart();
                                isStart = false;
                                isItemShow = false;
                            }
                            else if(input == "1")
                            {
                                isItemShow = false;
                                bool isEquipMenting = true;
                                ItemListShow(itemList, true);
                                while (isEquipMenting)
                                {
                                    input = Console.ReadLine();
                                    if(int.Parse(input) > 0 && int.Parse(input) <= itemList.Count)
                                    {
                                        itemList[int.Parse(input) - 1].isEquipping = !itemList[int.Parse(input) - 1].isEquipping;
                                        ItemEquip(player, itemList[int.Parse(input) - 1]);
                                        ItemListShow(itemList, true);
                                    }
                                    else if(int.Parse(input) == 0)
                                    {
                                        isEquipMenting = false;
                                        isItemShow = true;
                                        ItemListShow(itemList);
                                    }
                                    else
                                    {
                                        Console.WriteLine("잘못된 입력입니다");
                                        Console.Write("원하시는 행동을 입력해주세요.\n>>");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("잘못된 입력입니다");
                                Console.Write("원하시는 행동을 입력해주세요.\n>>");
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다");
                        break;
                }
            }*/

            while (state != GameState.END)
            {
                //탈출키는 따로처리 해야한다.
                switch (state)
                {
                    case GameState.LOBBY:
                        state = Lobby(player);
                        break;
                    case GameState.STATUS:
                        state = Status(player);
                        break;
                    case GameState.INVENTORY:
                        state = Inventory(player, player.haveItemList);
                        break;
                    case GameState.EQUIPMENT:
                        state = EquipMent(player, player.haveItemList);
                        break;
                    case GameState.SHOP:
                        state = Shop(player, shopItemList);
                        break;
                    case GameState.BUY:
                        state = Buy(player, shopItemList);
                        break;
                    case GameState.SELL:
                        state = Sell(player, shopItemList);
                        break;
                    case GameState.HEALING:
                        state = Healing(player);
                        break;
                    case GameState.DUNGEON:
                        state = Dungeon(player, dungeonManager);
                        break;
                    case GameState.PLAYDUNGEON:
                        state = PlayDungeon(player, dungeonManager);
                        break;
                }
            }                       
        }

        static void InitializeConsole()
        {
            Console.Title = "스파르타 TextRPG";
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetWindowSize(120, 50);
        }

        static void WriteCenter(string text, bool isWriteLine = true)
        {
            int width = Console.WindowWidth;
            int padding = (width - text.Length) / 2;
            if(padding < 0) padding = 0;
            if (isWriteLine) Console.WriteLine(new string(' ', padding) + text);
            else Console.Write(new string(' ', padding) + text);
        }
        static void MultiWriteCenter(string text, bool isWriteLine = true)
        {
            string[] texts = text.Split('\n');
            foreach (string s in texts)
            {
                WriteCenter(s, isWriteLine);
            }
        }

        static void ErrorMessage()
        {
            Console.WriteLine("\n잘못된 입력입니다");
            Console.Write("\n원하시는 행동을 입력해주세요.\n>>");
        }


        static GameState Lobby(Player player) //Main함수이름 바꾸기
        {
            GameStart();
            while(true)
            {
                string input = Console.ReadLine();
                switch(input)
                {
                    case "1":
                        return GameState.STATUS;
                    case "2":
                        return GameState.INVENTORY;
                    case "3":
                        RandomAdventure(player);
                        break;
                    case "4":
                        VillagePatrol(player);
                        break;
                    case "5":
                        Training(player);
                        break;
                    case "6":
                        return GameState.SHOP;
                    case "7":
                        return GameState.DUNGEON;
                    case "8":
                        return GameState.HEALING;
                    default:
                        ErrorMessage();
                        break;
                }
            }
        }

        static GameState Status(Player player)
        {
            player.CharacterInfo();
            string input = Console.ReadLine();
            switch(input)
            {
                case "0":
                    return GameState.LOBBY;
                default:
                    return GameState.STATUS;
                    
            }
        }

        static GameState Inventory(Player player, List<Item> items)
        {
            ItemListShow(player, items);
            while(true)
            {
                string input = Console.ReadLine();
                switch (input)
                {
                    case "0":
                        return GameState.LOBBY;
                    case "1":
                        return GameState.EQUIPMENT;
                    default:
                        ErrorMessage();
                        break;
                }
            }
        }

        static GameState EquipMent(Player player, List<Item> items)
        {
            ItemListShow(player, items, true);
            while (true)
            {
                string input = Console.ReadLine();
                if(int.TryParse(input, out int num))
                {
                    if (num == 0)
                        return GameState.INVENTORY;
                    if(num > 0 && num <= items.Count)
                    {
                        ItemEquip(player, items[num - 1]);
                        return GameState.EQUIPMENT;
                    }
                    else
                    {
                        ErrorMessage();
                    }
                }
                else
                {
                    ErrorMessage();
                }
            }
        }

        static GameState Shop(Player player, List<Item> shopItems)
        {
            ShopListShow(player, shopItems);
            Console.Write("원하시는 행동을 입력해주세요.\n>>");
            while (true)
            {
                string input = Console.ReadLine();
                switch(input)
                {
                    case "0":
                        return GameState.LOBBY;
                    case "1":
                        return GameState.BUY;
                    case "2":
                        return GameState.SELL;
                    default:
                        ErrorMessage();
                        break;
                }
            }
        }

        static GameState Buy(Player player, List<Item> shopItems)
        {
            ShopListShow(player, shopItems, true);
            Console.Write("원하시는 행동을 입력해주세요.\n>>");
            while (true)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out int num))
                {
                    if (num == 0)
                        return GameState.SHOP;
                    if (num > 0 && num <= shopItems.Count)
                    {
                        BuyItem(player, shopItems[num - 1], shopItems);
                    }
                    else
                    {
                        ErrorMessage();
                    }
                }
                else
                {
                    ErrorMessage();
                }
            }
        }

        static GameState Sell(Player player, List<Item> shopItems)
        {
            List<Item> sellLists = ShowSellItemList(player, shopItems);
            Console.Write("원하시는 행동을 입력해주세요.\n>>");
            while (true)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out int num))
                {
                    if (num == 0)
                        return GameState.SHOP;
                    if (num > 0 && num <= sellLists.Count)
                    {
                        SellItem(player, sellLists[num - 1], sellLists, shopItems);
                    }
                    else
                    {
                        ErrorMessage();
                    }
                }
                else
                {
                    ErrorMessage();
                }
            }
        }

        static GameState Healing(Player player)
        {
            HealingPopup(player);
            Console.Write("원하시는 행동을 입력해주세요.\n>>");
            while (true)
            {
                string input = Console.ReadLine();
                switch(input)
                {
                    case "0":
                        return GameState.LOBBY;
                    case "1":
                        Heal(player);
                        break;
                    default:
                        ErrorMessage();
                        break;
                }
            }
        }

        static GameState Dungeon(Player player, DungeonManager dungeonManager)
        {
            ShowDungeon(dungeonManager);
            while (true)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out int num))
                {
                    if (num == 0)
                        return GameState.LOBBY;
                    if (num > 0 && num <= dungeonManager.dungeons.Count)
                    {
                        dungeonManager.selectedDungeon = dungeonManager.dungeons[num-1];
                        return GameState.PLAYDUNGEON;
                    }
                    else
                    {
                        ErrorMessage();
                    }
                }
                else
                {
                    ErrorMessage();
                }
            }
        }

        static GameState PlayDungeon(Player player, DungeonManager dungeonManager)
        {
            DungeonResult(player, dungeonManager,dungeonManager.selectedDungeon);
            while(true)
            {
                string input = Console.ReadLine();
                switch(input)
                {
                    case "0":
                        return GameState.DUNGEON;
                    default:
                        ErrorMessage();
                        break;
                }
            }

        }
        static void GameStart()
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.\n이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");
            Console.WriteLine("1. 상태 보기\n2. 인벤토리\n3. 랜덤 모험\n" +
                "4. 마을 순찰하기\n5. 훈련하기\n6. 상점\n7. 던전입장\n8. 휴식하기\n");
            Console.Write("원하시는 행동을 입력해주세요.\n>>");
        }

        static void ItemListShow(Player player, List<Item> item, bool isEquip = false)
        {
            Console.Clear();
            player.haveItemList.Sort((x, y) => y.name.Length.CompareTo(x.name.Length));
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < item.Count; i++)
            {
                string weaponTypeStrength = (item[i].type == WEAPON_TYPE.ARMOR) ? "방어력" : "공격력";
                if(isEquip)
                {
                    string equip = (player.equipItems.Contains(item[i])) ? "[E]" : "";
                    Console.WriteLine($"- {i+1} {equip}{item[i].name}\t| {weaponTypeStrength} +{item[i].weaponStrength} | {item[i].description}");
                }
                else
                    Console.WriteLine($"- {item[i].name}\t| {weaponTypeStrength} +{item[i].weaponStrength} | {item[i].description}");
            }
            if(isEquip)
            {
                Console.WriteLine("\n0. 나가기\n");
            }
            else
                Console.WriteLine("\n1. 장착 관리\n0. 나가기\n");
            Console.Write("원하시는 행동을 입력해주세요.\n>>");
        }

        static void ItemEquip(Player player, Item item)
        {
            List<Item> list = player.equipItems;
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].type == item.type)
                    {
                        player.EquipOrUnEquip(list[i], false);
                        string name = list[i].name;
                        player.equipItems.Remove(list[i]);
                        if (name == item.name)
                            return;                   
                    }
                }
                player.EquipOrUnEquip(item, true);
                player.equipItems.Add(item);
            }
            else
            {
                player.EquipOrUnEquip(item, true);
                player.equipItems.Add(item);
            }
        }

        static void RandomAdventure(Player player)
        {
            if(player.Stamina < 10)
            {
                Console.WriteLine("\n스태미나 가 부족합니다.\n");
            }
            else
            {
                Random rndNum = new Random();
                player.Stamina -= 10;
                int num = rndNum.Next(0, 2);
                if(num == 0)
                {
                    Console.WriteLine("\n몬스터 조우! 골드 500 획득\n");
                    player.PlusMinusGold(500, true);
                }
                else
                {
                    Console.WriteLine("\n아무 일도 일어나지 않았다\n");
                }
            }
            Console.Write("원하시는 행동을 입력해주세요.\n>>");
        }

        static void VillagePatrol(Player player)
        {
            if (player.Stamina < 10)
            {
                Console.WriteLine("\n스태미나 가 부족합니다.\n");
            }
            else
            {
                Random rndNum = new Random();
                player.Stamina -= 10;
                int num = rndNum.Next(0,10);
                if(num == 0)
                {
                    Console.WriteLine("\n마을 아이들이 모여있다. 간식을 사줘볼까?\n500 G 소비\n");
                    player.PlusMinusGold(500, false);
                }
                else if(num == 1)
                {
                    Console.WriteLine("\n촌장님을 만나서 심부름을 했다.\n2000 G 획득\n");
                    player.PlusMinusGold(2000, true);
                }
                else if(num >=2 && num <=3)
                {
                    Console.WriteLine("\n길 읽은 사람을 안내해주었다.\n1000G 획득\n");
                    player.PlusMinusGold(1000, true);
                }
                else if(num >=4 && num <=6)
                {
                    Console.WriteLine("\n마을 주민과 인사를 나눴다. 선물을 받았다.\n500 G 획득\n");
                    player.PlusMinusGold(500, true);
                }
                else
                {
                    Console.WriteLine("\n아무 일도 일어나지 않았다\n");
                }
            }
            Console.Write("원하시는 행동을 입력해주세요.\n>>");
        }

        static void Training(Player player)
        {
            if (player.Stamina < 15)
            {
                Console.WriteLine("\n스태미나 가 부족합니다.\n");
            }
            else
            {
                Random rndNum = new Random();
                player.Stamina -= 15;
                int num = rndNum.Next(0, 100);
                if(num >= 0 && num < 15)
                {
                    Console.WriteLine("\n훈련이 잘 되었습니다!\n획득경험치 60\n");
                    player.PlusExp(60);
                }
                else if (num >= 15 && num < 75)
                {
                    Console.WriteLine("\n오늘하루 열심히 훈련했습니다.\n획득경험치 40\n");
                    player.PlusExp(40);
                }
                else
                {
                    Console.WriteLine("\n하기 싫다... 훈련이...\n획득경험치 30\n");
                    player.PlusExp(60);
                }
            }
            Console.Write("원하시는 행동을 입력해주세요.\n>>");
        }

        static void ShopListShow(Player player, List<Item> shopItem, bool isBuy = false)
        {
            Console.Clear();
            Console.WriteLine($"[보유 골드]\n{player.gold} G\n");
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < shopItem.Count; i++)
            {
                string weaponTypeStrength = (shopItem[i].type == WEAPON_TYPE.ARMOR) ? "방어력" : "공격력";
                string str = isBuy ? (i+1).ToString() : "";
                string gold = shopItem[i].isPurchase ? "구매완료" : shopItem[i].gold.ToString() + " G";
                Console.WriteLine($"- {str} {shopItem[i].name}\t|  {weaponTypeStrength} +" +
                    $"{shopItem[i].weaponStrength}  |  {shopItem[i].description} |  {gold}");
            }
            if (!isBuy)
                Console.Write("\n1. 아이템 구매\n2. 아이템 판매");
            Console.WriteLine("\n0. 나가기\n");
          
        }

        static void BuyItem(Player player, Item item, List<Item> shopItems)
        {
            if (item.isPurchase)
                Console.WriteLine("이미 구매한 아이템입니다.");
            else
            {
                if (item.gold <= player.gold)
                {
                    player.PlusMinusGold(item.gold, false);
                    player.haveItemList.Add(item);
                    item.isPurchase = true;
                    ShopListShow(player, shopItems, true);
                    Console.WriteLine("구매를 완료했습니다.");
                }
                else
                    Console.WriteLine("Gold가 부족합니다.");
            }
            Console.Write("\n원하시는 행동을 입력해주세요.\n>>");
        }

        static List<Item> ShowSellItemList(Player player, List<Item> shopItems)
        {
            Console.Clear();
            Console.WriteLine($"[보유 골드]\n{player.gold} G\n");
            Console.WriteLine("[아이템 목록]");
            List<Item> lists = new List<Item>();
            int num = 1;
            for(int i = 0; i < shopItems.Count; i++)
            {
                for (int j = 0; j < player.haveItemList.Count; j++) 
                {
                    if (shopItems[i].name == player.haveItemList[j].name)
                    {
                        Item item = shopItems[i];
                        string weaponTypeStrength = (item.type == WEAPON_TYPE.ARMOR) ? "방어력" : "공격력";
                        string gold =((int)(item.gold * 0.85)).ToString() + " G";
                        Console.WriteLine($"- {num} {item.name}\t|  {weaponTypeStrength}" +
                            $" +{item.weaponStrength}  |  {item.description}  |  {gold}");
                        lists.Add(item);
                        num++;
                    }
                }
            }
            Console.WriteLine("\n0. 나가기\n");
            return lists;
        }

        static void SellItem(Player player, Item item, List<Item> sellLists, List<Item> shopItems)
        {
            player.PlusMinusGold((int)(item.gold * 0.85), true);
            for (int i = 0; i < player.equipItems.Count; i++)
            {
                if (player.equipItems[i].name == item.name)
                {
                    player.EquipOrUnEquip(player.equipItems[i], false);
                }
            }
            for (int i = 0; i < player.haveItemList.Count; i++)
            {
                if (player.haveItemList[i].name == item.name)
                {
                    player.haveItemList.Remove(player.haveItemList[i]);
                }
            }
            for (int i = 0; i < shopItems.Count; i++)
            {
                if (shopItems[i].name == item.name)
                {
                    shopItems[i].isPurchase = false;
                }
            }
            ShowSellItemList(player, shopItems);
            Console.WriteLine("판매가 완료되었습니다.");
            Console.Write("\n원하시는 행동을 입력해주세요.\n>>");
        }

        static void HealingPopup(Player player)
        {
            Console.Clear();
            Console.WriteLine($"500 G 를 내면 체력을 회복할 수 있습니다.\n(보유 골드 : {player.gold} G)\n");
            Console.WriteLine("1. 휴식하기\n0. 나가기\n");
        }

        static void Heal(Player player)
        {
            if(player.gold < 500)
            {
                Console.WriteLine("\nGold가 부족합니다.");
            }
            else
            {
                player.PlusMinusGold(500, false);
                player.HealHp(100);
                player.HealStamina(20);
                HealingPopup(player);
                Console.WriteLine("휴식이 완료되었습니다.\n[체력 100][스태미나 20]이 회복됩니다.\n(단 최대치를 넘어갈 수는 없습니다.)");
            }
            Console.Write("\n원하시는 행동을 입력해주세요.\n>>");
        }

        static void ShowDungeon(DungeonManager dungeonManager)
        {
            Console.Clear();
            for (int i = 0; i < dungeonManager.dungeons.Count; i++)
            {
                Console.WriteLine($"{i+1}. {dungeonManager.dungeons[i].name}\t|  " +
                    $"방어력 {dungeonManager.dungeons[i].minDef} 이상 권정");
            }
            Console.WriteLine("0. 나가기");
            Console.Write("\n원하시는 행동을 입력해주세요.\n>>");
        }

        static void DungeonResult(Player player, DungeonManager dungeonManager, Dungeon dungeon)
        {
            Console.Clear();
            if (player.def >= dungeon.minDef)
            {
                dungeonManager.DungeonClear(player, dungeon);
            }
            else
            {
                Random rndNum = new Random();
                int result = rndNum.Next(0, 10);
                if(result < 4)
                {
                    dungeonManager.DungeonFail(player, dungeon);
                }

            }
        }
    }
}
