using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotHubDeviceSimulator.Models
{
    class DeviceConfiguration
    {
        public int ConfigId { get; set; }
        public int DataInterval { get; set; }
        public string Status { get; set; }
        public DeviceConfiguration PendingConfig { get; set; }
        
    }
}
