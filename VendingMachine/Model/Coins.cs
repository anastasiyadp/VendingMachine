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
        private int _nominal;
        private int _count;

        public int nominal
        {
            get => _nominal;
            set => _nominal = value;
        }

        public int count
        {
            get => _count;
            set => _count = value;
        }

        public Coins(int nominal, int count)
        {
            this.nominal = nominal;
            this.count = count;
        }

        public void SpendMoney()
        {
            count--;
        }

        public Coins DeepCopy()
        {
            Coins other = (Coins)this.MemberwiseClone();
            return other;
        }
    }
}