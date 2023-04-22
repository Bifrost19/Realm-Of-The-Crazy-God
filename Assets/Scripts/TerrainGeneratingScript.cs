using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneratingScript : MonoBehaviour
{
    public static List<GeneratedTile> allGeneratedTiles = new List<GeneratedTile>();
    public static int generateRange = 3; //when 3 - The count of the tiles is between 36 and 48
    public static int tileScale = 5;
    public static GeneratedTile currClosestGeneratedTile;
    public static GeneratedTile prevClosestGeneratedTile;
    public static int treeDensity = 2;

    public static bool IsFirstTime = true;

    void Start()
    {
        for (int i = -tileScale * generateRange; i < tileScale * generateRange; i += tileScale)
        {
            for (int j = - tileScale * generateRange; j < tileScale * generateRange; j+=tileScale)
            {
                GameObject tile = Instantiate(Resources.Load("Objects/PixelArt/Terrains/GrassTerrainGO", typeof(GameObject)) as GameObject,
                            new Vector3( j, i, -4f), Quaternion.identity);
                allGeneratedTiles.Add(new GeneratedTile(j, i, "Grass", tile));
            }
        }
        
        currClosestGeneratedTile = allGeneratedTiles[FindTheClosestTileToPlayer()];
        prevClosestGeneratedTile = currClosestGeneratedTile;

    }

    bool IsTileInList(float xCoord, float yCoord)
    {
        for (int i = 0; i < allGeneratedTiles.Count; i++)
        {
            if (allGeneratedTiles[i].getXCoord() == xCoord &&
               allGeneratedTiles[i].getYCoord() == yCoord)
                return true;
        }
        return false;
    }

    void RemoveTilesOutOfRange()
    {
        float allowedRange = (2 * generateRange + 1) * tileScale / 2;

        //Remove process
        for (int i = 0; i < allGeneratedTiles.Count; i++)
        {
            if(allGeneratedTiles[i].getXCoord() > currClosestGeneratedTile.getXCoord() + allowedRange ||
               allGeneratedTiles[i].getXCoord() < currClosestGeneratedTile.getXCoord() - allowedRange ||
               allGeneratedTiles[i].getYCoord() > currClosestGeneratedTile.getYCoord() + allowedRange ||
               allGeneratedTiles[i].getYCoord() < currClosestGeneratedTile.getYCoord() - allowedRange)
            {
                Destroy(allGeneratedTiles[i].getTileObject());
                allGeneratedTiles.RemoveAt(i);
                i--;
            }
        }
    }

    bool IsThereTreeOnTile(float xStart, float xEnd, float yStart, float yEnd)
    {
        for (int i = 0; i < TreeGenerationScript.allWorldOBjectsList.Count; i++)
        {
            GameObject curr = TreeGenerationScript.allWorldOBjectsList[i];
            if (curr.transform.position.x >= xStart && curr.transform.position.x <= xEnd &&
                curr.transform.position.y >= yStart && curr.transform.position.y <= yEnd)
                return true;
        }
        return false;
    }

    void GenerateTiles(GeneratedTile currentTile)
    {

        for (int i = Mathf.RoundToInt(currClosestGeneratedTile.getYCoord()) - tileScale * generateRange; i < Mathf.RoundToInt(currClosestGeneratedTile.getYCoord()) + tileScale * generateRange; i += tileScale)
        {
            for (int j = Mathf.RoundToInt(currClosestGeneratedTile.getXCoord()) - tileScale * generateRange; j < Mathf.RoundToInt(currClosestGeneratedTile.getXCoord()) + tileScale * generateRange; j += tileScale)
            {
                if(!IsTileInList(j, i))
                {
                    GameObject tile = Instantiate(Resources.Load("Objects/PixelArt/Terrains/GrassTerrainGO", typeof(GameObject)) as GameObject,
                                      new Vector3(j, i, -4f), Quaternion.identity);
                    allGeneratedTiles.Add(new GeneratedTile(j, i, "Grass", tile));

                    if (!IsThereTreeOnTile(j - tileScale / 2, j + tileScale / 2, i - tileScale / 2, i + tileScale / 2))
                    {
                        int treeCountOnTile = Random.RandomRange(1, treeDensity);
                        // Trees are generated inappropriate
                        for (int k = 0; k < treeCountOnTile; k++)
                        {
                            float xCoord = Random.RandomRange(j - tileScale / 2, j + tileScale / 2);
                            float yCoord = Random.RandomRange(i - tileScale / 2, i + tileScale / 2);

                            GameObject currTree = Instantiate(Resources.Load("Objects/FruitTree", typeof(GameObject)) as GameObject,
                                                              new Vector3(xCoord, yCoord, -5f), Quaternion.Euler(0, 0, EnemyClassScript.character.eulerAngles.z));

                            TreeGenerationScript.allWorldOBjectsList.Add(currTree);
                        }
                    }

                }
                
            }
        }

        prevClosestGeneratedTile = allGeneratedTiles[FindTheClosestTileToPlayer()];
    }

    void GenerateTerrain()
    {
        currClosestGeneratedTile = allGeneratedTiles[FindTheClosestTileToPlayer()];

        if(currClosestGeneratedTile.getXCoord() != prevClosestGeneratedTile.getXCoord() ||
           currClosestGeneratedTile.getYCoord() != prevClosestGeneratedTile.getYCoord())
        {
            RemoveTilesOutOfRange();
            GenerateTiles(currClosestGeneratedTile);
            //print(allGeneratedTiles.Count); //Debug reasons
        }
    }

    float EvaluateDifference(float playerCoord, float localCoord)
    {
        if ((playerCoord >= 0 && localCoord >= 0) || ((playerCoord <= 0 && localCoord <= 0)))
            return Mathf.Abs(Mathf.Abs(playerCoord) - Mathf.Abs(localCoord));
        else if ((playerCoord >= 0 && localCoord <= 0) || (playerCoord <= 0 && localCoord >= 0))
            return Mathf.Abs(playerCoord) + Mathf.Abs(localCoord);
        else return 0;
    }

    int FindTheClosestTileToPlayer()
    {
        float playerXCoord = GameObject.Find("Player").transform.position.x;
        float playerYCoord = GameObject.Find("Player").transform.position.y;

        float localXCoord;
        float localYCoord;
        float prevLocalXCoord;
        float prevLocalYCoord;
        float localCoordDifference = 0;
        float prevLocalCoordDifference = 0;

        // case +1 +1
        for (int i = Mathf.RoundToInt(playerXCoord); ; i++)
        {
            if(i % tileScale == 0)
            {
                localXCoord = i;
                prevLocalXCoord = i;
                localCoordDifference += EvaluateDifference(playerXCoord, localXCoord);
                break;
            }
        }

        for (int i = Mathf.RoundToInt(playerYCoord); ; i++)
        {
            if (i % tileScale == 0)
            {
                localYCoord = i;
                prevLocalYCoord = i;
                localCoordDifference += EvaluateDifference(playerYCoord, localYCoord);
                break;
            }
        }
        prevLocalCoordDifference = localCoordDifference;
        localCoordDifference = 0;
        ////////////////////////
        ///case +1 -1
        for (int i = Mathf.RoundToInt(playerXCoord); ; i++)
        {
            if (i % tileScale == 0)
            {
                localXCoord = i;
                localCoordDifference += EvaluateDifference(playerXCoord, localXCoord);
                break;
            }
        }

        for (int i = Mathf.RoundToInt(playerYCoord); ; i--)
        {
            if (i % tileScale == 0)
            {
                localYCoord = i;
                localCoordDifference += EvaluateDifference(playerYCoord, localYCoord);
                break;
            }
        }

        if(localCoordDifference > prevLocalCoordDifference)
        {
            localXCoord = prevLocalXCoord;
            localYCoord = prevLocalYCoord;
            localCoordDifference = 0;
        }
        else
        {
            prevLocalXCoord = localXCoord;
            prevLocalYCoord = localYCoord;
            prevLocalCoordDifference = localCoordDifference;
            localCoordDifference = 0;
        }
        ///////////////////////////////
        ///case -1 +1
        for (int i = Mathf.RoundToInt(playerXCoord); ; i--)
        {
            if (i % tileScale == 0)
            {
                localXCoord = i;
                localCoordDifference += EvaluateDifference(playerXCoord, localXCoord);
                break;
            }
        }

        for (int i = Mathf.RoundToInt(playerYCoord); ; i++)
        {
            if (i % tileScale == 0)
            {
                localYCoord = i;
                localCoordDifference += EvaluateDifference(playerYCoord, localYCoord);
                break;
            }
        }

        if (localCoordDifference > prevLocalCoordDifference)
        {
            localXCoord = prevLocalXCoord;
            localYCoord = prevLocalYCoord;
            localCoordDifference = 0;
        }
        else
        {
            prevLocalXCoord = localXCoord;
            prevLocalYCoord = localYCoord;
            prevLocalCoordDifference = localCoordDifference;
            localCoordDifference = 0;
        }
        ///////////////////////////////
        ///case -1 -1
        for (int i = Mathf.RoundToInt(playerXCoord); ; i--)
        {
            if (i % tileScale == 0)
            {
                localXCoord = i;
                localCoordDifference += EvaluateDifference(playerXCoord, localXCoord);
                break;
            }
        }

        for (int i = Mathf.RoundToInt(playerYCoord); ; i--)
        {
            if (i % tileScale == 0)
            {
                localYCoord = i;
                localCoordDifference += EvaluateDifference(playerYCoord, localYCoord);
                break;
            }
        }

        if (localCoordDifference > prevLocalCoordDifference)
        {
            localXCoord = prevLocalXCoord;
            localYCoord = prevLocalYCoord;
            localCoordDifference = 0;
        }
        else
        {
            prevLocalXCoord = localXCoord;
            prevLocalYCoord = localYCoord;
            prevLocalCoordDifference = localCoordDifference;
            localCoordDifference = 0;
        }

        //Search for coordinates in the list
        for (int i = 0; i < allGeneratedTiles.Count; i++)
        {
            if (localXCoord == allGeneratedTiles[i].getXCoord() && localYCoord == allGeneratedTiles[i].getYCoord())
                return i;
        }

        return 0;
    }

    void Update()
    {
        // In case the player starts immediately moving 
        if(IsFirstTime)
        {
            Invoke("GenerateTerrain", 1);
            IsFirstTime = false;
        }

        Invoke("GenerateTerrain", 2);
    }
}

public class GeneratedTile
{
    private float xCoord;
    private float yCoord;
    private string tileType;
    private GameObject tileObject;

    public float getXCoord() { return this.xCoord; }

    public float getYCoord() { return this.yCoord; }

    public string getTileType() { return this.tileType; }

    public GameObject getTileObject() { return this.tileObject; }

    public GeneratedTile(float xCoord, float yCoord, string tileType, GameObject tileObject)
    {
        this.xCoord = xCoord;
        this.yCoord = yCoord;
        this.tileType = tileType;
        this.tileObject = tileObject;
    }
}
