﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace EspionSpotify
{
    public class Analytics
    {
        private const string url = "https://www.google-analytics.com/collect";
        private const string tid = "UA-125662919-1";

        private readonly HttpClient client = new HttpClient();
        private readonly string cid;
        private readonly string cm;
        private readonly string ul;
        private readonly string cs;

        public DateTime LastRequest { get; set; } = new DateTime();
        public string LastAction { get; set; } = string.Empty;

        public Analytics(string clientId, string version)
        {
            cid = clientId;
            cm = version;
            ul = CultureInfo.CurrentCulture.Name;
            cs = Environment.OSVersion.ToString();
        }

        public static string GenerateCID()
        {
            return Guid.NewGuid().ToString();
        }

        public async Task<bool> LogAction(string action)
        {
            if (LastAction.Equals(action) && DateTime.Now - LastRequest < TimeSpan.FromMinutes(5)) return false;

            var data = new Dictionary<string, string>
            {
                { "v", "1" },
                { "tid", tid }, // App id
                { "t", "pageview" }, // Analytics type
                { "cid", cid }, // Client id
                { "cm", cm }, // Campaign medium, App version
                { "av", cm }, // App version
                { "cn", "Spytify" }, // Campaign name
                { "an", "Spytify" }, // App name
                { "cs", cs}, // Campaign source, OS Version
                { "ul", ul }, // User Language
                { "dh", "jwallet.github.io/spy-spotify" }, // Document host
                { "dl", $"/{action}" }, // Document link
                { "dt", action }, // Document title
                { "cd", action } // Screen name
            };

            var content = new FormUrlEncodedContent(data);
            var resp = await client.PostAsync(url, content);

            LastAction = action;
            LastRequest = DateTime.Now;

            return resp.IsSuccessStatusCode;
        }
    }
}
