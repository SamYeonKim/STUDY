using System;
using System.Diagnostics;
using System.Collections;
using System.IO;
using MessageLibrary;
using SharedMemory;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = System.Object;

namespace SimpleWebBrowser
{
    public class BrowserEngine
    {
        private static Object pixelLock;        
        private SharedArray<byte> srcTexBuffer;
        private byte[] dstTexBuffer = null;
        private SharedCommServer inCommServer;
        private SharedCommServer outCommServer;
        private Process pluginProcess;        
        private string runOnceJS = "";
        private bool connected = false;

        public Texture2D BrowserTexture { get; private set; }
        public bool Initialized { get; private set; }
        public bool CanGoBack { get; private set; }
        public bool CanGoForward { get; private set; }
        public delegate void PageLoaded(string url);
        public event PageLoaded OnPageLoaded;
        public delegate void PageLoadedError(Xilium.CefGlue.CefErrorCode errorCode, string errorText, string errorUrl);
        public event PageLoadedError OnPageLoadedError;
        public delegate void JavaScriptQuery(string message);
        public event JavaScriptQuery OnJavaScriptQuery;

        private int width = 512;
        private int height = 512;
        private string sharedMemFileName;
        private string inCommFile;
        private string outCommFile;
        private string initialUrl;
        private bool enableTransparent;

        public IEnumerator InitPlugin(int width, int height, string sharedfilename, string initialURL, bool transparent = false)
        {
            //Initialization (for now) requires a predefined path to PluginServer,
            //so change this section if you move the folder
            //Also change the path in deployment script.
#if UNITY_EDITOR
#if UNITY_64
            string PluginServerPath = Application.dataPath + @"\SimpleWebBrowser\Plugins\x86_x64";
#else
            string PluginServerPath = Application.dataPath + @"\SimpleWebBrowser\Plugins\x86";
#endif
#else
            //HACK
            string AssemblyPath=System.Reflection.Assembly.GetExecutingAssembly().Location;
            AssemblyPath = Path.GetDirectoryName(AssemblyPath); //Managed
            
            AssemblyPath = Directory.GetParent(AssemblyPath).FullName; //<project>_Data
            //AssemblyPath = Directory.GetParent(AssemblyPath).FullName;//required

            string PluginServerPath=AssemblyPath+@"\Plugins";
#endif
            Debug.Log("Starting server from:" + PluginServerPath);

            this.width = width;
            this.height = height;

            sharedMemFileName = sharedfilename;

            //randoms
            Guid inID = Guid.NewGuid();
            outCommFile = inID.ToString();

            Guid outID = Guid.NewGuid();
            inCommFile = outID.ToString();

            initialUrl = initialURL;
            enableTransparent = transparent;

            if ( BrowserTexture == null )
            {
                BrowserTexture = new Texture2D(this.width, this.height, TextureFormat.BGRA32, false, true);
                BrowserTexture.alphaIsTransparency = true;
            }

            pixelLock = new object();

            string args = BuildParamsString();

            connected = false;

            if ( inCommServer != null )
                inCommServer.Dispose();
            if ( outCommServer != null )
                outCommServer.Dispose();

            inCommServer = new SharedCommServer(false);
            outCommServer = new SharedCommServer(true);

            while ( !connected )
            {
                try
                {
                    pluginProcess = new Process()
                    {
                        StartInfo = new ProcessStartInfo()
                        {
                            WorkingDirectory = PluginServerPath,
                            FileName = PluginServerPath + @"\SharedPluginServer.exe",
                            Arguments = args
                        }
                    };

                    pluginProcess.Start();
                    pluginProcess.EnableRaisingEvents = true;
                    //_pluginProcess.WaitForInputIdle();
                    Initialized = false;
                }
                catch ( Exception ex )
                {
                    //log the file
                    Debug.Log("FAILED TO START SERVER FROM:" + PluginServerPath + @"\SharedPluginServer.exe");
                    throw;
                }
                
                yield return new WaitForSeconds(1.0f);

                inCommServer.Connect(inCommFile);
                bool b1 = inCommServer.GetIsOpen();
                outCommServer.Connect(outCommFile);
                bool b2 = outCommServer.GetIsOpen();

                connected = b1 && b2;

                pluginProcess.Exited += (object sender, EventArgs e) =>
                {
                    Debug.Log("Exited");
                    Initialized = false;
                    connected = false;
                };

                UpdateInitialized();
            }
        }

