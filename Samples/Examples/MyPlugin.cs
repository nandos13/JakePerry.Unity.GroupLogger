namespace JakePerry.Unity.Examples
{
    /// <summary>
    /// This is an example of a plugin that uses a group logger.
    /// See user code example here: <see cref="UserCode"/>.
    /// </summary>
    public static class MyPlugin
    {
        // This line defines a static logger that should be used for
        // all log messages in this plugin.
        public static GroupLogger Logger { get; } = new GroupLogger("MyPlugin");

        public static void DoTestLog()
        {
            Logger.Log("This is a test log!");

            // Output:
            // [MyPlugin] This is a test log!
        }
    }
}
