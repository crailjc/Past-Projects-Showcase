using Pokemon;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Plugin.SimpleAudioPlayer;
using System.Numerics;
using System.IO;
using System.Reflection;
using static System.Net.WebRequestMethods;

namespace PokemonProject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PokemonPage : ContentPage
    {
        public PokemonData pokemon;
        PokemonTeam pokemonTeam;
        ObservableCollection<move> moves;
        RestService _restService = new RestService();
        ISimpleAudioPlayer player = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
        public PokemonPage() {
            InitializeComponent();
            nameLabel.Text = MainPage.pokemonName;
            // Set the main image for the clicked on pokemon
            bigImage.Source = "sprites/" + MainPage.pokemonImageUrl + ".png";
            getPokemonInfo();
        }

        // Method to get the detailed pokemon info from the API call
        public async void getPokemonInfo() {

            // Play the sound if automatic sound is on
            bool playAuto = Preferences.Get("AutoSound", true);


            // Get the pokemon info 
            pokemon = await _restService.GetPokemonData("https://pokeapi.co/api/v2/pokemon/" + MainPage.pokemonName);
            
            // Set the appropriate labels
            nameLabel.Text = "Name (id): " + pokemon.name + " (" + pokemon.id.ToString() + ")";

            string type = "";
            // Add all the types together
            foreach (var tp in pokemon.typeinfo) {
                type += tp.dtInfo.ToString() + " ";
            }

            double unitCoef = Preferences.Get("UnitValue", 1.0);
            double height = (double)pokemon.height, weight = (double) pokemon.weight;

            // This is for standard since conversions are more difficult to do
            if (unitCoef == 0.3) {
                // Convert from legacy to metric then to standard
                height = (height * .1) * 3.28;
                weight = (weight * .1) * 2.20;
            } else {
                height *= unitCoef;
                weight *= unitCoef;
            }



            pokemonType.Text = "Type: " + type;
            GenaricInfo.Text = "Height: " + height.ToString() + " Weight: " + weight.ToString();

            // Add the moves to an observable collection
            moves = new ObservableCollection<move>();
            foreach (moves mv in pokemon.moves) {
                moves.Add(mv.moveInfo);
            }

            // Confirgure the list view for the moves
            ConfigureListView(ref MoveList, moves);

            if (playAuto) {
                loadAndPlay();
            }
        }

        public void ConfigureListView(ref ListView lv, ObservableCollection<move> data) {
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
                // var image = new Image { HeightRequest = 120, WidthRequest = 80 };
                nameLabel.SetBinding(Label.TextProperty, "name");

                // Return an assembled ViewCell.
                return new ViewCell
                {
                    View = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        HeightRequest = 40,
                        Padding = new Thickness(10, 10, 10, 0),
                        Children = {
                                nameLabel,
                        }
                    }
                };
            });
        }

        // When a move is selected make the API call and get the move information
        private async void MoveList_ItemSelected(object sender, SelectedItemChangedEventArgs e) {
            move selectedMove = e.SelectedItem as move;

            // When searching the moves a selected move can be removed from view while still selected
            // so it will be null make sure that a null is not passed
            if (selectedMove != null) {
                // Call the API to get the move information
                moveAP detailedMoveInfo = await _restService.GetMoveData(selectedMove.url);

                // Format the message with the move information in it 
                string messageText = "Accuracy: " + detailedMoveInfo.strAcc + " \n" +
                    "Power: " + detailedMoveInfo.strPower + "\n" +
                    "PP: " + detailedMoveInfo.pp + "\n" +
                    "Priority: " + detailedMoveInfo.priority + "\n" +
                    "Target(s): " + detailedMoveInfo.target.name + "\n" +
                    "Type: " + detailedMoveInfo.typeOfMove.name;

                // Display the move information
                await DisplayAlert(detailedMoveInfo.name, messageText, "OK");
            }
        }

        private async void addToTeam_Clicked(object sender, EventArgs e) {
            // Ask the user if they want to add a nickname to thie pokemon
            bool answer = await DisplayAlert("Question?", "Would you like to add a nickname to your pokemon?", "Yes", "No");

            if (answer == true) {
                // If the user wants to set a nickname ask them a question to get the name they want
                string pkNamePrompt = await DisplayPromptAsync("Set nickname", "What would you like your Pokemon's name to be?");
                Preferences.Set("PokemonName", pkNamePrompt);
            } else {
                // If no then just use the name of the pokemon
                Preferences.Set("PokemonName", pokemon.name);
            }
            // Set the pokemon id so it can be used when setting the picture on the team page
            Preferences.Set("PokemonId", pokemon.id);

            // Create the new page
            pokemonTeam = new PokemonTeam();
            await Navigation.PushAsync(pokemonTeam, true);
        }

        // When ever the text changes in the search box then find the move that 
        // starts with that string of text
        private void moveSearch_TextChanged(object sender, TextChangedEventArgs e) {
            var searchText = ((Entry)sender).Text;

            // Create a new observvable collection that will have all moves 
            // that match the search text
            ObservableCollection<move> searchedMoves = new ObservableCollection<move>();

            foreach (var item in moves) {
                if (item.name.StartsWith(searchText)) {
                    searchedMoves.Add(item);
                }
            }
            ConfigureListView(ref MoveList, searchedMoves);
        }
        
        // Play the sound for the associated pokemon
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e) {
            loadAndPlay();
        }

        public void loadAndPlay() {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            Stream audioStream = assembly.GetManifestResourceStream("PokemonProject" + "." + (pokemon.id.ToString() + ".mp3"));
            // C:\Users\josh6\Desktop\7th Semester Materials\CSE382\cse382\PokemonProject\PokemonProject\PokemonProject\Bulbasaur.mp3
            try {
                player.Load(audioStream);
                player.Play();
            } catch (Exception ex) {
                // Note only the first 20 sounds are used anything passed that is an error 
                Console.WriteLine("Sound file not found");
            }
        }

    }
}