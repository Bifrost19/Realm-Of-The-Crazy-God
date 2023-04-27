using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBagCheckScript : MonoBehaviour
{
    //this.gameObject = player
    public static GameObject lootPanel;
    public static GameObject currLootBag = null;
    public static bool isBagFirstTime = true;


    public static List<Vector3> lootBagPosVectors = new List<Vector3> { new Vector3(-7.96f, -3.07f, -6.2f),
                                                                        new Vector3(-6.6f, -3.07f, -6.2f),
                                                                        new Vector3(-5.24f, -3.07f, -6.2f),
                                                                        new Vector3(-3.9f, -3.07f, -6.2f),
                                                                        new Vector3(-8.01f, -4.13f, -6.2f),
                                                                        new Vector3(-6.63f, -4.13f, -6.2f),
                                                                        new Vector3(-5.23f, -4.13f, -6.2f),
                                                                        new Vector3(-3.91f, -4.13f, -6.2f)};


    public static void DisableLootPanel()
    {
        int childCount = lootPanel.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {           
            if (lootPanel.transform.GetChild(i).childCount > 0)
            {
                Destroy(lootPanel.transform.GetChild(i).GetChild(0).gameObject);
            }
        }
        lootPanel.SetActive(false);
        isBagFirstTime = false;
    }

    public static LootBag FindLootBagByName()
    {
        for (int i = 0; i < EnemyClassScript.lootBags.Count; i++)
        {
            if (EnemyClassScript.lootBags[i].Name == currLootBag.name)
                return EnemyClassScript.lootBags[i];
        }
        return null;
    }
    static void EnableLootPanel()
    {
        LootBag currBag = FindLootBagByName();
        for (int i = 0; i < 8; i++)
        {
            if(!currBag.LootSlots[i].IsEmpty)
            {
                string itemName = currBag.LootSlots[i].ItemName;
                GameObject buffer = Resources.Load("Objects/PixelArt/Potions/" + ConsumableDataBase.CutNumbersFromItemName(itemName), typeof(GameObject)) as GameObject;
                if (buffer != null) buffer.transform.localScale = new Vector3(1.28f, 1.35f, 0);
                if (buffer == null)
                {
                    buffer = Resources.Load("Objects/PixelArt/Weapons/" + ConsumableDataBase.CutNumbersFromItemName(itemName), typeof(GameObject)) as GameObject;
                    if(buffer != null)
                    {
                        buffer.transform.localScale = new Vector3(2.46f, 1.98f, 0);
                        buffer.GetComponent<BoxCollider>().size = new Vector3(0.41f, 0.52f, 0.005f);
                    }      
                }
                if (buffer == null)
                {
                    buffer = Resources.Load("Objects/PixelArt/Armors/" + ConsumableDataBase.CutNumbersFromItemName(itemName), typeof(GameObject)) as GameObject;
                    if (buffer != null)
                    {
                        buffer.transform.localScale = new Vector3(1.33f, 1.72f, 0);
                        buffer.GetComponent<BoxCollider>().size = new Vector3(0.41f, 0.52f, 0.005f);
                    }
                }
                if (buffer == null)
                {
                    buffer = Resources.Load("Objects/PixelArt/Abilities/" + ConsumableDataBase.CutNumbersFromItemName(itemName), typeof(GameObject)) as GameObject;
                    if (buffer != null)
                    {
                        buffer.transform.localScale = new Vector3(1.58f, 1.6f, 0);
                        buffer.GetComponent<BoxCollider>().size = new Vector3(0.41f, 0.52f, 0.005f);
                    }
                }
                if (buffer == null)
                {
                    buffer = Resources.Load("Objects/PixelArt/Rings/" + ConsumableDataBase.CutNumbersFromItemName(itemName), typeof(GameObject)) as GameObject;
                    if (buffer != null)
                    {
                        buffer.transform.localScale = new Vector3(1.85f, 1.77f, 0);
                        buffer.GetComponent<BoxCollider>().size = new Vector3(0.41f, 0.52f, 0.005f);
                    }
                }
                GameObject item = Instantiate(buffer, lootBagPosVectors[i], Quaternion.Euler(0, 0, EnemyClassScript.character.eulerAngles.z));
                item.name = currBag.LootSlots[i].ItemName;
                item.transform.parent = lootPanel.transform.GetChild(i);
            }
        }
        lootPanel.SetActive(true);
        MovementScript.isThereMovement = false;
        isBagFirstTime = true;
    }

    public static void CheckIfPlayerIsNearBag()
    {
        Vector3 playerPos = EnemyClassScript.character.transform.position;
        int bagIndex = 0;
        bool isThereBag = false;
        DisableLootPanel();

        for (int i = 0; i < EnemyClassScript.worldItemsList.Count; i++)
        {
            GameObject curr = EnemyClassScript.worldItemsList[i];
            if (Mathf.Abs(curr.transform.position.x - playerPos.x) < 1 &&
                Mathf.Abs(curr.transform.position.y - playerPos.y) < 1)
            {
                currLootBag = curr;
                bagIndex = i;
                isThereBag = true;
                break;
            }
        }

        for (int i = bagIndex + 1; i < EnemyClassScript.worldItemsList.Count; i++)
        {
            GameObject curr = EnemyClassScript.worldItemsList[i];
            if (Mathf.Abs(curr.transform.position.x - playerPos.x) < 1 &&
                Mathf.Abs(curr.transform.position.y - playerPos.y) < 1 &&
                (playerPos - currLootBag.transform.position).magnitude >=
                (playerPos - curr.transform.position).magnitude)
            {
                isThereBag = true;
                currLootBag = curr;
            }
        }
        
        if (isThereBag && !isBagFirstTime && !CameraRotationScript.isThereRotation)
        {
            EnableLootPanel();
        }
        else if (!isThereBag && isBagFirstTime)
        {
            DisableLootPanel();
            currLootBag = null;
        }

    }

    public void Awake()
    {
        lootPanel = GameObject.Find("MainCanvas").transform.Find("LootPanel").gameObject;
    }

    void FixedUpdate()
    {
        CheckIfPlayerIsNearBag();
    }
}
