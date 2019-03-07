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
    class VMAdapter: BaseAdapter<Drinks>
    {
        List<Drinks> drinks;
        Activity context;

        public VMAdapter(Activity context, List<Drinks> drinks) : base()
        {
            this.context = context;
            this.drinks = drinks;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Drinks this[int position]
        {
            get { return drinks[position]; }
        }

        public override int Count
        {
            get { return drinks.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Drinks drink = drinks[position];
            string name = drink.name;
            View view = convertView;
            if (view == null) view = context.LayoutInflater.Inflate(Resource.Layout.drinksRow, null);

            String nameDrink = view.FindViewById<TextView>(Resource.Id.textDrinkName).Text = drink.name;
            String priceDrink = view.FindViewById<TextView>(Resource.Id.textDrinkPrice).Text = drink.price.ToString();
            String countDrink = view.FindViewById<TextView>(Resource.Id.textDrinkCount).Text = drink.count.ToString();
            return view;
        }

       
    }
}