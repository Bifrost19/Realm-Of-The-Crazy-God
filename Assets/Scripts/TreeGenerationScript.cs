using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGenerationScript : MonoBehaviour
{
    public static GameObject[] allWorldObjects;
    public static List<GameObject> allWorldOBjectsList = new List<GameObject>();
    public static List<GameObject> allTreesInRangeForLayering = new List<GameObject>();
    public static float layerRange = 10;

    private void Awake()
    {
        //Spawning    
    }

    void Start()
    {
        allWorldObjects = GameObject.FindGameObjectsWithTag("WorldObject");

        for (int i = 0; i < allWorldObjects.Length; i++)
        {
            allWorldOBjectsList.Add(allWorldObjects[i]);
        }

        LayerTrees();
    }

    //Layering
    public static void GetTreesInRange()
    {
        Vector3 playerPos = GameObject.Find("Player").transform.position;
        for (int i = 0; i < allWorldOBjectsList.Count; i++)
        {
            if (allWorldOBjectsList[i].transform.position.x >= playerPos.x - layerRange && allWorldOBjectsList[i].transform.position.x <= playerPos.x + layerRange &&
                allWorldOBjectsList[i].transform.position.y >= playerPos.y - layerRange && allWorldOBjectsList[i].transform.position.y >= playerPos.y - layerRange)
            {
                allTreesInRangeForLayering.Add(allWorldOBjectsList[i]);
            }
        }
    }

    public static void SortTreesByHeight()
    {
        bool areTreesSorted = false;

        while(!areTreesSorted)
        {
            int sortCounter = 0;

            for (int i = 0; i < allTreesInRangeForLayering.Count; i++)
            {
                if(i != allTreesInRangeForLayering.Count - 1 && 
                    allTreesInRangeForLayering[i].transform.position.x * CameraRotationScript.strVector.x +
                    (allTreesInRangeForLayering[i].transform.position.y) * CameraRotationScript.strVector.y - 1 >
                    allTreesInRangeForLayering[i + 1].transform.position.x * CameraRotationScript.strVector.x +
                    (allTreesInRangeForLayering[i + 1].transform.position.y) * CameraRotationScript.strVector.y - 1)
                {
                    GameObject bufferObject = allTreesInRangeForLayering[i];
                    allTreesInRangeForLayering[i] = allTreesInRangeForLayering[i + 1];
                    allTreesInRangeForLayering[i + 1] = bufferObject;
                    sortCounter++;
                }
            }

            if (sortCounter == 0)
                areTreesSorted = true;
        }
    }

    public static void LayerTrees()
    {
        allTreesInRangeForLayering.Clear();
        GetTreesInRange();
        SortTreesByHeight();

        //Layer all world items(bags and other items)
        for (int i = 0; i < EnemyClassScript.worldItemsList.Count; i++)
        {
            LayerMovingEntity(EnemyClassScript.worldItemsList[i], 1);
        }

        Vector3 playerPos = GameObject.Find("Player").transform.position;
        int treesUnderPlayerPos = 0;

        //Find how many trees are under the player position
        for (int i = 0; i < allTreesInRangeForLayering.Count; i++)
        {
            if (allTreesInRangeForLayering[i].transform.position.x * CameraRotationScript.strVector.x +
                    (allTreesInRangeForLayering[i].transform.position.y) * CameraRotationScript.strVector.y - 1 >
                    playerPos.x * CameraRotationScript.strVector.x +
                    playerPos.y * CameraRotationScript.strVector.y)
            {
                break;
            }
            else treesUnderPlayerPos++;
        }

        int layerCounter = 1;

        //The trees under the player
        for (int i = treesUnderPlayerPos - 1; i >= 0; i--)
        {
            allTreesInRangeForLayering[i].GetComponent<SpriteRenderer>().sortingOrder = layerCounter;
            layerCounter += 2;
        }

        layerCounter = -1;

        //The trees over the player
        for (int i = treesUnderPlayerPos; i < allTreesInRangeForLayering.Count; i++)
        {
            allTreesInRangeForLayering[i].GetComponent<SpriteRenderer>().sortingOrder = layerCounter;
            layerCounter -= 2;
        }

    }

    public static void LayerMovingEntity(GameObject mObject, float heightVar)
    {
        for (int i = 0; i < allTreesInRangeForLayering.Count; i++)
        {
            if(mObject.transform.position.x * CameraRotationScript.strVector.x +
               mObject.transform.position.y * CameraRotationScript.strVector.y <
               allTreesInRangeForLayering[i].transform.position.x * CameraRotationScript.strVector.x +
               allTreesInRangeForLayering[i].transform.position.y * CameraRotationScript.strVector.y - heightVar)
            {
                mObject.GetComponent<SpriteRenderer>().sortingOrder = allTreesInRangeForLayering[i].GetComponent<SpriteRenderer>().sortingOrder + 1;
                if (mObject.transform.childCount > 0) mObject.transform.GetChild(0).GetComponent<Canvas>().sortingOrder = allTreesInRangeForLayering[i].GetComponent<SpriteRenderer>().sortingOrder + 1;
                return;
            }
        }

        //If the gameobject's position is higher than every tree in the list
        if(allTreesInRangeForLayering.Count > 0)
        {
            mObject.GetComponent<SpriteRenderer>().sortingOrder = allTreesInRangeForLayering[allTreesInRangeForLayering.Count - 1].GetComponent<SpriteRenderer>().sortingOrder - 1;
            if (mObject.transform.childCount > 0) 
            {
                mObject.transform.GetChild(0).GetComponent<Canvas>().sortingOrder = allTreesInRangeForLayering[allTreesInRangeForLayering.Count - 1].GetComponent<SpriteRenderer>().sortingOrder - 1;
            }
        }
        else
        {
            mObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
            if (mObject.transform.childCount > 0) mObject.transform.GetChild(0).GetComponent<Canvas>().sortingOrder = 0;
        }

    }

    public static void LayerMovingParticle(GameObject mParticle, float heightVar)
    {
        for (int i = 0; i < allTreesInRangeForLayering.Count; i++)
        {
            if (mParticle.transform.position.x * CameraRotationScript.strVector.x +
               mParticle.transform.position.y * CameraRotationScript.strVector.y <
               allTreesInRangeForLayering[i].transform.position.x * CameraRotationScript.strVector.x +
               allTreesInRangeForLayering[i].transform.position.y * CameraRotationScript.strVector.y - heightVar)
            {
                mParticle.GetComponent<SpriteRenderer>().sortingOrder = allTreesInRangeForLayering[i].GetComponent<SpriteRenderer>().sortingOrder + 1;
                return;
            }
        }

        //If the gameobject's position is higher than every tree in the list

        if (allTreesInRangeForLayering.Count > 0)
            mParticle.GetComponent<SpriteRenderer>().sortingOrder = allTreesInRangeForLayering[allTreesInRangeForLayering.Count - 1].GetComponent<SpriteRenderer>().sortingOrder - 1;
        else
            mParticle.GetComponent<SpriteRenderer>().sortingOrder = 0;
    }

    void Update()
    {
        
    }
}
