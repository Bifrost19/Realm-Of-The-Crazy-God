using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBagDisappearScript : MonoBehaviour
{
    public static float lootBagDestroyTime = 45f;

    private void DestroyBag()
    {
        for (int i = 0; i < EnemyClassScript.worldItemsList.Count; i++)
        {
            if (EnemyClassScript.worldItemsList[i].name == gameObject.name)
            {
                if (LootBagCheckScript.currLootBag != null &&
                    LootBagCheckScript.currLootBag.name == EnemyClassScript.worldItemsList[i].name)
                {
                    LootBagCheckScript.currLootBag = null;
                    MovementScript.isThereMovement = true;
                }
                EnemyClassScript.worldItemsList.RemoveAt(i);
                EnemyClassScript.lootBags.RemoveAt(i);
            }
        }
        Destroy(gameObject);
    }
    void Update()
    {
        Invoke("DestroyBag", lootBagDestroyTime);
    }
}
