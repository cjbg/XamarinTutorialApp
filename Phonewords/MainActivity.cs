﻿using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Phonewords
{
  [Activity(Label = "Phonewords", MainLauncher = true, Icon = "@drawable/icon")]
  public class MainActivity : Activity
  {
    int count = 1;

    protected override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);

      // Set our view from the "main" layout resource
      SetContentView(Resource.Layout.Main);

      EditText phoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
      Button translateButton = FindViewById<Button>(Resource.Id.TranslateButton);
      Button callButton = FindViewById<Button>(Resource.Id.CallButton);

      callButton.Enabled = false;

      string translatedNumber = string.Empty;

      translateButton.Click += (sender, e) =>
      {
        translatedNumber = PhonewordTranslator.ToNumber(phoneNumberText.Text);
        if (string.IsNullOrWhiteSpace(translatedNumber))
        {
          callButton.Text = "Call";
          callButton.Enabled = false;
        }
        else
        {
          callButton.Text = "Call " + translatedNumber;
          callButton.Enabled = true;
        }
      };

      callButton.Click += (sender, e) =>
      {
        // On "Call" button click, try to dial phone number.
        var callDialog = new AlertDialog.Builder(this);
        callDialog.SetMessage("Call " + translatedNumber + "?");
        callDialog.SetNeutralButton("Call", delegate {
          // Create intent to dial phone
          var callIntent = new Intent(Intent.ActionCall);
          callIntent.SetData(Android.Net.Uri.Parse("tel:" + translatedNumber));
          StartActivity(callIntent);
        });
        callDialog.SetNegativeButton("Cancel", delegate { });

        // Show the alert dialog to the user and wait for response.
        callDialog.Show();
      };
    }
  }
}

