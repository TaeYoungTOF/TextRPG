class Program
{
    private static Player player = new();
    private static Shop shop = new();

    public enum ClassName
    {
        Warrior, Archer, Mage, Thief
    }

    public struct Item
    {
        public int itemId;
        public bool isEquipped;
        public string itemName;
        public float? itemAtk;
        public float? itemDef;
        public string itemInfo;
        public int price;

        public Item(int itemId, bool isEquipped, string itemName, float? itemAtk, float? itemDef, string itemInfo, int price)
        {
            this.itemId = itemId;
            this.isEquipped = isEquipped;
            this.itemName = itemName;
            this.itemAtk = itemAtk;
            this.itemDef = itemDef;
            this.itemInfo = itemInfo;
            this.price = price;
        }
    }

    public class Player
    {
        public int Level { get; set; }
        public string Name { get; set; }
        public ClassName PlayerClass { get; set; }
        public int Gold { get; set; }
        public int Health { get; set;}

        public float TotalAtk => basicAtk + BonusAtk;
        private float basicAtk;
        public float BonusAtk { get; set; }
        public float TotalDef => basicDef + BonusDef;
        private float basicDef;
        public float BonusDef { get; set; }

        public List<Item> Inventory { get; private set;} 

        public Player()
        {
            Level = 1;
            Name = "Chad";
            PlayerClass = ClassName.Warrior;
            Gold = 1500;
            Health = 50;

            basicAtk = 10.0f;
            basicDef = 5.0f;

            BonusAtk = 0.0f;
            BonusDef = 0.0f;

            Inventory = [];
        }

        public bool HasItem(int id) => Inventory.Any(item => item.itemId == id);
        public void AddItem(Item item)
        {
            Inventory.Add(item);
        }
        public void EquipItem(Item item)
        {
            if (item.isEquipped)
            {
                if(item.itemAtk.HasValue) { BonusAtk += (float)item.itemAtk; }
                if(item.itemDef.HasValue) { BonusDef += (float)item.itemDef; }
            }
            else
            {
                if(item.itemAtk.HasValue) { BonusAtk -= (float)item.itemAtk; }
                if(item.itemDef.HasValue) { BonusDef -= (float)item.itemDef; }
            }
        }
    }

    public class Shop
    {
        public List<Item> Product { get; private set; }

        public Shop()
        {
            Product = [];
        }

        public void AddProduct(int itemId, bool itemEquipped, string itemName, float? itemAtk, float? itemDef, string itemInfo, int price)
        {
            Product.Add(new Item(itemId, itemEquipped, itemName, itemAtk, itemDef, itemInfo, price));
        }
        public bool SellProduct(Item item)
        {
            if (player.HasItem(item.itemId))
            {
                Console.Write("이미 구매한 아이템입니다.");
                return false;
            }
            else
            {
                if (item.price <= player.Gold)
                {
                    player.AddItem(item);
                    player.Gold -= item.price;
                    return true;
                }
                else
                {
                    Console.Write("Gold 가 부족합니다.");
                    return false;
                }

            }
        }
    }

    static void CallStartWindow()
    {
        int[] idx = [1, 2, 3, 5];
        string input;

        Console.Clear();
        Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
        Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
        Console.WriteLine("1. 상태보기");
        Console.WriteLine("2. 인벤토리");
        Console.WriteLine("3. 상점");
        Console.WriteLine("5. 휴식하기");
        Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");

        do
        {
            input = Console.ReadLine();
        }
        while (!IsInputValid(input, idx));

        switch(int.Parse(input))
        {
            case 1:
                CallStatusWindow();
                break;
            case 2:
                CallInventoryWindow();
                break;
            case 3:
                CallShopWindow();
                break;
            case 5:
                CallRestWindow();
                break;
            default:
                CallStartWindow();
                break;            
        }
    }
    static void CallStatusWindow()
    {
        int[] idx = [0];
        string input;

        Console.Clear();
        Console.WriteLine("[상태 보기]");
        Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

        CallStatus();

        Console.WriteLine("0. 나가기");
        Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");

        do
        {
            input = Console.ReadLine();
        }
        while (!IsInputValid(input, idx));

        CallStartWindow();
    }

    static void CallInventoryWindow()
    {
        int[] idx = [0, 1];
        string input;

        Console.Clear();
        Console.WriteLine("인벤토리");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
        Console.WriteLine("[아이템 목록]");

        CallInventory();

        Console.WriteLine("\n1. 장착관리");
        Console.WriteLine("0. 나가기");
        Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");

        do
        {
            input = Console.ReadLine();
        }
        while (!IsInputValid(input, idx));

        switch(int.Parse(input))
        {
            case 1:
                CallEquipmentWindow();
                break;
            case 0:
                CallStartWindow();
                break;
            default:
                CallInventoryWindow();
                break;            
        }
    }

    static void CallEquipmentWindow()
    {
        int[] idx = new int[player.Inventory.Count + 1];
        for (int i = 0; i < player.Inventory.Count + 1; i++)
        {
            idx[i] = i;
        }
        string input;

        Console.Clear();
        Console.WriteLine("인벤토리 - 장착 관리");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
        Console.WriteLine("[아이템 목록]");

        CallEquipment();

        Console.WriteLine("\n0.나가기");
        Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");

        do
        {
            input = Console.ReadLine();
        }
        while (!IsInputValid(input, idx));

        int select = int.Parse(input);
        if (select == 0) { CallInventoryWindow(); }
        else {
            int index = select - 1;
            Item updated = player.Inventory[index];
            updated.isEquipped = !updated.isEquipped;
            player.Inventory[index] = updated;
            player.EquipItem(player.Inventory[index]);
            CallEquipmentWindow();
        }
    }

    static void CallShopWindow()
    {
        int[] idx = [0, 1];
        string input;

        Console.Clear();
        Console.WriteLine("상점");
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
        Console.WriteLine("[보유 골드]");
        Console.WriteLine($"{player.Gold} G\n");
        Console.WriteLine("[아이템 목록]");

        CallShop();

        Console.WriteLine("\n1. 아이템 구매");
        Console.WriteLine("0. 나가기");
        Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");

        do
        {
            input = Console.ReadLine();
        }
        while (!IsInputValid(input, idx));

        switch(int.Parse(input))
        {
            case 1:
                Console.Clear();
                CallPurchaseWindow();
                break;
            case 0:
                CallStartWindow();
                break;
            default:
                CallShopWindow();
                break;
        }
    }

    static void CallPurchaseWindow()
    {
        int[] idx = new int[shop.Product.Count + 1];
        for (int i = 0; i < shop.Product.Count + 1; i++)
        {
            idx[i] = i;
        }
        string input;

        Console.WriteLine("상점 - 아이템 구매");
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
        Console.WriteLine("[보유 골드]");
        Console.WriteLine($"{player.Gold} G\n");
        Console.WriteLine("[아이템 목록]");

        CallPurchase();

        Console.WriteLine("\n0.나가기");
        Console.Write("\n원하시는 행동을 입력해주세요.");

        while (true)
        {
            Console.Write("\n>> ");
            input = Console.ReadLine();

            if (!IsInputValid(input, idx)) { continue; }
            int select = int.Parse(input);
            if (select == 0)
            {
                CallShopWindow();
                return;
            }

             bool success = shop.SellProduct(shop.Product[select - 1]);

             if (success)
             {
                Console.Clear();
                CallPurchaseWindow();
                return;
             }
        }
    }

    static void CallRestWindow()
    {
        int[] idx = [0, 1];
        string input;

        Console.Clear();
        Console.WriteLine("[휴식하기]");
        Console.WriteLine($"500 G 를 내면 체력이 100까지 회복됩니다.");
        Console.WriteLine($"현재 체력 : {player.Health}  |  보유 골드 : {player.Gold} G\n");
        Console.WriteLine("1. 휴식하기");
        Console.WriteLine("0. 나가기");
        Console.Write("\n원하시는 행동을 입력해주세요.");

        while (true)
        {
            Console.Write("\n>> ");
            input = Console.ReadLine();

            if (!IsInputValid(input, idx)) { continue; }
            int select = int.Parse(input);
            if (select == 0)
            {
                CallStartWindow();
                return;
            }

            if (player.Health >= 100) { Console.Write("체력이 충분합니다. 휴식을 취할 필요가 없을 것 같습니다."); }
            else if (player.Gold < 500) { Console.Write("Gold 가 부족합니다."); }
            else
            {
                Console.Clear();
                player.Gold -= 500;
                player.Health = 100;

                Console.WriteLine("충분한 휴식을 취한것 같습니다.");
                Console.WriteLine($"현재 체력 : {player.Health}  |  보유 골드 : {player.Gold} G");
                Console.WriteLine("\n아무 키나 누르면 출발합니다.");
                Console.ReadKey();
                CallRestWindow();
                return;
            }
        }
    }
    
    
    static bool IsInputValid(string Input, int[] idx)
    {
        if (!int.TryParse(Input, out int value))
        {
            Console.WriteLine("잘못된 입력입니다.");
            return false;
        }

        if (!idx.Contains(value))
        {
            Console.Write("잘못된 입력입니다.\n>>");
            return false;
        }
        else
        {
            return true;
        }
    }

    static void CallStatus()
    {
        Console.WriteLine($"Lv. {player.Level}");
        Console.WriteLine($"{player.Name} ( {player.PlayerClass} )");
        Console.Write($"공격력 : {player.TotalAtk}");
        Console.WriteLine(player.BonusAtk == 0 ? "" : $" (+{player.BonusAtk})");
        Console.Write($"방어력 : {player.TotalDef}");
        Console.WriteLine(player.BonusDef == 0 ? "" : $" (+{player.BonusDef})");
        Console.WriteLine("");
        Console.Write($"체 력 : {player.Health}");
        Console.WriteLine("");
        Console.WriteLine($"Gold : {player.Gold} G\n");
    }

    static void CallInventory()
    {
        foreach (var item in player.Inventory)
        {
            Console.Write(item.isEquipped ? "[E] " : "    ");
            Console.Write($"{item.itemName, -15}");
            if (item.itemAtk != null) { Console.Write($"| 공격력 +{item.itemAtk, -5} "); }
            if (item.itemDef != null) { Console.Write($"| 방어력 +{item.itemDef, -5} "); }
            Console.WriteLine($"| {item.itemInfo}");
        }
    }
    
    static void CallEquipment()
    {
        int i = 0;
        foreach (var item in player.Inventory)
        {
            i++;
            Console.Write($"{i} ");
            Console.Write(item.isEquipped ? "[E] " : "    ");
            Console.Write($"{item.itemName, -15}");
            if (item.itemAtk != null) { Console.Write($"| 공격력 +{item.itemAtk, -5} "); }
            if (item.itemDef != null) { Console.Write($"| 방어력 +{item.itemDef, -5} "); }
            Console.WriteLine($"| {item.itemInfo}");
        }
    }

    static void CallShop()
    {
        foreach (var item in shop.Product)
        {
            Console.Write($"{item.itemName, -15}");
            if (item.itemAtk != null) { Console.Write($"| 공격력 +{item.itemAtk, -5} "); }
            if (item.itemDef != null) { Console.Write($"| 방어력 +{item.itemDef, -5} "); }
            Console.Write($"| {item.itemInfo, -30}");
            Console.WriteLine(player.HasItem(item.itemId) ? "| 구매완료" : $"| {item.price} G");
        }
    }

    static void CallPurchase()
    {
        int i = 0;
        foreach (var item in shop.Product)
        {
            i++;
            Console.Write($"{i} ");
            Console.Write($"{item.itemName, -15}");
            if (item.itemAtk != null) { Console.Write($"| 공격력 +{item.itemAtk, -5} "); }
            if (item.itemDef != null) { Console.Write($"| 방어력 +{item.itemDef, -5} "); }
            Console.Write($"| {item.itemInfo, -30}");
            Console.WriteLine(player.HasItem(item.itemId) ? "| 구매완료" : $"| {item.price} G");
        }
    }

    static void SetProduct()
    {
        shop.AddProduct(1, false, "수련자 갑옷", null, 5.0f, "수련에 도움을 주는 갑옷입니다.", 1000);
        shop.AddProduct(2, false, "무쇠 갑옷", null, 9.0f, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000);
        shop.AddProduct(3, false, "스파르타의 갑옷", null, 15.0f, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500);
        shop.AddProduct(4, false, "낡은 검", 2.0f, null, "쉽게 볼 수 있는 낡은 검입니다.", 600);
        shop.AddProduct(5, false, "청동 도끼", 5.0f, null, "어디선가 사용됐던거 같은 도끼입니다.", 1500);
        shop.AddProduct(6, false, "스파르타의 창", 7.0f, null, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 2100);
    }
    
    
    static void Main(string[] args)
    {
        SetProduct();
        CallStartWindow();
    }
}