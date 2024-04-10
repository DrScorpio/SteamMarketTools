using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamMarketTools
{
    public class OnSaleGenerater
    {
        private const string BASE_URL = "https://steamcommunity.com/market/multisell?appid=730&contextid=2";
        public const string ITEM_PREFIX = "&items[]=";
        public List<String> ItemList;

        public OnSaleGenerater()
        {
            this.ItemList = new List<string>();
        }

        public String ShowURL()
        {
            var URL = BASE_URL;
            foreach (var itemURL in ItemList)
            {
                URL = URL + itemURL;
            }
            return URL;
        }

        public void AddItem(Item item)
        {
            this.ItemList.Add(item.URL);

        }

        public void RemoveItem(Item item)
        {
            this.ItemList.Remove(item.URL);
        }

        public void ClearItem()
        {
            this.ItemList.Clear();
        }
    }

    public class Item
    {
        private string name;
        private string uRL;

        public string Name { get => name; set => name = value; }
        public string URL { get => uRL; set => uRL = value; }

        public Item(string name, string id)
        {
            this.Name = name;
            this.URL = "&items[]=" + id;
        }
    }
}

