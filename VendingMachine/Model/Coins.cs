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
    class Coins
    {
        private int nominal;
        private int count;

        public int Nominal
        {
            get => nominal;
            set => nominal = value;
        }

        public int Count
        {
            get => count;
            set => count = value;
        }

        public Coins(int nominal, int count)
        {
            this.nominal = nominal;
            this.count = count;
        }

        public void SpendMoney()
        {
            this.count--;
            
        }
    }
}