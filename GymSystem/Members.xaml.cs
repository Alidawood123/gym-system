using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GymSystem
{
    public partial class Members : ContentPage
    {
        private List<Member> allMembers;

        public Members()
        {
            InitializeComponent();
            LoadMembers();
        }

        private void LoadMembers()
        {
            // Read all members from the CSV file
            string[] lines = File.ReadAllLines("C:\\Users\\HP\\source\\repos\\GymSystem\\GymSystem\\Resources\\Files\\members.csv");
            allMembers = new List<Member>();

            MembersList.Children.Clear();
            foreach (string line in lines.Skip(1)) // Skip header
            {
                string[] parts = line.Split(',');
                Member member = new Member
                {
                    FirstName = parts[0],
                    LastName = parts[1],
                    Email = parts[2],
                    Mobile = parts[3],
                    Gender = parts[4],
                    JoiningDate = DateTime.Parse(parts[5]),
                    PlanDuration = parts[6]
                };
                allMembers.Add(member);

                // Create a Label for each member and add it to the MembersList StackLayout
                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                Label nameLabel = new Label
                {
                    Text = $"{member.FirstName} , {member.LastName} , {member.Email} , {member.Mobile} , {member.Gender} , {member.JoiningDate} , {member.PlanDuration}",
                    
                    FontFamily = "UbuntuRegular",
                    WidthRequest = 700
                };
                Grid.SetColumn(nameLabel, 0);
                string red = "FF0000";
                string lightred = "F7D4C8";
               

                Button deleteButton = new Button
                {
                    Text = "Delete",
                    CommandParameter = member,
                    FontFamily = "UbuntuRegular",
                    WidthRequest = 100,
                    BackgroundColor = Color.FromHex(lightred),
                    TextColor = Color.FromHex(red)
                };
                deleteButton.Clicked += DeleteButton_Clicked;
                Grid.SetColumn(deleteButton, 2);

                grid.Children.Add(nameLabel);
                grid.Children.Add(deleteButton);

                MembersList.Children.Add(grid);

            }
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Member member = (Member)button.CommandParameter;
            DeleteMember(member);
        }


        private void Home_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Home());
        }
        private void ViewMembers_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Members());
        }
        private void ViewEquipment_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Equipments());
        }

        private void Logout_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }
        private void ViewInstructors_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Instructors());
        }
        private void Search_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SearchEntry.Text))
            {
                DisplayAlert("Error", "Please enter a search term.", "OK");
                return;
            }

            string searchTerm = SearchEntry.Text.ToLower();

            List<Member> searchResults = allMembers.Where(m =>
                m.FirstName.ToLower().Contains(searchTerm) ||
                m.Email.ToLower().Contains(searchTerm) ||
                m.Mobile.ToLower().Contains(searchTerm)
            ).ToList();

            DisplaySearchResults(searchResults);
        }

        private void DisplaySearchResults(List<Member> searchResults)
        {
            SearchMembersList.Children.Clear(); // Clear previous search results

            foreach (Member member in searchResults)
            {
                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                Label nameLabel = new Label
                {
                    Text = $"{member.FirstName} , {member.LastName} , {member.Email} , {member.Mobile} , {member.Gender} , {member.JoiningDate} , {member.PlanDuration} ",
                  
                    FontFamily = "UbuntuRegular",
                    WidthRequest = 700
                };
                Grid.SetColumn(nameLabel, 0);
                string red = "#FF0000";
                string lightred = "#F7D4C8";
                Button deleteButton = new Button
                {
                    
                    Text = "Delete",
                    CommandParameter = member,
                    WidthRequest = 100,
                    FontFamily = "UbuntuRegular",
                    BackgroundColor = Color.FromHex(lightred),
                    TextColor = Color.FromHex(red)
                };
                deleteButton.Clicked += DeleteButton_Clicked;
                Grid.SetColumn(deleteButton, 2);

                grid.Children.Add(nameLabel);
                grid.Children.Add(deleteButton);

                SearchMembersList.Children.Add(grid);
            }
        }

        private void DeleteMember(Member member)
        {
            // Remove the member from the list
            allMembers.Remove(member);

            // Update the CSV file with the new list of members
            UpdateMembersFile();
            MembersList.Children.Clear();
            // Refresh the UI (e.g., update a ListView)
            LoadMembers();
        }

        private void UpdateMembersFile()
        {
            // Update the CSV file with the current list of members
            using (StreamWriter writer = new StreamWriter("C:\\Users\\HP\\source\\repos\\GymSystem\\GymSystem\\Resources\\Files\\members.csv"))
            {
                writer.WriteLine("First Name,Last Name,Email,Mobile,Gender,Joining Date,Plan Duration");
                foreach (Member member in allMembers)
                {
                    writer.WriteLine($"{member.FirstName},{member.LastName},{member.Email},{member.Mobile},{member.Gender},{member.JoiningDate},{member.PlanDuration}");
                }
                LoadMembers();
            }
        }

        private void Register_Clicked(object sender, EventArgs e)
        {
            string firstName = FirstNameEntry.Text;
            string lastName = LastNameEntry.Text;
            string email = EmailEntry.Text;
            string mobile = MobileEntry.Text;
            string gender = GenderPicker.SelectedItem?.ToString(); // Ensure a gender is selected
            string joiningDate = JoiningDatePicker.Date.ToString("yyyy-MM-dd");
            string planDuration = PlanDurationPicker.SelectedItem?.ToString(); // Ensure a plan duration is selected

            // Validate email format
            if (!IsValidEmail(email))
            {
                DisplayAlert("Error", "Invalid email format.", "OK");
                return;
            }

            // Validate mobile number
            if (!IsValidMobile(mobile))
            {
                DisplayAlert("Error", "Mobile number must be a valid integer.", "OK");
                return;
            }

            // Validate gender selection
            if (string.IsNullOrEmpty(gender))
            {
                DisplayAlert("Error", "Please select a gender.", "OK");
                return;
            }

            // Validate plan duration selection
            if (string.IsNullOrEmpty(planDuration))
            {
                DisplayAlert("Error", "Please select a plan duration.", "OK");
                return;
            }

            Member newMember = new Member
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Mobile = mobile,
                Gender = gender,
                JoiningDate = DateTime.Parse(joiningDate),
                PlanDuration = planDuration
            };

            allMembers.Add(newMember);
            UpdateMembersFile();

            // Clear input fields after registration
            FirstNameEntry.Text = "";
            LastNameEntry.Text = "";
            EmailEntry.Text = "";
            MobileEntry.Text = "";
            GenderPicker.SelectedItem = null;
            JoiningDatePicker.Date = DateTime.Now;
            PlanDurationPicker.SelectedItem = null;
        }
        private void PlanDurationPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PlanDurationPicker.SelectedIndex == 0)
            {
                PlanDurationPicker.SelectedIndex = -1; // Reset to no selection
            }
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidMobile(string mobile)
        {
            return int.TryParse(mobile, out _);
        }

        // Class representing a member
        private class Member
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Mobile { get; set; }
            public string Gender { get; set; }
            public DateTime JoiningDate { get; set; }
            public string PlanDuration { get; set; }
        }
    }
}
