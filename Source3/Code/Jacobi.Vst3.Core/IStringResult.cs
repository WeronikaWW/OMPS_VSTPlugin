using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    [ComImport]
    [Guid(Interfaces.IStringResult)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IStringResult
    {
        [PreserveSig]
        void SetText(
            [MarshalAs(UnmanagedType.LPStr), In] String text);
    }
}
