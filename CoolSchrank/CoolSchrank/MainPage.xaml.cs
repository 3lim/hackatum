using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CoolSchrank
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DeviceClient deviceClient;
        private DispatcherTimer timer;
        private Random rnd;

        public MainPage()
        {
            this.InitializeComponent();

            deviceClient = DeviceClient.Create("hackaTUMhub", new DeviceAuthenticationWithRegistrySymmetricKey("coolschrank", "Sp8+AaL4QpFn6UAU831M+kHZv1gFDRorMjuS4Skuo4E="));

            rnd = new Random();

            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            timer.Start();

        }

        private async void Timer_Tick(object sender, object e)
        {


            // Take measurement
            var temp = rnd.Next(1000);

            // Create a datapoint
            var telemetryDataPoint = new
            {
                id = "coolschrank",
                temperature = temp,
                date = DateTime.Now
            };

            // Format data to a JSON message
            var json = JsonConvert.SerializeObject(telemetryDataPoint);
            var message = new Message(Encoding.ASCII.GetBytes(json));

            // Send message to the IoT Hub
            await deviceClient.SendEventAsync(message);
        }

    }
}
