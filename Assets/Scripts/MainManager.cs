using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.Text.RegularExpressions;
using Random = UnityEngine.Random;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    /// <summary>
    /// 创建按钮
    /// </summary>
    public Button button_Create;

    public GameObject editorPageGO;
    public InputField editorPageItemName;
    public GameObject itemDetailPageGO;


    Dictionary<string, List<MyItem>> itemDic = new Dictionary<string, List<MyItem>>();


    public Button button_AddItemHistory;
    public Button button_ConfirmEditor;
    public Button button_CancelEditor;
    /// <summary>
    /// 物品记录预制件
    /// </summary>
    public GameObject itemHistoryPrefab;
    /// <summary>
    /// 显示物品使用记录的父物体Context
    /// </summary>
    public Transform showHistoryTrans;
    /// <summary>
    /// 物品记录临时保存列表
    /// </summary>
    List<SingleItem> itemHistoryTempList = new List<SingleItem>();


    public GameObject itemDetailPrefab;
    public Transform itemDetailTrans;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        button_Create.onClick.AddListener(() =>
        {
            editorPageGO.gameObject.SetActive(true);
            foreach (var item in itemHistoryTempList)
            {
                Destroy(item.gameObject);
            }
            itemHistoryTempList.Clear();

        });

        button_AddItemHistory.onClick.AddListener(() => AddItemHistory());

        button_ConfirmEditor.onClick.AddListener(() => ConfirmEditor());
        button_CancelEditor.onClick.AddListener(() => CancelEditor());

        //读取TXT
        TXTManager.Instance.CreateFileInPersistentData("ItemLog.txt", JsonMapper.ToJson(itemDic));

        string s = TXTManager.Instance.LoadTXTtoString("ItemLog.txt");
        itemDic = JsonMapper.ToObject<Dictionary<string, List<MyItem>>>(s);
        RefreshHomePage();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //print(JsonUtility.ToJson(new Serialization<string, List<MyItem>>(itemDic)));
            print(Regex.Unescape(JsonMapper.ToJson(itemDic)));
            //Dictionary<string, List<MyItem>> newDic = new Dictionary<string, List<MyItem>>();
            //newDic= JsonMapper.ToObject<Dictionary<string, List<MyItem>>>(JsonMapper.ToJson(itemDic));
            //TXTManager.Instance.CreateFileInPersistentData("ItemLog.txt", JsonUtility.ToJson(new Serialization<string, List<MyItem>>(itemDic)));
        }
    }
    private void CancelEditor()
    {
        editorPageGO.gameObject.SetActive(false);
        foreach (var item in itemHistoryTempList)
        {
            Destroy(item.gameObject);
        }
        itemHistoryTempList.Clear();

        //更新主页物品详情记录
        RefreshHomePage();
    }

    void ConfirmEditor()
    {
        if (itemHistoryTempList.Count > 0)
        {
            List<MyItem> tempList = new List<MyItem>();
            for (int i = 0; i < itemHistoryTempList.Count; i++)
            {
                tempList.Add(new MyItem(itemHistoryTempList[i].startTime, itemHistoryTempList[i].endTime));
            }
            foreach (var item in itemHistoryTempList)
            {
                Destroy(item.gameObject);
            }
            itemHistoryTempList.Clear();


            if (itemDic.ContainsKey(editorPageItemName.text))
                itemDic.Remove(editorPageItemName.text);

            itemDic.Add(editorPageItemName.text, tempList);
        }
        editorPageGO.gameObject.SetActive(false);

        //新添加的数据写入TXT
        TXTManager.Instance.WriteTXT("ItemLog.txt", JsonMapper.ToJson(itemDic));

        //更新主页物品详情记录
        RefreshHomePage();

    }
    /// <summary>
    /// 已经编辑好的物品修改方法
    /// </summary>
    public void FixEditor(string itemName)
    {
        editorPageGO.gameObject.SetActive(true);
        if(itemDic.TryGetValue(itemName, out var singleItemList))
        {
            editorPageItemName.text = itemName;
        }

        foreach (var item in singleItemList)
        {
            var go = Instantiate(itemHistoryPrefab, showHistoryTrans).GetComponent<SingleItem>();
            go.startTimeText = item.startTime.ToString("yyyy年MM月dd日");
            go.endTimeText = item.endTime.ToString("yyyy年MM月dd日");
            itemHistoryTempList.Add(go.GetComponent<SingleItem>());
        }
    }

    private void RefreshHomePage()
    {
        foreach (Transform itemOld in itemDetailTrans)
        {
            Destroy(itemOld.gameObject);
        }
        foreach (var item in itemDic)
        {
            var itemDetail = Instantiate(itemDetailPrefab, itemDetailTrans).GetComponent<ItemDetail>();
            itemDetail.itemName.text = item.Key;
            itemDetail.itemHistoryCount.text = $"{item.Value.Count}";
            itemDetail.itemUsingDay.text = (DateTime.Now - item.Value[item.Value.Count - 1].startTime).Days.ToString() + "天";
        }
    }

    /// <summary>
    /// 添加物品历史使用记录方法
    /// </summary>
    void AddItemHistory()
    {
        //editor页面中按字典生成相应的列表
        //ShowItemHistoryInDic();
        var go = Instantiate(itemHistoryPrefab, showHistoryTrans);
        //TODO:生成记录添加到临时list
        itemHistoryTempList.Add(go.GetComponent<SingleItem>());

    }
    /// <summary>
    /// 显示之前物品记录
    /// </summary>
    /// <param name="itemName">物品名称</param>
    void ShowItemHistoryInDic(string itemName)
    {
        foreach (Transform item in showHistoryTrans)
        {
            Destroy(item.gameObject);
        }
        if (itemDic.TryGetValue(itemName, out var itemTemp))
        {
            foreach (var item in itemTemp)
            {
                var go = Instantiate(itemHistoryPrefab, showHistoryTrans);
                if (go.TryGetComponent<SingleItem>(out var singleItem))
                {
                    singleItem.startTimeText = item.startTime.ToString("yyyy年MM月dd日");
                    if (item.endTime != null)
                        singleItem.endTimeText = item.endTime.ToString("yyyy年MM月dd日");
                }
            }
        }

    }


    public RectTransform timeBar;
    public GameObject timePointPrefab;
    public GameObject timePointUseDaysPrefab;
    public Text itemNameText;
    DateTime defaultTime;
    /// <summary>
    /// 主页显示物品详情方法
    /// </summary>
    /// <param name="itemName">物品的名字</param>
    public void ShowItemDetailPage(string itemName)
    {
        itemDetailPageGO.SetActive(true);
        itemDic.TryGetValue(itemName, out var tempList);

        itemNameText.text = itemName;

        //timeBar.sizeDelta = new Vector2(6,
        //    ((tempList[tempList.Count - 1].endTime == null ? tempList[tempList.Count - 1].endTime : DateTime.Now) - tempList[0].startTime).Days *20+100);
        timeBar.sizeDelta = new Vector2(6, tempList.Count*100);

        foreach (Transform item in timeBar)
        {
            Destroy(item.gameObject);
        }
        
        foreach (var item in tempList)
        {
            Color randomColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));

            var startPoint = Instantiate(timePointPrefab, timeBar).GetComponent<TimePoint>();
            startPoint.pointDate.text = item.startTime.ToString("MM/dd");
            startPoint.transform.localPosition = new Vector3(0, tempList.IndexOf(item) * -50);
            startPoint.GetComponent<Image>().color = randomColor;

            var usingDays = Instantiate(timePointUseDaysPrefab, timeBar).GetComponent<Text>();
            usingDays.text = $"已用{(item.endTime == defaultTime ? DateTime.Now - item.startTime : item.endTime-item.startTime).Days}天     "+(item.endTime== defaultTime?"使用中":"已用完");
            usingDays.transform.localPosition= new Vector3(142, (tempList.IndexOf(item)+1) * -50);

            var endPoint = Instantiate(timePointPrefab, timeBar).GetComponent<TimePoint>();
            endPoint.pointDate.text = item.endTime == defaultTime ? DateTime.Now.ToString("MM/dd") : item.endTime.ToString("MM/dd");
            endPoint.transform.localPosition = new Vector3(0, (tempList.IndexOf(item)+2) * -50);
            endPoint.GetComponent<Image>().color = randomColor;
        }
    }

    public GameObject CalendarGO;
    public void ShowCalendar(InputField resultText)
    {
        CalendarGO.SetActive(true);
        MyCanlendar.Instance.outputResultText = resultText;
        MyCanlendar.Instance.ShowCanlendar();
    }

    public void DeleteItem(string itemName)
    {
        itemDic.Remove(itemName);
        ConfirmEditor();
        print($"{itemName}已删除");
    }
}
[Serializable]
public class MyItem
{
    //public string name;
    public DateTime startTime;
    public DateTime endTime;

