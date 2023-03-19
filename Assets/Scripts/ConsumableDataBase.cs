using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConsumableDataBase : MonoBehaviour
{
    public static List<Consumable> allConsumables = new List<Consumable>() { new Consumable("HealthPotion", "Health Potion", 100, 0, 0, 0, 0, 0, 0, 0),
                                                                             new Consumable("MagicPotion", "Magic Potion", 0, 100, 0, 0, 0, 0, 0, 0)};
    public static Consumable FindConsumableByName(string name)
    {
        for (int i = 0; i < allConsumables.Count; i++)
        {
            if (name.Contains(allConsumables[i].Name)) return allConsumables[i];
        }
        return null;
    }

    public static void GetBuffsFromConsumables(Consumable cons)
    {
        Player.setHealth(cons.HealthPoints);
        Player.setMagicPoints(cons.MagicPoints);
        Player.setDefense(cons.Defense);
        Player.setDexterity(cons.Dexterity);
        Player.setVitality(cons.Vitality);
        Player.setSpeed(cons.Speed);
        Player.setAttack(cons.Attack);
        Player.setWisdom(cons.Wisdom);
    }

    void CheckForConsume()
    {
        int slotNumber = 0;
        if (Input.GetKeyDown(KeyCode.Alpha1)) slotNumber = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) slotNumber = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) slotNumber = 3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) slotNumber = 4;
        if (Input.GetKeyDown(KeyCode.Alpha5)) slotNumber = 5;
        if (Input.GetKeyDown(KeyCode.Alpha6)) slotNumber = 6;
     
        if (slotNumber != 0 && EquippingScript.slotList[slotNumber + 3].isFull() &&
            EquippingScript.slotList[slotNumber + 3].getItemTag() == "Consumable")
        {
            Slot currSlot = EquippingScript.slotList[slotNumber + 3];
            GetBuffsFromConsumables(FindConsumableByName(currSlot.getItemName()));
            Destroy(GameObject.Find(currSlot.getItemName()));
            currSlot.setIsFull(false);
            currSlot.setItemName("");
            currSlot.setItemTag("");
        }
    }

    public static string CutNumbersFromItemName(string itemName)
    {
        int index = 0;
        for (int i = 0; i < itemName.Length; i++)
        {
            if (itemName[i] >= '0' && itemName[i] <= '9')
            {
                index = i;
                break;
            }
        }
        return (index != 0)? itemName.Substring(0, index) : itemName;
    }

    void CheckForDroppingOnGround()
    {
        if(EquippingScript.isThereGrabbedItem && Input.GetMouseButtonDown(0))
        {
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);

            if (raycastResults.Count == 0)
            {
                GameObject brownBag = Instantiate(Resources.Load("Objects/BrownBag", typeof(GameObject)) as GameObject,
                                 EnemyClassScript.character.position, Quaternion.Euler(0, 0, EnemyClassScript.character.eulerAngles.z));

                brownBag.name = "BrownBag" + EnemyClassScript.bagIndex.ToString();
                EnemyClassScript.bagIndex++;

                EnemyClassScript.worldItemsList.Add(brownBag);
                List<LootSlot> lootSlots = new List<LootSlot>{ new LootSlot("LootBagSlot1", EquippingScript.grabbedItem.name, false),
                                         new LootSlot("LootBagSlot2", "", true),
                                         new LootSlot("LootBagSlot3", "", true),
                                         new LootSlot("LootBagSlot4", "", true),
                                         new LootSlot("LootBagSlot5", "", true),
                                         new LootSlot("LootBagSlot6", "", true),
                                         new LootSlot("LootBagSlot7", "", true),
                                         new LootSlot("LootBagSlot8", "", true)};
                EnemyClassScript.lootBags.Add(new LootBag(brownBag.name, lootSlots));
                Destroy(EquippingScript.grabbedItem);
                EquippingScript.grabbedItem = null;
                EquippingScript.isThereGrabbedItem = false;
            }
        }
    }


    private void Update()
    {
        CheckForConsume();
        CheckForDroppingOnGround();
    }
}

public class Consumable
{
    private string name;
    private string separatedName;
    private int healthPoints;
    private int magicPoints;
    private int defense;
    private int dexterity;
    private int vitality;
    private int speed;
    private int attack;
    private int wisdom;

    public string Name
    { get { return name; }
      set { name = value; }
    }
    public string SeparatedName
    {
        get { return separatedName; }
        set { separatedName = value; }
    }
    public int HealthPoints
    {
        get { return healthPoints; }
        set { healthPoints = value; }
    }
    public int MagicPoints
    {
        get { return magicPoints; }
        set { magicPoints = value; }
    }
    public int Defense
    {
        get { return defense; }
        set { defense = value; }
    }
    public int Dexterity
    {
        get { return dexterity; }
        set { dexterity = value; }
    }
    public int Vitality
    {
        get { return vitality; }
        set { vitality = value; }
    }
    public int Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    public int Attack
    {
        get { return attack; }
        set { attack = value; }
    }
    public int Wisdom
    {
        get { return wisdom; }
        set { wisdom = value; }
    }

    public Consumable(string name, string separatedName, int healthPoints, int magicPoints, int defense, int dexterity, int vitality, int speed, int attack, int wisdom)
    {
        Name = name;
        SeparatedName = separatedName;
        HealthPoints = healthPoints;
        MagicPoints = magicPoints;
        Defense = defense;
        Dexterity = dexterity;
        Vitality = vitality;
        Speed = speed;
        Attack = attack;
        Wisdom = wisdom;
    }
}
