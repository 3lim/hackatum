using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Input;
using System.Net.Http;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CoolSchrank
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private HttpClient hc;
        private List<String> labels;
        private String apiUri = ""; // TODO: insert base Uri

        DispatcherTimer timer;

        public MainPage()
        {
            this.InitializeComponent();

            hc = new HttpClient();
            labels = new List<String>();

            labels.Add("Milch-1");
            labels.Add("Milch-2");
            labels.Add("Milch-3");
            labels.Add("Milch-4");
            labels.Add("Bier-1");
            labels.Add("Bier-2");
            labels.Add("Bier-3");
            labels.Add("Bier-4");
            labels.Add("Bier-5");
            labels.Add("RedBull");

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += Timer_Tick;
            timer.Start();

            // TODO: api call: get labels

            foreach(String label in labels)
            {
                GenerateListItem(labels.IndexOf(label), label);
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            ReLabel();
        }

        private void tswitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch tswitch = (ToggleSwitch)sender;

            int index = (int)tswitch.Tag;
            double value = (tswitch.IsOn) ? 1.0 : 0.0;
            SendData(index, value);
        }

        private async void image_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            GenerateListItem(await CreateNewSlot(), " ");
            ReOrderItems();
        }


        private void GenerateListItem(int index, String label)
        { 

            // Container
            RelativePanel container = new RelativePanel();
            container.Tag = index;

            Thickness border = container.BorderThickness;
            border.Bottom = 1;
            container.BorderThickness = border;
            container.BorderBrush = new SolidColorBrush(Colors.White);

            // Slot info
            TextBlock slot = new TextBlock();
            slot.Text = "Slot " + (index + 1) + ": ";
            slot.VerticalAlignment = VerticalAlignment.Center;
            slot.HorizontalAlignment = HorizontalAlignment.Left;

            Thickness slot_margin = slot.Margin;
            slot_margin.Left = 25;
            slot.Margin = slot_margin;

            // Switch
            ToggleSwitch tswitch = new ToggleSwitch();
            tswitch.Header = label;
            tswitch.Tag = index;
            tswitch.OffContent = "not available";
            tswitch.OnContent = "available";
            tswitch.IsOn = true;
            tswitch.Toggled += tswitch_Toggled;
            tswitch.HorizontalAlignment = HorizontalAlignment.Left;
            tswitch.VerticalAlignment = VerticalAlignment.Center;

            Thickness switch_margin = tswitch.Margin;
            switch_margin.Left = 150;
            tswitch.Margin = switch_margin;

            // Secure
            ToggleSwitch secure = new ToggleSwitch();
            secure.Tag = index;
            secure.Header = " ";
            secure.OffContent = "not secure";
            secure.OnContent = "secure";
            secure.HorizontalAlignment = HorizontalAlignment.Left;
            tswitch.VerticalAlignment = VerticalAlignment.Center;

            Thickness secure_margin = secure.Margin;
            secure_margin.Left = 350;
            secure.Margin = secure_margin;

            // Destroy
            Image dest = new Image();
            dest.Source = new BitmapImage(new Uri("ms-appx://CoolSchrank/Assets/Plus.png"));
            dest.VerticalAlignment = VerticalAlignment.Center;
            dest.Height = 50;
            dest.Tag = index;
            dest.PointerPressed += dest_PointerPressed;
            RelativePanel.SetAlignRightWithPanel(dest, true);
            
            RotateTransform rt = new RotateTransform();
            rt.Angle = 45;
            dest.RenderTransform = rt;

            // Adding
            container.Children.Add(slot);
            container.Children.Add(tswitch);
            container.Children.Add(secure);
            container.Children.Add(dest);
            list.Children.Add(container);
        }

        private void dest_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Image img = (Image)sender;
            int index = (int)img.Tag;
            DestroySlot(index);

            list.Children.Remove((Panel)img.Parent);
        }

        private void ReOrderItems()
        {

            List<Panel> items = new List<Panel>();
            foreach (Panel item in list.Children)
            {
                items.Add(item);
            }
            list.Children.Clear();

            items.Sort(new PanelComparer());

            foreach (Panel item in items)
            {
                list.Children.Add(item);
            }
        }
        
        private async void ReLabel()
        {
            /*Dictionary<int, string> labels = await GetLabels();

            foreach (Panel item in list.Children)
            {
                ToggleSwitch tswitch = (ToggleSwitch)item.Children[1];
                tswitch.Header = labels[(int)tswitch.Tag];
            }*/
        }

        // API  
            
        private async Task<int> CreateNewSlot()
        {
            /*HttpResponseMessage response = await hc.PostAsync(apiUri + "", new HttpMessageContent(new HttpRequestMessage())); // TODO: neue Slot id 
            var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(await response.Content.ReadAsStringAsync());
            return Int32.Parse(json["id"]); */
            return 3;
        }

        private async void DestroySlot(int index)
        {
            /*await hc.DeleteAsync(apiUri + ""); // TODO: Slot Freigeben*/
        }

        private async void SendData(int index, double value)
        {

            /*var dataSet = new
            {
                id = index,
                value = value,
            };

            await hc.PostAsJsonAsync(apiUri + "", dataSet); //TODO: Entrypoint*/
        }

        private async Task<Dictionary<int, string>> GetLabels()
        {
            HttpResponseMessage response = await hc.PostAsync(apiUri + "", new HttpMessageContent(new HttpRequestMessage())); // TODO: neue Slot id 
            var json = JsonConvert.DeserializeObject<Dictionary<int, string>>(await response.Content.ReadAsStringAsync());
            return json;
        }


        public class PanelComparer : IComparer<Panel>
        {
            public int Compare(Panel x, Panel y)
            {
                int tag1 = (int)x.Tag;
                int tag2 = (int)y.Tag;

                return tag1.CompareTo(tag2);
            }
        }

    }
}
