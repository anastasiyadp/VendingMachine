using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace VendingMachine.Model
{
    class Drinks
    {
        private string name;
        private int price;
        private int count;
    
        public string Name
        {
            get => name;
            set => name = value;
        }

        public int Price
        {
            get => price;
            set => price = value;
        }

        public int Count
        {
            get => count;
            set => count = value;
        }

        public Drinks(string name, int price, int count)
        {
            this.name = name;
            this.price = price;
            this.count = count;
        }

        void BuyDrink() {
            this.count--;
        }

    }
}