using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PokemonProject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class settings : ContentPage
    {
        public settings()
        {
            InitializeComponent();
            soundSwitch.IsToggled = Preferences.Get("AutoSound", true);

            UnitsPicker.SelectedItem = Preferences.Get("UnitsToUse", "Legacy");
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e) {
            Preferences.Set("AutoSound", soundSwitch.IsToggled);
        }

        private void UnitsPicker_SelectedIndexChanged(object sender, EventArgs e) {
            string selectedUnits = UnitsPicker.SelectedItem.ToString();
            Preferences.Set("UnitsToUse", selectedUnits);

            if (selectedUnits == "Legacy") {
                Preferences.Set("UnitValue", 1.0);
            } else if (selectedUnits == "Metric") {
                Preferences.Set("UnitValue", 0.1);
            } else {
                Preferences.Set("UnitValue", 0.3);
            }

            
        }
    }
}