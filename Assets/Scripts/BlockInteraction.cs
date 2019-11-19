using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInteraction : MonoBehaviour {

    public Camera cam;
    public GameObject TestClick;
    public TextMesh tm;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	if (Input.GetMouseButtonDown(0)) {

            DestroyBlock();

        }	
	}

    public void DestroyBlock() {

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 10)) {

            if (hit.collider.tag == "Chunk") {

                TestClick.transform.position = hit.point;

                Vector3 localIndex = hit.point - hit.transform.position + (new Vector3(0.5f, 0.5f, 0.5f));
                if (hit.normal.y == 1.0f) localIndex.y -= 0.5f;
                if (hit.normal.x == 1.0f) localIndex.x -= 0.5f;
                if (hit.normal.z == 1.0f) localIndex.z -= 0.5f;
                tm.text = localIndex.ToString() + " Normal: " + hit.normal.ToString();

                print(new Vector3Int((int)(localIndex.x), (int)(localIndex.y), (int)(localIndex.z)).ToString());



                hit.transform.GetComponent<Chunk>().DeleteBlock(new Vector3Int((int)(localIndex.x), (int)(localIndex.y), (int)(localIndex.z)));
                
                
            }

        }

    }

}
