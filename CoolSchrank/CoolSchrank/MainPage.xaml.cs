using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CoolSchrank
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private String[] labels = { "Milch-1", "Milch-2", "Milch-3", "Milch-4", "Bier-1", "Bier-2", "Bier-3", "Bier-4", "Brot", "Butter", "Salat", "Speck" }; 

        public MainPage()
        {
            this.InitializeComponent();

            foreach(String label in labels)
            {
                CheckBox cb = new CheckBox();
                cb.Content = label;
                cb.Click += Cb_Click;
                listView.Items.Add(cb);
            }
        }

        private void Cb_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            // api call
            // api.changed(cb.Content, cb.IsChecked);
            Debug.WriteLine(cb.Content, cb.IsChecked.ToString());
        }

        private void Timer_Tick(object sender, object e)
        {

            // Create a datapoint
            var telemetryDataPoint = new
            {
                id = "coolschrank",
                temperature = 10,
                date = DateTime.Now
            };
                        
            // Format data to a JSON message
            var json = JsonConvert.SerializeObject(telemetryDataPoint);
            var message = new Message(Encoding.ASCII.GetBytes(json));

        }
    }
}
