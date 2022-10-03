using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingDataBase : MonoBehaviour
{
    public static Ring[] ringList = { new Ring("RingOfTheCrimsonWardenImage", "Ring Of The Crimson Warden", 0, 100, 0, 0, 10, 0, 0, 8, 0) };

    public static Ring FindRingThroughName(string name)
    {

        for (int i = 0; i < ringList.Length; i++)
        {
            if (ringList[i].ringImageName.Contains(name))
            {
                return ringList[i];
            }

        }
        return null;
    }

}

public class Ring
{
    public string ringImageName;
    private string separatedName;
    private int tier;
    private float healthPoints;
    private float magicPoints;
    private float attack;
    private float defense;
    private float speed;
    private float dexterity;
    private float vitality;
    private float wisdom;

    public string getRingImageName() { return this.ringImageName; }

    public string getName() { return this.separatedName; }

    public int getTier() { return this.tier; }

    public float getHealthPoints() { return this.healthPoints; }

    public float getMagicPoints() { return this.magicPoints; }

    public float getAttack() { return this.attack; }

    public float getDefense() { return this.defense; }

    public float getSpeed() { return this.speed; }

    public float getDexterity() { return this.dexterity; }

    public float getVitality() { return this.vitality; }

    public float getWisdom() { return this.wisdom; }

    public Ring(string ringImageName, string separatedName, int tier, float healthPoints, float magicPoints, float attack, float defense, float speed,
                float dexterity, float vitality, float wisdom)
    {
        this.ringImageName = ringImageName;
        this.separatedName = separatedName;
        this.tier = tier;
        this.healthPoints = healthPoints;
        this.magicPoints = magicPoints;
        this.attack = attack;
        this.defense = defense;
        this.speed = speed;
        this.dexterity = dexterity;
        this.vitality = vitality;
        this.wisdom = wisdom;
    }
}
