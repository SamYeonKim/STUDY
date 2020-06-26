using MessageLibrary;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(RectTransform))]
public class WebBrowser : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,IPointerUpHandler    
{
    System.Diagnostics.Stopwatch watch;
    public bool visibility { get; private set; }
    private bool backgroundTransparent = false;
    private int width = 1024;
    private int height = 768;
    private string memoryFile = "MainSharedMem";
    private string jSInitializationCode = "";
    private RawImage rawImage = null;
    private string requestUrl;
    private string lastLoadedUrl = "";
    private SimpleWebBrowser.BrowserEngine mainEngine;
    private bool inputFocused = false;
    private int mousePosX = 0;
    private int mousePosY = 0;
    private Action<string> callOnError;
    private Action<string> callOnLoaded;
    private Action<string> callFromJS;
    private string userAgent;

    private double loadingTime;

    public void Init(int width, int height, bool transparent, Action<string> callOnError, Action<string> callOnLoad, Action<string> callFromJS, string userAgent)
    {
        watch = new System.Diagnostics.Stopwatch();
        this.callOnError = callOnError;
        this.callOnLoaded = callOnLoad;
        this.callFromJS = callFromJS;
        this.width = width;
        this.height = height;

        rawImage = gameObject.GetComponent<RawImage>();

        //Margin 설정을 편하게 하기 위해서, RectTransform의 Anchor 설정을 All Stretch 상태로 강제한다.
        var rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);

        backgroundTransparent = transparent;

        mainEngine = new SimpleWebBrowser.BrowserEngine();

        Guid memid = Guid.NewGuid();
        memoryFile = memid.ToString();

        jSInitializationCode = @"window.Unity = { call: function(msg) { window.cefQuery({ request: msg, onSuccess: function(response) { console.log(response); }, onFailure: function(err,msg) { console.log(err, msg); } }); }}";
        
        if (jSInitializationCode.Trim() != "")
            mainEngine.RunJSOnce(jSInitializationCode);

        mainEngine.OnJavaScriptQuery += OnJavaScriptQuery;
        mainEngine.OnPageLoaded += OnPageLoaded;
        mainEngine.OnPageLoadedError += OnPageLoadedError;

