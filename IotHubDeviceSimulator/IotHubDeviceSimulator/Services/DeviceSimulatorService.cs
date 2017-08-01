using IotHubDeviceSimulator.Connectors;
using IotHubDeviceSimulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IotHubDeviceSimulator.Services
{
    class DeviceSimulatorService
    {
        private AzureDeviceMqttConnector _azureConnector;

        public DeviceSimulatorService(AzureDeviceMqttConnector azureConnector)
        {
            _azureConnector = azureConnector;
        }

        public void GenerateSimulateMessages(int timeout)
        {
            var timer = new Timer((state) => {
                var self = (state as DeviceSimulatorService);
                var randomGenerator = new Random(500);
                var message = new DeviceMessage<MetricMessage>()
                {
                    MessageType = "metricData", Body = new MetricMessage()
                    {
                        Group1 = new { a1 = randomGenerator.Next(0,200), a2 = randomGenerator.Next(200,250), a3 = randomGenerator.Next(150,300) },
                        Group2 = new { b1 = randomGenerator.Next(0, 200), b2 = randomGenerator.Next(200, 250), b3 = randomGenerator.Next(150, 300) }
                    }
                   
                };
                self._azureConnector.SendMessageToCloud<MetricMessage>(message).Wait();
            }, this, 0, timeout);
        }
    }
}
