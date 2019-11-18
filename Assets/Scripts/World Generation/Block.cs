using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Block {

    

    BlockType bType;
    public bool isSolid;
    Chunk owner;
    GameObject parent;
    Vector3 position;

    


    

   

    int ConvertBlockIndexToLocal(int i) {
        if (i == -1)
            i = World.Instance.chunkSize - 1;
        else if (i == World.Instance.chunkSize)
            i = 0;
        return i;
    }


   
}