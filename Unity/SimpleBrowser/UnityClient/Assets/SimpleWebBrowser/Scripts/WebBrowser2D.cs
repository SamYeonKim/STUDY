﻿using MessageLibrary;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SimpleWebBrowser
{
    public class WebBrowser2D : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,
        IPointerUpHandler
    {

        #region General

        [Header("General settings")]
        public int Width = 1024;
        public int Height = 768;
        public string MemoryFile = "MainSharedMem";
        public bool RandomMemoryFile = true;
        public string InitialURL = "http://www.google.com";
        public bool EnableWebRTC = false;
        public bool BackgroundTransparent = false;
        public string url;

        [Header("Testing")]
        public bool EnableGPU = false;

        [Multiline]
        public string JSInitializationCode = "";

        #endregion


        [Header("2D setup")]
        [SerializeField]
        public RawImage Browser2D = null;

        [Header("UI settings")]
        [SerializeField]
        public BrowserUI mainUIPanel;
        public bool KeepUIVisible = false;

        [Header("Dialog settings")]
        [SerializeField]
        public GameObject DialogPanel;
        [SerializeField]
        public Text DialogText;
        [SerializeField]
        public Button OkButton;
        [SerializeField]
        public Button YesButton;
        [SerializeField]
        public Button NoButton;
        [SerializeField]
        public InputField DialogPrompt;

        //dialog states - threading
        private bool _showDialog = false;
        private string _dialogMessage = "";
        private string _dialogPrompt = "";
        private DialogEventType _dialogEventType;
        //query - threading
        private bool _startQuery = false;
        private string _jsQueryString = "";

        //status - threading
        private bool _setUrl = false;
        private string _setUrlString = "";

        #region JS Query events

        public delegate void JSQuery(string query);
        public event JSQuery OnJSQuery;

        #endregion

        private Material _mainMaterial;
        private BrowserEngine _mainEngine;
        private bool _focused = false;
        private int posX = 0;
        private int posY = 0;
        private Camera _mainCamera;

        #region Initialization

        //why Unity does not store the links in package?
        void InitPrefabLinks()
        {
            if ( Browser2D == null )
                Browser2D = gameObject.GetComponent<RawImage>();
            if ( mainUIPanel == null )
                mainUIPanel = gameObject.transform.Find("MainUI").gameObject.GetComponent<BrowserUI>();
            if ( DialogPanel == null )
                DialogPanel = gameObject.transform.Find("MessageBox").gameObject;
            if ( DialogText == null )
                DialogText = DialogPanel.transform.Find("MessageText").gameObject.GetComponent<Text>();
            if ( OkButton == null )
                OkButton = DialogPanel.transform.Find("OK").gameObject.GetComponent<Button>();
            if ( YesButton == null )
                YesButton = DialogPanel.transform.Find("Yes").gameObject.GetComponent<Button>();
            if ( NoButton == null )
                NoButton = DialogPanel.transform.Find("No").gameObject.GetComponent<Button>();
            if ( DialogPrompt == null )
                DialogPrompt = DialogPanel.transform.Find("Prompt").gameObject.GetComponent<InputField>();
        }

        void Awake()
        {
            _mainEngine = new BrowserEngine();

            if ( RandomMemoryFile )
            {
                Guid memid = Guid.NewGuid();
                MemoryFile = memid.ToString();
            }
        }

        IEnumerator Start()
        {
            InitPrefabLinks();
            mainUIPanel.InitPrefabLinks();

            yield return null;
            //yield return _mainEngine.InitPlugin(Width, Height, MemoryFile, InitialURL, EnableWebRTC, EnableGPU, BackgroundTransparent);

            // JSInitializationCode = @"
            //         window.Unity = {
            //             call: function(msg) {
            //                 window.cefQuery({
            //                     request: msg,
            //                     onSuccess: function(response) {}
            //                     onFailure: function(error_code, error_message) {}
            //                 })
            //             }
            //         }
            //     ";
            JSInitializationCode = @"window.Unity = { call: function(msg) { window.cefQuery({ request: msg, onSuccess: function(response) { console.log(response); }, onFailure: function(err,msg) { console.log(err, msg); } }); }}";
            //run initialization
            if ( JSInitializationCode.Trim() != "" )
                _mainEngine.RunJSOnce(JSInitializationCode);

            _mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

            Browser2D.texture = _mainEngine.BrowserTexture;
            Browser2D.uvRect = new Rect(0f, 0f, 1f, -1f);


            // _mainInput = MainUrlInput.GetComponent<Input>();
            mainUIPanel.KeepUIVisible = KeepUIVisible;
            if ( !KeepUIVisible )
                mainUIPanel.Hide();

            //attach dialogs and querys
            _mainEngine.OnJavaScriptQuery += _mainEngine_OnJavaScriptQuery;
            _mainEngine.OnPageLoaded += _mainEngine_OnPageLoaded;
            _mainEngine.OnPageLoadedError += _mainEngine_OnPageLoadedError;

            DialogPanel.SetActive(false);
        }

        private void _mainEngine_OnPageLoaded(string url)
        {
            _setUrl = true;
            _setUrlString = url;

            Debug.Log("OnPageLoaded : " + url);

            // JsDialog 테스트용
            // RunJavaScript(@"alert('helloworld')");

            RunJavaScript(@"window.cefQuery({ request: navigator.userAgent, onSuccess: function(response) { console.log(response); }, onFailure: function(err,msg) { console.log(err, msg); } });");
        }

        private void _mainEngine_OnPageLoadedError(Xilium.CefGlue.CefErrorCode errorCode, string errorText, string errorUrl)
        {
            Debug.Log(string.Format("OnPageLoadedError : Code : {0}, Msg : {1}, Url : {2}", errorCode.ToString(), errorText, errorUrl));
        }
        #endregion

        #region Queries and dialogs

        //make it thread-safe
        private void _mainEngine_OnJavaScriptQuery(string message)
        {
            _jsQueryString = message;
            _startQuery = true;

            Debug.Log("OnJavaScriptQuery : " + message);
        }

        public void RespondToJSQuery(string response)
        {
            Debug.Log("RespondToJSQuery : " + response);
            _mainEngine.SendQueryResponse(response);
        }

        private void _mainEngine_OnJavaScriptDialog(string message, string prompt, DialogEventType type)
        {
            Debug.Log("OnJavaScriptDialog : " + message);

            _showDialog = true;
            _dialogEventType = type;
            _dialogMessage = message;
            _dialogPrompt = prompt;

        }

        private void ShowDialog()
        {
            switch ( _dialogEventType )
            {
                case DialogEventType.Alert:
                {
                    DialogPanel.SetActive(true);
                    OkButton.gameObject.SetActive(true);
                    YesButton.gameObject.SetActive(false);
                    NoButton.gameObject.SetActive(false);
                    DialogPrompt.text = "";
                    DialogPrompt.gameObject.SetActive(false);
                    DialogText.text = _dialogMessage;
                    break;
                }
                case DialogEventType.Confirm:
                {
                    DialogPanel.SetActive(true);
                    OkButton.gameObject.SetActive(false);
                    YesButton.gameObject.SetActive(true);
                    NoButton.gameObject.SetActive(true);
                    DialogPrompt.text = "";
                    DialogPrompt.gameObject.SetActive(false);
                    DialogText.text = _dialogMessage;
                    break;
                }
                case DialogEventType.Prompt:
                {
                    DialogPanel.SetActive(true);
                    OkButton.gameObject.SetActive(false);
                    YesButton.gameObject.SetActive(true);
                    NoButton.gameObject.SetActive(true);
                    DialogPrompt.text = _dialogPrompt;
                    DialogPrompt.gameObject.SetActive(true);
                    DialogText.text = _dialogMessage;
                    break;
                }
            }
            _showDialog = false;
        }

        public void DialogResult(bool result)
        {
            DialogPanel.SetActive(false);
            _mainEngine.SendDialogResponse(result, DialogPrompt.text);

        }

        public void RunJavaScript(string js)
        {
            _mainEngine.SendExecuteJSEvent(js);
        }

        public void LoadUrl(string url)
        {
            _mainEngine.SendNavigateEvent(url, false, false);
        }

        public void LoadHtml(string fileName)
        {
            string url = "file://" + System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
            LoadUrl(url);
        }
        public void SetMargin(int left, int top, int right, int bottom)
        {
            var rectTransform = this.GetComponent<RectTransform>();

            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);

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
        #endregion

        #region UI

        public void OnNavigate()
        {
            // MainUrlInput.isFocused
            _mainEngine.SendNavigateEvent(mainUIPanel.UrlField.text, false, false);

        }

        public void GoBackForward(bool forward)
        {
            if ( forward )
                _mainEngine.SendNavigateEvent("", false, true);
            else
                _mainEngine.SendNavigateEvent("", true, false);
        }

        #endregion




        #region Events 

        public void OnPointerEnter(PointerEventData data)
        {
            _focused = true;
            mainUIPanel.Show();
            StartCoroutine("TrackPointer");
        }

        public void OnPointerExit(PointerEventData data)
        {
            _focused = false;
            mainUIPanel.Hide();
            StopCoroutine("TrackPointer");
        }

        //tracker
        IEnumerator TrackPointer()
        {
            var _raycaster = GetComponentInParent<GraphicRaycaster>();
            var _input = FindObjectOfType<StandaloneInputModule>();

            if ( _raycaster != null && _input != null && _mainEngine.Initialized )
            {
                while ( Application.isPlaying )
                {
                    Vector2 localPos = GetScreenCoords(_raycaster, _input);

                    int px = (int)localPos.x;
                    int py = (int)localPos.y;

                    ProcessScrollInput(px, py);

                    if ( posX != px || posY != py )
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
                            msg.Button = MouseButton.Left;
                        if ( Input.GetMouseButton(1) )
                            msg.Button = MouseButton.Right;
                        if ( Input.GetMouseButton(1) )
                            msg.Button = MouseButton.Middle;

                        posX = px;
                        posY = py;
                        _mainEngine.SendMouseEvent(msg);
                    }

                    yield return 0;
                }
            }
            //  else
            //      UnityEngine.Debug.LogWarning("Could not find GraphicRaycaster and/or StandaloneInputModule");
        }

        public void OnPointerDown(PointerEventData data)
        {

            if ( _mainEngine.Initialized )
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

            if ( _mainEngine.Initialized )
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

        #region Helpers
        private Vector2 GetScreenCoords(GraphicRaycaster ray, StandaloneInputModule input)
        {
            Vector2 localPos; // Mouse position  
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition,
                ray.eventCamera, out localPos);

            // local pos is the mouse position.
            RectTransform trns = transform as RectTransform;
            localPos.y = trns.rect.height - localPos.y;

            //now recalculate to texture
            localPos.x = (localPos.x * Width) / trns.rect.width;
            localPos.y = (localPos.y * Height) / trns.rect.height;

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
            _mainEngine.SendMouseEvent(msg);
        }
        private void ProcessScrollInput(int px, int py)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            scroll = scroll * _mainEngine.BrowserTexture.height;

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

                _mainEngine.SendMouseEvent(msg);
            }
        }
        #endregion

        void Update()
        {
            _mainEngine.UpdateTexture();
            _mainEngine.CheckMessage();            

            if ( !_mainEngine.Initialized )
                return;

            //Dialog
            if ( _showDialog )
            {
                ShowDialog();
            }

            //Query
            if ( _startQuery )
            {
                _startQuery = false;
                if ( OnJSQuery != null )
                    OnJSQuery(_jsQueryString);
            }

            //Status
            if ( _setUrl )
            {
                _setUrl = false;
                mainUIPanel.UrlField.text = _setUrlString;

            }

            if ( _focused && !mainUIPanel.UrlField.isFocused )
            {
                foreach ( char c in Input.inputString )
                {
                    _mainEngine.SendCharEvent((int)c, KeyboardEventType.CharKey);
                }
                ProcessKeyEvents();
            }
        }

        #region Keys
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
                _mainEngine.SendCharEvent((int)code, KeyboardEventType.Down);
            }                
            if ( Input.GetKeyUp(KeyCode.Backspace) )
            {
                _mainEngine.SendCharEvent((int)code, KeyboardEventType.Up);
            }
        }
        #endregion

        void OnDisable()
        {
            _mainEngine.Shutdown();
        }

        private void OnGUI()
        {
            url = GUI.TextField(new Rect(Screen.width * 0.8f, Screen.height * 0.1f, Screen.width * 0.15f, Screen.height * 0.1f), url);

            if ( url.EndsWith(".html") )
            {
                if ( GUI.Button(new Rect(Screen.width * 0.95f, Screen.height * 0.1f, Screen.width * 0.05f, Screen.height * 0.1f), "LoadHTML") )
                {
                    LoadHtml(url);
                }
            }
            else
            {
                if ( GUI.Button(new Rect(Screen.width * 0.95f, Screen.height * 0.1f, Screen.width * 0.05f, Screen.height * 0.1f), "LoadURL") )
                {
                    LoadUrl(url);
                }
            }

            if ( GUI.Button(new Rect(Screen.width * 0.95f, Screen.height * 0.3f, Screen.width * 0.05f, Screen.height * 0.1f), "SetMargin") )
            {
                SetMargin((int)(Screen.width * 0.2f), (int)(Screen.height * 0.2f), (int)(Screen.width * 0.2f), (int)(Screen.height * 0.2f));
            }
        }
    }
}