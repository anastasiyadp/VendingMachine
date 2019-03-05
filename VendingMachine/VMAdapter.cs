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
            string name = drink.Name;
            View view = convertView;
            if (view == null) view = context.LayoutInflater.Inflate(Resource.Layout.drinksRow, null);

            String nameDrink = view.FindViewById<TextView>(Resource.Id.textView1).Text = drink.Name;
            String countDrink = view.FindViewById<TextView>(Resource.Id.textView2).Text = drink.Price.ToString();
            String priceDrink = view.FindViewById<TextView>(Resource.Id.textView3).Text = drink.Count.ToString();

            Button drinkButton = view.FindViewById<Button>(Resource.Id.button1);
            drinkButton.Tag = name;
            drinkButton.SetOnClickListener(new ButtonClickListener(this.context));
            return view;
        }

        private class ButtonClickListener : Java.Lang.Object, View.IOnClickListener
        {
            private Activity activity;

            public ButtonClickListener(Activity activity)
            {
                this.activity = activity;
            }

            public void OnClick(View v)
            {
                string name = (string)v.Tag;
                string text = string.Format(name);

                Toast.MakeText(this.activity, $"Вы купили: {text}", ToastLength.Short).Show();
            }
        }

    }
}