using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeleportType { Blink, Dash }

public class TeleportController : HandController
{
    
    [SerializeField] private float teleportSpeed = 5f;
    [SerializeField]private LayerMask layerMask;
    [SerializeField] private GameObject teleportCapsule, debugCube;
    [SerializeField] private VRPlayerController player;
    [SerializeField] private GameObject lineRendererPrefab;
    [SerializeField] private TeleportType tType;

    private GameObject teleportIndicator, debugIndicator;
    private LineRenderer lineRenderer;

    public void Start() {
        teleportIndicator = Instantiate(teleportCapsule, Vector3.zero, Quaternion.identity);
        lineRenderer = Instantiate(lineRendererPrefab, transform).GetComponent<LineRenderer>();
        lineRenderer.transform.localPosition = Vector3.zero;
        teleportIndicator.SetActive(false);
        lineRenderer.gameObject.SetActive(false);
        debugIndicator = Instantiate(debugCube);
        debugIndicator.SetActive(false);

    }

    RaycastHit hit;

    public void Update() {

        if (controller.buttonOneUp)
        {
            if (teleportIndicator.activeInHierarchy)
            {


                if (tType == TeleportType.Dash)player.DashTeleport(teleportIndicator.transform.position, teleportSpeed);
                if (tType == TeleportType.Blink) player.BlinkTeleport(teleportIndicator.transform.position);
                teleportIndicator.SetActive(false);
                lineRenderer.gameObject.SetActive(false);
                debugIndicator.SetActive(true);
                debugIndicator.transform.position = teleportIndicator.transform.position;
            }
            else
            {

            }

        }


        if (controller.buttonOne) {
            print("tele");
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100, layerMask)) {

                if (hit.normal == Vector3.up)
                {
                    print("Vertical");
                }

                teleportIndicator.SetActive(true);
                teleportIndicator.transform.position = hit.point + Vector3.up;
                lineRenderer.gameObject.SetActive(true); 
                //lineRenderer.SetLine(lineRenderer.transform.position, hit.point);
                //lineRenderer.SetPosition(0, lineRenderer.transform.position);
                //lineRenderer.SetPosition(1, hit.point);

            } else {
                teleportIndicator.SetActive(false);
                lineRenderer.gameObject.SetActive(false);
            }
        }

        

    }


}
