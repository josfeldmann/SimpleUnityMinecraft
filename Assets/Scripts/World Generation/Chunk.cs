using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;

public class Chunk : MonoBehaviour {

    public BlockType[,,] chunkData;
   

    public IEnumerator ChunkRoutine(int x, int y, int z) {

        ReBuildChunk();

        Chunk next;
        if (World.Instance.WorldList.TryGetValue(new Vector3(x+1, y, z), out next)) {
            next.StartCoroutine(ChunkRoutine(x + 1, y, z));
        }

        yield return null;
        
    }


    int x, z, y;
    bool prevair, watertop;

    public void BuildMap() {

        chunkData = new BlockType[World.Instance.chunkSize, World.Instance.chunkheight, World.Instance.chunkSize];

       
        int currheight;

        for (x = 0; x < World.Instance.chunkSize; x++)
            for (z = 0; z < World.Instance.chunkSize; z++) {
                prevair = true;
                watertop = World.Instance.n.GenerateHeight(x + transform.position.x, z + transform.position.z) <= World.Instance.WaterHeight;
                for ( y = World.Instance.chunkheight - 1; y > 0; y--) {


                    
                    if (World.Instance.n.fBM3D((int)(x + transform.position.x), (int)(y + transform.position.y), (int)(z + transform.position.z), 0.1f, 3) < 0.42f) {
                        if (prevair && watertop && y < World.Instance.WaterHeight)
                            chunkData[x, y, z] = BlockType.WATER;
                            else if (y < World.Instance.WaterHeight && prevair) chunkData[x, y, z] = BlockType.WATER;
                            else chunkData[x, y, z] = BlockType.AIR;

                    }

                      
                     else if (y <= World.Instance.n.OldGenerateHeight(x + transform.position.x, z + transform.position.z)) {
                        if (prevair) {
                            prevair = false;
                            chunkData[x, y, z] = BlockType.GRASS;
                        }
                        else {
                            chunkData[x, y, z] = BlockType.DIRT;
                        }

                    }
                    else if (y < World.Instance.WaterHeight) {
                        chunkData[x, y, z] = BlockType.WATER;
                    }
                    else {
                        chunkData[x, y, z] = BlockType.AIR;
                    }
                    
                }
            }

    }

    Chunk testChunk;

    public bool isEmpty(int x, int y, int z) {

            


            return chunkData[x, y, z] == BlockType.AIR;

        
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

    public void Draw(BlockType bType, bool top, bool bot, bool front, bool back, bool left, bool right)
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

    void CreateQuad(BlockType bType, Cubeside side)
    {

        
        


        if (bType == BlockType.GRASS && side == Cubeside.TOP)
        {
            uv00 = World.Instance.blockUVs[0, 0];
            uv10 = World.Instance.blockUVs[0, 1];
            uv01 = World.Instance.blockUVs[0, 2];
            uv11 = World.Instance.blockUVs[0, 3];
        }
        else if (bType == BlockType.GRASS && side == Cubeside.BOTTOM)
        {
            uv00 = World.Instance.blockUVs[(int)(BlockType.DIRT + 1), 0];
            uv10 = World.Instance.blockUVs[(int)(BlockType.DIRT + 1), 1];
            uv01 = World.Instance.blockUVs[(int)(BlockType.DIRT + 1), 2];
            uv11 = World.Instance.blockUVs[(int)(BlockType.DIRT + 1), 3];
        }
        else
        {
            uv00 = World.Instance.blockUVs[(int)(bType + 1), 0];
            uv10 = World.Instance.blockUVs[(int)(bType + 1), 1];
            uv01 = World.Instance.blockUVs[(int)(bType + 1), 2];
            uv11 = World.Instance.blockUVs[(int)(bType + 1), 3];
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
        StartCoroutine(BuildChunkIENum());
    }



    // Use this for initialization

    private float DebugTime = 0;
    private bool beingModified = false;


    private  IEnumerator BuildChunkIENum()
    {

        int ymax = World.Instance.chunkheight - 1;
        int xmax = World.Instance.chunkSize-1;
        int zmax = World.Instance.chunkSize-1;
        beingModified = true;
        
        for (x = 0; x < World.Instance.chunkSize; x++) {
            vec.x = x;
            if (x%3 == 0)yield return 0;
            for (z = 0; z < World.Instance.chunkSize; z++) {
                vec.z = z;
                for (y = 0; y < World.Instance.chunkheight;y++) {
                    vec.y = y;

                    
                    if (chunkData[x,y,z] != BlockType.AIR)
                    Draw(chunkData[x,y,z],
                    y == ymax || chunkData[x, y + 1, z] == BlockType.AIR, 
                    y > 0 && chunkData[x, y - 1, z] == BlockType.AIR,
                    z == zmax || chunkData[x, y, z + 1] == BlockType.AIR,
                    z == 0 || chunkData[x, y, z - 1] == BlockType.AIR, 
                    x == 0 || chunkData[x - 1, y, z] == BlockType.AIR,
                    x == xmax || chunkData[x + 1, y, z] == BlockType.AIR);



                }
            }
        }
    



        
        CombineQuads();

        beingModified = false;
    }



    public void DeleteBlock(Vector3Int bl)
    {
        
        if (chunkData[bl.x, bl.y, bl.z] != BlockType.AIR && !beingModified)
        {
            chunkData[bl.x, bl.y, bl.z] = BlockType.AIR;
            triangles.Clear();
            vertices.Clear();
            uvs.Clear();
            normals.Clear();
            counter = 0;
            ReBuildChunk();
        }


    }

    void CombineQuads() {

        print("UpdatingMesh " + Time.time);

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