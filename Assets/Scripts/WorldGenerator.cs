using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldGenerator : MonoBehaviour
{
    public int maxWorldSize;
    public int numRooms;
    public int[,] roomGrid;
    public float treePercent, fountainPercent = 0.1f, shrubberyPercent = 0.1f;
    public float roomDistance = 5;
    public List<GameObject> FloorBases = new List<GameObject>();
    public GameObject BaseObj;
    Vector2 lastOpen = new Vector2();
    public GameObject playerPrefab, treePrefab, fountainPrefab, shrubberyPrefab;
    // Start is called before the first frame update
    int startx, starty;
    Sprite minimapIcon;
    Texture2D tex;
    int mapSpriteSize = 16;
    public Sprite[] texArray;
    public Color MiniMapBaseColor, MiniMapWallColor;
    public Image MiniMap;
    private VRPlayerController playerController;
    void Start()
    {
        GenFloor();
        playerController = Instantiate(playerPrefab, new Vector3(startx, 1, starty) * roomDistance, Quaternion.identity).GetComponent<VRPlayerController>();
        playerController.GetComponent<VRPlayerUIManager>().init(roomDistance);
        MakeMap();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            GenFloor();
        }
    }

    List<Vector2> PastTiles;

    public void GenFloor() {

        PastTiles = new List<Vector2>();

        foreach (GameObject floor in FloorBases) {
            Destroy(floor);
        }
        FloorBases.Clear();

        startx = maxWorldSize / 2;
        starty = maxWorldSize / 2;
        roomGrid = new int[maxWorldSize, maxWorldSize];
        int counter = 0;
        roomGrid[startx, starty] = 1;
        int currentx = startx;
        int dir;
        int currenty = starty;
        List<int> l = new List<int>();
        
        while (counter < numRooms) {
            
            l.Clear();
            if (currentx > 0 && roomGrid[currentx-1,currenty] == 0) {
                l.Add(0);
            }
            if (currentx < maxWorldSize-1 && roomGrid[currentx  + 1, currenty] == 0) {
                l.Add(1);
            }
            if (currenty > 0 && roomGrid[currentx, currenty-1] == 0) {
                l.Add(2);
            }
            if (currenty < maxWorldSize-1 && roomGrid[currentx, currenty + 1] == 0) {
                l.Add(3);
            }

            if (l.Count == 0) {
                currentx = (int)PastTiles[0].x;
                currenty = (int)PastTiles[0].y;
                PastTiles.RemoveAt(0);

            } else {

                if (l.Count > 1) PastTiles.Add(new Vector2(currentx, currenty));


            dir = l[Random.Range(0, l.Count)];
                if (dir == 0) {
                    currentx--;
                } else if (dir == 1) {
                    currentx++;
                } else if (dir == 2) {
                    currenty--;
                } else if (dir == 3) {
                    currenty++;
                }
                if (roomGrid[currentx, currenty] == 0) {
                    roomGrid[currentx, currenty] = 1;
                } else {
                    print("Duped");
                }
            }
            counter++;
        }

        

        for (int i = 0; i < maxWorldSize; i++) {
            for (int j = 0; j < maxWorldSize; j++) {
                if (roomGrid[i, j] == 1) {
                    if (Random.Range(0f, 1f) <= fountainPercent) Instantiate(fountainPrefab, new Vector3(i * roomDistance, 2, j * roomDistance), Quaternion.identity);
                    else if (Random.Range(0f, 1f) <= shrubberyPercent) Instantiate(shrubberyPrefab, new Vector3((i * roomDistance) + Random.Range(-1.5f,1.5f), 1.4f, (j * roomDistance) + Random.Range(-1.5f, 1.5f)), Quaternion.identity);
                    FloorBases.Add(Instantiate(BaseObj, new Vector3(i, 0, j) * roomDistance, Quaternion.identity, transform));
                } else {
                    if (Random.Range(0f,1f) < treePercent) {
                        Instantiate(treePrefab, new Vector3(i, .01f, j) * roomDistance, Quaternion.identity);
                    }
                }
            }
        }

    }

    public void MakeMap() {

        mapSpriteSize = (int)texArray[0].rect.width;

        tex = new Texture2D(maxWorldSize * mapSpriteSize, maxWorldSize * mapSpriteSize, TextureFormat.ARGB32, false);
        MiniMapBaseColor = new Color(0, 0, 1, 1);
        MiniMapWallColor = new Color(0, 1, 0, 1);
        int pixelx, pixely;

        for (int x = 0; x < maxWorldSize; x++) {
            
            for (int y = 0; y < maxWorldSize; y++) {

               
                    for (pixelx = 0; pixelx < texArray[0].rect.width; pixelx++) {
                        for (pixely = 0; pixely < texArray[0].rect.height; pixely++) {
                        if (roomGrid[x, y] == 1) {
                            tex.SetPixel((x * mapSpriteSize) + pixelx, (y * mapSpriteSize) + pixely, MiniMapBaseColor);
                        } else {
                            tex.SetPixel((x * mapSpriteSize) + pixelx, (y * mapSpriteSize) + pixely, MiniMapWallColor);
                        }
                        }
                    }
                


            }
        }

        tex.Apply();


        minimapIcon = Sprite.Create(tex, new Rect(new Vector2(), new Vector2(maxWorldSize * mapSpriteSize, maxWorldSize * mapSpriteSize)), new Vector2());
        tex.filterMode = FilterMode.Point;
        playerController.mapImage.sprite = minimapIcon;
    }

}
