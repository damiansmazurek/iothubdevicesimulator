using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using IotHubDeviceSimulator.Models;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Azure.Devices.Shared;

namespace IotHubDeviceSimulator.Connectors
{
    class AzureDeviceMqttConnector
    {
        private const string telemetricConfigSectionName = "telemetricConfiguration";
        private DeviceClient device;
        public delegate void MessageHandler(Message hubMessage);
        public delegate void ConfigurationHandler(DeviceConfiguration configuration);
   
        public void ConnectDeviceToHub(string connectionString)
        {
            device = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Mqtt);
            
        }

        public async Task<string> SendMessageToCloud<T>(DeviceMessage<T> deviceMessage)
        {
            var messageString = JsonConvert.SerializeObject(deviceMessage.Body);
            var message = new Message(Encoding.ASCII.GetBytes(messageString));
            message.MessageId = Guid.NewGuid().ToString();
            message.Properties.Add("messageType", deviceMessage.MessageType);
            await device.SendEventAsync(message);
            Console.WriteLine("{0} > Sending message. MessageType: {1}, MessageBody: {2}", DateTime.Now, deviceMessage.MessageType, messageString);
            return message.MessageId;
        }

        public async Task ReceiveMessageFromCloud(MessageHandler callback)
        {
            while (true)
            {
                var result = await device.ReceiveAsync();
                callback(result);
            }
        }
    
        public async Task SendFile(string blobName, Stream file)
        {
           await device.UploadToBlobAsync(blobName, file);
        }

        public async Task ReadDeviceTwin(ConfigurationHandler callback)
        {
            while (true)
            {
               var twin = await device.GetTwinAsync();
               var deviceConfiguration = twin.Properties.Desired[telemetricConfigSectionName] as DeviceConfiguration;
               callback(deviceConfiguration);
            }
        }

        public async Task SendDeviceTwinToHub(DeviceConfiguration configuration)
        {
            var properties = new TwinCollection();
            properties[telemetricConfigSectionName] = configuration;
            await device.UpdateReportedPropertiesAsync(properties);
        }

        public async Task RegisterDeviceMethod(string methodName, MethodCallback callback)
        {
            await device.SetMethodHandlerAsync(methodName, callback, null);
        }

}
}