        this.userAgent = userAgent;
        visibility = true;
    }
    public void RunJavaScript(string js)
    {
        mainEngine.SendExecuteJSEvent(js);
    }
    public void LoadUrl(string url)
    {
        watch.Start();
        if ( mainEngine == null )
        {
            Debug.Log("MainEngine is NULL");
            return;
        }

        if ( !mainEngine.Initialized )
        {
            StartCoroutine(CoLoadUrl(url));
        }
        else
        {
            mainEngine.SendNavigateEvent(url, false, false);
            this.requestUrl = url;
        }
    }
    public void LoadHtml(string htmlContent, string baseUrl = "html")
    {
        if ( mainEngine == null )
        {
            Debug.Log("MainEngine is NULL");
            return;
        }

        if ( !mainEngine.Initialized )
        {
            StartCoroutine(CoLoadHtml(htmlContent, baseUrl));
        }
        else
        {
            mainEngine.SendNavigateEventForLoadHtml(htmlContent);
            this.requestUrl = baseUrl;
        }
    }
    public void SetMargin(int left, int top, int right, int bottom)
    {
        var rectTransform = this.GetComponent<RectTransform>();

        int width = Screen.width;
        int height = Screen.height;

        float leftRate = left / (float)width;
        float rightRate = right / (float)width;
        float topRate = top / (float)height;
        float bottomRate = bottom / (float)height;

        float leftResult = leftRate + rightRate < 1 ? leftRate * width : 0.0f;
        float rightResult = leftRate + rightRate < 1 ? rightRate * -width : 0.0f;
        float topResult = topRate + bottomRate < 1 ? topRate * -height : 0.0f;
        float bottomResult = topRate + bottomRate < 1 ? bottomRate * height : 0.0f;

        rectTransform.offsetMin = new Vector2(leftResult, bottomResult);
        rectTransform.offsetMax = new Vector2(rightResult, topResult);
    }
    public void SetVisibility(bool visibility)
    {
        if ( string.IsNullOrEmpty(lastLoadedUrl) )
            return;

        this.visibility = visibility;   
        rawImage.enabled = visibility;
    }
    public void GoBack()
    {
        mainEngine.SendNavigateEvent("", true, false);
    }
    public void GoForward()
    {
        mainEngine.SendNavigateEvent("", false, true);
    }
    public bool CanGoForward()
    {
        if ( mainEngine == null )
        {
            return false;
        }
        
        return mainEngine.CanGoForward;
    }
    public bool CanGoBack()
    {
        if ( mainEngine == null )
        {
            return false;
        }

        return mainEngine.CanGoBack;
    }
    
    private void OnDisable()
    {
        if ( mainEngine != null )
        {
            mainEngine.Shutdown();
        }        
    }
    private void OnPageLoaded(string url)
    {
        if(callOnLoaded != null)
        {
            callOnLoaded(url);
        }

        lastLoadedUrl = url;
        if ( !rawImage.enabled ) 
        {
            rawImage.enabled = true;
        }

        watch.Stop();
        loadingTime = watch.Elapsed.TotalSeconds;
        Debug.Log("OnPageLoaded : " + url + ", elapsed : " + loadingTime);
        watch.Reset();

        RunJavaScript(@"window.Unity.call(navigator.userAgent);");
    }
    private void OnPageLoadedError(Xilium.CefGlue.CefErrorCode errorCode, string errorText, string errorUrl)
    {
        if(callOnError != null)
        {
            callOnError(errorCode.ToString());
        }

        Debug.Log(string.Format("OnPageLoadedError : Code : {0}, Msg : {1}, Url : {2}", errorCode.ToString(), errorText, errorUrl));
    }
    private void OnJavaScriptQuery(string message)
    {
        if(callFromJS != null)
        {
            callFromJS(message);
        }
        Debug.Log("OnJavaScriptQuery : " + message);
    }
    private IEnumerator CoLoadUrl(string url)
    {       
        yield return mainEngine.InitPlugin(width, height, memoryFile, url, backgroundTransparent, userAgent);
        rawImage.texture = mainEngine.BrowserTexture;
        rawImage.uvRect = new Rect(0f, 0f, 1f, -1f);

        this.requestUrl = url;
    }

    private IEnumerator CoLoadHtml(string html, string baseUrl)
    {
        yield return mainEngine.InitPlugin(width, height, memoryFile, baseUrl, backgroundTransparent, userAgent, html);
        rawImage.texture = mainEngine.BrowserTexture;
        rawImage.uvRect = new Rect(0f, 0f, 1f, -1f);

        this.requestUrl = baseUrl;
    }

