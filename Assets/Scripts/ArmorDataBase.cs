using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorDataBase : MonoBehaviour
{
    //All armors in the game are here
    public static Armor[] armorList = { new Armor("AzzureChestplateImage", "Azzure Chestplate", 12, 150, 8, 100, "LightArmor") };

    public static Armor FindArmorThroughName(string name)
    {

        for (int i = 0; i < armorList.Length; i++)
        {
            if (armorList[i].armorImageName.Contains(name))
            {
                return armorList[i];
            }

        }
        return null;
    }

}

public class Armor
{
    public string armorImageName;
    private string separatedName;
    private int defense;
    private int healthPoints;
    private int magicPoints;
    private int tier;
    private string armorType;

    public string getName() { return this.separatedName; }

    public int getDefense() { return this.defense; }

    public int getHealthPoints() { return this.healthPoints; }

    public int getTier() { return this.tier; }

    public int getMagicPoints() { return this.magicPoints; }

    public string getArmorType() { return this.armorType; }

    public Armor(string armorImageName, string separatedName, int defense, int healthPoints, int tier, int magicPoints, string armorType)
    {
        this.armorImageName = armorImageName;
        this.separatedName = separatedName;
        this.defense = defense;
        this.healthPoints = healthPoints;
        this.tier = tier;
        this.magicPoints = magicPoints;
        this.armorType = armorType;
    }
}