    public MyItem()
    {

    }
    public MyItem(DateTime startTime, DateTime endTime)
    {
        this.startTime = startTime;
        this.endTime = endTime;
    }
}

//[Serializable]
//public class Serialization<T>
//{
//    [SerializeField]
//    List<T> target;
//    public List<T> ToList() { return target; }

//    public Serialization(List<T> target)
//    {
//        this.target = target;
//    }
//}

//// Dictionary<TKey, TValue>
//[Serializable]
//public class Serialization<TKey, TValue> : ISerializationCallbackReceiver
//{
//    [SerializeField]
//    List<TKey> keys;
//    [SerializeField]
//    List<TValue> values;

//    Dictionary<TKey, TValue> target;
//    public Dictionary<TKey, TValue> ToDictionary() { return target; }

//    public Serialization(Dictionary<TKey, TValue> target)
//    {
//        this.target = target;
//    }

//    public void OnBeforeSerialize()
//    {
//        keys = new List<TKey>(target.Keys);
//        values = new List<TValue>(target.Values);
//    }

//    public void OnAfterDeserialize()
//    {
//        var count = Math.Min(keys.Count, values.Count);
//        target = new Dictionary<TKey, TValue>(count);
//        for (var i = 0; i < count; ++i)
//        {
//            target.Add(keys[i], values[i]);
//        }
//    }
//}
