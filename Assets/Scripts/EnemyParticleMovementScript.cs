using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParticleMovementScript : MonoBehaviour
{
    public Vector3 characterPos;
    public Vector3 initialPos;
    public Enemy currEnemy = null;
    public Vector3 dirMoveVec;

    void Start()
    {
        characterPos = GameObject.Find("Player").transform.position;
        initialPos = this.gameObject.transform.position;

        string enemyNameThroughParticle = ParticleMovementScript1.CutParticleFromName(this.gameObject.name);

        //Use the enemy object as enemy type data
        for (int i = 0; i < EnemyClassScript.allNearbyEnemiesList.Count; i++)
        {
            if (EnemyClassScript.allNearbyEnemiesList[i].getEnemyGO().name.Contains(enemyNameThroughParticle))
                currEnemy = EnemyClassScript.allNearbyEnemiesList[i];
        }
    }

    private void OnCollisionEnter(Collision collision)
    { 
        if(collision.collider.name == "Player")
        {
            Player.setHealth(-currEnemy.getDamage() + Player.getDefense());
            Destroy(gameObject);
        }
    }

    void Update()
    {
        Vector3 mVector = dirMoveVec;
        this.gameObject.transform.position += mVector.normalized * currEnemy.getShootingParticleSpeed() / 1000;
        TreeGenerationScript.LayerMovingParticle(gameObject, 1);

        if ((this.gameObject.transform.position - initialPos).magnitude > currEnemy.getWeaponRange())
            Destroy(gameObject);
    }

}
