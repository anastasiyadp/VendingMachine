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
using static Android.Widget.GridLayout;

namespace VendingMachine
{
    [Activity(Label = "@string/setting_activity")]
    public class SettingVMActivity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_settings);
          
            //LinearLayout linearLayout = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            GridLayout gridLayout = FindViewById<GridLayout>(Resource.Id.gridLayout1);
            gridLayout.ColumnCount = 5;
            Button saveButton = FindViewById<Button>(Resource.Id.buttonSave);
            Button backButton = FindViewById<Button>(Resource.Id.buttonBack);
            LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);

            IList<String> listCoinNominal = Intent.GetStringArrayListExtra("CoinNominal");
            IList<String> listCoinCount = Intent.GetStringArrayListExtra("CoinCount");

            List<Coins> test3 = new List<Coins>();
         

           
            for(int i=0; i< listCoinNominal.Count; i++)
            {
                test3.Add(new Coins(Convert.ToInt32(listCoinNominal[i]), Convert.ToInt32(listCoinCount[i])));
                TextView textCoinNominal = new TextView(this)
                {
                    Text = listCoinNominal[i]
                };
                TextView text1 = new TextView(this)
                {
                    Text = "р.",
                };
                TextView textCoinCount = new TextView(this)
                {
                    Text = listCoinCount[i],
                    Id =i
                };
                TextView text2 = new TextView(this)
                {
                    Text = "шт."
                };
                EditText editTextNewCoinCount = new EditText(this)
                {
                    Text = "",
                    Id = i+1000
                };


                textCoinNominal.LayoutParameters = layoutParams;
                textCoinCount.LayoutParameters = layoutParams;
                editTextNewCoinCount.LayoutParameters = layoutParams;

                Spec rowSpec = GridLayout.InvokeSpec(i,1);
                Spec colSpec1 = GridLayout.InvokeSpec(0,1, GridLayout.Center);
                Spec colSpec2 = GridLayout.InvokeSpec(1,1,GridLayout.Center);
                Spec colSpec3 = GridLayout.InvokeSpec(2,1, GridLayout.Center);
                Spec colSpec4 = GridLayout.InvokeSpec(3,1, GridLayout.Center);
                Spec colSpec5 = GridLayout.InvokeSpec(4,1, GridLayout.Center);
                gridLayout.AddView(textCoinNominal, new GridLayout.LayoutParams(rowSpec, colSpec1));
                gridLayout.AddView(text1, new GridLayout.LayoutParams(rowSpec, colSpec2));
                gridLayout.AddView(textCoinCount, new GridLayout.LayoutParams(rowSpec, colSpec3));
                gridLayout.AddView(text2, new GridLayout.LayoutParams(rowSpec, colSpec4));
                gridLayout.AddView(editTextNewCoinCount, new GridLayout.LayoutParams(rowSpec, colSpec5));

            }

            saveButton.Click += (sender, e) =>
            {
                for (int i = 0; i < listCoinNominal.Count; i++)
                {
                    EditText editSettingCoinCount = FindViewById<EditText>(i+1000);
                    TextView textNewCoinCount = FindViewById<TextView>(i);

                    if (editSettingCoinCount.Text != "")
                        textNewCoinCount.Text = editSettingCoinCount.Text;
                    else
                        textNewCoinCount.Text = textNewCoinCount.Text;

                    listCoinCount[i] = textNewCoinCount.Text;
                    editSettingCoinCount.Text = "";
                }
            };

            backButton.Click += (sender, e) => {
                var intent = new Intent();
                intent.PutStringArrayListExtra("CoinNominal", listCoinNominal);
                intent.PutStringArrayListExtra("CoinCount", listCoinCount);
                SetResult(Result.Ok,intent);
                Finish();             
            };

        }
    }
}