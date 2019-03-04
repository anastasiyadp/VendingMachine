using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content;
using System;
using Java.Util;
using System.Collections.Generic;
using VendingMachine.Model;

namespace VendingMachine
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        List<Drinks> list;
        List<Coins> peoplesCoins, VMCoins;
        int allMoney;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.activity_main);

            Button settingButton = FindViewById<Button>(Resource.Id.SettingVMButton);
            ListView listViewDrinks = FindViewById<ListView>(Resource.Id.listView1);
            ListView listViewCoins = FindViewById<ListView>(Resource.Id.listView2);
            ListView listViewCoinsVM = FindViewById<ListView>(Resource.Id.listView3);
            TextView textView = FindViewById<TextView>(Resource.Id.textView4);

            list = new List<Drinks>() {
                new Drinks("Чай", 13, 10),
                new Drinks("Кофе", 18, 30),
                new Drinks("Кофе с молоком", 21, 20),
                new Drinks("Сок", 35, 15 )
            };

            peoplesCoins = new List<Coins>() {
                new Coins(1, 10),
                new Coins(2, 30),
                new Coins(5,20),
                new Coins(10,15) 
            };

            VMCoins = new List<Coins>(){
               new Coins(1, 100),
               new Coins(2, 100),
               new Coins(5, 100),
               new Coins(10, 100)
            };
            
            listViewDrinks.Adapter = new VMAdapter(this, list);
            listViewDrinks.ItemClick += (sender, e) =>
            {
                var t = list[e.Position];

                if (allMoney >= t.Price)
                {
                    allMoney -= t.Price;
                    t.Count--;
                    ((BaseAdapter)listViewDrinks.Adapter).NotifyDataSetChanged();
                    AllMoney();
                    Toast.MakeText(this, $"Вы купили  { t.Name}", Android.Widget.ToastLength.Short).Show();
                }
                else
                    Toast.MakeText(this, "Не хватает средств!", Android.Widget.ToastLength.Short).Show();
            };

            listViewCoins.Adapter = new CoinsAdapter(this, peoplesCoins);
            listViewCoins.ItemClick += (sender, e) => 
            {
                var t = peoplesCoins[e.Position];
                Toast.MakeText(this, "+" + t.Nominal.ToString() + " р.", Android.Widget.ToastLength.Short).Show();
                if (t.Count!=0) t.SpendMoney();
                allMoney += t.Nominal;
                AllMoney();
                ((BaseAdapter)listViewCoins.Adapter).NotifyDataSetChanged();
            };

            listViewCoinsVM.Adapter = new CoinsAdapter(this, VMCoins);

            settingButton.Click += (sender, e) => {
                var intent = new Intent(this,typeof(SettingVMActivity));
                StartActivity(intent);
            };

            void AllMoney()
            {
                textView.Text = $"Внесено: {allMoney}  р.";
            }
        }

        
       
       
    }
}