using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInteraction :MonoBehaviour {

    
    public GameObject TestClick;
    public TextMesh tm;

    public float buildLength = 10f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    //ShowTriangle();

	//if (controller.mainTriggerDown) {

        //    DestroyBlock();

   // }

   // if (controller.gripTriggerDown){
       // AddBlock();
   // }
	}

    public void DestroyBlock() {

        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, buildLength)) {
            print(hit.collider.name);

            if (hit.collider.tag == "Chunk") {

                TestClick.transform.position = hit.point;

                Vector3 localIndex = hit.point - hit.transform.position + (new Vector3(0.5f, 0.5f, 0.5f));
                if (hit.normal.y == 1.0f) localIndex.y -= 0.5f;
                if (hit.normal.x == 1.0f) localIndex.x -= 0.5f;
                if (hit.normal.z == 1.0f) localIndex.z -= 0.5f;
                tm.text = localIndex.ToString() + " Normal: " + hit.normal.ToString();

                print(new Vector3Int((int)(localIndex.x), (int)(localIndex.y), (int)(localIndex.z)).ToString());

                Chunk c = hit.transform.GetComponent<Chunk>();

                c.DeleteBlock(new Vector3Int((int)(localIndex.x), (int)(localIndex.y), (int)(localIndex.z)));
                
                
            }

        }

    }


public void AddBlock(){
    RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, buildLength)) {

            if (hit.collider.tag == "Chunk") {

                TestClick.transform.position = hit.point;

                Vector3 localIndex = hit.point - hit.transform.position + (new Vector3(0.5f, 0.5f, 0.5f));
                if (hit.normal.y == 1.0f) localIndex.y -= 0.5f;
                if (hit.normal.x == 1.0f) localIndex.x -= 0.5f;
                if (hit.normal.z == 1.0f) localIndex.z -= 0.5f;
                tm.text = localIndex.ToString() + " Normal: " + hit.normal.ToString();

                print(new Vector3Int((int)(localIndex.x), (int)(localIndex.y), (int)(localIndex.z)).ToString());



                hit.transform.GetComponent<Chunk>().AddBlock(new Vector3Int((int)(localIndex.x), (int)(localIndex.y), (int)(localIndex.z)) + new Vector3Int((int)hit.normal.x, (int)hit.normal.y, (int)hit.normal.z), 2);
                
                
            
            }

        }
}

public void ShowTriangle(){
     RaycastHit hit;
        if (!Physics.Raycast(transform.position, transform.forward, out hit, buildLength))
            return;

        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (meshCollider == null || meshCollider.sharedMesh == null)
            return;

        Mesh mesh = meshCollider.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
        Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
        Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];
        Transform hitTransform = hit.collider.transform;
        p0 = hitTransform.TransformPoint(p0);
        p1 = hitTransform.TransformPoint(p1);
        p2 = hitTransform.TransformPoint(p2);
        Debug.DrawLine(p0, p1);
        Debug.DrawLine(p1, p2);
        Debug.DrawLine(p2, p0);
}


}
