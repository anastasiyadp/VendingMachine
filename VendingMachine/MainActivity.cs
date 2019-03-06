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
using Android.Views;

namespace VendingMachine
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        List<Drinks> listDrinks;
        static List<Coins> peoplesCoins, VMCoins;
        int allMoney;
        public static String TAG = "MainActivity";
        public static int MY_CODE = 123;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.activity_main);

            Button settingButton = FindViewById<Button>(Resource.Id.buttonSettingVM);
            Button oddMoneyButton = FindViewById<Button>(Resource.Id.buttonOddMoney);
            ListView listViewDrinks = FindViewById<ListView>(Resource.Id.listViewDrinks);
            ListView listViewCoins = FindViewById<ListView>(Resource.Id.listViewCoins);
            ListView listViewCoinsVM = FindViewById<ListView>(Resource.Id.listViewCoinsVM);
            TextView textView = FindViewById<TextView>(Resource.Id.textViewSumCoins);

            listDrinks = new List<Drinks>() {
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
            
            listViewDrinks.Adapter = new VMAdapter(this, listDrinks);
            listViewDrinks.ItemClick += (sender, e) =>
            {
                Drinks drink = listDrinks[e.Position];
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

                StartActivityForResult(intent, MY_CODE);
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
            if (requestCode == MY_CODE)
            {
                if (resultCode == Result.Ok)
                {
                    IList<String> test3 = data.GetStringArrayListExtra("mi3");
                    IList<String> test4 = data.GetStringArrayListExtra("mi4");

                    List<Coins> testList = new List<Coins>();
                    for (int i = 0; i < test3.Count; i++)
                    {
                        testList.Add(new Coins(Convert.ToInt32(test3[i]), Convert.ToInt32(test4[i])));
                    }

                    VMCoins=testList;
                    ListView listViewCoinsVM = FindViewById<ListView>(Resource.Id.listViewCoinsVM);
                    listViewCoinsVM.Adapter = new CoinsAdapter(this, VMCoins);
                    ((BaseAdapter)listViewCoinsVM.Adapter).NotifyDataSetChanged();
                }
            }
        }


    }
}