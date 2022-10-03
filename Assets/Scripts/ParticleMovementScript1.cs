using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMovementScript1 : MonoBehaviour
{
    public static float destRange;
    public float speed;
    public Vector3 characterPos;
    public Vector3 mouseVec;

    private void Start()
    {
        destRange = WeaponDataBase.FindWeaponThroughName(CutParticleFromName(gameObject.name)).getWeaponRange();
        characterPos = GameObject.Find("Player").transform.position;
        mouseVec = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
    }
    void Update()
    {
        Vector3 mVector = new Vector3(mouseVec.x - characterPos.x, mouseVec.y - characterPos.y, 0);
        gameObject.transform.position += mVector.normalized * Time.deltaTime * speed;
        TreeGenerationScript.LayerMovingParticle(gameObject, 1);
        Vector3 moveVec = gameObject.transform.position - characterPos;
        if (moveVec.magnitude > destRange)
            Destroy(gameObject);
    }

    
    private void OnCollisionEnter(Collision collision)
    {

        if(collision.collider.tag == "WorldEntity")
        {
            Enemy currHitEnemy = null;
            for (int i = 0; i < EnemyClassScript.allNearbyEnemiesList.Count; i++)
            {
                if (EnemyClassScript.allNearbyEnemiesList[i].getEnemyGO().name == collision.collider.name)
                {
                    currHitEnemy = EnemyClassScript.allNearbyEnemiesList[i];
                    break;
                }
            }

            currHitEnemy.setHealth(-Player.getDamage() + currHitEnemy.getDefense());
            Destroy(gameObject);
        }
    }

    public static string CutParticleFromName(string name)
    {
        string name1 = name;
        //remove "Particle(Clone)"
        return name1.Remove(name.Length - 15, 15);
    }
}
