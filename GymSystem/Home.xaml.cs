using Microsoft.Maui.Controls;

namespace GymSystem
{
    public partial class Home : ContentPage
    {
        public Home()
        {
            InitializeComponent();
        }
        private void Home_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Home());
        }
        private void ViewMembers_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Members());
        }
        private void ViewInstructors_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Instructors());
        }

        private void ViewEquipment_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Equipments());
        }

        private void Logout_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }
    }
}
