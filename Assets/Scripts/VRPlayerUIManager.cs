using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRPlayerUIManager : MonoBehaviour
{

    [SerializeField] private ControllerInput primaryHand, secondaryHand;
    [SerializeField] private GameObject masterUI, masterUIParent;
    [SerializeField] private HealthBarLoader healthBarLoader;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private float mainUIDistance; 

    public  GameObject playerPointer;
    // Start is called before the first frame update
    private bool mainUIShowing;

    // Update is called once per frame
    void Update()
    {
        if (secondaryHand.menuButtonDown) ToggleUI();
    }

    public void init(float roomsize)
    {
        mapManager.init(roomsize);
        masterUI.SetActive(false);
        playerPointer.SetActive(false);
        masterUI.transform.SetParent(null);

    }


    public void ToggleUI()
    {

        if (!mainUIShowing)
        {
            masterUI.transform.position =
                masterUIParent.transform.position + masterUIParent.transform.forward * mainUIDistance;
            masterUI.transform.LookAt(masterUIParent.transform, Vector3.up);
            masterUI.transform.Rotate(0,180,0);
        }

        mainUIShowing = !mainUIShowing;
        masterUI.SetActive(mainUIShowing);
        playerPointer.SetActive(mainUIShowing);

        
    }

    public void UpdateDamageVisual(float currentHP, float maxHP, float currentShields, float maxShields)
    {
        healthBarLoader.UpdateHealthBar(currentHP, maxHP, currentShields, maxShields);
    }
}
