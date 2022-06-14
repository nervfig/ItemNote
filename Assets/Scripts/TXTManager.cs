using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class TXTManager : MonoBehaviour
{
    public static TXTManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }
    
    public void CreateFileInPersistentData(string fileName, string content)
    {
        string path = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(path))
        {
            return;
        }
        else
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            fs.Close();
            WriteTXT(fileName, Regex.Unescape(content));
        }
        print("TXT写入完成||"+ Regex.Unescape(content));
    }
    public void WriteTXT(string fileName, string s)
    {
        string url = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            url = Application.persistentDataPath + "/" + fileName;
            File.WriteAllText(url, Regex.Unescape(s));
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            url = Application.persistentDataPath + "/" + fileName;
            FileStream fs = new FileStream(url, FileMode.Create);
            byte[] bytes = new UTF8Encoding().GetBytes(Regex.Unescape(s).ToString());
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }
    }

    public string LoadTXTtoString(string fileName)
    {
        string url = Application.persistentDataPath + "/" + fileName;
        string s = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            s = File.ReadAllText(url);
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            FileStream fs = new FileStream(url, FileMode.Open);
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            //将读取到的二进制转换成字符串
            s = new UTF8Encoding().GetString(bytes);
            fs.Close();
        }
        print("读取到的数据为："+s);
        return s;
    }
}
