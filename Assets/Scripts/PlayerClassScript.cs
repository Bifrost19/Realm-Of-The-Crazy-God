using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerClassScript : MonoBehaviour
{

    public GameObject healthBar;
    public static int healingCounter = 0;
    public static int magicPointsHealingCounter = 0;

    private void Start()
    {
        GameObject.Find("ClassText").GetComponent<Text>().text = "Class: " + Player.getCharClass();
        Player.setStats();
    }

    public void healPlayer()
    {
        if (healingCounter >= Player.getVitality() * 10)
        {
            Player.setHealth(3);
            healingCounter = 0;
        }
        healingCounter++;
    }

    public void healMagicPoints()
    {
        if (magicPointsHealingCounter >= Player.getWisdom() * 10)
        {
            Player.setMagicPoints(2);
            magicPointsHealingCounter = 0;
        }
        magicPointsHealingCounter++;
    }

    void Update()
    {
        //Passive HP & MP healing
        healPlayer();
        healMagicPoints();
    }
}

public static class Player
{
    private static string charClass = "Rogue";
    private static float health = 500;
    private static float maxHealth = 500;
    private static float magicPoints = 200;
    private static float maxMagicPoints = 200;
    private static float damage = 0;
    private static float defense = 13;
    private static float attack = 10;
    private static float speed = 25;
    private static float dexterity = 18;
    private static float vitality = 20;
    private static float wisdom = 20;

    //Getters
    public static string getCharClass() { return charClass; }

    public static float getHealth() { return health; }

    public static float getMaxHealth() { return maxHealth; }

    public static float getMagicPoints() { return magicPoints; }

    public static float getMaxMagicPoints() { return maxMagicPoints; }

    public static float getDamage() { return damage; }

    public static float getDefense() { return defense; }

    public static float getAttack() { return attack; }

    public static float getSpeed() { return speed; }

    public static float getDexterity() { return dexterity; }

    public static float getVitality() { return vitality; }

    public static float getWisdom() { return wisdom; }

    public static void setStats()
    {
        setMaxHealth();
        setMaxMagicPoints();
        setDamage();
        setDefense();
        setAttack();
        setSpeed();
        setDexterity();
        setVitality();
        setWisdom();
    }

    //Setters
    public static void setCharClass(string _charClass) { charClass = _charClass; }

    public static void setHealth(float _health)
    {
        
        if(health + _health <= 0)
        {
            health = 0;
            GameObject.Find("PlayerGreenHealthBar").GetComponent<RectTransform>().anchorMax = new Vector3(-0.063f, 0, 0);
            GameObject.Find("BigGreenHealthBar").GetComponent<RectTransform>().anchorMax = new Vector3(-0.162f, 0, 0);

            GameObject.Find("HealBarText").GetComponent<Text>().text = health + "/" + maxHealth;
        }
        else if (health + _health >= maxHealth)
        {
            
            health = maxHealth;
            GameObject.Find("PlayerGreenHealthBar").GetComponent<RectTransform>().anchorMax = new Vector3(0, 0, 0);
            GameObject.Find("BigGreenHealthBar").GetComponent<RectTransform>().anchorMax = new Vector3(0, 0, 0);

            GameObject.Find("HealBarText").GetComponent<Text>().text = health + "/" + maxHealth;
        }
        else
        {
            //Small health bar
            float scaleValue1 = (63 * _health) / (maxHealth * 1000);
            float anchorMax = GameObject.Find("PlayerGreenHealthBar").GetComponent<RectTransform>().anchorMax.x;
            GameObject.Find("PlayerGreenHealthBar").GetComponent<RectTransform>().anchorMax = new Vector3(anchorMax + scaleValue1, 0, 0);
            
            //Big health bar
            float scaleValue2 = (162 * _health) / (maxHealth * 1000);
            float bigBarAnchorMax = GameObject.Find("BigGreenHealthBar").GetComponent<RectTransform>().anchorMax.x;
            GameObject.Find("BigGreenHealthBar").GetComponent<RectTransform>().anchorMax = new Vector3(bigBarAnchorMax + scaleValue2, 0, 0);

            health += _health;
            GameObject.Find("HealBarText").GetComponent<Text>().text = health + "/" + maxHealth;
        }
    }

