using Jacobi.Vst3.Core;

namespace Jacobi.Vst3.Plugin
{
    public abstract class Bus
    {
        protected Bus(string name, BusTypes busType, BusInfo.BusFlags flags)
        {
            Name = name;
            BusType = busType;
            Flags = flags;

            IsActive = (flags & BusInfo.BusFlags.DefaultActive) != 0;
        }

        public MediaTypes MediaType { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public BusTypes BusType { get; set; }

        public BusInfo.BusFlags Flags { get; set; }

        public virtual bool GetInfo(ref BusInfo info)
        {
            info.MediaType = MediaType;
            info.BusType = BusType;
            info.Flags = Flags;
            info.Name = Name;

            return true;
        }
    }
}
