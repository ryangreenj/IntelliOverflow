using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace IOVSPlugin
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("3c136347-67d5-417d-9b42-c463e52be5f6")]
    public class IOWebBrowser : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IOWebBrowser"/> class.
        /// </summary>
        public IOWebBrowser() : base(null)
        {
            this.Caption = "Intelli Overflow Browser";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new IOWebBrowserControl();
        }

        public void RouteToLink(string uri)
        {
            IOWebBrowserControl control = (IOWebBrowserControl)this.Content;

            control.GoToUri(uri);
        }
    }
}
