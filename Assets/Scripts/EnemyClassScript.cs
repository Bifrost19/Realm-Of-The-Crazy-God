using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClassScript : MonoBehaviour
{
    public static List<DropRate> allEnemiesDropRates = new List<DropRate>() { new DropRate("ViolentWanderer", new List<Tuple<string, int>> { Tuple.Create("HealthPotion", 50), Tuple.Create("MagicPotion", 50), Tuple.Create("ColossusDaggerImage", 85), Tuple.Create("RingOfTheFallenVultureImage", 95) }),
                                                                              new DropRate("AncientScavenger", new List<Tuple<string, int>> { Tuple.Create("HealthPotion", 50), Tuple.Create("MagicPotion", 50), Tuple.Create("AzzureChestplateImage", 85)}) };


    public static List<GameObject> allNearbyEnemiesGOList = new List<GameObject>();
    public static List<Enemy> allNearbyEnemiesList = new List<Enemy>();
    public GameObject[] enemyParticles;
    public static Transform character;

    //Spawn variables
    public GameObject[] enemyTypes;
    public static float spawnRange = 50f;
    public static int quantityPerSpawn = 10;
    public static float spawnFrequency = 100000f;
    public static float spawnCounter = 0f;
    public static int instanceCounter = 0;

    //Drop rates
    public static int brownBagDropRate = 50;
    public static List<GameObject> worldItemsList = new List<GameObject>();
    public static List<LootBag> lootBags = new List<LootBag>();

    public static int bagIndex = 1;
    public static int nameCounter = 1;

    public static DropRate FindDropRateWithName(string name)
    {
        foreach(DropRate rate in allEnemiesDropRates)
        {
            if (rate.EnemyName == name) return rate;
        }
        return null;
    }

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
            Vector3 spawnVec = new Vector3(UnityEngine.Random.Range(charPos.x - spawnRange, charPos.x + spawnRange),
                                           UnityEngine.Random.Range(charPos.y - spawnRange, charPos.y + spawnRange),
                                           -4.48f);

            int spawnRand = UnityEngine.Random.RandomRange(0, 2);
            GameObject enemy = Instantiate(enemyTypes[spawnRand], spawnVec, Quaternion.identity);
            enemy.name = enemyTypes[spawnRand].name + instanceCounter.ToString();
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
                                                   20, 30, findParticleThroughEnemyName("ViolentWanderer"), 90));
            }
            else if(allNearbyEnemiesGOList[i].name.Contains("AncientScavenger"))
            {
                allNearbyEnemiesList.Add(new Enemy("AncientScavenger", 700, 700, 15, 75, 35, 25, allNearbyEnemiesGOList[i],
                                                   25, 45, findParticleThroughEnemyName("AncientScavenger"), 0));
            }

        }
    }

    public void Awake() //must be updated in future
    {
        SpawnEntity();
        CollectEnemyEntitiesInList();
        character = GameObject.Find("Player").transform;
    }

    public void CheckForEnemyHealth()
    {
        for (int i = 0; i < allNearbyEnemiesList.Count; i++)
        {
            if (allNearbyEnemiesList[i].getHealth() <= 0)
            {
                allNearbyEnemiesList[i].destroyEnemy();
                allNearbyEnemiesList.RemoveAt(i);
                CameraRotationScript.worldEntitiesList.RemoveAt(i);
            }

        }
    }

    public static void SpawnDrop(GameObject enemy, string enemyName)
    {
        Vector3 dropSpawnVec = enemy.transform.position;

        int fullCounter = 0;
        DropRate currDropRate = FindDropRateWithName(enemyName);
        List<LootSlot> lootSlots = new List<LootSlot>();

        foreach (Tuple<string, int> item in currDropRate.Items)
        {
            int randDropNum = UnityEngine.Random.Range(0, 100);

            if(randDropNum >= item.Item2)
            {
                lootSlots.Add(new LootSlot("LootBagSlot" + (fullCounter + 1).ToString(), item.Item1 + (nameCounter++).ToString(), false));
                fullCounter++;
            }
        }

        //Add the empty loot slots in the bags
        for (int i = fullCounter; i < 8; i++)
        {
            lootSlots.Add(new LootSlot("LootBagSlot" + (i + 1).ToString(), "", true));
        }

        if(fullCounter > 0)
        {
            GameObject brownBag = Instantiate(Resources.Load("Objects/BrownBag", typeof(GameObject)) as GameObject,
                                              dropSpawnVec, Quaternion.Euler(0, 0, character.eulerAngles.z));

            brownBag.name = "BrownBag" + bagIndex.ToString();
            bagIndex++;

            worldItemsList.Add(brownBag);
            lootBags.Add(new LootBag(brownBag.name, lootSlots));
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

public class DropRate
{
    private string enemyName;
    private List<Tuple<string, int>> items;

    public string EnemyName
    {
        get { return enemyName; }
        set { enemyName = value; }
    }

    public List<Tuple<string, int>> Items
    { get { return items; }
      set { items = value; }
    }

    public DropRate(string enemyName, List<Tuple<string, int>> items)
    {
        this.enemyName = enemyName;
        this.items = items;
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
    public float particleOrientation;

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

    public float getParticleOrientation() { return this.particleOrientation; }

    public Enemy(string name, float maxHealth, float health, float weaponRange, float damage, float dexterity, float defense,
                 GameObject gameObjectEnemy, float shootingParticleSpeed, float movementSpeed, GameObject particle,
                 float particleOrientation)
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
        this.particleOrientation = particleOrientation;
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
    public void destroyEnemy()
    {
        EnemyClassScript.SpawnDrop(enemy, name);
        GameObject.Destroy(this.enemy);
    }
}

public class LootBag
{
    private string name;
    private List<LootSlot> lootSlots = new List<LootSlot>(8);
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public List<LootSlot> LootSlots
    { 
        get { return lootSlots; }
        set { lootSlots = value; }
    }

    public LootBag(string name, List<LootSlot> lootSlots)
    {
        Name = name;
        LootSlots = lootSlots;
    }
}

public class LootSlot
{
    private string name;
    private string itemName;
    private bool isEmpty;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public string ItemName
    {
        get { return itemName; }
        set { itemName = value; }
    }
    public bool IsEmpty
    {
        get { return isEmpty; }
        set { isEmpty = value; }
    }
    public LootSlot(string name, string itemName, bool isEmpty)
    {
        Name = name;
        ItemName = itemName;
        IsEmpty = isEmpty;
    }

}
