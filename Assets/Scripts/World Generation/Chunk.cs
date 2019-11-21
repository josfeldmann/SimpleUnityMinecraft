using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;

public class Chunk : MonoBehaviour {

    public const int grass = 0, dirt = 1, stones = 2, water = 3, air = 250;



    public int[,,] chunkData;
   

    float currChunkHeight;

    public Vector3Int ChunkIndex;

    public IEnumerator ChunkRoutine(int x, int y, int z) {

        

        currChunkHeight = ((float)y)/ World.Instance.worldheight;


        
        ReBuildChunk();

        Chunk next;
        if (World.Instance.WorldList.TryGetValue(new Vector3Int(x+1, y, z), out next)) {
            next.StartCoroutine(ChunkRoutine(x + 1, y, z));
        }

        yield return null;
        
    }

public void SetNeighbours(Vector3Int vec1){
    ChunkIndex = vec1;

        

        World.Instance.WorldList.TryGetValue(new Vector3Int(vec1.x,vec1.y-1,vec1.z), out botNeighbour);
        World.Instance.WorldList.TryGetValue(new Vector3Int(vec1.x,vec1.y+1,vec1.z), out topNeighbour);
        World.Instance.WorldList.TryGetValue(new Vector3Int(vec1.x+1,vec1.y,vec1.z), out rightNeighbour);
        World.Instance.WorldList.TryGetValue(new Vector3Int(vec1.x-1,vec1.y,vec1.z), out leftNeighbour);
        World.Instance.WorldList.TryGetValue(new Vector3Int(vec1.x,vec1.y,vec1.z+1), out frontNeighbor);
        World.Instance.WorldList.TryGetValue(new Vector3Int(vec1.x,vec1.y,vec1.z-1), out backNeighbour);
       
}



    int x, z, y;
    bool prevair, watertop;

    public void BuildMap() {

        chunkData = new int[World.Instance.chunkSize, World.Instance.chunkheight, World.Instance.chunkSize];

       
        int currheight;

        for (x = 0; x < World.Instance.chunkSize; x++)
            for (z = 0; z < World.Instance.chunkSize; z++) {
                prevair = true;
                watertop = World.Instance.n.GenerateHeight(x + transform.position.x, z + transform.position.z) <= World.Instance.WaterHeight;
                for ( y = World.Instance.chunkheight-1; y >= 0; y--) {


                    
                    if (World.Instance.n.fBM3D((int)(x + transform.position.x), (int)(y + transform.position.y), (int)(z + transform.position.z), 0.1f, 3) < 0.42f) {
                        if (prevair && watertop && y < World.Instance.WaterHeight)
                            chunkData[x, y, z] = water;
                            else if (y < World.Instance.WaterHeight && prevair) chunkData[x, y, z] = water;
                            else chunkData[x, y, z] = air;

                    }

                      
                     else if (y + transform.position.y <= World.Instance.n.OldGenerateHeight(x + transform.position.x, z + transform.position.z)) {
                        if (prevair) {
                            prevair = false;
                            chunkData[x, y, z] = grass;
                        }
                        else {
                            chunkData[x, y, z] = dirt;
                        }

                    }
                    else if (y < World.Instance.WaterHeight) {
                        chunkData[x, y, z] = water;
                    }
                    else {
                        chunkData[x, y, z] = air;
                    }
                    
                }
            }

    }

    Chunk testChunk;

    public bool isEmpty(int x, int y, int z) {

            


            return chunkData[x, y, z] == air;

        
    }

    public Vector3 vec = new Vector3();

   

    public List<Vector3> vertices = new List<Vector3>();
    public List<Vector3> normals = new List<Vector3>();
    public List<Vector2> uvs = new List<Vector2>();
    public List<int> triangles = new List<int>();

    

    //all possible UVs
    Vector2 uv00;
    Vector2 uv10;
    Vector2 uv01;
    Vector2 uv11;


    Vector3 p0 = new Vector3(-0.5f, -0.5f, 0.5f);
    Vector3 p1 = new Vector3(0.5f, -0.5f, 0.5f);
    Vector3 p2 = new Vector3(0.5f, -0.5f, -0.5f);
    Vector3 p3 = new Vector3(-0.5f, -0.5f, -0.5f);
    Vector3 p4 = new Vector3(-0.5f, 0.5f, 0.5f);
    Vector3 p5 = new Vector3(0.5f, 0.5f, 0.5f);
    Vector3 p6 = new Vector3(0.5f, 0.5f, -0.5f);
    Vector3 p7 = new Vector3(-0.5f, 0.5f, -0.5f);

