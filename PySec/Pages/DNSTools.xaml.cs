using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PySec.Pages
{
    public partial class DNSTools : Page
    {
        public DNSTools()
        {
            InitializeComponent();
        }

        // Reverse DNS Fetch Method
        private async void FetchReverseDNSData(object sender, RoutedEventArgs e)
        {
            string dns = UrlInputReverseDNS.Text.Trim();

            if (string.IsNullOrEmpty(dns))
            {
                MessageBox.Show("Please enter a valid IP address.");
                return;
            }

            await ReverseDNS(dns);
        }

        private async Task ReverseDNS(string dns)
        {
            string url = $"https://api.hackertarget.com/reversedns/?q={dns}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string response = await client.GetStringAsync(url);
                    ResultTextBoxReverseDNS.Text = response;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching Reverse DNS data: {ex.Message}");
            }
        }

        // DNS Lookup Fetch Method
        private async void FetchDNSLookupData(object sender, RoutedEventArgs e)
        {
            string domain = UrlInputDNSLookup.Text.Trim();

            if (string.IsNullOrEmpty(domain))
            {
                MessageBox.Show("Please enter a valid domain name.");
                return;
            }

            await DNSLookup(domain);
        }

        private async Task DNSLookup(string domain)
        {
            string url = $"https://api.hackertarget.com/dnslookup/?q={domain}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string response = await client.GetStringAsync(url);
                    ResultTextBoxDNSLookup.Text = response;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching DNS Lookup data: {ex.Message}");
            }
        }

        // DNS Host Records Fetch Method
        private async void FetchDNSHostRecordsData(object sender, RoutedEventArgs e)
        {
            string subdomain = UrlInputDNSHostRecords.Text.Trim();

            if (string.IsNullOrEmpty(subdomain))
            {
                MessageBox.Show("Please enter a valid domain name.");
                return;
            }

            await DNSHostRecords(subdomain);
        }

        private async Task DNSHostRecords(string subdomain)
        {
            string url = $"https://api.hackertarget.com/hostsearch/?q={subdomain}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string response = await client.GetStringAsync(url);
                    ResultTextBoxDNSHostRecords.Text = response;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching DNS Host Records data: {ex.Message}");
            }
        }

        // DNS Security Extensions Check Method
        private async void FetchDNSSecurityExtensionsCheckData(object sender, RoutedEventArgs e)
        {
            string domain = UrlInputDNSSecurityExtensionsCheck.Text.Trim();

            if (string.IsNullOrEmpty(domain))
            {
                MessageBox.Show("Please enter a valid domain name.");
                return;
            }

            await DNSSecurityExtensions(domain);
        }

        private async Task DNSSecurityExtensions(string domain)
        {
            string url = "https://siterelic.com/siterelic-api/dnssec";
            var data = new { url = $"https://{domain}" };
            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsync(url, content);
                    string responseString = await response.Content.ReadAsStringAsync();
                    ResultTextBoxDNSSecurityExtensionsCheck.Text = responseString;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching DNS Security Extensions data: {ex.Message}");
            }
        }

        // DNS Record Fetch Method
        private async void FetchDNSRecordData(object sender, RoutedEventArgs e)
        {
            string domain = UrlInputDNSRecord.Text.Trim();

            if (string.IsNullOrEmpty(domain))
            {
                MessageBox.Show("Please enter a valid domain name.");
                return;
            }

            await DNSRecord(domain);
        }

        private async Task DNSRecord(string domain)
        {
            string url = "https://siterelic.com/siterelic-api/dnsrecord";
            var data = new { url = $"https://{domain}" };
            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsync(url, content);
                    string responseString = await response.Content.ReadAsStringAsync();
                    ResultTextBoxDNSRecord.Text = responseString;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching DNS Record data: {ex.Message}");
            }
        }
    }
}
