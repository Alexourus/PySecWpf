using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PySec.Pages
{
    public partial class WebVulnTools : Page
    {
        public WebVulnTools()
        {
            InitializeComponent();
        }

        // Fetch DMARC Data Method
        private async void FetchDMARCLookupData(object sender, RoutedEventArgs e)
        {
            string domain = UrlInputDMARCLookup.Text.Trim();

            if (string.IsNullOrEmpty(domain))
            {
                ResultTextBoxDMARCLookup.Text = "Please enter a valid domain name.";
                return;
            }

            await DMARCLookup(domain);
        }

        private async Task DMARCLookup(string domain)
        {
            string url = "https://tools.iplocation.net/dmarc-lookup";
            string data = $"url={domain}&submit=";
            var content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsync(url, content);
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (responseString.Contains("v=DMARC1"))
                    {
                        ResultTextBoxDMARCLookup.Text = "[+] DMARC record found.";
                    }
                    else if (responseString.Contains("No record found"))
                    {
                        ResultTextBoxDMARCLookup.Text = "[-] No DMARC record found!";
                    }
                    else
                    {
                        ResultTextBoxDMARCLookup.Text = "[!] Unexpected response.";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultTextBoxDMARCLookup.Text = $"Error checking DMARC record: {ex.Message}";
            }
        }

        // Fetch ASN Data Method
        private async void FetchASNLookupData(object sender, RoutedEventArgs e)
        {
            string asnlookup = UrlInputASNLookup.Text.Trim();

            if (string.IsNullOrEmpty(asnlookup))
            {
                ResultTextBoxASNLookup.Text = "Please enter a valid IP Address or ASN.";
                return;
            }

            await ASNLookup(asnlookup);
        }

        private async Task ASNLookup(string asnlookup)
        {
            string url = $"https://api.hackertarget.com/aslookup/?q={asnlookup}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string response = await client.GetStringAsync(url);
                    ResultTextBoxASNLookup.Text = response;
                }
            }
            catch (Exception ex)
            {
                ResultTextBoxASNLookup.Text = $"Error fetching ASN Lookup data: {ex.Message}";
            }
        }

        // Fetch Zone Transfer Data Method
        private async void FetchZoneTransferData(object sender, RoutedEventArgs e)
        {
            string zonetransfer = UrlInputZoneTransfer.Text.Trim();

            if (string.IsNullOrEmpty(zonetransfer))
            {
                ResultTextBoxZoneTransfer.Text = "Please enter a valid domain name.";
                return;
            }

            await ZoneTransfer(zonetransfer);
        }

        private async Task ZoneTransfer(string zonetransfer)
        {
            string url = $"https://api.hackertarget.com/zonetransfer/?q={zonetransfer}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string response = await client.GetStringAsync(url);
                    ResultTextBoxZoneTransfer.Text = response;
                }
            }
            catch (Exception ex)
            {
                ResultTextBoxZoneTransfer.Text = $"Error fetching Zone Transfer data: {ex.Message}";
            }
        }

        // Fetch CloudFlare Data Method
        /*
        private async void FetchCloudFlareResolverData(object sender, RoutedEventArgs e)
        {
            string domain = UrlInputCloudFlareResolver.Text.Trim(); 

            if (string.IsNullOrEmpty(domain))
            {
                ResultTextBoxCloudFlareResolver.Text = "Please enter a valid domain name.";
                return;
            }

            await CloudFlareResolver(domain);
        }

        private async Task CloudFlareResolver(string domain)
        {
            string url = "https://siterelic.com/siterelic-api/cloudflare";
            string data = $"{{\"url\": \"https://{domain}\"}}";
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsync(url, content);
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(responseString))
                    {
                        ResultTextBoxCloudFlareResolver.Text = $"CloudFlare Info: {responseString}";
                    }
                    else
                    {
                        ResultTextBoxCloudFlareResolver.Text = "No data received from CloudFlare API.";
                    }
                }

            }
            catch (Exception ex)
            {
                ResultTextBoxCloudFlareResolver.Text = $"Error fetching CloudFlare data: {ex.Message}";
            }
        } */

        // Fetch JS Vulnerability Data Method
        private async void FetchJSVulnerabilityCheckData(object sender, RoutedEventArgs e)
        {
            string domain = UrlInputJSVulnerabilityCheck.Text.Trim();

            if (string.IsNullOrEmpty(domain))
            {
                ResultTextBoxJSVulnerabilityCheck.Text = "Please enter a valid domain address.";
                return;
            }

            await JSVulnerabilityScanner(domain);
        }

        private async Task JSVulnerabilityScanner(string domain)
        {
            string tokenUrl = "https://domsignal.com/tools/api/api-accessToken/";
            string tokenData = "{\"serviceType\": \"js-vulnerability-scanner\"}";
            var tokenContent = new StringContent(tokenData, Encoding.UTF8, "application/json");

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var tokenResponse = await client.PostAsync(tokenUrl, tokenContent);
                    string tokenResponseString = await tokenResponse.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(tokenResponseString))
                    {
                        ResultTextBoxJSVulnerabilityCheck.Text = "Error retrieving access token.";
                        return;
                    }

                    string token = JsonConvert.DeserializeObject<dynamic>(tokenResponseString)?["credentials"]["accessToken"];
                    if (string.IsNullOrEmpty(token))
                    {
                        ResultTextBoxJSVulnerabilityCheck.Text = "Access token not found in response.";
                        return;
                    }

                    string scannerUrl = "https://domsignal.com/tools/api/gf/lighthouse/";
                    string scannerData = $"{{\"url\": \"{domain}\", \"type\": \"js-vulnerability-scanner\"}}";
                    var scannerContent = new StringContent(scannerData, Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Add("public-authorization", token);
                    client.DefaultRequestHeaders.Add("Origin", "https://domsignal.com");
                    client.DefaultRequestHeaders.Add("Referer", "https://domsignal.com/ipv6-test");

                    var scannerResponse = await client.PostAsync(scannerUrl, scannerContent);
                    string scannerResponseString = await scannerResponse.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(scannerResponseString))
                    {
                        ResultTextBoxJSVulnerabilityCheck.Text = "Error: No response received for vulnerability scan.";
                        return;
                    }

                    string lighthouseUrl = JsonConvert.DeserializeObject<dynamic>(scannerResponseString)?["data"];
                    if (string.IsNullOrEmpty(lighthouseUrl))
                    {
                        ResultTextBoxJSVulnerabilityCheck.Text = "Error: Lighthouse URL not found in response.";
                        return;
                    }

                    string vulnResponseString = await client.GetStringAsync(lighthouseUrl);
                    var vulnData = JsonConvert.DeserializeObject<dynamic>(vulnResponseString);
                    var vulnerabilities = vulnData?["audits"]["no-vulnerable-libraries"]["details"]["items"];

                    if (vulnerabilities != null && vulnerabilities.Count > 0)
                    {
                        ResultTextBoxJSVulnerabilityCheck.Text = JsonConvert.SerializeObject(vulnerabilities, Formatting.Indented);
                    }
                    else
                    {
                        ResultTextBoxJSVulnerabilityCheck.Text = $"No vulnerable libraries found for the website: {domain}";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultTextBoxJSVulnerabilityCheck.Text = $"Error processing vulnerability scan: {ex.Message}";
            }
        }

    }
}
