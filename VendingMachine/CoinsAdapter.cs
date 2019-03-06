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
using VendingMachine.Model;

namespace VendingMachine
{
    class CoinsAdapter : BaseAdapter<Coins>
    {
        List<Coins> coins;
        Activity context;

        public CoinsAdapter(Activity context, List<Coins> coins) : base()
        {
            this.context = context;
            this.coins = coins;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public override Coins this[int position]
        {
            get { return coins[position]; }
        }
        public override int Count
        {
            get { return coins.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Coins coin = coins[position];
            View view = convertView;
            if (view == null) 
                view = context.LayoutInflater.Inflate(Resource.Layout.coinsRow, null);
            String nameCoin = view.FindViewById<TextView>(Resource.Id.textCoinNominal).Text = coin.Nominal.ToString();
            String countCoin = view.FindViewById<TextView>(Resource.Id.textCoinCount).Text = coin.Count.ToString();

            return view;
        }
    }
}