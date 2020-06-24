using System;
using System.Runtime.InteropServices;
using MessageLibrary;
using Xilium.CefGlue;
using Xilium.CefGlue.Wrapper;


namespace SharedPluginServer
{

    //Main CEF worker
    public class CefWorker:IDisposable
    {
        private static readonly log4net.ILog log =
    log4net.LogManager.GetLogger(typeof(CefWorker));

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        private WorkerCefClient _client;

        private static bool _initialized = false;

        public CefMessageRouterBrowserSide BrowserMessageRouter { get; private set; }

        private WorkerCefMessageRouterHandler _queryHandler;

        private bool canGoBack;
        private bool canGoForward;

        #region Status

        public delegate void PageLoaded(string url, int status);

        public event PageLoaded OnPageLoaded;

        public delegate void PageLoadedError(CefErrorCode errorCode, string errorText, string failedUrl);

        public event PageLoadedError OnPageLoadedError;

        //CanGoBack, CanGoForward가 갱신 되는 시점 보다 PageLoaded가 먼저이기 때문에, PageLoaded에서 받은 값을 변수로 저장해 놨다가.
        //나중에 OnLoadingStateChange 에서 OnPageLoaded를 호출 하도록 한다.
        CefFrame lastLoadedFrame;
        int lastLoadedStatus;
        public void InvokePageLoaded(CefFrame frame, string url, int status)
        {
            lastLoadedFrame = frame;
            lastLoadedStatus = status;
            //OnPageLoaded?.Invoke(url,status);
        }
        public void InvokePageLoadedError(CefErrorCode errorCode, string errorText, string failedUrl)
        {
            lastLoadedFrame = null;
            OnPageLoadedError?.Invoke(errorCode, errorText, failedUrl);
        }
        #endregion

        #region Dialogs
        public delegate void CefJSDialog(string message,string prompt,DialogEventType type);

        public event CefJSDialog OnJSDialog;

        public void InvokeCefDialog(string message, string prompt, DialogEventType type)
        {
            OnJSDialog?.Invoke(message,prompt,type);
        }
        
        public void ContinueDialog(bool res, string input)
        {
            _client.ContinueDialog(res, input);
        }
        #endregion

        #region JS Bindings

        public delegate void BrowserJSQuery(string query);

        public event BrowserJSQuery OnBrowserJSQuery;

        #endregion

        #region IDisposable
        ~CefWorker()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                log.Info("=============SHUTTING DOWN========");
                Shutdown();
            }

        }
        #endregion

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="width">Browser rect width</param>
        /// <param name="height">Browser rect height</param>
        /// <param name="starturl"></param>
        public void Init(int width,int height,string starturl, bool transparent = false)
        {
            RegisterMessageRouter();

            CefWindowInfo cefWindowInfo = CefWindowInfo.Create();
            cefWindowInfo.SetAsWindowless(IntPtr.Zero, transparent);
            var cefBrowserSettings = new CefBrowserSettings();

            cefBrowserSettings.JavaScript=CefState.Enabled;
            //cefBrowserSettings.CaretBrowsing=CefState.Enabled;
            cefBrowserSettings.TabToLinks=CefState.Enabled;
            cefBrowserSettings.WebSecurity=CefState.Disabled;
            cefBrowserSettings.WebGL=CefState.Enabled;
            cefBrowserSettings.WindowlessFrameRate = 30;
            cefBrowserSettings.BackgroundColor = new CefColor(0);

            _client = new WorkerCefClient(width, height,this);
            
            string url = "http://www.yandex.ru/";
            if (starturl != "")
                url = starturl;
                    CefBrowserHost.CreateBrowser(cefWindowInfo, _client, cefBrowserSettings, url);

            _initialized = true;
        }
        public void OnLoadingStateChange(CefFrame frame, bool canGoBack, bool canGoForward)
        {                        
            this.canGoBack = canGoBack;
            this.canGoForward = canGoForward;

            if ( lastLoadedFrame != null )
            {
                OnPageLoaded?.Invoke(lastLoadedFrame.Url, lastLoadedStatus);
            }
        }
        public void SetMemServer(SharedMemServer memServer)
        {
            _client.SetMemServer(memServer);
        }

        #region Queries

        private void RegisterMessageRouter()
        {
            if (!CefRuntime.CurrentlyOn(CefThreadId.UI))
            {
                PostTask(CefThreadId.UI, this.RegisterMessageRouter);
                return;
            }

            // window.cefQuery({ request: 'my_request', onSuccess: function(response) { console.log(response); }, onFailure: function(err,msg) { console.log(err, msg); } });
            BrowserMessageRouter = new CefMessageRouterBrowserSide(new CefMessageRouterConfig());
            _queryHandler=new WorkerCefMessageRouterHandler();
            _queryHandler.OnBrowserQuery += Handler_OnBrowserQuery;
            BrowserMessageRouter.AddHandler(_queryHandler);
        }

        private void Handler_OnBrowserQuery(string query)
        {
            log.Info("Handler_OnBrowserQuery : " + query);
            OnBrowserJSQuery?.Invoke(query);
        }

        public void AnswerQuery(string resp)
        {
            log.Info("AnswerQuery : " + resp);
            _queryHandler.Callback(resp);
        }
#endregion

        #region Task helper

        public static void PostTask(CefThreadId threadId, Action action)
        {
            CefRuntime.PostTask(threadId, new ActionTask(action));
        }

        internal sealed class ActionTask : CefTask
        {
            public Action _action;

            public ActionTask(Action action)
            {
                _action = action;
            }

            protected override void Execute()
            {
                _action();
                _action = null;
            }
        }

        public delegate void Action();

        #endregion  

        public byte[] GetBitmap()
        {
            return _client.GetBitmap();
        }

        public int GetWidth()
        {
            return _client.GetWidth();
        }

        public int GetHeight()
        {
            return _client.GetWidth();
        }

        public void Shutdown()
        {
            _client.Shutdown();
          // 
        }

#region Navigation and controls
        public void Navigate(string url)
        {
            _client.Navigate(url);
        }
        public void GoBack()
        {
            _client.GoBack();
        }
        public void GoForward()
        {
            _client.GoForward();
        }
        public bool CanGoBack()
        {
            return this.canGoBack;
        }
        public bool CanGoForward()
        {
            return this.canGoForward;
        }

        public void ExecuteJavaScript(string jscode)
        {
            _client.ExecuteJavaScript(jscode);
        }
        #endregion

        #region Mouse and keyboard
        public void MouseEvent(int x, int y,bool updown,MouseButton button)
        {
            _client.MouseEvent(x,y,updown,button);
        }

        public void MouseMoveEvent(int x, int y,MouseButton button)
        {
            _client.MouseMoveEvent(x, y,button);
        }

        public void KeyboardEvent(int character,KeyboardEventType type)
        {
            _client.KeyboardEvent(character,type);
        }

        public void FocusEvent(int focus)
        {
            _client.FocusEvent(focus);
        }

        public void MouseLeaveEvent()
        {
            _client.MouseLeaveEvent();
        }

        public void MouseWheelEvent(int x, int y, int delta)
        {
            _client.MouseWheelEvent(x,y,delta);
        }
        #endregion
    }
}
