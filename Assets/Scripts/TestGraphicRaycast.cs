using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestGraphicRaycast : MonoBehaviour
{


    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GraphicRaycaster gr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + transform.forward * 100);

        PointerEventData pointerData = new PointerEventData(EventSystem.current);

        pointerData.position = transform.position; // use the position from controller as start of raycast instead of mousePosition.

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            //WorldUI is my layer name
            if (results[0].gameObject.layer == LayerMask.NameToLayer("WorldUI"))
            {
                string dbg = "Root Element: {0} \n GrandChild Element: {1}";
                Debug.Log(string.Format(dbg, results[results.Count - 1].gameObject.name, results[0].gameObject.name));
                //Debug.Log("Root Element: "+results[results.Count-1].gameObject.name);
                //Debug.Log("GrandChild Element: "+results[0].gameObject.name);
                results.Clear();
            }
        }
    }
}
