#pragma once

#include "HostCommandsImpl.h"

namespace Jacobi {
namespace Vst {
namespace Plugin {
namespace Interop {

    /// <summary>
    /// The HostCommandStub calls the host callback function.
    /// </summary>
    ref class HostCommandStub : Jacobi::Vst::Core::Plugin::IVstHostCommandProxy
    {
    public:
        /// <summary>
        /// Disposes the managed resources and calls the finalizer.
        /// </summary>
        ~HostCommandStub();
        /// <summary>
        /// Disposes the unmanaged resources.
        /// </summary>
        !HostCommandStub();

        // IVstHostCommandStub

        /// <summary>
        /// Updates the unmanaged <b>Vst2Plugin</b> structure with the new values in the <paramref name="pluginInfo"/>.
        /// </summary>
        /// <param name="pluginInfo">Must not be null.</param>
        /// <remarks>When AudioInputCount, AudioOutputCount or InitialDelay have changed the IoChanged() method is called automatically.</remarks>
        virtual System::Boolean UpdatePluginInfo(Jacobi::Vst::Core::Plugin::VstPluginInfo^ pluginInfo);

        /// <summary>
        /// Access to the host commands. Can be null early in the initialization process.
        /// </summary>
        virtual property Jacobi::Vst::Core::IVstHostCommands20^ Commands { 
            Jacobi::Vst::Core::IVstHostCommands20^ get() { return _commands; }
        }

    internal:
        HostCommandStub(::Vst2HostCommand hostCommandHandler);
        void Initialize(::Vst2Plugin* pluginInfo) 
        {
            if(pluginInfo == NULL) { throw gcnew System::ArgumentNullException("pluginInfo"); } 
            _commands->Initialize(pluginInfo);
        }

        bool IsInitialized() { return (_commands != nullptr); }

    private:
        Jacobi::Vst::Plugin::Interop::HostCommandsImpl^ _commands;
        ::Vst2HostCommand _hostCommand;
    };

}}}} // Jacobi::Vst::Plugin::Interop