    public void Draw(int bType, bool top, bool bot, bool front, bool back, bool left, bool right)
    {
        if (front)
            CreateQuad(bType,Cubeside.FRONT);
        if (back)
            CreateQuad(bType, Cubeside.BACK);
        if (top)
            CreateQuad(bType, Cubeside.TOP);
        if (bot)
            CreateQuad(bType, Cubeside.BOTTOM);
        if (left)
            CreateQuad(bType, Cubeside.LEFT);
        if (right)
            CreateQuad(bType, Cubeside.RIGHT);
    }


    enum Cubeside { BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK };

    int counter = 0;

    void CreateQuad(int bType, Cubeside side)
    {

        
        


        if (bType == grass && side == Cubeside.TOP)
        {
            uv00 = World.Instance.blockUVs[0, 0];
            uv10 = World.Instance.blockUVs[0, 1];
            uv01 = World.Instance.blockUVs[0, 2];
            uv11 = World.Instance.blockUVs[0, 3];
        }
        else if (bType == grass && side == Cubeside.BOTTOM)
        {
            uv00 = World.Instance.blockUVs[(dirt + 1), 0];
            uv10 = World.Instance.blockUVs[(dirt + 1), 1];
            uv01 = World.Instance.blockUVs[(dirt + 1), 2];
            uv11 = World.Instance.blockUVs[(dirt + 1), 3];
        }
        else
        {
            uv00 = World.Instance.blockUVs[(bType + 1), 0];
            uv10 = World.Instance.blockUVs[(bType + 1), 1];
            uv01 = World.Instance.blockUVs[(bType + 1), 2];
            uv11 = World.Instance.blockUVs[(bType + 1), 3];
        }

        //all possible vertices 


        switch (side)
        {
            case Cubeside.BOTTOM:
               // print("Bot");
                
                vertices.Add(p0 + vec);
                vertices.Add(p1 + vec);
                vertices.Add(p2 + vec);
                vertices.Add(p3 + vec);



                normals.Add(Vector3.down);
                normals.Add(Vector3.down);
                normals.Add(Vector3.down);
                normals.Add(Vector3.down);

                
                break;
            case Cubeside.TOP:
                
                vertices.Add(p7 + vec);
                vertices.Add(p6 + vec);
                vertices.Add(p5 + vec);
                vertices.Add(p4 + vec);

                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);


                break;
            case Cubeside.LEFT:
                

                vertices.Add(p7 + vec);
                vertices.Add(p4 + vec);
                vertices.Add(p0 + vec);
                vertices.Add(p3 + vec);
                normals.Add(Vector3.left);
                normals.Add(Vector3.left);
                normals.Add(Vector3.left);
                normals.Add(Vector3.left);

              
                break;
            case Cubeside.RIGHT:
                

                vertices.Add(p5 + vec);
                vertices.Add(p6 + vec);
                vertices.Add(p2 + vec);
                vertices.Add(p1 + vec);
                normals.Add(Vector3.right);
                normals.Add(Vector3.right);
                normals.Add(Vector3.right);
                normals.Add(Vector3.right);

               
                break;
            case Cubeside.FRONT:
                
                vertices.Add(p4 + vec);
                vertices.Add(p5 + vec);
                vertices.Add(p1 + vec);
                vertices.Add(p0 + vec);

                normals.Add(Vector3.forward);
                normals.Add(Vector3.forward);
                normals.Add(Vector3.forward);
                normals.Add(Vector3.forward);

                
                break;
            case Cubeside.BACK:
                

                vertices.Add(p6 + vec);
                vertices.Add(p7 + vec);
                vertices.Add(p3 + vec);
                vertices.Add(p2 + vec);

                normals.Add(Vector3.back);
                normals.Add(Vector3.back);
                normals.Add(Vector3.back);
                normals.Add(Vector3.back);

                break;
        }

       
        uvs.Add(uv11);
        uvs.Add(uv01);
        uvs.Add(uv00);
        uvs.Add(uv10);
        
