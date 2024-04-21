using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GymSystem
{
    public partial class Equipments : ContentPage
    {
        private List<Equipment> allEquipments;

        public Equipments()
        {
            InitializeComponent();
            LoadEquipments();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
           // LoadEquipments();
        }
        private void LoadEquipments()
        {
            // Read all equipments from the CSV file
            string[] lines = File.ReadAllLines("C:\\Users\\HP\\source\\repos\\GymSystem\\GymSystem\\Resources\\Files\\equipments.csv");
            allEquipments = new List<Equipment>();

            foreach (string line in lines.Skip(1)) // Skip header
            {
                string[] parts = line.Split(',');
                Equipment equipment = new Equipment
                {
                    Name = parts[0],
                    Description = parts[1],
                    Quantity = int.Parse(parts[2])
                };
                allEquipments.Add(equipment);

                // Create a Label for each equipment and add it to the EquipmentsList StackLayout
                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                Label nameLabel = new Label
                {
                    Text = $"{equipment.Name}, {equipment.Description}, {equipment.Quantity}",
                   
                    FontFamily = "UbuntuRegular",
                    WidthRequest = 700
                };
                Grid.SetColumn(nameLabel, 0);

                string red = "#FF0000";
                string lightred = "#F7D4C8";

                Button deleteButton = new Button
                {
                    Text = "Delete",
                    CommandParameter = equipment,
                    FontFamily = "UbuntuRegular",
                    WidthRequest = 100,
                    BackgroundColor = Color.FromHex(lightred),
                    TextColor = Color.FromHex(red)
                };
                deleteButton.Clicked += DeleteButton_Clicked;
                Grid.SetColumn(deleteButton, 2);

                grid.Children.Add(nameLabel);
                grid.Children.Add(deleteButton);

                EquipmentsList.Children.Add(grid);
            }
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Equipment equipment = (Equipment)button.CommandParameter;
            DeleteEquipment(equipment);
        }

        private void Home_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Home());
        }
        private void ViewEquipment_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Equipments());
        }
        private void ViewInstructors_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Instructors());
        }
        private void ViewMembers_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Members());
        }

        private void Logout_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }

        private void Search_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SearchEntry.Text))
            {
                DisplayAlert("Error", "Please enter a search term.", "OK");
                return;
            }

            string searchTerm = SearchEntry.Text.ToLower();

            List<Equipment> searchResults = allEquipments.Where(m =>
                m.Name.ToLower().Contains(searchTerm)
            ).ToList();

            DisplaySearchResults(searchResults);
        }

        private void DisplaySearchResults(List<Equipment> searchResults)
        {
            SearchEquipmentsList.Children.Clear(); // Clear previous search results

            foreach (Equipment equipment in searchResults)
            {
                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                Label nameLabel = new Label
                {
                    Text = $"{equipment.Name}, {equipment.Description}, {equipment.Quantity}",
                    
                    FontFamily = "UbuntuRegular",
                    WidthRequest = 700
                };
                Grid.SetColumn(nameLabel, 0);

                string red = "#FF0000";
                string lightred = "#F7D4C8";

                Button deleteButton = new Button
                {
                    Text = "Delete",
                    CommandParameter = equipment,
                    WidthRequest = 100,
                    FontFamily = "UbuntuRegular",
                    BackgroundColor = Color.FromHex(lightred),
                    TextColor = Color.FromHex(red)
                };
                deleteButton.Clicked += DeleteButton_Clicked;
                Grid.SetColumn(deleteButton, 2);

                grid.Children.Add(nameLabel);
                grid.Children.Add(deleteButton);

                SearchEquipmentsList.Children.Add(grid);
            }
        }

        private void DeleteEquipment(Equipment equipment)
        {
            // Remove the equipment from the list
            allEquipments.Remove(equipment);

            // Update the CSV file with the new list of equipments
            UpdateEquipmentsFile();

            // Refresh the UI (e.g., update a ListView)
            LoadEquipments();
        }

        private void UpdateEquipmentsFile()
        {
            // Update the CSV file with the current list of equipments
            using (StreamWriter writer = new StreamWriter("C:\\Users\\HP\\source\\repos\\GymSystem\\GymSystem\\Resources\\Files\\equipments.csv"))
            {
                writer.WriteLine("Name,Description,Quantity");
                foreach (Equipment equipment in allEquipments)
                {
                    writer.WriteLine($"{equipment.Name},{equipment.Description},{equipment.Quantity}");
                }
                LoadEquipments();
            }
        }

        private void AddEquipment_Clicked(object sender, EventArgs e)
        {
            string name = NameEntry.Text;
            string description = DescriptionEntry.Text;
            string quantityText = QuantityEntry.Text;

            // Validate quantity
            if (!int.TryParse(quantityText, out int quantity))
            {
                DisplayAlert("Error", "Quantity must be a valid integer.", "OK");
                return;
            }

            Equipment newEquipment = new Equipment
            {
                Name = name,
                Description = description,
                Quantity = quantity
            };

            allEquipments.Add(newEquipment);
            UpdateEquipmentsFile();

            // Clear input fields after adding equipment
            NameEntry.Text = "";
            DescriptionEntry.Text = "";
            QuantityEntry.Text = "";
        }

        // Class representing an equipment
        private class Equipment
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public int Quantity { get; set; }
        }
    }
}
      