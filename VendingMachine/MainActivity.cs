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
        static List<Coins> peoplesCoins, VMCoins;
        int allMoney;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.activity_main);

            Button settingButton = FindViewById<Button>(Resource.Id.SettingVMButton);
            Button oddMoneyButton = FindViewById<Button>(Resource.Id.OddMonyeButton);
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
                Drinks drink = list[e.Position];
                BuyDrink(drink);
            };
            
            listViewCoins.Adapter = new CoinsAdapter(this, peoplesCoins);
            listViewCoins.ItemClick += (sender, e) => 
            {
                Coins coin = peoplesCoins[e.Position];
                DepositMoney(coin);
            };

            listViewCoinsVM.Adapter = new CoinsAdapter(this, VMCoins);

            oddMoneyButton.Click += (sender, e) =>
            {
                GetOddMoney();
            };

            settingButton.Click += (sender, e) => {
                var intent = new Intent(this,typeof(SettingVMActivity));
                List<string> co = new List<string>();
                List<string> co2 = new List<string>();
                foreach (var element in VMCoins) {
                    co.Add(element.Nominal.ToString());
                    co2.Add(element.Count.ToString());
                }
                intent.PutStringArrayListExtra("mi",co);
                intent.PutStringArrayListExtra("mi2",co2);

                StartActivityForResult(intent,0);
            };

            void AllMoney()
            {
                textView.Text = $"Внесено: {allMoney}  р.";
            }

            void DepositMoney(Coins coin)
            {
                if (coin.Count == 0) Toast.MakeText(this, $"Монеты номиналом {coin.Nominal} р. нет!", Android.Widget.ToastLength.Short).Show();
                else
                {
                    coin.SpendMoney();
                    Toast.MakeText(this, $"+ {coin.Nominal.ToString()} р.", Android.Widget.ToastLength.Short).Show();
                    allMoney += coin.Nominal;
                    VMCoins.Find(x => x.Nominal.Equals(coin.Nominal)).Count++;
                    AllMoney();

                    ((BaseAdapter)listViewCoins.Adapter).NotifyDataSetChanged();
                    ((BaseAdapter)listViewCoinsVM.Adapter).NotifyDataSetChanged();
                }
            }

            void BuyDrink(Drinks drink)
            {
                if (drink.Count == 0) Toast.MakeText(this, "Напиток закончился!", Android.Widget.ToastLength.Short).Show();
                else
                {
                    if (allMoney >= drink.Price)
                    {
                        allMoney -= drink.Price;
                        drink.Count--;
                        ((BaseAdapter)listViewDrinks.Adapter).NotifyDataSetChanged();
                        AllMoney();
                        Toast.MakeText(this, $"Вы купили  { drink.Name}", Android.Widget.ToastLength.Short).Show();
                    }
                    else Toast.MakeText(this, "Не хватает средств!", Android.Widget.ToastLength.Short).Show();
                }

            }

            void GetOddMoney()
            {
                int money = allMoney;
                List<Coins> test = VMCoins;
                //test.Sort();
                test.Reverse();
                
                foreach(var x in test)
                {
                    while(money/x.Nominal >= 1)
                    {
                        x.Count--;
                        money -= x.Nominal;
                        peoplesCoins.Find(y => y.Nominal.Equals(x.Nominal)).Count++;
                    }
                }
                test.Reverse();
                VMCoins = test;
                allMoney = money;
                AllMoney();
                ((BaseAdapter)listViewCoins.Adapter).NotifyDataSetChanged();
                ((BaseAdapter)listViewCoinsVM.Adapter).NotifyDataSetChanged();
            }

        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
                int put_name = data.GetIntExtra("Main_Put_Edit_Position2", 0);
                VMCoins[1].Count = put_name;
                //put_name and put_position should now hold the results you want, you can do whatever you want with these two values now in your MainActivity
            }
        }


    }
}