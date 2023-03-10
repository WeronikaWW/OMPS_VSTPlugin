using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    [StructLayout(LayoutKind.Sequential, Pack = Platform.StructurePack)]
    public struct RoutingInfo
    {
        public static readonly int Size = Marshal.SizeOf<RoutingInfo>();

        [MarshalAs(UnmanagedType.I4)]
        public MediaTypes MediaType;	// media type see \ref MediaTypes

        [MarshalAs(UnmanagedType.I4)]
        public Int32 BusIndex;			// bus index

        [MarshalAs(UnmanagedType.I4)]
        public Int32 Channel;			// channel (-1 for all channels)
    }
}
