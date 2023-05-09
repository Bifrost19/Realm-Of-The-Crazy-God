using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquippingScript : MonoBehaviour
{
    public static Slot[] slotList = { new Slot("EquipmentSlot1", new Vector3(6.907f, 1.124f, -6.13f), "Weapon", true, "ColossusDaggerImage", "Weapon"),
                                      new Slot("EquipmentSlot2", new Vector3(8.366f, 1.124f, -6.13f), "Armor", true, "AzzureChestplateImage", "Armor"),
                                      new Slot("EquipmentSlot3", new Vector3(6.907f, -0.156f, -6.13f), "Ability", true, "CloakOfTheTormentedSoulsImage", "Ability"),
                                      new Slot("EquipmentSlot4", new Vector3(8.366f, -0.156f, -6.13f), "Ring", true, "RingOfTheCrimsonWardenImage", "Ring"),
                                      new Slot("Slot1", new Vector3(6.907f, -1.558f, -6.13f), "", false, "", ""),
                                      new Slot("Slot2", new Vector3(8.366f, -1.558f, -6.13f), "", false, "", ""),
                                      new Slot("Slot3", new Vector3(6.907f, -2.917f, -6.13f), "", false, "", ""),
                                      new Slot("Slot4", new Vector3(8.366f, -2.917f, -6.13f), "", false, "", ""),
                                      new Slot("Slot5", new Vector3(6.907f, -4.31f, -6.13f), "", false, "", ""),
                                      new Slot("Slot6", new Vector3(8.366f, -4.31f, -6.13f), "", false, "", "")};

    public static bool isThereGrabbedItem = false;
    public static GameObject grabbedItem = null;
    public static GameObject lastSlot = null;
    public static bool isThereConsumedItem = false;

    public static Slot FindSlotThroughName(string name)
    {
        for (int i = 0; i < 10; i++)
        {
            if (slotList[i].getName() == name)
                return slotList[i];
        }
        return null;
    }

    public static Slot FindISlotThroughName(string name)
    {
        for (int i = 0; i < 10; i++)
        {
            if (slotList[i].getName() == name)
                return slotList[i];
        }
        return null;
    }

    public static LootSlot FindLootSlotThroughName(string lootBagSlotName)
    {
        LootBag currBag = LootBagCheckScript.FindLootBagByName();
        for (int i = 0; i < 8; i++)
        {
            if (currBag.LootSlots[i].Name == lootBagSlotName)
                return currBag.LootSlots[i];
        }
        return null;
    }

    void DragItemOnScreen()
    {
        if(isThereGrabbedItem)
        {
            Vector3 mouseVec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grabbedItem.transform.position = new Vector3(mouseVec.x, mouseVec.y, -5f);
        }
    }

    bool IsItemTypeAppropriate(string slotName, string itemTag)
    {
        if(slotName == "EquipmentSlot1" && itemTag != "Weapon" || slotName == "EquipmentSlot2" && itemTag != "Armor" ||
           slotName == "EquipmentSlot3" && itemTag != "Ability" || slotName == "EquipmentSlot4" && itemTag != "Ring")
        {
            return false;
        }
        return true;
    }

    bool IsCurrBagEmpty()
    {
        LootBag currBag = LootBagCheckScript.FindLootBagByName();
        for (int i = 0; i < 8; i++)
        {
            if (!currBag.LootSlots[i].IsEmpty) return false; 
        }
        return true;
    }

    void CheckForConsumeWithShift()
    {
        RaycastHit hit;

        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) && Input.GetMouseButtonDown(0) &&
            hit.collider.tag == "Consumable" && Input.GetKey(KeyCode.LeftShift))
        {
            ConsumableDataBase.slotNumber = ConsumableDataBase.GetHitSlotNumber(hit.collider.name) - 3;
            ConsumableDataBase.CheckForConsume();
            isThereConsumedItem = true;
        }
    }

    void CheckForGrabbingInLootSlots()
    {
        if (!isThereGrabbedItem)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) && Input.GetMouseButtonDown(0))
            {
                if (hit.collider.transform.parent != null && hit.collider.transform.parent.name.Contains("Loot") &&
                    (hit.collider.tag == "Weapon" || hit.collider.tag == "Armor" || hit.collider.tag == "Ability" || hit.collider.tag == "Ring" || hit.collider.tag == "Consumable"))
                {    
                    
                    grabbedItem = hit.collider.gameObject;
                    GameObject itemParent = grabbedItem.transform.parent.gameObject;
                    grabbedItem.transform.parent = null;
                    grabbedItem.GetComponent<SpriteRenderer>().sortingLayerName = "OverUI";
                    Destroy(WeaponDataBase.panelBuffer);
                    LootSlot selectedSlot = FindLootSlotThroughName(itemParent.name);
                    selectedSlot.IsEmpty = true;
                    selectedSlot.ItemName = "";
                    isThereGrabbedItem = true;

                    if (IsCurrBagEmpty())
                    {
                        for (int i = 0; i < EnemyClassScript.worldItemsList.Count; i++)
                        {
                            if(LootBagCheckScript.currLootBag == EnemyClassScript.worldItemsList[i])
                            {
                                EnemyClassScript.worldItemsList.RemoveAt(i);
                                EnemyClassScript.lootBags.RemoveAt(i);
                                break;
                            }
                        }

                        Destroy(GameObject.Find(LootBagCheckScript.currLootBag.name));
                        LootBagCheckScript.DisableLootPanel();
                    }

                }
            }
        }
    }

    void CheckForGrabbing()
    {
        if (!isThereGrabbedItem)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) && Input.GetMouseButtonDown(0))
            {      
                if (hit.collider.transform.parent != null && hit.collider.transform.parent.tag == "InventorySlot" &&
                    (hit.collider.tag == "Weapon" || hit.collider.tag == "Armor" || hit.collider.tag == "Ability" || hit.collider.tag == "Ring" || hit.collider.tag == "Consumable"))
                {
                    grabbedItem = hit.collider.gameObject;
                    GameObject itemParent = grabbedItem.transform.parent.gameObject;
                    grabbedItem.transform.parent = null;
                    grabbedItem.GetComponent<SpriteRenderer>().sortingLayerName = "OverUI";
                    Destroy(WeaponDataBase.panelBuffer);
                    Slot selectedSlot = FindISlotThroughName(itemParent.name);
                    lastSlot = itemParent;

                    selectedSlot.setIsFull(false);
                    selectedSlot.setItemName("");
                    selectedSlot.setItemTag("");
                    isThereGrabbedItem = true;

                    if (hit.collider.tag == "Weapon" && selectedSlot.getName() == "EquipmentSlot1")
                    {
                        Player.setDamage(-WeaponDataBase.FindWeaponThroughName(grabbedItem.name).getDamage());
                    }
                    else if (hit.collider.tag == "Armor" && selectedSlot.getName() == "EquipmentSlot2")
                    {
                        Player.setMaxHealth(-ArmorDataBase.FindArmorThroughName(grabbedItem.name).getHealthPoints());
                        Player.setMaxMagicPoints(-ArmorDataBase.FindArmorThroughName(grabbedItem.name).getMagicPoints());
                        Player.setDefense(-ArmorDataBase.FindArmorThroughName(grabbedItem.name).getDefense());
                    }
                    else if (hit.collider.tag == "Ring" && selectedSlot.getName() == "EquipmentSlot4")
                    {
                        Player.setMaxHealth(-RingDataBase.FindRingThroughName(grabbedItem.name).getHealthPoints());
                        Player.setMaxMagicPoints(-RingDataBase.FindRingThroughName(grabbedItem.name).getMagicPoints());
                        Player.setDefense(-RingDataBase.FindRingThroughName(grabbedItem.name).getDefense());
                        Player.setAttack(-RingDataBase.FindRingThroughName(grabbedItem.name).getAttack());
                        Player.setSpeed(-RingDataBase.FindRingThroughName(grabbedItem.name).getSpeed());
                        Player.setDexterity(-RingDataBase.FindRingThroughName(grabbedItem.name).getDexterity());
                        Player.setVitality(-RingDataBase.FindRingThroughName(grabbedItem.name).getVitality());
                        Player.setWisdom(-RingDataBase.FindRingThroughName(grabbedItem.name).getWisdom());
                    }
                    else if (hit.collider.tag == "Ability" && selectedSlot.getName() == "EquipmentSlot3")
                    {
                        AbilityDataBase.currAbility = null;
                    }
                }

            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && !CameraRotationScript.isThereRotation)
            {
                PointerEventData pointer = new PointerEventData(EventSystem.current);
                pointer.position = Input.mousePosition;

                List<RaycastResult> raycastResults = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointer, raycastResults);

                if (raycastResults.Count > 0)
                {
                    foreach (var go in raycastResults)
                    {
                        if (go.gameObject.tag == "InventorySlot")
                        {
                            Slot selectedSlot = FindSlotThroughName(go.gameObject.name);

                            if (IsItemTypeAppropriate(selectedSlot.getName(), grabbedItem.tag))
                            {
                                GameObject selectedSlotGO = GameObject.Find(selectedSlot.getName());
                                grabbedItem.transform.position = selectedSlot.getPos();
                                grabbedItem.transform.SetParent(selectedSlotGO.transform);
                                grabbedItem.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                                GameObject prevGrabbedItem = grabbedItem;
                                grabbedItem.GetComponent<SpriteRenderer>().sortingLayerName = "UI";

                                bool isSelectedSlotFull = selectedSlot.isFull();

                                if (isSelectedSlotFull)
                                {
                                    grabbedItem = GameObject.Find(selectedSlot.getItemName());
                                    grabbedItem.transform.parent = null;
                                    grabbedItem.GetComponent<SpriteRenderer>().sortingLayerName = "OverUI";
                                    Destroy(WeaponDataBase.panelBuffer);
                                    isThereGrabbedItem = true;

                                }
                                else
                                {
                                    lastSlot = null;
                                    isThereGrabbedItem = false;
                                }

                                selectedSlot.setIsFull(true);
                                selectedSlot.setItemName(prevGrabbedItem.name);
                                selectedSlot.setItemTag(prevGrabbedItem.tag);

                                if (selectedSlot.getName() == "EquipmentSlot1" && WeaponDataBase.FindWeaponThroughName(prevGrabbedItem.name) != null)
                                {
                                    Player.setDamage();

                                    //Remove the buffs of the previous weapon
                                    if(isSelectedSlotFull)
                                    {
                                        Weapon currGrabbedItem = WeaponDataBase.FindWeaponThroughName(grabbedItem.name);
                                        Player.setDamage(-currGrabbedItem.getDamage());
                                    }
                                }
                                else if (selectedSlot.getName() == "EquipmentSlot2" && ArmorDataBase.FindArmorThroughName(prevGrabbedItem.name) != null)
                                {
                                    Player.setMaxHealth(ArmorDataBase.FindArmorThroughName(prevGrabbedItem.name).getHealthPoints());
                                    ///////////////////if() goes here
                                    Player.setMaxMagicPoints(ArmorDataBase.FindArmorThroughName(prevGrabbedItem.name).getMagicPoints());
                                    Player.setDefense(ArmorDataBase.FindArmorThroughName(prevGrabbedItem.name).getDefense());

                                    //Remove the buffs of the previous armor
                                    if(isSelectedSlotFull)
                                    {
                                        Armor currGrabbedItem = ArmorDataBase.FindArmorThroughName(grabbedItem.name);
                                        Player.setMaxHealth(-currGrabbedItem.getHealthPoints());
                                        Player.setMaxMagicPoints(-currGrabbedItem.getMagicPoints());
                                        Player.setDefense(-currGrabbedItem.getDefense());
                                    }
                                }
                                else if (selectedSlot.getName() == "EquipmentSlot4" && RingDataBase.FindRingThroughName(prevGrabbedItem.name) != null)
                                {
                                    Player.setMaxHealth(RingDataBase.FindRingThroughName(prevGrabbedItem.name).getHealthPoints());
                                    Player.setMaxMagicPoints(RingDataBase.FindRingThroughName(prevGrabbedItem.name).getMagicPoints());
                                    Player.setDefense(RingDataBase.FindRingThroughName(prevGrabbedItem.name).getDefense());
                                    Player.setAttack(RingDataBase.FindRingThroughName(prevGrabbedItem.name).getAttack());
                                    Player.setSpeed(RingDataBase.FindRingThroughName(prevGrabbedItem.name).getSpeed());
                                    Player.setDexterity(RingDataBase.FindRingThroughName(prevGrabbedItem.name).getDexterity());
                                    Player.setVitality(RingDataBase.FindRingThroughName(prevGrabbedItem.name).getVitality());
                                    Player.setWisdom(RingDataBase.FindRingThroughName(prevGrabbedItem.name).getWisdom());

                                    //Remove the buffs of the previous ring
                                    if(isSelectedSlotFull)
                                    {
                                        Ring currGrabbedItem = RingDataBase.FindRingThroughName(grabbedItem.name);
                                        Player.setMaxHealth(-currGrabbedItem.getHealthPoints());
                                        Player.setMaxMagicPoints(-currGrabbedItem.getMagicPoints());
                                        Player.setDefense(-currGrabbedItem.getDefense());
                                        Player.setAttack(-currGrabbedItem.getAttack());
                                        Player.setSpeed(-currGrabbedItem.getSpeed());
                                        Player.setDexterity(-currGrabbedItem.getDexterity());
                                        Player.setVitality(-currGrabbedItem.getVitality());
                                        Player.setWisdom(-currGrabbedItem.getWisdom());
                                    }
                                }
                                else if (selectedSlot.getName() == "EquipmentSlot3" && AbilityDataBase.FindAbilityThroughName(prevGrabbedItem.name) != null)
                                {
                                    AbilityDataBase.currAbility = AbilityDataBase.FindAbilityThroughName(prevGrabbedItem.name);

                                    if (isSelectedSlotFull)
                                    {
                                        //If there will be any stats for ability
                                    }
                                }

                            }

                        }
                    }
                }
            }
        }

    }

    void Update()
    {
        CheckForConsumeWithShift();
        if(isThereConsumedItem)
        {
            Invoke("CheckForGrabbing", 0.1f);
            isThereConsumedItem = false;
        }
        else CheckForGrabbing();
        CheckForGrabbingInLootSlots();
        DragItemOnScreen();
    }
}

public class Slot
{
    private string name;
    private Vector3 position;
    private string tag;
    private bool isFullV;
    private string itemName;
    private string itemTag;
    public string getName() { return this.name; }

    public Vector3 getPos() { return this.position; }

    public string getTag() { return this.tag; }

    public bool isFull() { return this.isFullV; }

    public void setIsFull(bool isFull) { this.isFullV = isFull; }

    public string getItemName() { return this.itemName; }

    public string getItemTag() { return this.itemTag; }
    public void setItemName(string itemName) { this.itemName = itemName; }

    public void setItemTag(string itemTag) { this.itemTag = itemTag; }
    public void setPos(Vector3 pos) { this.position = pos; }

    public Slot(string name, Vector3 position, string tag, bool isFull, string itemName, string itemTag)
    {
        this.name = name;
        this.position = position;
        this.tag = tag;
        this.isFullV = isFull;
        this.itemName = itemName;
        this.itemTag = itemTag;
    }
}
