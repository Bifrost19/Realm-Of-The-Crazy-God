using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquippingScript : MonoBehaviour
{
    public static Slot[] slotList = { new Slot("EquipmentSlot1", new Vector3(6.907f, 1.124f, -6.13f), "Weapon", true, "ColossusDaggerImage"),
                                      new Slot("EquipmentSlot2", new Vector3(8.366f, 1.124f, -6.13f), "Armor", true, "AzzureChestplateImage"),
                                      new Slot("EquipmentSlot3", new Vector3(6.907f, -0.156f, -6.13f), "Ability", true, "CloakOfTheTormentedSoulsImage"),
                                      new Slot("EquipmentSlot4", new Vector3(8.366f, -0.156f, -6.13f), "Ring", true, "RingOfTheCrimsonWardenImage"),
                                      new Slot("Slot1", new Vector3(6.907f, -1.558f, -6.13f), "", false, ""),
                                      new Slot("Slot2", new Vector3(8.366f, -1.558f, -6.13f), "", false, ""),
                                      new Slot("Slot3", new Vector3(6.907f, -2.917f, -6.13f), "", false, ""),
                                      new Slot("Slot4", new Vector3(8.366f, -2.917f, -6.13f), "", false, ""),
                                      new Slot("Slot5", new Vector3(6.907f, -4.31f, -6.13f), "", false, ""),
                                      new Slot("Slot6", new Vector3(8.366f, -4.31f, -6.13f), "", false, "")};

    public static bool isThereGrabbedItem = false;
    public static GameObject grabbedItem = null;

    public static Slot FindSlotThroughName(string name)
    {
        for (int i = 0; i < 10; i++)
        {
            if (slotList[i].getName() == name)
                return slotList[i];
        }
        return null;
    }

    public static Slot FindSlotThroughItemName(string name)
    {
        for (int i = 0; i < 10; i++)
        {
            if (slotList[i].getItemName() == name)
                return slotList[i];
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

    void CheckForGrabbing()
    {
        if (!isThereGrabbedItem)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) && Input.GetMouseButtonDown(0))
            {
                if(hit.collider.tag == "Weapon" || hit.collider.tag == "Armor" || hit.collider.tag == "Ability" || hit.collider.tag == "Ring")
                {
                    grabbedItem = hit.collider.gameObject;
                    grabbedItem.transform.parent = null;
                    grabbedItem.GetComponent<SpriteRenderer>().sortingLayerName = "OverUI";
                    Destroy(WeaponDataBase.panelBuffer);
                    Slot selectedSlot = FindSlotThroughItemName(grabbedItem.name);

                    selectedSlot.setIsFull(false);
                    selectedSlot.setItemName("");
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

                                if (selectedSlot.isFull())
                                {
                                    grabbedItem = GameObject.Find(selectedSlot.getItemName());
                                    grabbedItem.transform.parent = null;
                                    grabbedItem.GetComponent<SpriteRenderer>().sortingLayerName = "OverUI";
                                    Destroy(WeaponDataBase.panelBuffer);
                                    isThereGrabbedItem = true;

                                }
                                else isThereGrabbedItem = false;

                                selectedSlot.setIsFull(true);
                                selectedSlot.setItemName(prevGrabbedItem.name);

                                if (selectedSlot.getName() == "EquipmentSlot1" && WeaponDataBase.FindWeaponThroughName(prevGrabbedItem.name) != null)
                                {
                                    Player.setDamage();
                                }
                                else if (selectedSlot.getName() == "EquipmentSlot2" && ArmorDataBase.FindArmorThroughName(prevGrabbedItem.name) != null)
                                {
                                    Player.setMaxHealth(ArmorDataBase.FindArmorThroughName(prevGrabbedItem.name).getHealthPoints());
                                    ///////////////////if() goes here
                                    Player.setMaxMagicPoints(ArmorDataBase.FindArmorThroughName(prevGrabbedItem.name).getMagicPoints());
                                    Player.setDefense(ArmorDataBase.FindArmorThroughName(prevGrabbedItem.name).getDefense());
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
                                }
                                else if (selectedSlot.getName() == "EquipmentSlot3" && AbilityDataBase.FindAbilityThroughName(prevGrabbedItem.name) != null)
                                {
                                    AbilityDataBase.currAbility = AbilityDataBase.FindAbilityThroughName(prevGrabbedItem.name);
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
        CheckForGrabbing();
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
    public string getName() { return this.name; }

    public Vector3 getPos() { return this.position; }

    public string getTag() { return this.tag; }

    public bool isFull() { return this.isFullV; }

    public void setIsFull(bool isFull) { this.isFullV = isFull; }

    public string getItemName() { return this.itemName; }

    public void setItemName(string itemName) { this.itemName = itemName; }

    public void setPos(Vector3 pos) { this.position = pos; }

    public Slot(string name, Vector3 position, string tag, bool isFull, string itemName)
    {
        this.name = name;
        this.position = position;
        this.tag = tag;
        this.isFullV = isFull;
        this.itemName = itemName;
    }
}
