using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowItemDetail : MonoBehaviour
{
    public Image itemIcon;
    public Text itemName;

    public RectTransform timeBar;

    public Button button_Editor;
    public Button button_Delete;
    void Start()
    {
        button_Editor.onClick.AddListener(OnTouchEditor);
        button_Delete.onClick.AddListener(OnTouchDelete);
    }

    private void OnTouchDelete()
    {
        MainManager.Instance.DeleteItem(itemName.text);
        gameObject.SetActive(false);
    }

    private void OnTouchEditor()
    {
        MainManager.Instance.FixEditor(itemName.text);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
