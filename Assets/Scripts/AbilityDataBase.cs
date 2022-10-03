using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityDataBase : MonoBehaviour
{
    public static Ability[] allAbilitiesList = { new Cloak("CloakOfTheTormentedSoulsImage", "Cloak Of The Tormented Souls", "Cloak", 70, 5, 5, 4.5f) };
    public static bool IsInvisible = false;
    public static bool IsAbilityCast = false;

    public static Ability currAbility = null;
    public static Ability prevAbility = null;

    public static int cooldownCounter = 0;
    public static bool IsInCooldown = false;
    public static int durationCounter = 0;

    public static Ability FindAbilityThroughName(string name)
    {

        for (int i = 0; i < allAbilitiesList.Length; i++)
        {
            if (allAbilitiesList[i].getItemName().Contains(name))
            {
                return allAbilitiesList[i];
            }

        }
        return null;
    }

    void Start()
    {
        currAbility = FindAbilityThroughName(EquippingScript.slotList[2].getItemName());    
    }

    void DeleteWarningText()
    {
        GameObject.Find("WarningText").GetComponent<Text>().text = "";
    }

    bool AreThereEnoughMagicPoints()
    {
        if (Player.getMagicPoints() - currAbility.getMagicPointsCost() < 0)
            return false;
        else 
            return true;
    }

    void Update()
    {
        if(currAbility != null && !IsInCooldown && AreThereEnoughMagicPoints() && Input.GetKeyDown("space"))
        {
            print("Ability cast!"); ///
            IsInCooldown = true;
            IsAbilityCast = true;
            currAbility.useAbility();
            Player.setMagicPoints(-currAbility.getMagicPointsCost());
            prevAbility = currAbility;
            if (currAbility.getType() == "Cloak") IsInvisible = true;
        }
        else if(currAbility == null && Input.GetKeyDown("space"))
        {
            GameObject.Find("WarningText").GetComponent<Text>().text = "Equip your ability in the third grey slot";
            Invoke("DeleteWarningText", 1f);
        }
        else if(currAbility != null && !IsInCooldown && !AreThereEnoughMagicPoints() && Input.GetKeyDown("space"))
        {
            GameObject.Find("WarningText").GetComponent<Text>().text = "Not enough magic points to cast the ability";
            Invoke("DeleteWarningText", 1f);
        }

        if(currAbility != null && cooldownCounter >= currAbility.getCooldown() * 600)
        {
            IsInCooldown = false;
            cooldownCounter = 0;
        }
        cooldownCounter++;

        if(currAbility != null && durationCounter > currAbility.getDuration() * 600)
        {
            IsAbilityCast = false;
            if(currAbility.getType() == "Cloak") IsInvisible = false;
            durationCounter = 0;
            currAbility.normalizeState();
        }
        else if (IsAbilityCast && currAbility == null && durationCounter > prevAbility.getDuration() * 600)
        {
            IsAbilityCast = false;
            if (prevAbility.getType() == "Cloak") IsInvisible = false;
            durationCounter = 0;
            prevAbility.normalizeState();
        }

        if(IsAbilityCast)
        {
            durationCounter++;
        }
    }

}

public class Ability
{
    protected string itemName;
    protected string separatedItemName;
    protected string type;
    protected float magicPointsCost;
    protected int tier;
    protected float cooldown;
    protected float duration;

    public string getItemName() { return this.itemName; }

    public string getSeparatedItemName() { return this.separatedItemName; }

    public string getType() { return this.type; }

    public float getMagicPointsCost() { return this.magicPointsCost; }

    public int getTier() { return this.tier; }

    public float getCooldown() { return this.cooldown; }

    public float getDuration() { return this.duration; }

    virtual public void useAbility() { /*Default ability method */ }

    virtual public void normalizeState() { /*Default ability method */ }

    public Ability(string itemName, string separatedItemName, string type, float magicPointsCost, int tier, float cooldown, float duration)
    {
        this.itemName = itemName;
        this.separatedItemName = separatedItemName;
        this.type = type;
        this.magicPointsCost = magicPointsCost;
        this.tier = tier;
        this.cooldown = cooldown;
        this.duration = duration;
    }

    public Ability() { }
}

public class Cloak : Ability
{

    public override void useAbility()
    {
        Color bufferColor = GameObject.Find("PlayerImage").GetComponent<SpriteRenderer>().color;
        bufferColor.a = 0.15f;
        GameObject.Find("PlayerImage").GetComponent<SpriteRenderer>().color = bufferColor;
    }

    public override void normalizeState()
    {
        Color bufferColor = GameObject.Find("PlayerImage").GetComponent<SpriteRenderer>().color;
        bufferColor.a = 1f;
        GameObject.Find("PlayerImage").GetComponent<SpriteRenderer>().color = bufferColor;
    }

    public Cloak(string itemName, string separatedItemName, string type, float magicPointsCost, int tier, float cooldown, float duration)
    {
        this.itemName = itemName;
        this.separatedItemName = separatedItemName;
        this.type = type;
        this.magicPointsCost = magicPointsCost;
        this.tier = tier;
        this.cooldown = cooldown;
        this.duration = duration;
    }
}
