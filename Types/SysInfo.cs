using System.Collections.Generic;


namespace MeilisearchDotnet.Types {

    public class SysInfoGlobal<T> {
        public T totalMemory { get; set; }
        public T usedMemory { get; set; }
        public T totalSwap { get; set; }
        public T usedSwap { get; set; }
        public T inputData { get; set; }
        public T outputData { get; set; }
    }

    public class SysInfoProcess<T> {
        public T Memory { get; set; }
        public T Cpu { get; set; }
    }

    public class SysInfoType<T> {
        public T MemoryUsage { get; set; }
        public IEnumerable<T> ProcessorUsage { get; set; }
        public SysInfoGlobal<T> Global { get; set; }
        public SysInfoProcess<T> Process { get; set; }
    }

    public class SysInfo: SysInfoType<double?> {}
    public class SysInfoPretty: SysInfoType<string> {}

}