#region UGUI Mouse Event
    public void OnPointerEnter(PointerEventData data)
    {
        inputFocused = true;
        StartCoroutine(TrackPointer());
    }

    public void OnPointerExit(PointerEventData data)
    {
        inputFocused = false;
        StopCoroutine(TrackPointer());
    }    
    public void OnPointerDown(PointerEventData data)
    {
        if ( mainEngine.Initialized )
        {
            var _raycaster = GetComponentInParent<GraphicRaycaster>();
            var _input = FindObjectOfType<StandaloneInputModule>();
            Vector2 pixelUV = GetScreenCoords(_raycaster, _input);

            switch ( data.button )
            {
                case PointerEventData.InputButton.Left:
                {
                    SendMouseButtonEvent((int)pixelUV.x, (int)pixelUV.y, MouseButton.Left,
                        MouseEventType.ButtonDown);
                    break;
                }
                case PointerEventData.InputButton.Right:
                {
                    SendMouseButtonEvent((int)pixelUV.x, (int)pixelUV.y, MouseButton.Right,
                        MouseEventType.ButtonDown);
                    break;
                }
                case PointerEventData.InputButton.Middle:
                {
                    SendMouseButtonEvent((int)pixelUV.x, (int)pixelUV.y, MouseButton.Middle,
                        MouseEventType.ButtonDown);
                    break;
                }
            }
        }
    }
    public void OnPointerUp(PointerEventData data)
    {
        if ( mainEngine.Initialized )
        {
            var _raycaster = GetComponentInParent<GraphicRaycaster>();
            var _input = FindObjectOfType<StandaloneInputModule>();

            Vector2 pixelUV = GetScreenCoords(_raycaster, _input);

            switch ( data.button )
            {
                case PointerEventData.InputButton.Left:
                {
                    SendMouseButtonEvent((int)pixelUV.x, (int)pixelUV.y, MouseButton.Left, MouseEventType.ButtonUp);
                    break;
                }
                case PointerEventData.InputButton.Right:
                {
                    SendMouseButtonEvent((int)pixelUV.x, (int)pixelUV.y, MouseButton.Right,
                        MouseEventType.ButtonUp);
                    break;
                }
                case PointerEventData.InputButton.Middle:
                {
                    SendMouseButtonEvent((int)pixelUV.x, (int)pixelUV.y, MouseButton.Middle,
                        MouseEventType.ButtonUp);
                    break;
                }
            }
        }
    }    