        private string BuildParamsString()
        {
            string ret = width.ToString() + " " + height.ToString() + " ";
            ret = ret + initialUrl + " ";
            ret = ret + sharedMemFileName + " ";
            ret = ret + outCommFile + " ";
            ret = ret + inCommFile + " ";

            if ( enableTransparent )
                ret = ret + " 1" + " ";
            else
                ret = ret + " 0" + " ";

            return ret;
        }

        #region SendEvents
        public void SendNavigateEvent(string url, bool back, bool forward)
        {
            if ( Initialized )
            {
                NavigateEvent ge = new NavigateEvent()
                {
                    Type = GenericEventType.Navigate,
                    GenericType = MessageLibrary.BrowserEventType.Generic,
                    NavigateUrl = url
                };

                if ( back )
                    ge.Type = GenericEventType.GoBack;
                else if ( forward )
                    ge.Type = GenericEventType.GoForward;

                EventPacket ep = new EventPacket()
                {
                    Event = ge,
                    Type = MessageLibrary.BrowserEventType.Generic
                };

                outCommServer.WriteMessage(ep);
            }
        }

        public void SendShutdownEvent()
        {
            if ( Initialized )
            {
                GenericEvent ge = new GenericEvent()
                {
                    Type = GenericEventType.Shutdown,
                    GenericType = MessageLibrary.BrowserEventType.Generic
                };

                EventPacket ep = new EventPacket()
                {
                    Event = ge,
                    Type = MessageLibrary.BrowserEventType.Generic
                };

                outCommServer.WriteMessage(ep);
            }
        }

        public void PushMessages()
        {
            if ( Initialized )
                outCommServer.PushMessages();
        }

        public void SendDialogResponse(bool ok, string dinput)
        {
            if ( Initialized )
            {
                DialogEvent de = new DialogEvent()
                {
                    GenericType = MessageLibrary.BrowserEventType.Dialog,
                    success = ok,
                    input = dinput
                };

                EventPacket ep = new EventPacket
                {
                    Event = de,
                    Type = MessageLibrary.BrowserEventType.Dialog
                };

                outCommServer.WriteMessage(ep);
            }
        }

        public void SendQueryResponse(string response)
        {
            if ( Initialized )
            {
                JSEvent ge = new JSEvent()
                {
                    Type = GenericEventType.JSQueryResponse,
                    GenericType = BrowserEventType.Generic,
                    JsCode = response
                };

                EventPacket ep = new EventPacket()
                {
                    Event = ge,
                    Type = BrowserEventType.Generic
                };

                outCommServer.WriteMessage(ep);
            }
        }

        public void SendCharEvent(int character, KeyboardEventType type)
        {
            if ( Initialized )
            {
                KeyboardEvent keyboardEvent = new KeyboardEvent()
                {
                    Type = type,
                    Key = character
                };
                EventPacket ep = new EventPacket()
                {
                    Event = keyboardEvent,
                    Type = MessageLibrary.BrowserEventType.Keyboard
                };

                outCommServer.WriteMessage(ep);
            }
        }

        public void SendMouseEvent(MouseMessage msg)
        {
            if ( Initialized )
            {
                EventPacket ep = new EventPacket
                {
                    Event = msg,
                    Type = MessageLibrary.BrowserEventType.Mouse
                };

                outCommServer.WriteMessage(ep);
            }
        }

