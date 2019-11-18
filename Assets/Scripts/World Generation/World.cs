using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum BlockType { GRASS, DIRT, STONE, WATER, LEAVES, TRUNK, AIR };


public class World : Singleton<World> {

    
    public int seed;
    public Material mat;
    public int chunkSize = 16;
    public int worldheight = 1;
    public int chunkheight = 150;
    public int initialWorldSize = 1;
    public int WaterHeight = 15;
    public GameObject chunk;
    public Dictionary<Vector3, Chunk> WorldList;
    public Vector3 PlayerStartPostion;
    public Noise n;
    public GameObject Player;
    public Image LoadingBar;

    /* 0 = Grass
     * 1 = Dirt
     * 2 = Stone
     * 3 = Water
     * 6 = 
     * 
     */


    public Vector2[,] blockUVs = { 

		/*GRASS TOP*/		{new Vector2( 0.125f, 0.375f ), new Vector2( 0.1875f, 0.375f),
                                new Vector2( 0.125f, 0.4375f ),new Vector2( 0.1875f, 0.4375f )},
		/*GRASS SIDE*/		{new Vector2( 0.1875f, 0.9375f ), new Vector2( 0.25f, 0.9375f),
                                new Vector2( 0.1875f, 1.0f ),new Vector2( 0.25f, 1.0f )},
		/*DIRT*/			{new Vector2( 0.125f, 0.9375f ), new Vector2( 0.1875f, 0.9375f),
                                new Vector2( 0.125f, 1.0f ),new Vector2( 0.1875f, 1.0f )},
		/*STONE*/			{new Vector2( 0, 0.875f ), new Vector2( 0.0625f, 0.875f),
                                new Vector2( 0, 0.9375f ),new Vector2( 0.0625f, 0.9375f )},
        /*WATER*/           {new Vector2( 0.875f, 0.1875f ), new Vector2( 0.9375f, 0.1875f),
                                new Vector2( 0.875f, 0.25f ),new Vector2( 0.9375f, 0.25f )},
        
        /*LEAVES*/          {new Vector2( 0.3125f, 0.75f ), new Vector2( 0.375f, 0.75f),
                                new Vector2( 0.3125f, 0.8125f ),new Vector2( 0.375f, 0.8125f )},
        
        /*TRUNK*/           {new Vector2( 0.4375f, 0.625f ), new Vector2( 0.5f, 0.625f),
                                new Vector2( 0.4375f, 0.6875f ),new Vector2( 0.5f, 0.6875f )},

                        };


    IEnumerator BuildWorld() {

        Player.SetActive(false);

        Random.seed = seed;

        WorldList = new Dictionary<Vector3, Chunk>();
        n = GetComponent<Noise>();
        n.offsetX = Random.Range(0f, 99999f);
        n.offsetZ = Random.Range(0f, 99999f);

        print("x = " + n.offsetX + " z = " + n.offsetZ);

        int total = (initialWorldSize * 2) * (initialWorldSize * 2);
        int current = 0;
        for (int x = -initialWorldSize; x < initialWorldSize; x++)
            for (int z = -initialWorldSize; z < initialWorldSize; z++)
                for (int y = 0; y < worldheight; y++) {

                    
                    
                    GameObject gem = GameObject.Instantiate(chunk, new Vector3(x * chunkSize, y * chunkheight, z * chunkSize) , Quaternion.identity);
                    gem.name = new Vector3(x, y, z).ToString();
                    WorldList.Add(new Vector3(x, y, z), gem.GetComponent<Chunk>());
                    gem.GetComponent<Chunk>().BuildMap();
                    
                }

        foreach (KeyValuePair<Vector3, Chunk> c in WorldList) {
            /*if (c.Key.x == -initialWorldSize) {
                  c.Value.StartCoroutine(c.Value.ChunkRoutine((int)-initialWorldSize, (int) c.Key.y, (int)c.Key.z));
              }*/

            c.Value.BuildChunk();

            current++;
            LoadingBar.fillAmount = (float)current / (float)total;
            yield return new WaitForSeconds(0.0f);

        }

        yield return null;

        Player.SetActive(true);
        LoadingBar.transform.parent.gameObject.SetActive(false);
        
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(BuildWorld());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
