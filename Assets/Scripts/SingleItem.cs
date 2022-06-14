using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleItem : MonoBehaviour
{
    InputField inputField_Start;
    InputField inputField_End;

    public DateTime startTime;
    public DateTime endTime;

    public string startTimeText 
    {
        set {
            inputField_Start.text = value;
        }
    }
    public string endTimeText
    {
        set
        {
            inputField_End.text = value;
        }
    }

    void Awake()
    {
        inputField_Start = transform.Find("Item_StartDate").GetComponentInChildren<InputField>();
        startTime = DateTime.Now;
        inputField_Start.text = DateTime.Now.ToString("yyyy年MM月dd日");

        inputField_End = transform.Find("Item_EndDate").GetComponentInChildren<InputField>();

        //inputField_Start.onEndEdit.AddListener(ChangeStartTime);
        //inputField_End.onEndEdit.AddListener(ChangeEndTime);

      
    }
    public void RefreshData()
    {
        DateTime.TryParseExact(inputField_Start.text, "yyyy年MM月dd日",
        System.Globalization.CultureInfo.InvariantCulture,
        System.Globalization.DateTimeStyles.None,
        out startTime);

        DateTime.TryParseExact(inputField_End.text, "yyyy年MM月dd日",
         System.Globalization.CultureInfo.InvariantCulture,
         System.Globalization.DateTimeStyles.None,
         out endTime);
    }
    public void ChangeStartTime(string t)
    {
    
        if(!string.IsNullOrEmpty(t))
        {
            DateTime.TryParseExact(/*inputField_Start.text*/t, "yyyy年MM月dd日",
          System.Globalization.CultureInfo.InvariantCulture,
          System.Globalization.DateTimeStyles.None,
          out  startTime);

            print("输入框数值改变:" + startTime.ToString());
        }
       
    }

    public void ChangeEndTime(string t)
    {

        if (!string.IsNullOrEmpty(t))
        {
            DateTime.TryParseExact(/*inputField_Start.text*/t, "yyyy年MM月dd日",
          System.Globalization.CultureInfo.InvariantCulture,
          System.Globalization.DateTimeStyles.None,
          out endTime);

            print("输入框数值改变:" + endTime.ToString());
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
