using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyCanlendar : MonoBehaviour
{
    public static MyCanlendar Instance;

    Button leftArrow;
    Button rightArrow;

    public DateTime nowCanlendarDate;

    Text nowCanlendarDateText;

    public GameObject daysPrefab;
    public GameObject daysPrefab_Empty;

    Transform canlendar_Bottom;

    public InputField outputResultText;
    private void Awake()
    {
        Instance = this;
    //}
    //void Start()
    //{
        nowCanlendarDate = DateTime.Now;

        leftArrow = transform.Find("Canlendar_Top").Find("Button_LeftArrow").GetComponent<Button>();
        rightArrow = transform.Find("Canlendar_Top").Find("Button_RightArrow").GetComponent<Button>();

        leftArrow.onClick.AddListener(() => OnMonthButtonClick(-1));
        rightArrow.onClick.AddListener(() => OnMonthButtonClick(1));

        nowCanlendarDateText = transform.Find("Canlendar_Top").transform.Find("NowDay").GetComponent<Text>();
        nowCanlendarDateText.text = nowCanlendarDate.ToString("yyyy.MM.dd");

        canlendar_Bottom = transform.Find("Canlendar_Bottom");

        RefreshCanlendar();
    }
    public void ShowCanlendar()
    {


        if (!DateTime.TryParseExact(outputResultText.text, "yyyy年MM月dd日",
        System.Globalization.CultureInfo.InvariantCulture,
        System.Globalization.DateTimeStyles.None,
        out nowCanlendarDate))
            nowCanlendarDate = DateTime.Now;


        RefreshCanlendar();
    }

    private void OnMonthButtonClick(int addMonthCount)
    {
        nowCanlendarDate = nowCanlendarDate.AddMonths(addMonthCount);

        nowCanlendarDateText.text = nowCanlendarDate.ToString("yyyy.MM.dd");
        print($"nowCanlendarDate:+{nowCanlendarDate.ToString()}");

        RefreshCanlendar();

    }

    Toggle nowDayToggle;
    List<Toggle> allToggleList = new List<Toggle>();

    private void RefreshCanlendar()
    {
        for (int i = 7; i < canlendar_Bottom.childCount; i++)
        {
            Destroy(canlendar_Bottom.GetChild(i).gameObject);
        }
        allToggleList.Clear();



        DateTime.TryParseExact(nowCanlendarDate.ToString("yyyy.MM.01"), "yyyy.MM.dd",
        System.Globalization.CultureInfo.InvariantCulture,
        System.Globalization.DateTimeStyles.None,
        out var monthStartDay);
        for (int i = 1; i < (int)monthStartDay.DayOfWeek; i++)
        {
            var daysGO = Instantiate(daysPrefab_Empty, canlendar_Bottom);
        }
        nowCanlendarDateText.text = nowCanlendarDate.ToString("yyyy.MM.dd");
        for (int i = 0; i < DateTime.DaysInMonth(nowCanlendarDate.Year, nowCanlendarDate.Month); i++)
        {
            var daysGO = Instantiate(daysPrefab, canlendar_Bottom);
            daysGO.GetComponent<Text>().text = (i + 1).ToString();
            daysGO.name = (i + 1).ToString();
            daysGO.GetComponent<Toggle>().group = canlendar_Bottom.GetComponent<ToggleGroup>();



                        //除当天外全部为off
            if (nowCanlendarDate.Day==i+1)
            {
                //daysGO.GetComponent<Toggle>().isOn = true;
                print("小奶茶鼠显示时间为:"+(i+1));

                nowDayToggle = daysGO.GetComponent<Toggle>();
            }


            allToggleList.Add(daysGO.GetComponent<Toggle>());

            //daysGO.GetComponent<Toggle>().onValueChanged.AddListener(isOn => 
            //{
            //    if(isOn)
            //    {
            //        nowCanlendarDate = new DateTime(nowCanlendarDate.Year, nowCanlendarDate.Month, int.Parse(daysGO.name));
            //        nowCanlendarDateText.text = nowCanlendarDate.ToString("yyyy.MM.dd");
            //        if (outputResultText)
            //        {
            //            outputResultText.text = nowCanlendarDate.ToString("yyyy年MM月dd日");


            //            outputResultText.transform.parent.GetComponentInParent<SingleItem>().RefreshData();

            //            outputResultText = null;
            //            gameObject.SetActive(false);
            //        }
                       
            //    }
          

            //    //gameObject.SetActive(false);
            //});

        }
        //开启当前日期奶茶鼠后再添加点击事件
        nowDayToggle.isOn = true;
        foreach (var toggle in allToggleList)
        {
            toggle.onValueChanged.AddListener(isOn =>
            {
                if (isOn)
                {
                    nowCanlendarDate = new DateTime(nowCanlendarDate.Year, nowCanlendarDate.Month, int.Parse(toggle.name));
                    nowCanlendarDateText.text = nowCanlendarDate.ToString("yyyy.MM.dd");
                    if (outputResultText)
                    {
                        outputResultText.text = nowCanlendarDate.ToString("yyyy年MM月dd日");


                        outputResultText.transform.parent.GetComponentInParent<SingleItem>().RefreshData();

                        outputResultText = null;
                        gameObject.SetActive(false);
                    }

                }


                //gameObject.SetActive(false);
            });
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