    public static void setMagicPoints(float _magicPoints)
    {
        if (magicPoints + _magicPoints <= 0)
        {
            magicPoints = 0;
            GameObject.Find("BigBlueMagicBar").GetComponent<RectTransform>().anchorMax = new Vector3(-0.162f, 0, 0);

            GameObject.Find("MagicBarText").GetComponent<Text>().text = magicPoints + "/" + maxMagicPoints;
        }
        else if (magicPoints + _magicPoints >= maxMagicPoints)
        {

            magicPoints = maxMagicPoints;
            GameObject.Find("BigBlueMagicBar").GetComponent<RectTransform>().anchorMax = new Vector3(0, 0, 0);

            GameObject.Find("MagicBarText").GetComponent<Text>().text = magicPoints + "/" + maxMagicPoints;
        }
        else
        {
            //Big magic bar
            float scaleValue2 = (162 * _magicPoints) / (maxMagicPoints * 1000);
            float bigBarAnchorMax = GameObject.Find("BigBlueMagicBar").GetComponent<RectTransform>().anchorMax.x;
            GameObject.Find("BigBlueMagicBar").GetComponent<RectTransform>().anchorMax = new Vector3(bigBarAnchorMax + scaleValue2, 0, 0);

            magicPoints += _magicPoints;
            GameObject.Find("MagicBarText").GetComponent<Text>().text = magicPoints + "/" + maxMagicPoints;
        }
    }

    //Overload functions to separate the outside buffs from the item buffs
    //The functions without arguments are called when an item is equipped
    //The other functions are called when the player is buffed with a stat from other source

    public static void setMaxHealth()
    {
        float prevMaxHealth = maxHealth;
        maxHealth += ArmorDataBase.FindArmorThroughName(EquippingScript.slotList[1].getItemName()).getHealthPoints();
        maxHealth += RingDataBase.FindRingThroughName(EquippingScript.slotList[3].getItemName()).getHealthPoints();
        setHealth(-health);
        setHealth(prevMaxHealth);
    }

    public static void setMaxHealth(float _maxHealth) 
    {
        maxHealth += _maxHealth;
        float prevHealth = health;
        setHealth(-health);
        setHealth(prevHealth);
    }

    public static void setMaxMagicPoints()
    {
        float prevMagicPoints = maxMagicPoints;
        maxMagicPoints += ArmorDataBase.FindArmorThroughName(EquippingScript.slotList[1].getItemName()).getMagicPoints();
        maxMagicPoints += RingDataBase.FindRingThroughName(EquippingScript.slotList[3].getItemName()).getMagicPoints();
        setMagicPoints(-magicPoints);
        setMagicPoints(prevMagicPoints);
    }

    public static void setMaxMagicPoints(float _maxMagicPoints)
    {
        //Debug.Log("MMP: " + maxMagicPoints + "|" + _maxMagicPoints);
        maxMagicPoints += _maxMagicPoints;
        float prevMagicPoints = magicPoints;
        setMagicPoints(-magicPoints);
        setMagicPoints(prevMagicPoints);
    }

    public static void setDamage()
    {
        damage += WeaponDataBase.FindWeaponThroughName(EquippingScript.slotList[0].getItemName()).getDamage();
    }

    public static void setDamage(float _damage) { damage += _damage; }

    public static void setDefense()
    {
        defense += ArmorDataBase.FindArmorThroughName(EquippingScript.slotList[1].getItemName()).getDefense();
        defense += RingDataBase.FindRingThroughName(EquippingScript.slotList[3].getItemName()).getDefense();
    }

    public static void setDefense(float _defense) { defense += _defense; }

    public static void setAttack()
    {
        attack += RingDataBase.FindRingThroughName(EquippingScript.slotList[3].getItemName()).getAttack();
    }

    public static void setAttack(float _attack) { attack += _attack; }

    public static void setSpeed()
    {
        speed += RingDataBase.FindRingThroughName(EquippingScript.slotList[3].getItemName()).getSpeed();
    }

    public static void setSpeed(float _speed) { speed += _speed; }

    public static void setDexterity()
    {
        dexterity += RingDataBase.FindRingThroughName(EquippingScript.slotList[3].getItemName()).getDexterity();
    }

    public static void setDexterity(float _dexterity) { dexterity += _dexterity; }

    public static void setVitality()
    {
        vitality += RingDataBase.FindRingThroughName(EquippingScript.slotList[3].getItemName()).getVitality();
    }

    public static void setVitality(float _vitality) { vitality += _vitality; }

    public static void setWisdom()
    {
        wisdom += RingDataBase.FindRingThroughName(EquippingScript.slotList[3].getItemName()).getWisdom();
    }

    public static void setWisdom(float _wisdom) { wisdom += _wisdom; }
}
