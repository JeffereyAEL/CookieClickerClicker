using System;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Threading;
using Gecko;
using Gecko.DOM;
using Gecko.Events;

namespace CookieClickerClicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        protected WindowsFormsHost Host;
        protected GeckoWebBrowser Browser;

        protected bool bBrowserLoaded = false;

        protected bool bPaused = false;
        
        protected System.Windows.Threading.DispatcherTimer Update;
        
        protected System.Windows.Threading.DispatcherTimer ProductReady;
        
        protected GeckoHtmlElement BigCookie;

        protected GeckoHtmlElement NumCookiesTitle;
        
        protected GeckoHtmlElement ProductList;

        protected string ProductClass = "product";

        private int Margins = 10;

        private long NumCookies;
        
        public MainWindow()
        {
            InitializeComponent();
            Gecko.Xpcom.Initialize("Firefox");
            DebugWrapper.Width = 150;
            FillWindow();
        }
        
        private static long StripStringNum(string numberAsString)
        {
            numberAsString = Regex.Replace(numberAsString, "[,]", "");
            return long.Parse(numberAsString);
        }
        
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            FillWindow();
        }
        
        private void WebContainer_OnLoaded(object sender, RoutedEventArgs e)
        {
            Host = new WindowsFormsHost();
            Host.Width = BrowserWrapper.Width;
            Host.Height = BrowserWrapper.Height;
            Browser = new GeckoWebBrowser();
            Host.Child = Browser;
            Browser.DocumentCompleted += OnDocumentCompleted;
            BrowserWrapper.Children.Add(Host);
            Browser.Navigate("http://orteil.dashnet.org/cookieclicker/");

            DebugBox.Items.Add("WebContainer loaded");
            bBrowserLoaded = true;
        }

        private void FillWindow()
        {
            AppContainer.Width = this.Width - Margins * 2;
            AppContainer.Height = this.Height - Margins * 2;
            AppContainer.Margin = new Thickness(Margins);

            DebugWrapper.Height = AppContainer.Height;
            Canvas.SetTop(DebugWrapper, 0);
            Canvas.SetLeft(DebugWrapper, 0);
            PauseBtn.Height = 80;
            PauseBtn.Width = DebugWrapper.Width;
            DebugBox.Height = DebugWrapper.Height - PauseBtn.Height - Margins;
            DebugBox.Width = DebugWrapper.Width;
            Canvas.SetTop(DebugBox, PauseBtn.Height + Margins);

            ProductTierWrapper.Height = AppContainer.Height;
            ProductTierWrapper.Width = DebugWrapper.Width;
            Canvas.SetTop(ProductTierWrapper, 0);
            Canvas.SetLeft(ProductTierWrapper, DebugWrapper.Width + Margins);
            ProductTierBox.Width = ProductTierWrapper.Width;
            ProductTierBox.Height = ProductTierWrapper.Height;
            
            BrowserWrapper.Height = AppContainer.Height;
            BrowserWrapper.Width = AppContainer.Width - Margins * 4 - DebugWrapper.Width - ProductTierWrapper.Width;
            Canvas.SetTop(BrowserWrapper, 0);
            Canvas.SetLeft(BrowserWrapper, DebugWrapper.Width + ProductTierWrapper.Width + Margins * 2);

            if (bBrowserLoaded)
            {
                Host.Width = BrowserWrapper.Width;
                Host.Height = BrowserWrapper.Height;
            }
        }
        
        public void OnUpdate(object o, EventArgs args)
        {
           BigCookie.Click();
        }

        protected long GetNumCookies()
        {
            throw new NotImplementedException();
            // use NumCookiesTitle to update NumCookies
        }
        
        /*
         * recreates most valuable products list from all unlocked products
         */
        protected void UpdateProductsTierList()
        {
            // recreate product tier list from all unlocked products
        }
        
        /*
         * 
         */
        protected void OnUpgradeReady(object o, EventArgs args)
        {
            // TODO: set timer for expected time till buy for FA upgrade
            // currently check every 5 seconds
            
            // TODO: upgrade this to a smarter less short sighted algorithm
            // check if we can afford first available upgrade
            // if so
            //      buy upgrade
            //      if we can afford new first available upgrade, repeat
            // if we bought any upgrade
            //      UpdateProductTierList();
        }
        private void OnProductReady(object o, EventArgs args)
        {
            // TODO: set timer for expected time till by for tier 1 product
            // currently check every 5 seconds
            
            // check product count to see if there are any unlocks
            // add new ones to class list of unlocked products
            // UpdateProductTierList();

            // check if we can purchase tier 1 product
            // if so
            //      buy most efficient products that are available.
            //      update that product in tier list
            //      if can afford tier 1 product, repeat
        }
        
        protected int DocLoadedCount;
        private void OnDocumentCompleted(object o, GeckoDocumentCompletedEventArgs args)
        {
            ++DocLoadedCount;
            if (DocLoadedCount == 4)
            {
                DebugBox.Items.Add("Document loaded");
                bBrowserLoaded = true;
                
                // basic elements
                BigCookie = Browser.Document.GetElementById("cookies") as GeckoHtmlElement;
                // NumCookiesTitle = Browser.Document.GetElementById("cookies") as GeckoHtmlElement;
                
                // The basic cookie click
                Update = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(100)
                };
                Update.Tick += OnUpdate;
                Update.Start();

                // handle product related initializations
                ProductReady = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(10)
                };
                Update.Tick += OnProductReady;
                Update.Start();

                ProductList = Browser.Document.GetElementById("products") as GeckoHtmlElement;
                var elements = Browser.Document.GetElementsByClassName("product unlocked");
                DebugBox.Items.Add($"Num of products = {elements.Length}");
                // UpdateProductTierList();
                
                // temporary code
                // if (NumCookiesTitle == null) DebugBox.Items.Add("NumCookies Title is null");
                // GeckoInputElement cookies = new GeckoInputElement(NumCookiesTitle.DomObject);
                // DebugBox.Items.Add($"cookies:{cookies.Value}");
            }
        }

        private void PauseBtn_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!bBrowserLoaded) return;
            if (e.ChangedButton != MouseButton.Left) return;
            bPaused = !bPaused;
            PauseBtn.Content = bPaused ? "Unpause" : "Pause";
            Update.IsEnabled = !bPaused;
            ProductReady.IsEnabled = !bPaused;
        }
    }
}