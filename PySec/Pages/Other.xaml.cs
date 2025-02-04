using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PySec.Pages
{
    public partial class Other : Page
    {
        public Other()
        {
            InitializeComponent();
        }

        // Fetch Email Validation Data Method
        private async void FetchEmailValidatorData(object sender, RoutedEventArgs e)
        {
            string email = UrlInputEmailValidator.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                ResultTextBoxEmailValidator.Text = "Please enter a valid email address.";
                return;
            }

            await EmailValidation(email);
        }

        private async Task EmailValidation(string email)
        {
            string url = "https://tools.iplocation.net/verify-email-address";
            string data = $"address=&email={email}&submit=Verify+Email+Address";
            var content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsync(url, content);
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (responseString.Contains("is a valid email"))
                    {
                        ResultTextBoxEmailValidator.Text = "[+] This email is valid and active.";
                    }
                    else if (responseString.Contains("is an invalid email"))
                    {
                        ResultTextBoxEmailValidator.Text = "[-] This email is not working.";
                    }
                    else
                    {
                        ResultTextBoxEmailValidator.Text = "[!] Unexpected response.";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultTextBoxEmailValidator.Text = $"Error validating email: {ex.Message}";
            }
        }

        // Fetch Have I been Pwned Data Method
        private async void FetchHaveIbeenPwnedData(object sender, RoutedEventArgs e)
        {
            string email = UrlInputHaveIbeenPwned.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                ResultTextBoxHaveIbeenPwned.Text = "Please enter a valid email address.";
                return;
            }

            await HaveIbeenPwned(email);
        }

        private async Task HaveIbeenPwned(string email)
        {
            string proxy = email.Replace("@", "%40");
            string url = "https://tools.iplocation.net/data-breach-check";
            string data = $"email={proxy}&submit=Breached%3F";
            var content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsync(url, content);
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (responseString.Contains("Congratulations"))
                    {
                        ResultTextBoxHaveIbeenPwned.Text = "[+] Private email address";
                    }
                    else if (responseString.Contains("We found"))
                    {
                        ResultTextBoxHaveIbeenPwned.Text = "[-] Email is public!";
                    }
                    else
                    {
                        ResultTextBoxHaveIbeenPwned.Text = "[!] Unexpected response.";
                    }
                }
            }
            catch (Exception ex)
            {
                ResultTextBoxHaveIbeenPwned.Text = $"Error checking proxy: {ex.Message}";
            }
        }

        // Fetch URL Shortener Bypasser Data Method
        private async void FetchURLShortenerBypasserData(object sender, RoutedEventArgs e)
        {
            string url = UrlInputURLShortenerBypasser.Text.Trim();

            if (string.IsNullOrEmpty(url))
            {
                ResultTextBoxURLShortenerBypasser.Text = "Please enter a valid URL.";
                return;
            }

            await ShortURL(url);
        }

        private async Task ShortURL(string url)
        {
            string resultMessage = "Wait for result...";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        resultMessage = response.RequestMessage.RequestUri.ToString();
                    }
                    else
                    {
                        resultMessage = $"{response.StatusCode} - {response.ReasonPhrase}";
                    }
                }
            }
            catch (Exception ex)
            {
                resultMessage = $"Error: {ex.Message}";
            }

            ResultTextBoxURLShortenerBypasser.Text = resultMessage;
        }
    }
}
