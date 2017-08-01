using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IotHubDeviceSimulator.Models
{
    class EventMessage
    {
        public string Message { get; set; }
        public string Level { get; set; }
        public int AlarmId { get; set; }
    }
}
