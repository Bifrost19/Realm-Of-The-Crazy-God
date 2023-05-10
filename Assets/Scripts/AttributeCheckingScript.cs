using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeCheckingScript : MonoBehaviour
{
    public static GameObject character;
    public static Vector3 weaponAttributePos = new Vector3(0.591f, 0.03f, 0);
    public static Vector3 armorAttributePos = new Vector3(0.159f, 0.049f, 0);
    public static GameObject weaponAttribute = null;
    public static GameObject armorAttribute = null;

    void Start()
    {
        character = GameObject.Find("Player");
        SpawnWeaponAttribute();
        SpawnArmorAttribute();
    }

    public static void SpawnWeaponAttribute()
    {
        if (EquippingScript.slotList[0].isFull())
        {
            string weaponName = WeaponDataBase.CutNumsFromItemImageName(EquippingScript.slotList[0].getItemName());
            weaponName = weaponName.Substring(0, weaponName.Length - 5);

            Vector3 spawnVec = character.transform.position + Quaternion.Euler(0, 0, character.transform.eulerAngles.z) * (weaponAttributePos - new Vector3(0.1f, 0.1f, 0));

            weaponAttribute = Instantiate(Resources.Load("Objects/Attributes/" + weaponName, typeof(GameObject)) as GameObject,
                                                                    new Vector3(spawnVec.x, spawnVec.y, -4.661f), Quaternion.Euler(0, 0, character.transform.eulerAngles.z));

            weaponAttribute.transform.parent = character.transform;
        }
    }

    public static void SpawnArmorAttribute()
    {
        if (EquippingScript.slotList[1].isFull())
        {
            string armorName = WeaponDataBase.CutNumsFromItemImageName(EquippingScript.slotList[1].getItemName());
            armorName = armorName.Substring(0, armorName.Length - 5);

            Vector3 spawnVec = character.transform.position + Quaternion.Euler(0, 0, character.transform.eulerAngles.z) * (armorAttributePos - new Vector3(0.1f, 0.1f, 0));

            armorAttribute = Instantiate(Resources.Load("Objects/Attributes/" + armorName, typeof(GameObject)) as GameObject,
                                                    new Vector3(spawnVec.x, spawnVec.y, -4.667f), Quaternion.Euler(0, 0, character.transform.eulerAngles.z));

            armorAttribute.transform.parent = character.transform;
        }
    }

    public static void RemoveWeaponAttribute()
    {
        Destroy(weaponAttribute);
        weaponAttribute = null;
    }

    public static void RemoveArmorAttribute()
    {
        Destroy(armorAttribute);
        armorAttribute = null;
    }
}
