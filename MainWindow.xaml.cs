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
        public MainWindow()
        {
            InitializeComponent();
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
    }
}