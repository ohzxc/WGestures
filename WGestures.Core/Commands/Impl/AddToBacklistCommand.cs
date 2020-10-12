using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WGestures.Common.Annotation;
using WGestures.Common.OsSpecific.Windows;
using Win32;
using System.IO;
using WGestures.Core.Persistence;

namespace WGestures.Core.Commands.Impl
{
    [Named("添加到黑名单"), Serializable]
    public class AddToBacklistCommand : AbstractCommand, IGestureParserAware, IGestureContextAware {

        public override string Description() {
            return "添加到黑名单";
        }

        public override void Execute()
        {
            var cursorWin = Native.WindowFromPoint(new Native.POINT() { x = Context.StartPoint.X, y = Context.StartPoint.Y });
            //var win = Native.GetHoveringWindow();

            DoOperation(cursorWin);

            //Parser.Pause();     // 暂停WGestures
        }

        private void DoOperation(IntPtr win) {
            
            //topLevelWin是本进程（？）内的顶层窗口
            //rootWindow可能会跨进程
            var topLevelWin = Native.GetTopLevelWindow(win);
            var rootWin = Native.GetAncestor(topLevelWin, Native.GetAncestorFlags.GetRoot);
            var procId = Native.GetProcessIdByWindowHandle(win);
                
            if (rootWin == IntPtr.Zero) return;

            Debug.WriteLine(string.Format("win     : {0:X}", win.ToInt64()));
            Debug.WriteLine(string.Format("root    : {0:X}", rootWin.ToInt64()));
            Debug.WriteLine(string.Format("topLevel: {0:X}", topLevelWin.ToInt64()));
            Debug.WriteLine("Selected Proc: " + procId);

            string appPath = null;
            string appName = null;

            var file = Native.GetProcessFile(procId);
            if (!string.IsNullOrEmpty(file)) {
                appPath = file;
                appName = Path.GetFileNameWithoutExtension(file);
            }

            if (appPath != null) {
                AddToIntentStore(appPath, appName);
            }
        }

        private void AddToIntentStore(string appPath, string appName) {
            var app = new WGestures.Core.ExeApp() { ExecutablePath = appPath, Name = appName };
            app.IsGesturingEnabled = false;

            var IntentStore = Parser.IntentFinder.IntentStore;

            ExeApp found;
            bool exist = IntentStore.TryGetExeApp(appPath, out found);
            if (exist) {
                //ShowErrorMsg("应用程序已经在列表中，请重新选择");
            }
            else {
                IntentStore.Add(app);
            }
        }


        public GestureContext Context { set; private get; }
        public GestureParser Parser { set; private get; }
    }
}
