using UnityEngine;

namespace JakePerry.Unity.Examples
{
    /// <summary>
    /// This is an example of user code that interacts with a plugin.
    /// See the plugin code here: <see cref="MyPlugin"/>.
    /// </summary>
    public class UserCode : MonoBehaviour
    {
        public bool disablePluginLogs;

        private void Start()
        {
            // Disable all logs from the MyPlugin plugin if we've decided to do so...
            if (disablePluginLogs)
                MyPlugin.Logger.SetAllLogTypesEnabled(false);

            // Ask the plugin to log a test message. The log will only display
            // if we didn't disable logs on the line above.
            MyPlugin.DoTestLog();
        }
    }
}
