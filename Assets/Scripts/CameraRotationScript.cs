using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationScript : MonoBehaviour
{
    public Transform cameraTr;
    public Transform character;
    public float rotationSpeed;
    public static Vector3 strVector = new Vector3(0, 1, 0);
    public static float prevRotation = 0f;
    public static bool isThereRotation = false;
    public static float deltaAngle = 0f;

    public static GameObject[] worldEntities;
    public static List<GameObject> worldEntitiesList = new List<GameObject>();

    private void Start()
    {
        //Get all entity gameobjects and save them in array
        worldEntities = GameObject.FindGameObjectsWithTag("WorldEntity");

        //Convert from array to list
        for (int i = 0; i < worldEntities.Length; i++)
        {
            worldEntitiesList.Add(worldEntities[i]);
        }
    }

    void Update()
    {

        if(Input.GetKey("q") || Input.GetKey("e"))
        {
            if (!isThereRotation) prevRotation = character.eulerAngles.z;

            if (Input.GetKey("q"))
            {               
                character.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
                cameraTr.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

                for (int i = 0; i < worldEntitiesList.Count; i++)
                {
                    worldEntitiesList[i].transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
                }

                for (int i = 0; i < TreeGenerationScript.allWorldOBjectsList.Count; i++)
                {
                    TreeGenerationScript.allWorldOBjectsList[i].transform.RotateAround(TreeGenerationScript.allWorldOBjectsList[i].transform.GetChild(0).position, Vector3.forward, rotationSpeed * Time.deltaTime);
                }

                if (EquippingScript.isThereGrabbedItem) 
                    EquippingScript.grabbedItem.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            }
            if (Input.GetKey("e"))
            {
                character.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
                cameraTr.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);

                for (int i = 0; i < worldEntitiesList.Count; i++)
                {
                    worldEntitiesList[i].transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
                }

                for (int i = 0; i < TreeGenerationScript.allWorldOBjectsList.Count; i++)
                {
                    TreeGenerationScript.allWorldOBjectsList[i].transform.RotateAround(TreeGenerationScript.allWorldOBjectsList[i].transform.GetChild(0).position, Vector3.back, rotationSpeed * Time.deltaTime);
                }

                if (EquippingScript.isThereGrabbedItem)
                    EquippingScript.grabbedItem.transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
            }

            deltaAngle = character.eulerAngles.z - prevRotation;
            if (deltaAngle > 180) deltaAngle -= 360;
            if (deltaAngle < -180) deltaAngle += 360;

            strVector = getStrVec();
            isThereRotation = true;

            //print(strVector);
            //print(Vector3.Angle(strVector, Vector3.right) + " | " + strVector.y);
            TreeGenerationScript.LayerTrees();
        }
        else
        {
            if(isThereRotation)
            {
                WeaponDataBase.prevRotVec = Quaternion.Euler(0, 0, deltaAngle) * WeaponDataBase.prevRotVec;
                RotateSlotPosVectors();
            }
               
            isThereRotation = false;
        }

    }

    void RotateSlotPosVectors()
    {

        for (int i = 0; i < 4; i++)
        {
            Vector3 slotVec = GameObject.Find("EquipmentSlot" + (i + 1).ToString()).transform.position;
            Vector3 offsetVec = Quaternion.Euler(0, 0, character.eulerAngles.z) * new Vector3(0.1f, -0.15f, 0);
            EquippingScript.slotList[i].setPos(new Vector3(slotVec.x, slotVec.y, -6.13f) + offsetVec);
        }
        for (int i = 4; i < 10; i++)
        {
            Vector3 slotVec = GameObject.Find("Slot" + (i - 3).ToString()).transform.position;
            Vector3 offsetVec = Quaternion.Euler(0, 0, character.eulerAngles.z) * new Vector3(0.1f, -0.15f, 0);
            EquippingScript.slotList[i].setPos(new Vector3(slotVec.x, slotVec.y, -6.13f) + offsetVec);
        }
    }

    Vector3 getStrVec()
    {
        float angle = character.eulerAngles.z / MovementScript.piRelation;
        return new Vector3(-Mathf.Sin(angle), Mathf.Cos(angle), 0);
    }
}
