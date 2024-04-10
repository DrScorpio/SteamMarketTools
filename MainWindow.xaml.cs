using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SteamMarketTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public OnSaleGenerater generater;
        public MainWindow()
        {
            InitializeComponent();

            GetItems(ItemBox);

            this.generater = new OnSaleGenerater();

            UrlBox.Text = generater.ShowURL();
        }



        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int t = 0;
            if (e.Text == " ")
                e.Handled = true;
            if (!int.TryParse(e.Text, out t) && e.Text != ".")
                e.Handled = true;
        }

        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                double d = 0;
                if (!double.TryParse(text, out d))
                { e.CancelCommand(); }
            }
            else { e.CancelCommand(); }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void DoCalculate(object sender, RoutedEventArgs e)
        {
            if(PrimeCostBox.Text == "")
            {
                MessageBox.Show("请输入收购价格!");
                return;
            }
            if(MarketOrderPriceBox.Text == "" && MarketSellingPriceBox.Text == "") {
                MessageBox.Show("请输入市场售出价格!");
                return;
            }
            double primeC = Double.Parse(PrimeCostBox.Text);
            double usePrice = primeC;
            if(MarketSellingPriceBox.Text != "" && MarketOrderPriceBox.Text != "")
            {
                double orderSellingP = Double.Parse(MarketOrderPriceBox.Text);
                double SellingP = Double.Parse(MarketSellingPriceBox.Text);
                double taxedOrder = Math.Round(SteamMarketCalculator.TaxedPrice(orderSellingP), 2);
                double taxedSell = Math.Round(SteamMarketCalculator.TaxedPrice(SellingP), 2);
                usePrice = Math.Max(taxedOrder, taxedSell);
            }else if (MarketOrderPriceBox.Text != "")
            {
                usePrice = Double.Parse(MarketOrderPriceBox.Text);
            }else if (MarketSellingPriceBox.Text != "")
            {
                usePrice = Double.Parse(MarketSellingPriceBox.Text);
            }
            var discount = SteamMarketCalculator.GetDiscount(primeC,usePrice);
            DiscountText.Text = discount.ToString();
        }

        private void OrderChanged(object sender, TextChangedEventArgs e)
        {
            if (MarketOrderPriceBox.Text != "")
            {
                double orderSellingP = Double.Parse(MarketOrderPriceBox.Text);
                double taxedOrder = Math.Round(SteamMarketCalculator.TaxedPrice(orderSellingP), 2);
                orderDiscount.Text = taxedOrder.ToString();
            }
        }

        private void SellChanged(object sender, TextChangedEventArgs e)
        {
            if (MarketSellingPriceBox.Text != "")
            {
                double sellingP = Double.Parse(MarketSellingPriceBox.Text);
                double taxedSelling = Math.Round(SteamMarketCalculator.TaxedPrice(sellingP), 2);
                sellDiscount.Text = taxedSelling.ToString();
            }
        }


        public void GetItems(ListBox box)
        {
            try
            {
                // 读取文本文件
                using (StreamReader sr = new StreamReader("Item.data"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var l = line.Split(' ');
                        var it = new Item(l[0], l[1]);
                        box.Items.Add(it);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (ItemBox.SelectedItem == null)
            {
                MessageBox.Show("请在左侧选择要添加的内容");
                return;
            }
            Item item = (Item)ItemBox.SelectedItem;
            generater.AddItem(item);
            SelectedItemBox.Items.Add(item);
            ItemBox.Items.Remove(item);
            UrlBox.Text = generater.ShowURL();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItemBox.SelectedItem == null)
            {
                MessageBox.Show("请在右侧选择要删除的内容");
                return;
            }
            Item item = (Item)SelectedItemBox.SelectedItem;
            generater.RemoveItem(item);
            SelectedItemBox.Items.Remove(item);
            ItemBox.Items.Add(item);
            UrlBox.Text = generater.ShowURL();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            foreach (Item item in SelectedItemBox.Items)
            {
                ItemBox.Items.Add(item);
            }
            SelectedItemBox.Items.Clear();
            generater.ClearItem();
            UrlBox.Text = generater.ShowURL();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {

            Clipboard.SetText(UrlBox.Text);
        }


        // 关闭
        private void CustomWindowBtnClose_Click(object sender, RoutedEventArgs e)
        {
            Window win = Application.Current.MainWindow;
            win.Close();
        }

        // 最小化
        private void CustomWindowBtnMinimized_Click(object sender, RoutedEventArgs e)
        {
            Window win = Application.Current.MainWindow;
            win.WindowState = WindowState.Minimized;
        }

        // 最大化、还原
        private void CustomWindowBtnMaxNormal_Click(object sender, RoutedEventArgs e)
        {
            Window win = Application.Current.MainWindow;
            if (win.WindowState == WindowState.Maximized)
            {
                win.WindowState = WindowState.Normal;
            }
            else
            {
                // 不覆盖任务栏
                win.MaxWidth = SystemParameters.WorkArea.Width;
                win.MaxHeight = SystemParameters.WorkArea.Height;
                win.WindowState = WindowState.Maximized;
            }
        }
    }
}