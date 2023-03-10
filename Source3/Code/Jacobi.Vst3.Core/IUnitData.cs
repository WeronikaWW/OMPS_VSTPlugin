using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    [ComImport]
    [Guid(Interfaces.IUnitData)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IUnitData
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 UnitDataSupported(
            [MarshalAs(UnmanagedType.I4), In] Int32 unitID);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetUnitData(
            [MarshalAs(UnmanagedType.I4), In] Int32 unitId,
            [MarshalAs(UnmanagedType.Interface), In] IBStream data);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetUnitData(
            [MarshalAs(UnmanagedType.I4), In] Int32 unitId,
            [MarshalAs(UnmanagedType.Interface), In] IBStream data);
    }
}
