using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private RectTransform playerIcon;
    private float roomSize;


    public void init(float r)
    {
        roomSize = r;
    }

    // Update is called once per frame
    void Update()
    {
        playerIcon.anchoredPosition = new Vector2(player.transform.position.x, player.transform.position.z) * 2 / roomSize;
    }
}