        triangles.Add(3 + counter);
        triangles.Add(1 + counter);
        triangles.Add(0 + counter);
        triangles.Add(3 + counter);
        triangles.Add(2 + counter);
        triangles.Add(1 + counter);
        counter += 4;






    }







    public void ReBuildChunk(){
        
        BuildChunkThing();
    }



    // Use this for initialization

    private float DebugTime = 0;
    private bool beingModified = false;

    public Chunk topNeighbour,botNeighbour,leftNeighbour,rightNeighbour,frontNeighbor,backNeighbour;


    int xmax, ymax, zmax;

    private void BuildChunkThing()
    {


        // DateTime before = DateTime.Now;

        


         ymax = World.Instance.chunkheight - 1;
         xmax = World.Instance.chunkSize-1;
         zmax = World.Instance.chunkSize-1;
        beingModified = true;
        
        for (x = 0; x < World.Instance.chunkSize; x++) {
            vec.x = x;
           /// if (x%3 == 0)yield return 0;
            for (z = 0; z < World.Instance.chunkSize; z++) {
                vec.z = z;
                for (y = 0; y < World.Instance.chunkheight;y++) {
                    vec.y = y;

                    /*
                    if (chunkData[x,y,z] != air)
                    Draw(chunkData[x,y,z],
                    y < ymax && chunkData[x, y + 1, z] == air, 
                    y > 0 && chunkData[x, y - 1, z] == air,
                    z < zmax && chunkData[x, y, z + 1] == air,
                    z > 0 && chunkData[x, y, z - 1] == air, 
                    x > 0 && chunkData[x - 1, y, z] == air,
                    x < xmax && chunkData[x + 1, y, z] == air);
                    */


                    if (chunkData[x,y,z] != air)
                    Draw(chunkData[x,y,z],
                    (y < ymax && chunkData[x, y + 1, z] == air) || (y == ymax && topNeighbour!= null && topNeighbour.chunkData[x,0,z] == air) ,
                    (y > 0 && chunkData[x, y - 1, z] == air) || (y == 0 && botNeighbour!= null && botNeighbour.chunkData[x,ymax,z] == air) ,
                    (z < zmax && chunkData[x, y, z + 1] == air) || (z == zmax && frontNeighbor != null && frontNeighbor.chunkData[x,y,0] == air),
                    (z > 0 && chunkData[x, y, z - 1] == air) || (z == 0 && backNeighbour != null && backNeighbour.chunkData[x,y,zmax] == air), 
                    (x > 0 && chunkData[x - 1, y, z] == air) || (x == 0 && leftNeighbour != null && leftNeighbour.chunkData[xmax,y,z] == air),
                    (x < xmax && chunkData[x + 1, y, z] == air) || (x == xmax && rightNeighbour != null && rightNeighbour.chunkData[0,y,z] == air)) ;



                }
            }
        }


// DateTime after = DateTime.Now; 
//         TimeSpan duration = after.Subtract(before);
//         Debug.Log("Vertices Took: " + duration.Milliseconds);
    


// before = DateTime.Now;
        
        CombineQuads();


    //  after = DateTime.Now; 
    //  duration = after.Subtract(before);
    //  Debug.Log("buildingMesh Took: " + duration.Milliseconds);

        beingModified = false;


        //yield return null;
    }

     public void AddBlock(Vector3Int bl, int bType)
    {
        
        if (chunkData[bl.x, bl.y, bl.z] == air && !beingModified)
        {
            chunkData[bl.x, bl.y, bl.z] = bType;
            triangles.Clear();
            vertices.Clear();
            uvs.Clear();
            normals.Clear();
            counter = 0;
            ReBuildChunk();
        }


    }




    public void DeleteBlock(Vector3Int bl)
    {
        
        if (chunkData[bl.x, bl.y, bl.z] != air && !beingModified)
        {
            chunkData[bl.x, bl.y, bl.z] = air;
            triangles.Clear();
            vertices.Clear();
            uvs.Clear();
            normals.Clear();
            counter = 0;
            ReBuildChunk();
        }

        if (bl.x == 0 && leftNeighbour != null && leftNeighbour.chunkData[xmax, bl.y,bl.z] != air) leftNeighbour.ReBuildChunk(); 
        else if (bl.x == xmax && rightNeighbour != null && rightNeighbour.chunkData[0, bl.y,bl.z] != air) rightNeighbour.ReBuildChunk(); 

        if (bl.z == 0 && backNeighbour != null && backNeighbour.chunkData[bl.x, bl.y,zmax] != air) backNeighbour.ReBuildChunk(); 
        else if (bl.z == xmax && frontNeighbor != null && rightNeighbour.chunkData[bl.x, bl.y,0] != air) frontNeighbor.ReBuildChunk(); 

        
        if (bl.y == 0 && botNeighbour != null && botNeighbour.chunkData[bl.x, ymax,bl.z] != air) botNeighbour.ReBuildChunk(); 
        else if (bl.z == ymax && topNeighbour != null && topNeighbour.chunkData[bl.x, 0,bl.z] != air) topNeighbour.ReBuildChunk(); 



    }

    void CombineQuads() {

        //print("UpdatingMesh " + Time.time);

        Mesh m = new Mesh();

        float3[] fa = new float3[3];
        m.vertices = vertices.ToArray();
        m.triangles = triangles.ToArray();
        m.uv = uvs.ToArray();
        m.normals = normals.ToArray();
        m.RecalculateBounds();
      

        //2. Create a new mesh on the parent object
        MeshFilter mf = gameObject.GetComponent<MeshFilter>();
        mf.mesh = m;

        //3. Add combined meshes on children as the parent's mesh
       

        //4. Create a renderer for the parent
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.material = World.Instance.mat;

        MeshCollider Mc = GetComponent<MeshCollider>();
        Mc.sharedMesh = mf.mesh;
        


    }
}