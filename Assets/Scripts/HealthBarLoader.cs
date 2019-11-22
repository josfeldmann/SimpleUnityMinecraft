using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HealthBarLoader : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI healthText, shieldText;
    [SerializeField] private Image healthImage, shieldImage;


    public void UpdateHealthBar(float currentHP, float maxHP, float shields, float maxShields)
    {
        
        if (shields > 0)
        {
            shieldImage.fillAmount = shields / maxShields;
            shieldText.text = shields.ToString();
        }

        healthImage.fillAmount = currentHP / maxHP;
        healthText.text = currentHP.ToString();

    }





}
