# JakePerry.Unity.GroupLogger
Groups log messages from a package or code system

The `GroupLogger` type can be used to group log messages from a single plugin, package, code system, etc. Once grouped, these logs can be enabled or disabled together, independent of the settings set on the current `ILogger`.

Grouped logs also optionally show an identifier tag at the beginning of the message in the format: `"[GroupTag] message..."`.

Example code below shows how a plugin might group all its logs, allowing the user of the plugin to enable or disable the logs at their discretion.

```cs
public static class MyPlugin
{
    // The logger that would be used for all logs produced by this plugin.
    public static GroupLogger Logger { get; } = new GroupLogger("MyPlugin");

    public static void DoTestLog()
    {
        Logger.Log("This is a test log!");

        // Output:
        // [MyPlugin] This is a test log!
    }
}

public class UserCode : MonoBehaviour
{
    public bool disablePluginLogs;

    private void Start()
    {
        // Uncomment to disable all logs from the MyPlugin plugin.
        //MyPlugin.Logger.SetAllLogTypesEnabled(false);

        // If disabled above, this line will not log anything
        MyPlugin.DoTestLog();
    }
}
```
