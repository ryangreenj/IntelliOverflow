using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace IOVSPlugin
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class ErrorListSearch
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 258;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("a794eb1d-56e9-4cb5-bbbc-c27bcb499722");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorListSearch"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private ErrorListSearch(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static ErrorListSearch Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in ErrorListSearch's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new ErrorListSearch(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            string errorCode = "";
            string errorText = "";
            string errorProject = "";
            string errorFile = "";

            if (GetSelectedError(out errorText, out errorCode, out errorProject, out errorFile))
            {
                this.package.JoinableTaskFactory.RunAsync(async delegate
                {
                    ToolWindowPane window = await this.package.ShowToolWindowAsync(typeof(IOWindow), 0, true, this.package.DisposalToken);
                    if ((null == window) || (null == window.Frame))
                    {
                        throw new NotSupportedException("Cannot create tool window");
                    }
                    else
                    {
                        IOWindow ioWindow = (IOWindow)window;
                        ioWindow.SendErrorToSearch(errorText, errorCode, errorProject, errorFile);
                    }
                });
            }
        }

        private bool GetSelectedError(out string errorText, out string errorCode, out string errorProject, out string errorFile)
        {
            errorText = "";
            errorCode = "";
            errorProject = "";
            errorFile = "";

            EnvDTE80.DTE2 dte;

            dte = (EnvDTE80.DTE2) this.ServiceProvider.GetServiceAsync(typeof(EnvDTE.DTE)).Result;

            var errorList = dte.ToolWindows.ErrorList as IErrorList;
            var selected = errorList.TableControl.SelectedEntry;

            if (selected != null)
            {
                object content;

                if (selected.TryGetValue("text", out content))
                {
                    errorText = (string)content;
                }
                else
                {
                    return false;
                }
                
                // These values are not as important as error text and may not always be present, so don't return if can't get
                if (selected.TryGetValue("errorcode", out content))
                {
                    errorCode = (string)content;
                }

                if (selected.TryGetValue("project", out content))
                {
                    errorProject = (string)content;
                }

                if (selected.TryGetValue("file", out content))
                {
                    errorFile = (string)content;
                }

                return true;
            }
            else // User clicks search without an error selected
            {
                string title = "Search on Stack Overflow";
                string message = "Please select the Error/Warning/Message you would like to search.";

                VsShellUtilities.ShowMessageBox(
                    this.package,
                    message,
                    title,
                    OLEMSGICON.OLEMSGICON_INFO,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

                return false;
            }
        }
    }
}
