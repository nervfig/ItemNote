using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimePoint : MonoBehaviour
{
    Image pointImage;
    public Text pointDate;
    void Awake()
    {
        pointImage = GetComponent<Image>();
        pointDate = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