        public void SendExecuteJSEvent(string js)
        {
            if ( Initialized )
            {
                JSEvent ge = new JSEvent()
                {
                    Type = GenericEventType.ExecuteJS,
                    GenericType = BrowserEventType.Generic,
                    JsCode = js
                };

                EventPacket ep = new EventPacket()
                {
                    Event = ge,
                    Type = BrowserEventType.Generic
                };

                outCommServer.WriteMessage(ep);
            }
        }

        public void SendPing()
        {
            if ( Initialized )
            {
                GenericEvent ge = new GenericEvent()
                {
                    Type = GenericEventType.Navigate, //could be any
                    GenericType = BrowserEventType.Ping,

                };

                EventPacket ep = new EventPacket()
                {
                    Event = ge,
                    Type = BrowserEventType.Ping
                };

                outCommServer.WriteMessage(ep);
            }
        }


        #endregion


        #region Helpers

        /// <summary>
        /// Used to run JS on initialization, for example, to set CSS
        /// </summary>
        /// <param name="js">JS code</param>
        public void RunJSOnce(string js)
        {
            runOnceJS = js;
        }

        #endregion
        public void UpdateTexture()
        {
            if ( Initialized )
            {
                UpdateInitialized();

                if ( !string.IsNullOrEmpty(runOnceJS) )
                {
                    SendExecuteJSEvent(runOnceJS);
                    runOnceJS = null;
                }
            }
            else
            {
                if ( connected )
                {
                    try
                    {
                        srcTexBuffer = new SharedArray<byte>(sharedMemFileName);

                        Initialized = true;
                    }
                    catch ( Exception ex )
                    {
                        //SharedMem and TCP exceptions
                        Debug.Log("Exception on init:" + ex.Message + ".Waiting for plugin server");
                    }
                }
            }
        }
        //Receiver
        public void CheckMessage()
        {
            if ( Initialized )
            {
                try
                {
                    // Ensure that no other threads try to use the stream at the same time.
                    EventPacket ep = inCommServer.GetMessage();
                    if ( ep != null )
                    {
                        //main handlers
                        if ( ep.Type == BrowserEventType.Generic )
                        {
                            GenericEvent ge = ep.Event as GenericEvent;
                            if ( ge != null )
                            {
                                if ( ge.Type == GenericEventType.JSQuery )
                                {
                                    if ( OnJavaScriptQuery != null )
                                        OnJavaScriptQuery(((JSEvent)ge).JsCode);
                                }
                            }

                            if ( ge.Type == GenericEventType.PageLoaded )
                            {
                                NavigateEvent navigateEvent = (NavigateEvent)ge;
                                CanGoBack = navigateEvent.CanGoBack;
                                CanGoForward = navigateEvent.CanGoForward;

                                if ( OnPageLoaded != null )                                
                                    OnPageLoaded(navigateEvent.NavigateUrl);
                            }
                            else if ( ge.Type == GenericEventType.PageLoadedError )
                            {
                                ErrorEvent errorEvent = (ErrorEvent)ge;
                                if ( OnPageLoadedError != null )
                                    OnPageLoadedError((Xilium.CefGlue.CefErrorCode)errorEvent.ErrorCode, errorEvent.ErrorText, errorEvent.ErrorFailedUrl);
                            }
                        }
                    }
                }
                catch ( Exception e )
                {
                    Debug.Log("Error reading from socket,waiting for plugin server to start...");
                }
            }
        }
        public void Shutdown()
        {
            GameObject.DestroyImmediate(BrowserTexture);
            SendShutdownEvent();
        }
        public void UpdateInitialized()
        {
            if ( Initialized )
            {
                SendPing();

                if ( dstTexBuffer == null )
                {
                    long arraySize = srcTexBuffer.Length;
                    dstTexBuffer = new byte[arraySize];
                }
                srcTexBuffer.CopyTo(dstTexBuffer, 0);

                lock ( pixelLock )
                {
                    BrowserTexture.LoadRawTextureData(dstTexBuffer);
                    BrowserTexture.Apply();
                }
            }
        }
    }
}