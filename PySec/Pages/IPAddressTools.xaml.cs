using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace PySec.Pages
{
    public partial class IPAddressTools : Page
    {
        public IPAddressTools()
        {
            InitializeComponent();
        }

        private async void FetchGeolocationIPData(object sender, RoutedEventArgs e)
        {
            string geoip = UrlInputGeolocationIP.Text;
            await GeolocationIP($"https://api.hackertarget.com/geoip/?q={geoip}");
        }

        private async Task GeolocationIP(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string response = await client.GetStringAsync(url);
                    ResultTextBoxGeolocationIP.Text = response;
                }
                catch (Exception ex)
                {
                    ResultTextBoxGeolocationIP.Text = $"Error: {ex.Message}";
                }
            }
        }

        private async void FetchReverseIPLookupData(object sender, RoutedEventArgs e)
        {
            string reverseip = UrlInputReverseIPLookup.Text;
            await ReverseIPLookup($"https://ipinfo.io/widget/demo/{reverseip}?dataset=proxy-vpn-detection");
        }

        private async Task ReverseIPLookup(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string response = await client.GetStringAsync(url);
                    ResultTextBoxReverseIPLookup.Text = response;
                }
                catch (Exception ex)
                {
                    ResultTextBoxReverseIPLookup.Text = $"Error: {ex.Message}";
                }
            }
        }

        private async void FetchPv6CheckData(object sender, RoutedEventArgs e)
        {
            string domain = UrlInputPvCheck.Text.Trim();

            if (string.IsNullOrEmpty(domain))
            {
                MessageBox.Show("Please enter a valid domain name.");
                return;
            }

            await IPv6Check(domain);
        }

        private async Task IPv6Check(string domain)
        {
            string accessTokenUrl = "https://domsignal.com/tools/api/api-accessToken/";
            string testUrl = "https://domsignal.com/tools/api/gf/dnsrecord/";

            try
            {
                var tokenData = new { serviceType = "ipv6-test" };
                string jsonTokenData = JsonConvert.SerializeObject(tokenData);
                var tokenContent = new StringContent(jsonTokenData, Encoding.UTF8, "application/json");

                using (HttpClient client = new HttpClient())
                {
                    var tokenResponse = await client.PostAsync(accessTokenUrl, tokenContent);
                    if (!tokenResponse.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Error retrieving access token.");
                        return;
                    }

                    string tokenResponseString = await tokenResponse.Content.ReadAsStringAsync();
                    dynamic tokenJson = JsonConvert.DeserializeObject(tokenResponseString);
                    string accessToken = tokenJson.credentials.accessToken;

                    var ipv6Data = new { url = domain, type = "ipv6-test" };
                    string jsonIPv6Data = JsonConvert.SerializeObject(ipv6Data);
                    var ipv6Content = new StringContent(jsonIPv6Data, Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Add("public-authorization", accessToken);
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:127.0) Gecko/20100101 Firefox/127.0");

                    var ipv6Response = await client.PostAsync(testUrl, ipv6Content);
                    if (!ipv6Response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Error performing IPv6 test.");
                        return;
                    }

                    string ipv6ResponseString = await ipv6Response.Content.ReadAsStringAsync();
                    dynamic ipv6Json = JsonConvert.DeserializeObject(ipv6ResponseString);

                    if (ipv6Json.data.ToString().Contains("AAAA"))
                    {
                        ResultTextBoxIPvCheck.Text = $"This website supports IPv6 proxy connections --> {domain}";
                    }
                    else
                    {
                        ResultTextBoxIPvCheck.Text = $"No IPv6 proxy support for this website --> {domain}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error performing IPv6 test: {ex.Message}");
            }
        }
    }
}
