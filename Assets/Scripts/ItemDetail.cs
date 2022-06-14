using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetail : MonoBehaviour
{
    public Image itemIcon;
    public Text itemName;
    public Text itemHistoryCount;
    public Text itemUsingDay;

    Button imageButton;

    void Awake()
    {
        itemIcon = GetComponentInChildren<Image>();
        itemName = transform.Find("ItemName").GetComponent<Text>();
        itemHistoryCount = transform.Find("ItemHistoryCount").GetComponent<Text>();
        itemUsingDay = transform.Find("ItemUsingDay").GetComponent<Text>();

        imageButton = GetComponentInChildren<Button>();
        imageButton.onClick.AddListener(OpenItemDetail);
    }

    private void OpenItemDetail()
    {
        print($"打开{itemName}物品详情页");
        MainManager.Instance.ShowItemDetailPage(itemName.text.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
