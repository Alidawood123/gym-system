using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GymSystem
{
    public partial class Instructors : ContentPage
    {
        private List<Instructor> allInstructors;

        public Instructors()
        {
            InitializeComponent();
            LoadInstructors();
        }

        private void LoadInstructors()
        {
            // Read all instructors from the CSV file
            string[] lines = File.ReadAllLines("C:\\Users\\HP\\source\\repos\\GymSystem\\GymSystem\\Resources\\Files\\instructors.csv");
            allInstructors = new List<Instructor>();

            InstructorsList.Children.Clear();
            foreach (string line in lines.Skip(1)) // Skip header
            {
                string[] parts = line.Split(',');
                Instructor instructor = new Instructor
                {
                    FirstName = parts[0],
                    LastName = parts[1],
                    IDNumber = parts[2],
                    Gender = parts[3],
                    Mobile = parts[4]
                };
                allInstructors.Add(instructor);

                // Create a Label for each instructor and add it to the InstructorsList StackLayout
                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                Label nameLabel = new Label
                {
                    Text = $"{instructor.FirstName} , {instructor.LastName} , {instructor.IDNumber} , {instructor.Gender} , {instructor.Mobile}",
                    FontFamily = "UbuntuRegular",
                    WidthRequest = 700
                };
                Grid.SetColumn(nameLabel, 0);

                string red = "FF0000";
                string lightred = "F7D4C8";
                Button deleteButton = new Button
                {
                    Text = "Delete",
                    CommandParameter = instructor,
                    FontFamily = "UbuntuRegular",
                    WidthRequest = 100,
                    BackgroundColor = Color.FromHex(lightred),
                    TextColor = Color.FromHex(red)
                };
                deleteButton.Clicked += DeleteButton_Clicked;
                Grid.SetColumn(deleteButton, 2);

                grid.Children.Add(nameLabel);
                grid.Children.Add(deleteButton);

                InstructorsList.Children.Add(grid);
            }
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Instructor instructor = (Instructor)button.CommandParameter;
            DeleteInstructor(instructor);
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
        private void ViewInstructors_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Instructors());
        }
        private void Logout_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }
        private void DeleteInstructor(Instructor instructor)
        {
            // Remove the instructor from the list
            allInstructors.Remove(instructor);

            // Update the CSV file with the new list of instructors
            UpdateInstructorsFile();

            // Refresh the UI
            InstructorsList.Children.Clear();
            LoadInstructors();
        }
        private (bool isValid, string errorMessage) IsValidMobile(string mobile)
        {
            if (string.IsNullOrEmpty(mobile) || !long.TryParse(mobile, out _))
            {
                return (false, "Mobile number must be a valid integer.");
            }

            return (true, "");
        }

        private (bool isValid, string errorMessage) IsValidIDNumber(string idNumber)
        {
            if (string.IsNullOrEmpty(idNumber) || !long.TryParse(idNumber, out _) || idNumber.Length != 8)
            {
                return (false, "ID number must be a valid 8-digit number.");
            }

            return (true, "");
        }

        private void Register_Clicked(object sender, EventArgs e)
        {
            string firstName = FirstNameEntry.Text;
            string lastName = LastNameEntry.Text;
            string idNumber = IDNumberEntry.Text;
            string gender = GenderPicker.SelectedItem?.ToString(); // Ensure a gender is selected
            string mobile = MobileEntry.Text;

            // Validate mobile number
            var mobileValidation = IsValidMobile(mobile);
            if (!mobileValidation.isValid)
            {
                DisplayAlert("Error", mobileValidation.errorMessage, "OK");
                return;
            }

            // Validate ID number
            var idNumberValidation = IsValidIDNumber(IDNumberEntry.Text);
            if (!idNumberValidation.isValid)
            {
                DisplayAlert("Error", idNumberValidation.errorMessage, "OK");
                return;
            }

            // Validate gender selection
            if (string.IsNullOrEmpty(gender))
            {
                DisplayAlert("Error", "Please select a gender.", "OK");
                return;
            }

            Instructor newInstructor = new Instructor
            {
                FirstName = firstName,
                LastName = lastName,
                IDNumber = idNumber,
                Gender = gender,
                Mobile = mobile
            };

            allInstructors.Add(newInstructor);
            UpdateInstructorsFile();

            // Clear input fields after adding new instructor
            FirstNameEntry.Text = "";
            LastNameEntry.Text = "";
            IDNumberEntry.Text = "";
            GenderPicker.SelectedItem = null;
            MobileEntry.Text = "";
        }
        private void Search_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SearchEntry.Text))
            {
                DisplayAlert("Error", "Please enter a search term.", "OK");
                return;
            }

            string searchTerm = SearchEntry.Text.ToLower();

            List<Instructor> searchResults = allInstructors.Where(i =>
                i.FirstName.ToLower().Contains(searchTerm) ||
                i.LastName.ToLower().Contains(searchTerm) ||
                i.IDNumber.ToLower().Contains(searchTerm) ||
                i.Mobile.ToLower().Contains(searchTerm)
            ).ToList();

            DisplaySearchResults(searchResults);
        }

        private void DisplaySearchResults(List<Instructor> searchResults)
        {
            SearchInstructorsList.Children.Clear();

            foreach (var instructor in searchResults)
            {
                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                Label nameLabel = new Label
                {
                    Text = $"{instructor.FirstName} , {instructor.LastName} , {instructor.IDNumber} , {instructor.Gender} , {instructor.Mobile}",
                    FontFamily = "UbuntuRegular",
                    WidthRequest = 700
                };
                Grid.SetColumn(nameLabel, 0);

                string red = "FF0000";
                string lightred = "F7D4C8";
                Button deleteButton = new Button
                {
                    Text = "Delete",
                    CommandParameter = instructor,
                    FontFamily = "UbuntuRegular",
                    WidthRequest = 100,
                    BackgroundColor = Color.FromHex(lightred),
                    TextColor = Color.FromHex(red)
                };
                deleteButton.Clicked += DeleteButton_Clicked;
                Grid.SetColumn(deleteButton, 2);

                grid.Children.Add(nameLabel);
                grid.Children.Add(deleteButton);

                SearchInstructorsList.Children.Add(grid);
            }
        }

        private void UpdateInstructorsFile()
        {
            // Update the CSV file with the current list of instructors
            using (StreamWriter writer = new StreamWriter("C:\\Users\\HP\\source\\repos\\GymSystem\\GymSystem\\Resources\\Files\\instructors.csv"))
            {
                writer.WriteLine("First Name,Last Name,ID Number,Gender,Mobile");
                foreach (Instructor instructor in allInstructors)
                {
                    writer.WriteLine($"{instructor.FirstName},{instructor.LastName},{instructor.IDNumber},{instructor.Gender},{instructor.Mobile}");
                }
            }
            LoadInstructors();
        }

        // Class representing an instructor
        private class Instructor
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string IDNumber { get; set; }
            public string Gender { get; set; }
            public string Mobile { get; set; }
        }
    }
}
