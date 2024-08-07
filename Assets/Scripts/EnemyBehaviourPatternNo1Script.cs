using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourPatternNo1Script : MonoBehaviour
{
    public GameObject character;
    public int shootingCounter = 0;
    public Enemy currEnemy = null;

    private void Start()
    {
        character = GameObject.Find("Player");

        //Use the enemy object as enemy type data
        for (int i = 0; i < EnemyClassScript.allNearbyEnemiesList.Count; i++)
        {
            if (EnemyClassScript.allNearbyEnemiesList[i].getEnemyGO().name == this.gameObject.name)
                currEnemy = EnemyClassScript.allNearbyEnemiesList[i];
        }

    }

    public void ShootParticle()
    {
        switch(currEnemy.getShootingPattern())
        {
            case 1:
                ShootParticlePattern1();
                break;

            case 2:
                ShootParticlePattern2();
                break;
        }
    }

    public void ShootParticlePattern1()
    {

        Vector3 normVec = new Vector3(character.transform.position.x - this.gameObject.transform.position.x,
        character.transform.position.y - this.gameObject.transform.position.y,
        0);

        float angle = Vector3.Angle(normVec, CameraRotationScript.strVector);

        if (Camera.main.WorldToScreenPoint(this.gameObject.transform.position).x < Screen.width / 2)
            angle = -angle;

        GameObject particle = Instantiate(currEnemy.getParticle(), new Vector3(this.gameObject.transform.position.x,
                                              this.gameObject.transform.position.y,
                                              -4.48f),
                                              Quaternion.Euler(0, 0, this.gameObject.transform.eulerAngles.z + currEnemy.getParticleOrientation() + angle));

        particle.GetComponent<EnemyParticleMovementScript>().dirMoveVec = new Vector3(character.transform.position.x - this.gameObject.transform.position.x,
                                                                                      character.transform.position.y - this.gameObject.transform.position.y, 0);
    }

    public void ShootParticlePattern2()
    {
        Vector3 normVec = new Vector3(character.transform.position.x - this.gameObject.transform.position.x,
        character.transform.position.y - this.gameObject.transform.position.y,
        0);

        float angle = Vector3.Angle(normVec, CameraRotationScript.strVector);

        if (Camera.main.WorldToScreenPoint(this.gameObject.transform.position).x < Screen.width / 2)
            angle = -angle;

        for (int i = 0; i < 8; i++)
        {
           GameObject particle = Instantiate(currEnemy.getParticle(), new Vector3(this.gameObject.transform.position.x,
                                              this.gameObject.transform.position.y,
                                              -4.48f),
                                              Quaternion.Euler(0, 0, this.gameObject.transform.eulerAngles.z + currEnemy.getParticleOrientation() + angle + i * 45 + 180));

            particle.GetComponent<EnemyParticleMovementScript>().dirMoveVec = Quaternion.Euler(0, 0, i * 45) * new Vector3(this.gameObject.transform.position.x - character.transform.position.x,
                                                                                                                             this.gameObject.transform.position.y - character.transform.position.y, 0);
        }
    }

    public void moveEnemy()
    {
        float dX = character.transform.position.x - this.gameObject.transform.position.x;
        float dY = character.transform.position.y - this.gameObject.transform.position.y;

        this.gameObject.transform.position += new Vector3( dX , dY, 0).normalized * currEnemy.getMovementSpeed() / 10000;
    }

    void Update()
    {
        if(!AbilityDataBase.IsInvisible)
        {
            if (Vector2.Distance(this.gameObject.transform.position, character.transform.position) < 10f)
            {
                moveEnemy();

                if (shootingCounter >= currEnemy.getDexterity() * 10)
                {
                    ShootParticle();
                    shootingCounter = 0;
                }
            }

            shootingCounter++;
        }

        TreeGenerationScript.LayerMovingEntity(gameObject, 1);
    }
}
