using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponDataBase : MonoBehaviour
{
    //All weapons in the game are here
    public static Weapon[] weaponList = { new Weapon("ColossusDaggerImage", "Colossus Dagger", 100, 8, 8, "Dagger")};
    public static int weaponListCount = 1;

    public static bool isThereCursorOnImage = false;
    public GameObject panelCanvas;
    public static GameObject panelBuffer;
    public Transform character;
    public static Vector3 prevRotVec = new Vector3(-3.3f, -1f, 0);

    public static string CutNumsFromItemImageName(string name)
    {
        int index = 0;
        for (int i = 0; i < name.Length; i++)
        {
            if (name[i] >= '0' && name[i] <= '9')
            {
                index = i;
                break;
            }
        }
        return name.Substring(0, index);
    }

    public static Weapon FindWeaponThroughName(string name)
    {
        for (int i = 0; i < weaponListCount; i++)
        {
            if (weaponList[i].weaponImageName.Contains(CutNumsFromItemImageName(name)))
                return weaponList[i];
        }
        return null;
    }

    //Armor
    void PopPanelWithName(Armor armor, GameObject panel)
    {
        panel.transform.FindChild("Image").FindChild("WeaponName").GetComponent<Text>().text = armor.getName();
        panel.transform.FindChild("Image").FindChild("TierText").GetComponent<Text>().text = "T" + armor.getTier();

        int textCounter = 0;
        panel.transform.FindChild("Image").FindChild("DamageText").GetComponent<Text>().text = "";
        panel.transform.FindChild("Image").FindChild("RangeText").GetComponent<Text>().text = "";
        panel.transform.FindChild("Image").FindChild("StatText1").GetComponent<Text>().text = "";

        if (armor.getHealthPoints() != 0)
        {
            printTextOnPanel(panel, textCounter, "HP: +" + armor.getHealthPoints());
            textCounter++;
        }
        if (armor.getMagicPoints() != 0)
        {
            printTextOnPanel(panel, textCounter, "MP: +" + armor.getMagicPoints());
            textCounter++;
        }
        if (armor.getDefense() != 0)
        {
            printTextOnPanel(panel, textCounter, "Defense: +" + armor.getDefense());
            textCounter++;
        }
    }

    //Weapon
    void PopPanelWithName(Weapon weapon, GameObject panel)
    {
        panel.transform.FindChild("Image").FindChild("WeaponName").GetComponent<Text>().text = weapon.getName();
        panel.transform.FindChild("Image").FindChild("DamageText").GetComponent<Text>().text = "Damage: " + weapon.getDamage();
        panel.transform.FindChild("Image").FindChild("RangeText").GetComponent<Text>().text = "Range: " + weapon.getWeaponRange();
        panel.transform.FindChild("Image").FindChild("TierText").GetComponent<Text>().text = "T" + weapon.getTier();
        panel.transform.FindChild("Image").FindChild("StatText1").GetComponent<Text>().text = "";
    }

    //Ability
    void PopPanelWithName(Ability ability, GameObject panel)
    {
        panel.transform.FindChild("Image").FindChild("WeaponName").GetComponent<Text>().text = ability.getSeparatedItemName();
        panel.transform.FindChild("Image").FindChild("TierText").GetComponent<Text>().text = "T" + ability.getTier();
        panel.transform.FindChild("Image").FindChild("DamageText").GetComponent<Text>().text = "Magic cost: " + ability.getMagicPointsCost();

        string abilityDurationText = "";
        if (ability.getType() == "Cloak") abilityDurationText = "Seconds invisible: " + (ability as Cloak).getDuration() + " seconds";

        panel.transform.FindChild("Image").FindChild("RangeText").GetComponent<Text>().text = abilityDurationText;
        panel.transform.FindChild("Image").FindChild("StatText1").GetComponent<Text>().text = "Cooldown: " + ability.getCooldown() + " seconds";

    }

    //Ring
    void PopPanelWithName(Ring ring, GameObject panel)
    {
        panel.transform.FindChild("Image").FindChild("WeaponName").GetComponent<Text>().text = ring.getName();
        if (ring.getTier() == 0)
        {
            panel.transform.FindChild("Image").FindChild("TierText").GetComponent<Text>().color = Color.magenta;
            panel.transform.FindChild("Image").FindChild("TierText").GetComponent<Text>().text = " UT";
        }
        else
            panel.transform.FindChild("Image").FindChild("TierText").GetComponent<Text>().text = "T" + ring.getTier();

        int textCounter = 0;
        panel.transform.FindChild("Image").FindChild("DamageText").GetComponent<Text>().text = "";
        panel.transform.FindChild("Image").FindChild("RangeText").GetComponent<Text>().text = "";
        panel.transform.FindChild("Image").FindChild("StatText1").GetComponent<Text>().text = "";

        if (ring.getHealthPoints() != 0)
        {
            printTextOnPanel(panel, textCounter, "HP: +" + ring.getHealthPoints());
            textCounter++;
        }
        if(ring.getMagicPoints() != 0)
        {
            printTextOnPanel(panel, textCounter, "MP: +" + ring.getMagicPoints());
            textCounter++;
        }
        if (ring.getDefense() != 0)
        {
            printTextOnPanel(panel, textCounter, "Defense: +" + ring.getDefense());
            textCounter++;
        }
        if (ring.getAttack() != 0)
        {
            printTextOnPanel(panel, textCounter, "Attack: +" + ring.getAttack());
            textCounter++;
        }
        if (ring.getSpeed() != 0)
        {
            printTextOnPanel(panel, textCounter, "Speed: +" + ring.getSpeed());
            textCounter++;
        }
        if (ring.getDexterity() != 0)
        {
            printTextOnPanel(panel, textCounter, "Dexterity: +" + ring.getDexterity());
            textCounter++;
        }
        if (ring.getVitality() != 0)
        {
            printTextOnPanel(panel, textCounter, "Vitality: +" + ring.getVitality());
            textCounter++;
        }
        if (ring.getWisdom() != 0)
        {
            printTextOnPanel(panel, textCounter, "Wisdom: +" + ring.getWisdom());
            textCounter++;
        }
    }

    //Consumable
    void PopPanelWithName(Consumable cons, GameObject panel)
    {
        panel.transform.FindChild("Image").FindChild("WeaponName").GetComponent<Text>().text = cons.SeparatedName;
        panel.transform.FindChild("Image").FindChild("TierText").GetComponent<Text>().text = "  C";
        panel.transform.FindChild("Image").FindChild("TierText").GetComponent<Text>().color = Color.red;
        int textCounter = 0;
        panel.transform.FindChild("Image").FindChild("DamageText").GetComponent<Text>().text = "";
        panel.transform.FindChild("Image").FindChild("RangeText").GetComponent<Text>().text = "";
        panel.transform.FindChild("Image").FindChild("StatText1").GetComponent<Text>().text = "";

        if(cons.HealthPoints != 0)
        {
            printTextOnPanel(panel, textCounter, "HP: +" + cons.HealthPoints);
            textCounter++;
        }
        if (cons.MagicPoints != 0)
        {
            printTextOnPanel(panel, textCounter, "MP: +" + cons.MagicPoints);
            textCounter++;
        }
        if (cons.Defense != 0)
        {
            printTextOnPanel(panel, textCounter, "Defense: +" + cons.Defense);
            textCounter++;
        }
        if (cons.Dexterity != 0)
        {
            printTextOnPanel(panel, textCounter, "Dexterity: +" + cons.Dexterity);
            textCounter++;
        }
        if (cons.Vitality != 0)
        {
            printTextOnPanel(panel, textCounter, "Vitality: +" + cons.Vitality);
            textCounter++;
        }
        if (cons.Speed != 0)
        {
            printTextOnPanel(panel, textCounter, "Speed: +" + cons.Speed);
            textCounter++;
        }
        if (cons.Attack != 0)
        {
            printTextOnPanel(panel, textCounter, "Attack: +" + cons.Attack);
            textCounter++;
        }
        if (cons.Wisdom != 0)
        {
            printTextOnPanel(panel, textCounter, "Wisdom: +" + cons.Wisdom);
            textCounter++;
        }
    }

    void printTextOnPanel(GameObject panel, int textCounter, string displayText)
    {
        string panelName = "";

        switch(textCounter)
        {
            case 0:
                panelName = "DamageText";
                break;

            case 1:
                panelName = "RangeText";
                break;

            case 2:
                panelName = "StatText1";
                break;
        }

        panel.transform.FindChild("Image").FindChild(panelName).GetComponent<Text>().text = displayText;
    }

    void CheckForMouseCursorOverItemImage()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) && !CameraRotationScript.isThereRotation)
        {
            if ((hit.collider.tag == "Weapon" || hit.collider.tag == "Armor" || hit.collider.tag == "Ring" || hit.collider.tag == "Ability" || hit.collider.tag == "Consumable") && !isThereCursorOnImage && !EquippingScript.isThereGrabbedItem)
            {
                Vector3 panelPosVec = hit.collider.transform.position + prevRotVec + Vector3.back;
                if (Input.mousePosition.x > Screen.width/2)
                {
                    if (Input.mousePosition.y > Screen.height / 2)
                        panelPosVec = hit.collider.transform.position + prevRotVec + Vector3.back;
                    else
                        panelPosVec = hit.collider.transform.position + prevRotVec + Vector3.back + Quaternion.Euler(0, 0, character.eulerAngles.z) * new Vector3(0, 2, 0);
                }     
                else
                {
                    if (Input.mousePosition.y > Screen.height / 2)
                        panelPosVec = hit.collider.transform.position + prevRotVec + Vector3.back;
                    else
                        panelPosVec = hit.collider.transform.position + prevRotVec + Vector3.back + Quaternion.Euler(0, 0, character.eulerAngles.z) * new Vector3(6, 2, 0);
                }
                    

                GameObject panel = Instantiate(Resources.Load("Objects/InfoPanel", typeof(GameObject)) as GameObject,
                                      panelPosVec
                                      , Quaternion.Euler(0, 0, character.eulerAngles.z));

                panel.transform.SetParent(panelCanvas.transform);
                panel.transform.localScale = new Vector3(1, 1, 1);

                if (hit.collider.tag == "Weapon")
                {
                    PopPanelWithName(FindWeaponThroughName(hit.collider.name), panel);
                }
                else if(hit.collider.tag == "Armor")
                {
                    PopPanelWithName(ArmorDataBase.FindArmorThroughName(hit.collider.name), panel);
                }
                else if(hit.collider.tag == "Ring")
                {
                    PopPanelWithName(RingDataBase.FindRingThroughName(hit.collider.name), panel);
                }
                else if(hit.collider.tag == "Ability")
                {
                    PopPanelWithName(AbilityDataBase.FindAbilityThroughName(hit.collider.name), panel);
                }
                else if (hit.collider.tag == "Consumable")
                {
                    PopPanelWithName(ConsumableDataBase.FindConsumableByName(hit.collider.name), panel);
                }

                panelBuffer = panel;
                isThereCursorOnImage = true;
            }
        }
        else
        {
            if (panelBuffer != null) Destroy(panelBuffer);
            isThereCursorOnImage = false;
        } 

    }

    void Update()
    {
        CheckForMouseCursorOverItemImage();
    }
}

public class Weapon
{
    public string weaponImageName;
    private string separatedName;
    private float damage;
    private float weaponRange;
    private int tier;
    private string weaponType;

    public string getName() { return this.separatedName; }

    public float getDamage() { return this.damage; }

    public float getWeaponRange() { return this.weaponRange; }

    public int getTier() { return this.tier; }

    public string getWeaponType() { return this.weaponType; }

    public Weapon(string weaponImageName, string separatedName, float damage, float weaponRange, int tier, string weaponType)
    {
        this.weaponImageName = weaponImageName;
        this.separatedName = separatedName;
        this.damage = damage;
        this.weaponRange = weaponRange;
        this.tier = tier;
        this.weaponType = weaponType;
    }
}
