using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms.Integration;

namespace MHCI.Hackathon.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WebKit.WebKitBrowser _browser;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region WebKitBrowser Routines
        /// <summary>
        /// Sends javascript calls to the underlying webkit engine
        /// </summary>
        /// <param name="js">string containing javascript code</param>
        public void MakeJSCall(string js)
        {
            try
            {
                _browser.StringByEvaluatingJavaScriptFromString(js);
            }
            catch (System.Exception h)
            {
                InitializeComponent(); // Restart UI
            }
        }
        #endregion

        /// <summary>
        /// Initializes several code routines after the webkit containing element loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browserContainer_Loaded(object sender, RoutedEventArgs e)
        {
            WindowsFormsHost host = new WindowsFormsHost();

            _browser = new WebKit.WebKitBrowser();
            host.Child = _browser;

            this.browserContainer.Children.Add(host);
            
            string url = "";
            _browser.Navigate(url);

            /*
            this.ViewModel.PropertyChanged += (o, f) =>
            {
                if (f.PropertyName.Equals("URI"))
                {
                    MakeJSCall(this.ViewModel.URI);
                    //playSoundFeedback();
                }
            };*/

            //setupTimer.Start();


        }

    }
}
