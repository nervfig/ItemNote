using System.Collections;
using UnityEngine;
using System.IO;
using System;
using ZXing;
using ZXing.QrCode;
using UnityEngine.UI;

public class MyQR : MonoBehaviour
{
    public static MyQR Instance;
    private void Awake()
    {
        Instance = this;
    }
    /// <summary>
    /// 返回指定URL的二维码
    /// </summary>
    /// <param name="str">url,比如http://www.xxx.com/showpic.php?filename=1001.jpg</param>
    /// <returns></returns>
    public Texture2D ShowCode(string str)
    {
        Texture2D encoded = new Texture2D(256, 256);
        //QRCodes = str;
        var textForEncoding = str;
        if (textForEncoding != null)
        {
            //二维码写入图片
            var color32 = Encode1(textForEncoding, encoded.width, encoded.height);
            encoded.SetPixels32(color32);
            encoded.Apply();
            print("二维码生成完毕"); 
            return encoded;
        }
        return null;
    }

    //定义方法生成二维码
    private static Color32[] Encode1(string textForEncoding, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }
}