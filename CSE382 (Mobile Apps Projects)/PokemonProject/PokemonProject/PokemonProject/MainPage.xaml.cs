using Pokemon;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PokemonProject
{
    public partial class MainPage : ContentPage {
        RestService _restService;
        PokemonPage pokemonpage;
        PokemonTeam pokemonTeamPage;
        settings settingsPage;
        public static string pokemonName = "";
        public static string pokemonImageUrl = "";
        ObservableCollection<firstPK> observablePokemon;
        public MainPage() {
            InitializeComponent();
            _restService = new RestService();
            DB.OpenConnection();
            loadPokemon();
        }

        public void loadPokemon() {
            observablePokemon = new ObservableCollection<firstPK>();
            // Get all of the basic pokemon info from the data base
            var Allpokemon = DB.conn.Table<firstPK>().ToList();

            // replace the url with just the image with id number 
            // the pokemon are in order so their id will be 1,2,3,etc
            int i = 1;
            foreach (var item in Allpokemon) {
                item.PokemonID = i;
                item.PokemonUrl = "sprites/" + (i).ToString() + ".png";
                observablePokemon.Add(item);
                i++;
            }
            // Configure the list view to be image then name of the pokemon
            ConfigureListView(ref PokemonList, observablePokemon);
        }

        public void ConfigureListView(ref ListView lv, ObservableCollection<firstPK> data) {
            lv.ItemsSource = data;

            // Define template for displaying each item.
            // (Argument of DataTemplate constructor is called for 
            //      each item; it must return a Cell derivative.)
            lv.ItemTemplate = new DataTemplate(() =>
            {
                // Create views with bindings for displaying each property.
                Label nameLabel = new Label
                {
                    FontSize = 18,
                    TextColor = Color.White,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };

                Label numLabel = new Label
                {
                    FontSize = 18,
                    TextColor = Color.White,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start
                };


                var image = new Image { HeightRequest = 120, WidthRequest = 80 };
                nameLabel.SetBinding(Label.TextProperty, "PokemonName");
                numLabel.SetBinding(Label.TextProperty, "PokemonID");
                image.SetBinding(Image.SourceProperty, "PokemonUrl");

                // Return an assembled ViewCell.
                return new ViewCell
                {
                    View = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        HeightRequest = 40,
                        Padding = new Thickness(10, 10, 10, 0),
                        Children = {
                                image,
                                nameLabel,
                                numLabel,
                        }
                    }
                };
            });
        }

        // A once an item is selected a new page will open showing more detailed information about the pokemon
        private async void PokemonList_ItemSelected(object sender, SelectedItemChangedEventArgs e) {
            // Check to make sure that a selected item is not null (happens when searching the pokemon)
            if (PokemonList.SelectedItem != null) {
                string itemString = PokemonList.SelectedItem.ToString();
                string id = PokemonList.SelectedItem.ToString();
                // Here the itemString will be formated like this: "bulbasaur (sprites/1.png)"
                // a.Substring(a.IndexOf('/') + 1, a.IndexOf(".") - a.IndexOf('/') -1);
                // ^ This is the substring to get the id number out of the image path

                pokemonName = itemString.Substring(0, itemString.IndexOf(" "));
                pokemonImageUrl = id.Substring(id.IndexOf('/') + 1, id.IndexOf(".") - id.IndexOf('/') - 1);

                PokemonList.SelectedItem = null;
                pokemonpage = new PokemonPage();
                await Navigation.PushAsync(pokemonpage, true);
            }
        }

        // Once text has been put in this entry it is compaired against the pokemon
        private void PokeEntry_TextChanged(object sender, TextChangedEventArgs e) {
            var searchText = ((Entry)sender).Text;

            ObservableCollection<firstPK> searchedPokemon = new ObservableCollection<firstPK>();

            foreach (var item in observablePokemon) {
                // If the search text matches the pokemon id or starts with the given 
                // string then add it to the new observable collection
                if (item.PokemonID.ToString().Contains(searchText) || item.PokemonName.StartsWith(searchText)) {
                    searchedPokemon.Add(item);
                }
            }
            ConfigureListView(ref PokemonList, searchedPokemon);
        }

        // The team button has been clicked directly go to team page
        private async void TeamButton_Clicked(object sender, EventArgs e) {
            Preferences.Set("PokemonName", "FromMainPage");
            pokemonTeamPage = new PokemonTeam();
            await Navigation.PushAsync(pokemonTeamPage, true);
        }

        private async void SettingsButton_Clicked(object sender, EventArgs e) {
            settingsPage = new settings();
            await Navigation.PushAsync(settingsPage, true);
        }
    }
}
