using System;



namespace MessageLibrary
{

    public enum BrowserEventType
    {
        Ping=-1,
        Generic=0,
        Mouse=1,
        Keyboard=2,
        Dialog = 3,
        StopPacket=4
    }

    [Serializable]
    public abstract class AbstractEvent
    {
        public BrowserEventType GenericType;
    }

    [Serializable]
    public class EventPacket
    {
        public BrowserEventType Type;

        public AbstractEvent Event;
    }

    public enum GenericEventType
    {
        Shutdown=0,
        Navigate=1,
        GoBack=2,
        GoForward=3,
        ExecuteJS=4,
        JSQuery=5,
        JSQueryResponse=6,
        PageLoaded=7,
        PageLoadedError = 8
    }  

    [Serializable]
    public class GenericEvent : AbstractEvent
    {
        public GenericEventType Type;                
    }

    [Serializable]
    public class ErrorEvent : GenericEvent
    {
        public int ErrorCode;
        public string ErrorText;
        public string ErrorFailedUrl;
    }

    [Serializable]
    public class NavigateEvent : GenericEvent
    {
        public string NavigateUrl;
        public bool CanGoForward;
        public bool CanGoBack;
    }

    [Serializable]
    public class JSEvent : GenericEvent
    {
        public string JsCode;
    }
}
