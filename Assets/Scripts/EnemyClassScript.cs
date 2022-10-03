using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClassScript : MonoBehaviour
{

    public static List<GameObject> allNearbyEnemiesGOList = new List<GameObject>();
    public static List<Enemy> allNearbyEnemiesList = new List<Enemy>();
    public GameObject[] enemyParticles;

    //Spawn variables
    public GameObject[] enemyTypes;
    public static float spawnRange = 50f;
    public static int quantityPerSpawn = 10;
    public static float spawnFrequency = 100000f;
    public static float spawnCounter = 0f;
    public static int instanceCounter = 0;

    public GameObject findParticleThroughEnemyName(string name)
    {
        for (int i = 0; i < enemyParticles.Length; i++)
        {
            if (enemyParticles[i].name.Contains(name))
                return enemyParticles[i];
        }
        return null;
    }

    public void SpawnEntity()
    {
        Vector3 charPos = this.gameObject.transform.position;

        for (int i = 0; i < quantityPerSpawn; i++)
        {
            Vector3 spawnVec = new Vector3(Random.Range(charPos.x - spawnRange, charPos.x + spawnRange),
                                           Random.Range(charPos.y - spawnRange, charPos.y + spawnRange),
                                           -4.48f);
            GameObject enemy = Instantiate(enemyTypes[0], spawnVec, Quaternion.identity);
            enemy.name = enemyTypes[0].name + instanceCounter.ToString();
            instanceCounter++;
        }
    }

    public void CollectEnemyEntitiesInList()
    {
        GameObject[] allNearbyEnemies;

        allNearbyEnemies = GameObject.FindGameObjectsWithTag("WorldEntity");

        //Transform array to list for more convinient work
        for (int i = 0; i < allNearbyEnemies.Length; i++)
        {
            allNearbyEnemiesGOList.Add(allNearbyEnemies[i].gameObject);
        }

        for (int i = 0; i < allNearbyEnemiesGOList.Count; i++)
        {
            if (allNearbyEnemiesGOList[i].name.Contains("ViolentWanderer"))
            {
                allNearbyEnemiesList.Add(new Enemy("ViolentWanderer", 1000, 1000, 12, 50, 25, 20, allNearbyEnemiesGOList[i],
                                                   20, 30, findParticleThroughEnemyName("ViolentWanderer")));
            }

        }
    }

    public void Awake() //must be updated in future
    {
        SpawnEntity();
        CollectEnemyEntitiesInList();
    }

    public void CheckForEnemyHealth()
    {
        for (int i = 0; i < allNearbyEnemiesList.Count; i++)
        {
            if (allNearbyEnemiesList[i].getHealth() <= 0)
            {
                allNearbyEnemiesList[i].destoyEnemy();
                allNearbyEnemiesList.RemoveAt(i);
                CameraRotationScript.worldEntitiesList.RemoveAt(i);
            }

        }
    }

    void Update()
    {
        CheckForEnemyHealth();

        //Spawning---
        //if(spawnCounter >= spawnFrequency)
        //{
        //    SpawnEntity();
        //    spawnCounter = 0;
        //}
        //spawnCounter++;
    }
}

public class Enemy
{
    private string name;
    private float maxHealth;
    private float health;
    private float weaponRange;
    private float damage;
    private float dexterity;
    private float defense;
    private GameObject enemy;
    public float shootingParticleSpeed;
    public float movementSpeed;
    public GameObject particle;

    public string getName() { return this.name; }

    public float getMaxHealth() { return this.maxHealth; }

    public float getHealth() { return this.health; }

    public float getWeaponRange() { return this.weaponRange; }

    public float getDamage() { return this.damage; }

    public float getDexterity() { return this.dexterity; }

    public float getDefense() { return this.defense; }

    public GameObject getEnemyGO() { return this.enemy; }

    public float getShootingParticleSpeed() { return this.shootingParticleSpeed; }

    public float getMovementSpeed() { return this.movementSpeed; }

    public GameObject getParticle() { return this.particle; }

    public Enemy(string name, float maxHealth, float health, float weaponRange, float damage, float dexterity, float defense,
                 GameObject gameObjectEnemy, float shootingParticleSpeed, float movementSpeed, GameObject particle)
    {
        this.name = name;
        this.maxHealth = maxHealth;
        this.health = health;
        this.weaponRange = weaponRange;
        this.damage = damage;
        this.dexterity = dexterity;
        this.defense = defense;
        this.enemy = gameObjectEnemy;
        this.shootingParticleSpeed = shootingParticleSpeed;
        this.movementSpeed = movementSpeed;
        this.particle = particle;
    }

    //Setters
    public void setHealth(float health)
    {
        if (this.health + health <= 0)
        {
            this.health = 0;
            this.enemy.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().anchorMax = new Vector3(-0.055888f, 0, 0);
        }
        else
        {
            float scaleValue = (10 * health) / (this.maxHealth * 10000);

            float anchorMax = this.enemy.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().anchorMax.x;
            this.enemy.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().anchorMax = new Vector3(anchorMax + scaleValue, 0, 0);
            this.health += health;
        }
    }

    //Other methods
    public void destoyEnemy()
    {
       GameObject.Destroy(this.enemy);
    }
}
