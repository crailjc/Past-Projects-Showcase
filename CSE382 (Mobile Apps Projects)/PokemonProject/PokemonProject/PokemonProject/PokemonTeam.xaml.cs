using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PokemonProject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PokemonTeam : ContentPage {
        string pName = Preferences.Get("PokemonName", "No Name");
        public PokemonTeam() {
            InitializeComponent();
            loadTeam();
        }

        public async void loadTeam() {
            string tempPK;
            int tempID, numSet = 0;
            string[] pkList = new string[6];
            for (int i = 1; i <= 6; i++) {
                tempPK = Preferences.Get("Pk" + i + "_Name", "No Team");
                tempID = Preferences.Get("Pk" + i + "_Img", 0);

                // Set the images and name for PK that exist
                if (tempPK != "No Team") {
                    setImg(i, tempPK, tempID);
                    pkList[numSet] = tempPK;
                    numSet++;
                }

            }

            // The team has been maxed out and the user is not
            // coming from the main page
            if (numSet == 6 && pName != "FromMainPage") {
                // Ask the user if they want to replace a pokemon
                string action = await DisplayActionSheet("Your team is full select pokemon to replace or cancel", "Cancel", "Remove",
                       "1: " + pkList[0], "2: " + pkList[1], "3: " + pkList[2], "4: " + pkList[3], "5: " + pkList[4], "6: " + pkList[5]);
                Console.Write(action);

                // Clear the selected pokemon
                if (action != "Cancel" && action != "Remove") {
                    // Console.WriteLine("In the if");
                    clearPokemon(action.Substring(0, 1));
                }
            }
            // Once the team has been loaded then check the team
            checkTeam();
        }

        // Check the current team to see what spot is going to be replaced
        public void checkTeam() {
            int teamNum = 1, pId = Preferences.Get("PokemonId", 0);

            // If the user is comming directly from the main page do
            // not check if there is a pokemon to add instead just return
            if (pName == "FromMainPage") {
                return;
            }

            // Loop through the six team slots and find the empty one
            while (teamNum <= 6) {
                string tempPK;
                tempPK = Preferences.Get("Pk" + teamNum + "_Name", "No Team");

                // The current teamNum is not assigned then assign the pokemon
                if (tempPK == "No Team") {
                    setImg(teamNum, pName, pId);
                    teamNum = 7;
                }
                teamNum++;
            }
        }
        
        // Method to set the correct image and text
        public void setImg(int teamNum, string pName, int pId) {
            // If one then pokemon one and img one
            if (teamNum == 1) {
                // Set name and pic
                Pokemon1.Source = "sprites/" + pId + ".png";
                Pk1_Name.Text = pName;
                // Set the preferences
                Preferences.Set("Pk" + teamNum + "_Name", pName);
                Preferences.Set("Pk" + teamNum + "_Img", pId);
            } else if (teamNum == 2) {
                Pokemon2.Source = "sprites/" + pId + ".png";
                Pk2_Name.Text = pName;
                Preferences.Set("Pk" + teamNum + "_Name", pName);
                Preferences.Set("Pk" + teamNum + "_Img", pId);
            } else if (teamNum == 3) {
                Pokemon3.Source = "sprites/" + pId + ".png";
                Pk3_Name.Text = pName;
                Preferences.Set("Pk" + teamNum + "_Name", pName);
                Preferences.Set("Pk" + teamNum + "_Img", pId);
            } else if (teamNum == 4) {
                Pokemon4.Source = "sprites/" + pId + ".png";
                Pk4_Name.Text = pName;
                Preferences.Set("Pk" + teamNum + "_Name", pName);
                Preferences.Set("Pk" + teamNum + "_Img", pId);
            } else if (teamNum == 5) {
                Pokemon5.Source = "sprites/" + pId + ".png";
                Pk5_Name.Text = pName;
                Preferences.Set("Pk" + teamNum + "_Name", pName);
                Preferences.Set("Pk" + teamNum + "_Img", pId);
            } else if (teamNum == 6) {
                Pokemon6.Source = "sprites/" + pId + ".png";
                Pk6_Name.Text = pName;
                Preferences.Set("Pk" + teamNum + "_Name", pName);
                Preferences.Set("Pk" + teamNum + "_Img", pId);
            }
        }

        // Clear the pokemon that has been clicked 
        private void Clear_Clicked(object sender, EventArgs e) {
            // Here I am using the autmoationID so I know specifically what button is being 
            // clicked without having a different event for each button
            string pkNum = ((Button)sender).AutomationId;

            clearPokemon(pkNum);
        }

        // General method to clear a pokemon from the team
        // this method deletes based on the pokemon id number
        public void clearPokemon(string pkNum) {
            // set the values to their default
            if (pkNum == "1") {
                Pk1_Name.Text = "No Name";
                Pokemon1.Source = "sprites/0.png";
                Preferences.Set("Pk" + pkNum + "_Name", "No Team");
                Preferences.Set("Pk" + pkNum + "_Img", 0);
            } else if (pkNum == "2") {
                Pk2_Name.Text = "No Name";
                Pokemon2.Source = "sprites/0.png";
                Preferences.Set("Pk" + pkNum + "_Name", "No Team");
                Preferences.Set("Pk" + pkNum + "_Img", 0);
            } else if (pkNum == "3") {
                Pk3_Name.Text = "No Name";
                Pokemon3.Source = "sprites/0.png";
                Preferences.Set("Pk" + pkNum + "_Name", "No Team");
                Preferences.Set("Pk" + pkNum + "_Img", 0);
            } else if (pkNum == "4") {
                Pk4_Name.Text = "No Name";
                Pokemon4.Source = "sprites/0.png";
                Preferences.Set("Pk" + pkNum + "_Name", "No Team");
                Preferences.Set("Pk" + pkNum + "_Img", 0);
            } else if (pkNum == "5") {
                Pk5_Name.Text = "No Name";
                Pokemon5.Source = "sprites/0.png";
                Preferences.Set("Pk" + pkNum + "_Name", "No Team");
                Preferences.Set("Pk" + pkNum + "_Img", 0);
            } else if (pkNum == "6") {
                Pk6_Name.Text = "No Name";
                Pokemon6.Source = "sprites/0.png";
                Preferences.Set("Pk" + pkNum + "_Name", "No Team");
                Preferences.Set("Pk" + pkNum + "_Img", 0);
            }
        }

        // This method will clear all of the team cells 
        private async void ClearTeam_Clicked(object sender, EventArgs e) {
            // Make sure the user understands that they are about delete the entire team
            bool answer = await DisplayAlert("Clear Team?", "Are you sure you want to clear your entire team?", "Yes", "No");

            // If true clear the entire team else nothing
            if (answer) {
                // Clear all 6 pokemons
                for (int i = 1; i <= 6; i++){
                    clearPokemon(i.ToString());
                }
            }
        }

    }
}