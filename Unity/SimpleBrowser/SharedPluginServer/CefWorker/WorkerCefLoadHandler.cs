using Xilium.CefGlue;

namespace SharedPluginServer
{
    class WorkerCefLoadHandler : CefLoadHandler
    {
        private CefWorker _mainWorker;

        public WorkerCefLoadHandler(CefWorker mainWorker)
        {
            _mainWorker = mainWorker;
        }

        protected override void OnLoadStart(CefBrowser browser, CefFrame frame, CefTransitionType transitionType)
        {
           
        }

        protected override void OnLoadEnd(CefBrowser browser, CefFrame frame, int httpStatusCode)
        {            
            if (frame.IsMain)
            {
                _mainWorker.InvokePageLoaded(frame, frame.Url,httpStatusCode);             
            }
        }

        protected override void OnLoadError(CefBrowser browser, CefFrame frame, CefErrorCode errorCode, string errorText, string failedUrl)
        {
            if ( frame.IsMain )
            {
                _mainWorker.InvokePageLoadedError(errorCode, errorText, failedUrl);
            }
        }

        protected override void OnLoadingStateChange(CefBrowser browser, bool isLoading, bool canGoBack, bool canGoForward)
        {
            _mainWorker.OnLoadingStateChange(browser.GetMainFrame(), canGoBack, canGoForward);
        }
    }
}