namespace Jacobi.Vst.Plugin.Framework
{
    using System;

    /// <summary>
    /// Flags that discribe plugin capabilities that could not be defined as interfaces.
    /// </summary>
    [Flags]
    public enum VstPluginCapabilities
    {
        /// <summary>The plugin has no extra capabilities to report to the host.</summary>
        None = 0x00,
        /// <summary>Indicates to the host that the plugin will request Time Information.</summary>
        ReceiveTimeInfo = 0x02,
    }
}
