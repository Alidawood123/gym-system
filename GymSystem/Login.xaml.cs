using Microsoft.Maui.Controls;

namespace GymSystem
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Clicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            // Check credentials (pseudo code)
            if (IsValidCredentials(username, password))
            {
                // Navigate to Home.xaml
                Navigation.PushAsync(new Home());
            }
            else
            {
                DisplayAlert("Login Failed", "Invalid username or password", "OK");
            }
        }

        private bool IsValidCredentials(string username, string password)
        {
            // Read credentials from a CSV file
            string[] lines = File.ReadAllLines("C:\\Users\\HP\\source\\repos\\GymSystem\\GymSystem\\Resources\\Files\\access.csv");
            foreach (string line in lines.Skip(1)) // Skip header
            {
                string[] parts = line.Split(',');
                string storedUsername = parts[0];
                string storedPassword = parts[1];

                if (username == storedUsername && password == storedPassword)
                {
                    return true; // Credentials are valid
                }
            }

            return false; // Credentials are invalid
        }
    }
}
