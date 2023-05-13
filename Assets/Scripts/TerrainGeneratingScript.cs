using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TerrainGeneratingScript : MonoBehaviour
{
    public static List<GeneratedTile> allGeneratedTiles = new List<GeneratedTile>();
    public static int generateRange = 6; //when 3 - The count of the tiles is between 36 and 48
    public static int tileScale = 2;
    public static GeneratedTile currClosestGeneratedTile;
    public static GeneratedTile prevClosestGeneratedTile;
    public static int treeProbability = 90;

    public static bool IsFirstTime = true;

    //Biome variables
    public static int biomeProbability = 999;

    void Start()
    {
        for (int i = -tileScale * (generateRange + 1); i < tileScale * (generateRange + 1); i += tileScale)
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
            Vector3 treePosVector = curr.transform.GetChild(0).transform.position;

            if (treePosVector.x >= xStart && treePosVector.x <= xEnd &&
                treePosVector.y >= yStart && treePosVector.y <= yEnd)
                return true;
        }
        return false;
    }

    bool isThereAnotherTileOnCoord(int x, int y)
    {
        if(Physics.Raycast(new Vector3(x, y, -4.5f), Vector3.forward))
            return true;

        return false;
    }

    void GenerateBiome(int x, int y)
    {
        int biomeLength = Random.RandomRange(15, 70);
        for (int i = 0; i < biomeLength; i++)
        {
            if (!isThereAnotherTileOnCoord(x + tileScale * i, y))
            {
                GameObject tile = Instantiate(Resources.Load("Objects/PixelArt/Terrains/SandTerrainGO", typeof(GameObject)) as GameObject,
                              new Vector3(x + tileScale * i, y, -4f), Quaternion.identity);
                allGeneratedTiles.Add(new GeneratedTile(x + tileScale * i, y, "Sand", tile));
            }
        }

        //Generate upper half of the biome
        int upperHalfVar = Random.RandomRange(15, 70);
        int outerBiomeLength = biomeLength;
        int xCoord = x;
        int yCoord = y;
        for (int i = 0; i < upperHalfVar; i++)
        {
            int tileTypeCase = Random.RandomRange(1, 4);

            if (tileTypeCase == 1)
            {
                xCoord -= tileScale;
                yCoord += tileScale;
            }
            else if (tileTypeCase == 2)
            {
                yCoord += tileScale;
            }
            else if (tileTypeCase == 3)
            {
                xCoord += tileScale;
                yCoord += tileScale;
            }

            int innerBiomeLength = Random.RandomRange(outerBiomeLength - 1, outerBiomeLength + 2);

            for (int j = 0; j < innerBiomeLength; j++)
            {
                if (!isThereAnotherTileOnCoord(xCoord + tileScale * j, yCoord))
                {
                    GameObject tile = Instantiate(Resources.Load("Objects/PixelArt/Terrains/SandTerrainGO", typeof(GameObject)) as GameObject,
                                 new Vector3(xCoord + tileScale * j, yCoord, -4f), Quaternion.identity);
                    allGeneratedTiles.Add(new GeneratedTile(xCoord + tileScale * j, yCoord, "Sand", tile));
                }
            }
            outerBiomeLength = innerBiomeLength;
        }

        //Generate lower half of the biome
        int lowerHalfVar = Random.RandomRange(15, 70);
        xCoord = x;
        yCoord = y;
        outerBiomeLength = biomeLength;
        for (int i = 0; i < lowerHalfVar; i++)
        {
            int tileTypeCase = Random.RandomRange(1, 4);

            if (tileTypeCase == 1)
            {
                xCoord -= tileScale;
                yCoord -= tileScale;
            }
            else if (tileTypeCase == 2)
            {
                yCoord -= tileScale;
            }
            else if (tileTypeCase == 3)
            {
                xCoord += tileScale;
                yCoord -= tileScale;
            }

            int innerBiomeLength = Random.RandomRange(outerBiomeLength - 1, outerBiomeLength + 2);

            for (int j = 0; j < innerBiomeLength; j++)
            {
                if (!isThereAnotherTileOnCoord(xCoord + tileScale * j, yCoord))
                {
                    GameObject tile = Instantiate(Resources.Load("Objects/PixelArt/Terrains/SandTerrainGO", typeof(GameObject)) as GameObject,
                                        new Vector3(xCoord + tileScale * j, yCoord, -4f), Quaternion.identity);
                    allGeneratedTiles.Add(new GeneratedTile(xCoord + tileScale * j, yCoord, "Sand", tile));
                }

            }
            outerBiomeLength = innerBiomeLength;
        }
    }

    void GenerateTiles(GeneratedTile currentTile)
    {

        for (int i = Mathf.RoundToInt(currClosestGeneratedTile.getYCoord()) - tileScale * generateRange; i < Mathf.RoundToInt(currClosestGeneratedTile.getYCoord()) + tileScale * generateRange; i += tileScale)
        {
            for (int j = Mathf.RoundToInt(currClosestGeneratedTile.getXCoord()) - tileScale * generateRange; j < Mathf.RoundToInt(currClosestGeneratedTile.getXCoord()) + tileScale * generateRange; j += tileScale)
            {
                if(!IsTileInList(j, i))
                {
                    int biomeVar = Random.RandomRange(0, 1000);

                    if(biomeVar >= biomeProbability)
                    {
                        GenerateBiome(j, i);
                    }
                    else
                    {
                        float angleRand = Random.RandomRange(0, 4);

                        GameObject tile = Instantiate(Resources.Load("Objects/PixelArt/Terrains/GrassTerrainGO", typeof(GameObject)) as GameObject,
                                          new Vector3(j, i, -4f), Quaternion.Euler(0, 0, 90 * angleRand));
                        allGeneratedTiles.Add(new GeneratedTile(j, i, "Grass", tile));

                        if (!IsThereTreeOnTile(j - tileScale / 2, j + tileScale / 2, i - tileScale / 2, i + tileScale / 2))
                        {
                            int treeProbabilityOnTile = Random.RandomRange(1, 100);

                            if (treeProbabilityOnTile >= treeProbability)
                            {
                                float xCoord = Random.RandomRange(j - tileScale / 2.5f, j + tileScale / 2.5f);
                                float yCoord = Random.RandomRange(i - tileScale / 2.5f, i + tileScale / 2.5f);

                                GameObject currTree = Instantiate(Resources.Load("Objects/FruitTree", typeof(GameObject)) as GameObject,
                                                                  new Vector3(xCoord, yCoord, -5f), Quaternion.Euler(0, 0, EnemyClassScript.character.eulerAngles.z));

                                //Move tree on its actual visual position
                                currTree.transform.position += Quaternion.Euler(0, 0, EnemyClassScript.character.transform.eulerAngles.z) * new Vector3(0.18f, 1.4f, 0);

                                TreeGenerationScript.allWorldOBjectsList.Add(currTree);
                            }

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
            //RemoveTilesOutOfRange();
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
