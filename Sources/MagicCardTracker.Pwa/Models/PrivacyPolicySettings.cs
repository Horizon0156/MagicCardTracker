using System;

namespace MagicCardTracker.Pwa.Models
{
    internal class PrivacyPolicySettings
    {
        public static string Key => "PrivacyPolicy";

        public string HosterName { get; set; }  

        public string HosterMailAdress { get; set; }
    }
}
