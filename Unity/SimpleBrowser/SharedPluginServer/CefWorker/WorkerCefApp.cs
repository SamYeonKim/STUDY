using Xilium.CefGlue;

namespace SharedPluginServer
{
    class WorkerCefApp : CefApp
    {
        private readonly WorkerCefRenderProcessHandler _renderProcessHandler;

        public WorkerCefApp()
        {
            _renderProcessHandler = new WorkerCefRenderProcessHandler();
        }

        protected override CefRenderProcessHandler GetRenderProcessHandler()
        {
            return _renderProcessHandler;
        }

        protected override void OnBeforeCommandLineProcessing(string processType, CefCommandLine commandLine)
        {
            commandLine.AppendSwitch("disable-gpu");
            commandLine.AppendSwitch("disable-gpu-compositing");
            commandLine.AppendSwitch("enable-begin-frame-scheduling");
            commandLine.AppendSwitch("disable-smooth-scrolling");
            commandLine.AppendSwitch("winhttp-proxy-resolver");            
            commandLine.AppendSwitch("no-proxy-server", "1");
        }
    }
}