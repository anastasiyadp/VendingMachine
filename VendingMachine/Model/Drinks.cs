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
        private string _name;
        private int _price;
        private int _count;
    
        public string name
        {
            get => _name;
            set => _name = value;
        }

        public int price
        {
            get => _price;
            set => _price = value;
        }

        public int count
        {
            get => _count;
            set => _count = value;
        }

        public Drinks(string name, int price, int count)
        {
            this.name = name;
            this.price = price;
            this.count = count;
        }

        public void BuyDrink() {
            count--;
        }

    }
}