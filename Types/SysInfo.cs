using System.Collections.Generic;

namespace MeilisearchDotnet.Types
{

    public class SysInfoGlobal
    {
        public double? TotalMemory { get; set; }
        public double? UsedMemory { get; set; }
        public double? TotalSwap { get; set; }
        public double? UsedSwap { get; set; }
        public double? InputData { get; set; }
        public double? OutputData { get; set; }
    }

    public class SysInfoProcess
    {
        public double? Memory { get; set; }
        public double? Cpu { get; set; }
    }

    public class SysInfo
    {
        public double? MemoryUsage { get; set; }
        public IEnumerable<double?> ProcessorUsage { get; set; }
        public SysInfoGlobal Global { get; set; }
        public SysInfoProcess Process { get; set; }
    }

    /*****************************************************************************/

    public class SysInfoGlobalPretty
    {
        public string TotalMemory { get; set; }
        public string UsedMemory { get; set; }
        public string TotalSwap { get; set; }
        public string UsedSwap { get; set; }
        public string InputData { get; set; }
        public string OutputData { get; set; }
    }

    public class SysInfoProcessPretty
    {
        public string Memory { get; set; }
        public string Cpu { get; set; }
    }

    public class SysInfoPretty
    {
        public string MemoryUsage { get; set; }
        public IEnumerable<string> ProcessorUsage { get; set; }
        public SysInfoGlobalPretty Global { get; set; }
        public SysInfoProcessPretty Process { get; set; }
    }

}