#endregion
    IEnumerator TrackPointer()
    {
        var _raycaster = GetComponentInParent<GraphicRaycaster>();
        var _input = FindObjectOfType<StandaloneInputModule>();

        if ( _raycaster != null && _input != null && mainEngine.Initialized )
        {
            while ( Application.isPlaying )
            {
                Vector2 localPos = GetScreenCoords(_raycaster, _input);

                int px = (int)localPos.x;
                int py = (int)localPos.y;

                ProcessScrollInput(px, py);

                if ( mousePosX != px || mousePosY != py )
                {
                    MouseMessage msg = new MouseMessage
                    {
                        Type = MouseEventType.Move,
                        X = px,
                        Y = py,
                        GenericType = MessageLibrary.BrowserEventType.Mouse,
                        // Delta = e.Delta,
                        Button = MouseButton.None
                    };

                    if ( Input.GetMouseButton(0) )
                    {
                        msg.Button = MouseButton.Left;
                    }
                    if ( Input.GetMouseButton(1) )
                    {
                        msg.Button = MouseButton.Right;
                    }
                    if ( Input.GetMouseButton(1) )
                    {
                        msg.Button = MouseButton.Middle;
                    }
                        
                    mousePosX = px;
                    mousePosY = py;
                    mainEngine.SendMouseEvent(msg);
                }

                yield return 0;
            }
        }
    }
    private Vector2 GetScreenCoords(GraphicRaycaster ray, StandaloneInputModule input)
    {
        Vector2 localPos; // Mouse position  
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition,
            ray.eventCamera, out localPos);

        // local pos is the mouse position.
        RectTransform trns = transform as RectTransform;
        localPos.y = trns.rect.height - localPos.y;

        //now recalculate to texture
        localPos.x = (localPos.x * width) / trns.rect.width;
        localPos.y = (localPos.y * height) / trns.rect.height;

        return localPos;
    }
    private void SendMouseButtonEvent(int x, int y, MouseButton btn, MouseEventType type)
    {
        MouseMessage msg = new MouseMessage
        {
            Type = type,
            X = x,
            Y = y,
            GenericType = MessageLibrary.BrowserEventType.Mouse,
            Button = btn
        };
        mainEngine.SendMouseEvent(msg);
    }
    private void ProcessScrollInput(int px, int py)
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        scroll = scroll * mainEngine.BrowserTexture.height;

        int scInt = (int)scroll;

        if ( scInt != 0 )
        {
            MouseMessage msg = new MouseMessage
            {
                Type = MouseEventType.Wheel,
                X = px,
                Y = py,
                GenericType = MessageLibrary.BrowserEventType.Mouse,
                Delta = scInt,
                Button = MouseButton.None
            };

            if ( Input.GetMouseButton(0) )
            {
                msg.Button = MouseButton.Left;
            }
            if ( Input.GetMouseButton(1) )
            {
                msg.Button = MouseButton.Right;
            }
            if ( Input.GetMouseButton(1) )
            {
                msg.Button = MouseButton.Middle;
            }

            mainEngine.SendMouseEvent(msg);
        }
    }

    void Update()
    {
        if ( string.IsNullOrEmpty(requestUrl) )
            return;

        if ( !visibility )
            return;

        mainEngine.UpdateTexture();
        mainEngine.CheckMessage();

        if ( !mainEngine.Initialized )
            return;

        if ( inputFocused )
        {
            foreach ( char c in Input.inputString )
            {
                mainEngine.SendCharEvent((int)c, KeyboardEventType.CharKey);
            }
            ProcessKeyEvents();
        }
    }

    private void ProcessKeyEvents()
    {
        foreach ( KeyCode k in Enum.GetValues(typeof(KeyCode)) )
        {
            CheckKey(k);
        }
    }
    private void CheckKey(KeyCode code)
    {
        if ( Input.GetKeyDown(code) )
        {
            mainEngine.SendCharEvent((int)code, KeyboardEventType.Down);
        }                
        if ( Input.GetKeyUp(KeyCode.Backspace) )
        {
            mainEngine.SendCharEvent((int)code, KeyboardEventType.Up);
        }
    }
    
    private void OnGUI()
    {
        GUI.Label(new Rect(Screen.width * 0.8f, 0, Screen.width * 0.2f, Screen.height * 0.1f), loadingTime.ToString());
        requestUrl = GUI.TextField(new Rect(Screen.width * 0.8f, Screen.height * 0.1f, Screen.width * 0.15f, Screen.height * 0.1f), requestUrl);
        if ( string.IsNullOrEmpty(requestUrl) )
            return;

        if ( GUI.Button(new Rect(Screen.width * 0.95f, Screen.height * 0.1f, Screen.width * 0.05f, Screen.height * 0.1f), "LoadURL") )
        {
            LoadUrl(requestUrl);
        }
        
        if ( GUI.Button(new Rect(Screen.width * 0.95f, Screen.height * 0.2f, Screen.width * 0.05f, Screen.height * 0.1f), "LoadHTML") )
        {
            LoadHtml("<!DOCTYPE html><html><body><h1>My Second Heading</h1><p>My second paragraph.</p></body></html>");
        }

        if ( GUI.Button(new Rect(Screen.width * 0.95f, Screen.height * 0.3f, Screen.width * 0.05f, Screen.height * 0.1f), "SetMargin") )
        {
            SetMargin((int)(Screen.width * 0.2f), (int)(Screen.height * 0.2f), (int)(Screen.width * 0.2f), (int)(Screen.height * 0.2f));
        }

        if ( mainEngine != null )
        {
            if ( mainEngine.CanGoBack )
            {
                if ( GUI.Button(new Rect(Screen.width * 0.9f, Screen.height * 0.4f, Screen.width * 0.05f, Screen.height * 0.1f), "<<") )
                {
                    GoBack();
                }
            }

            if ( mainEngine.CanGoForward )
            {
                if ( GUI.Button(new Rect(Screen.width * 0.95f, Screen.height * 0.4f, Screen.width * 0.05f, Screen.height * 0.1f), ">>") )
                {
                    GoForward();
                }
            }
        }

        if ( GUI.Button(new Rect(Screen.width * 0.9f, Screen.height * 0.5f, Screen.width * 0.05f, Screen.height * 0.1f), "SetVisibility") )
        {
            SetVisibility(!visibility);
        }
    }
}