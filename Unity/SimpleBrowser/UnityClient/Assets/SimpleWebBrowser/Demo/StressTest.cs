using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleWebBrowser;

public class StressTest : MonoBehaviour
{
    public RectTransform browserTf;
    public int width = 1024;
    public int height = 768;
    public int count = 1;

    public int deltaX = 10;
    public int deltaY = 10;

    string[] urlVariation = {
        "https://google.com",
         "https://naver.com",
          "https://ncsoft.com",
           "https://unity.com",
            "https://bitbucket.org/vitaly_chashin/simpleunitybrowser/src/default/UnityClient/Assets/SimpleWebBrowser/PluginServer",
            "http://danawa.com/",
            "https://www.daum.net/",
            "https://www.nate.com/",
            "https://leetcode.com/",
            "https://kr.louisvuitton.com/kor-kr/homepage",
             };

    private string strWidth;
    private string strHeight;
    private string strCount;

    private List<WebBrowser> createdBrowser = new List<WebBrowser>();
    
    private void Start() {
        strWidth = width.ToString();    
        strHeight = height.ToString();
        strCount = count.ToString();
    }
    private void OnGUI() {

        GUILayout.BeginHorizontal();

        strWidth = GUILayout.TextField(strWidth, GUILayout.Width(Screen.width * 0.2f));
        strHeight = GUILayout.TextField(strHeight, GUILayout.Width(Screen.width * 0.2f));
        strCount = GUILayout.TextField(strCount, GUILayout.Width(Screen.width * 0.2f));

        if ( GUILayout.Button("Start Test") ) 
        {
            width = int.Parse(strWidth);
            height = int.Parse(strHeight);
            count = int.Parse(strCount);

            for (int i = 0; i < count; i++)
            {
                var newBrowserGo = GameObject.Instantiate(browserTf.gameObject, Vector3.zero, Quaternion.identity, browserTf.parent );
                newBrowserGo.SetActive(false);

                RectTransform transform = newBrowserGo.GetComponent<RectTransform>();
                transform.Translate(deltaX * i, deltaY* i , 0);

                // #region For WebBrowser2D
                // WebBrowser2D browser = newBrowserGo.GetComponent<WebBrowser2D>();
                // browser.Width = width;
                // browser.Height = height;

                // // browser.InitialURL = urlVariation[i%urlVariation.Length];
                // browser.InitialURL = "";
                // #endregion

                // WebBrowser browserReal = newBrowserGo.GetComponent<WebBrowser>();
                // browserReal.width = width;
                // browserReal.height = height;

                // newBrowserGo.SetActive(true);

                // browserReal.Init(false, null, null, null, "");

                // browserReal.LoadUrl(urlVariation[i%urlVariation.Length]);

                var webViewObject = newBrowserGo.GetComponent<WebViewObject>();

                webViewObject.Init(null, false, "Hello World");
                webViewObject.SetMargins(50, 50, 50, 50);

                webViewObject.SetVisibility(false);
                var webBrowser = webViewObject.GetComponent<WebBrowser>();

                newBrowserGo.SetActive(true);
                webViewObject.LoadURL(urlVariation[i%urlVariation.Length]);
                webViewObject.SetVisibility(true);

                createdBrowser.Add(webBrowser);
            }
        }

        if ( GUILayout.Button("CLear") )
        {
            foreach (var item in createdBrowser)
            {
                item.gameObject.SetActive(false);
                GameObject.Destroy(item.gameObject);
            }

            createdBrowser.Clear();
        }
        GUILayout.EndHorizontal();
    }
}
