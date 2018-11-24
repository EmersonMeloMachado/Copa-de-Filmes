using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CopaDeFilmes.App.Droid
{
    public class VerificarPermissao
    {
        public static string[] Permissions =
        {
            Manifest.Permission.Camera,
            Manifest.Permission.Internet,
            Manifest.Permission.ReadSms,
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.ReadPhoneState,
            Manifest.Permission.AccessWifiState,
            Manifest.Permission.AccessFineLocation
        };

        const int RequestLocationId = 0;

        public static void GetPermissions(Activity activity)
        {
            var listPermissionsNeeded = new List<string>();
            int[] perm = { };

            foreach(var item in Permissions)
            {
                var result = activity.CheckSelfPermission(item);
                if(result != Permission.Granted)
                {
                    listPermissionsNeeded.Add(item);
                }
            }

            if(listPermissionsNeeded.Any())
                activity.RequestPermissions(Permissions, RequestLocationId);
        }
    }
}