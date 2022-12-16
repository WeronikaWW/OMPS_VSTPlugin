﻿namespace Jacobi.Vst.Core.Host
{
    /// <summary>
    /// The IVstPluginCommandStub interface is implemented by the command stub for the Plugin commands
    /// in the Interop assembly.
    /// </summary>
    public interface IVstPluginCommandStub
    {
        /// <summary>
        /// Gets or sets the plugin context this instance is part of.
        /// </summary>
        IVstPluginContext PluginContext { get; set; }

        /// <summary>
        /// All Plugin Commands.
        /// </summary>
        public IVstPluginCommands24 Commands { get; }
    }
}
