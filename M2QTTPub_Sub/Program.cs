using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace M2QTTPub_Sub
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            //*********************Publish******************

            // create client instance
            MqttClient pubClient = new MqttClient("iot.eclipse.org");// IPAddress.Parse(MQTT_BROKER_ADDRESS));

            string clientId = Guid.NewGuid().ToString();
            pubClient.Connect(clientId);

            string strValue = Convert.ToString("John Test value");

            // publish a message on "/home/temperature" topic with QoS 2
            pubClient.Publish("/home/temperature", Encoding.UTF8.GetBytes(strValue), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);


            //***********************Subscribe****************

            // create client instance
            MqttClient subClient = new MqttClient("iot.eclipse.org"); ;// IPAddress.Parse(MQTT_BROKER_ADDRESS));

            // register to message received
            subClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            string subClientId = Guid.NewGuid().ToString();
            subClient.Connect(subClientId);

            // subscribe to the topic "/home/temperature" with QoS 2
            subClient.Subscribe(new string[] { "/home/temperature" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

            Console.ReadLine();
        }


        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // handle message received
            var message = Encoding.Default.GetString(e.Message);
            Console.WriteLine("Message received: " + message);
        }
    }
}
