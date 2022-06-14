using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchOpenCalendar : MonoBehaviour, IPointerClickHandler
{
    InputField resultText;
    public void OnPointerClick(PointerEventData eventData)
    {
        MainManager.Instance.ShowCalendar(resultText);
    }

    // Start is called before the first frame update
    void Start()
    {
        resultText = GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
