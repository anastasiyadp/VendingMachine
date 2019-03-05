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
    [Activity(Label = "@string/setting_activity")]
    public class SettingVMActivity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_settings);
            Button button = FindViewById<Button>(Resource.Id.button1);
            ListView listView = FindViewById<ListView>(Resource.Id.listView1);
            LinearLayout linearLayout = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            Button backButton = FindViewById<Button>(Resource.Id.backButton);

            IList<String> test = Intent.GetStringArrayListExtra("mi");
            IList<String> test2 = Intent.GetStringArrayListExtra("mi2");

            List<Coins> test3 = new List<Coins>();
            for(int i=0; i<test.Count; i++)
            {
                test3.Add(new Coins(Convert.ToInt32(test[i]), Convert.ToInt32(test2[i])));
                TextView text = new TextView(this)
                {
                    Text = test[i]               
                };
               
                TextView text2 = new TextView(this)
                {
                    Text = test2[i],
                    Id =i
                };
                EditText editText = new EditText(this)
                {
                    Text = "",
                    Id = i+1000
                };
                text.LayoutParameters = layoutParams;
                text2.LayoutParameters = layoutParams;
                editText.LayoutParameters = layoutParams;
                linearLayout.AddView(text);
                linearLayout.AddView(text2);
                linearLayout.AddView(editText);
            }

            button.Click += (sender, e) =>
            {
                for (int i = 0; i < test.Count; i++)
                {
                    EditText edit = FindViewById<EditText>(i+1000);
                    TextView final = FindViewById<TextView>(i);
                    int x;
                    if (edit.Text != "") x = int.Parse(final.Text) + int.Parse(edit.Text);
                    else x = int.Parse(final.Text) + 0;
                    final.Text = x.ToString();
                    edit.Text = "";
                }
            };

            backButton.Click += (sender, e) => {
                var intent = new Intent(this, typeof(MainActivity));
                int put_position = 5;
                intent.PutExtra("Main_Put_Edit_Position2", put_position);
                SetResult(Result.Ok,intent);
                Finish();             
            };

        }
    }
}