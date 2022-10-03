using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShootingScript : MonoBehaviour
{
    public static bool isWeaponEquipped = true;
    public Transform character;
    public GameObject equippedWeapon;
    public static int counter = 0;
    public static bool isRotating = false;

    GameObject chooseParticle(string weaponName)
    {
        switch (weaponName)
        {
            case "ColossusDagger":      
            return Resources.Load("Objects/Particles/Weapons/ColossusDaggerParticle", typeof(GameObject)) as GameObject;    
        default:
                return null;
        }
    }

    void ShootParticles(GameObject particle)
    {
        Vector3 mouseVec = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                        Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        Vector3 charPos = character.position;
        Vector3 shootVector = (mouseVec - charPos);
        float angle = Mathf.Acos(Vector3.Dot(CameraRotationScript.strVector, shootVector) / (shootVector.magnitude * CameraRotationScript.strVector.magnitude));

        if (Camera.main.WorldToScreenPoint(mouseVec).x > Screen.width / 2)
            angle = -angle;

        Instantiate(particle, new Vector3(character.position.x, character.position.y, character.position.z - 4.48f), Quaternion.Euler(0,0, character.eulerAngles.z + 90 + angle * MovementScript.piRelation));
    }

    void DeleteWarningText()
    {
        GameObject.Find("WarningText").GetComponent<Text>().text = "";
    }

    void Update()
    {
        isWeaponEquipped = EquippingScript.FindSlotThroughName("EquipmentSlot1").isFull();

        bool isThereUIHit = false;

        if (EventSystem.current.IsPointerOverGameObject()) isThereUIHit = true;
        
        if (Input.GetMouseButton(0) && counter > (2000 / Player.getDexterity()) && isWeaponEquipped && !isThereUIHit)
        {
            counter = 0;
            isRotating = true;
            ShootParticles(chooseParticle(equippedWeapon.name));
        }
        else if(!isWeaponEquipped && Input.GetMouseButtonDown(0) && !isThereUIHit)
        {
            //print("You have to equip a weapon first!");
            GameObject.Find("WarningText").GetComponent<Text>().text = "Equip your weapon in the first grey slot!";
            Invoke("DeleteWarningText", 1f);
        }
        counter++;
    }
}
