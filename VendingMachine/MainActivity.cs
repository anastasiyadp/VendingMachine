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
        List<Coins> listPeoplesCoins, listVMCoins, listCoins;
        int allMoney;
        public int MY_CODE = 123;

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

            listPeoplesCoins = new List<Coins>() {
                new Coins(1, 10),
                new Coins(2, 30),
                new Coins(5,20),
                new Coins(10,15) 
            };

            listVMCoins = new List<Coins>(){
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

            listViewCoins.Adapter = new CoinsAdapter(this, listPeoplesCoins);
            listViewCoins.ItemClick += (sender, e) => 
            {
                Coins coin = listPeoplesCoins[e.Position];
                DepositMoney(coin);
            };

            listViewCoinsVM.Adapter = new CoinsAdapter(this, listVMCoins);

            oddMoneyButton.Click += (sender, e) =>
            {
                if (allMoney == 0) Toast.MakeText(this, "Внесенных средств нет!", Android.Widget.ToastLength.Short).Show();
                else
                {
                    listVMCoins.Reverse();
                    GetOddMoney(listVMCoins);
                }
            };

            settingButton.Click += (sender, e) => {
                var intent = new Intent(this,typeof(SettingVMActivity));
                List<string> listCoinNominal = new List<string>();
                List<string> listCoinCount = new List<string>();
                foreach (var element in listVMCoins) {
                    listCoinNominal.Add(element.nominal.ToString());
                    listCoinCount.Add(element.count.ToString());
                }
                intent.PutStringArrayListExtra("CoinNominal", listCoinNominal);
                intent.PutStringArrayListExtra("CoinCount", listCoinCount);

                StartActivityForResult(intent, MY_CODE);
            };

            void AllMoney()
            {
                textView.Text = $"Внесено: {allMoney}  р.";
            }

            void DepositMoney(Coins coin)
            {
                String result;
                if (coin.count == 0) result = $"Монеты номиналом {coin.nominal} р. нет!";
                else
                {
                    coin.SpendMoney();
                    result = $"+ {coin.nominal.ToString()} р.";
                    allMoney += coin.nominal;
                    listVMCoins.Find(x => x.nominal.Equals(coin.nominal)).count++;
                    AllMoney();

                    ((BaseAdapter)listViewCoins.Adapter).NotifyDataSetChanged();
                    ((BaseAdapter)listViewCoinsVM.Adapter).NotifyDataSetChanged();
                }
                Toast.MakeText(this, result, Android.Widget.ToastLength.Short).Show();
            }

             void BuyDrink(Drinks drink)
            {
                String result;
                if (drink.count == 0) result = "Напиток закончился!";
                else
                {
                    if (allMoney >= drink.price)
                    {
                        allMoney -= drink.price;
                        drink.count--;                        
                        AllMoney();
                        result = $"Вы купили  {drink.name}";
                        ((BaseAdapter)listViewDrinks.Adapter).NotifyDataSetChanged();
                    }
                    else result = "Не хватает средств!";
                }
                Toast.MakeText(this, result, Android.Widget.ToastLength.Short).Show();
            }

            //void GetOddMoney()
            //{
            //    listVMCoins.Reverse(); //Подразумевается, что список уже отсортирован
                
            //    foreach(var coin in listVMCoins)
            //    {
            //        while(allMoney / coin.nominal >= 1)
            //        {
            //            coin.count--;
            //            allMoney -= coin.nominal;
            //            listPeoplesCoins.Find(x => x.nominal.Equals(x.nominal)).count++;
            //        }
            //    }
            //    listVMCoins.Reverse();
            //    AllMoney();
            //    ((BaseAdapter)listViewCoins.Adapter).NotifyDataSetChanged();
            //    ((BaseAdapter)listViewCoinsVM.Adapter).NotifyDataSetChanged();
            //}


            void GetOddMoney(List<Coins> listMachine)
            {
                List<Coins> changeListPeopleCoins = new List<Coins>();
                List<Coins> changeListVMCoins = new List<Coins>();
                List<Coins> newListVMCoins = new List<Coins>();
                int sum = allMoney;
                //listMachine.Reverse(); //Подразумевается, что список уже отсортирован


                listPeoplesCoins.ForEach((item) =>
                {
                    changeListPeopleCoins.Add(item.DeepCopy());
                });

                listMachine.ForEach((item) =>
                {
                    changeListVMCoins.Add(item.DeepCopy());
                });

                listVMCoins.ForEach((item) =>
                {
                    newListVMCoins.Add(item.DeepCopy());
                });

                foreach (var coin in changeListVMCoins)
                {
                    if (coin.count != 0)
                    {
                        while (sum / coin.nominal >= 1)
                        {
                            newListVMCoins.Find(x => x.nominal.Equals(coin.nominal)).count--;
                            sum -= coin.nominal;
                            changeListPeopleCoins.Find(x => x.nominal.Equals(coin.nominal)).count++;
                        }
                    }
                }
                if (sum != 0 && changeListVMCoins.Count != 1)
                {
                    changeListVMCoins.RemoveAt(0);
                    GetOddMoney(changeListVMCoins);
                }
                else
                {
                    if (sum != 0)
                        Toast.MakeText(this, "Автомат не может выдать сдачу!", Android.Widget.ToastLength.Short).Show();
                    else
                    {
                        allMoney = sum;
                        AllMoney();

                        listVMCoins.Clear();
                        newListVMCoins.ForEach((item) =>
                        {
                            listVMCoins.Add(item.DeepCopy());
                        });

                        listPeoplesCoins.Clear();
                        changeListPeopleCoins.ForEach((item) =>
                        {
                            listPeoplesCoins.Add(item.DeepCopy());
                        });

                        listVMCoins.Reverse();

                        ((BaseAdapter)listViewCoins.Adapter).NotifyDataSetChanged();
                        ((BaseAdapter)listViewCoinsVM.Adapter).NotifyDataSetChanged();
                    }
                }
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == MY_CODE)
            {
                if (resultCode == Result.Ok)
                {
                    IList<String> listCoinNominal = data.GetStringArrayListExtra("CoinNominal");
                    IList<String> listCoinCount = data.GetStringArrayListExtra("CoinCount");

                    List<Coins> newlistVMCoins = new List<Coins>();
                    for (int i = 0; i < listCoinNominal.Count; i++)
                    {
                        newlistVMCoins.Add(new Coins(Convert.ToInt32(listCoinNominal[i]), Convert.ToInt32(listCoinCount[i])));
                    }

                    listVMCoins= newlistVMCoins;
                    ListView listViewCoinsVM = FindViewById<ListView>(Resource.Id.listViewCoinsVM);
                    listViewCoinsVM.Adapter = new CoinsAdapter(this, listVMCoins);
                    ((BaseAdapter)listViewCoinsVM.Adapter).NotifyDataSetChanged();
                }
            }
        }


    }
}