using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SQLite;
using Xamarin.Essentials;
using System.Reflection;
using System.Security.Cryptography;
using Pokemon;
using System.Net.Http;
using SQLitePCL;

namespace Pokemon {
    public class DB {
        
        private static string DBName = "firstPokemon.db";
        public static SQLiteConnection conn;
        public static void OpenConnection() {
            string libFolder = FileSystem.AppDataDirectory;
            string fname = System.IO.Path.Combine(libFolder, DBName);
            conn = new SQLiteConnection(fname);
            conn.CreateTable<firstPK>();
            var data = conn.Table<firstPK>();

            if (data.FirstOrDefault() == null) {
                PopulateDB();
            }
        }

        // Populate the DB with the name of all pokemon currently in the API
        public static async void PopulateDB() {
            RestService _restService = new RestService();
            // Get only the first 905 entries everything past that point are just alteratives of the pokemon
            FirstPokemon firstPokemon = await _restService.GetFirstPokemonData("https://pokeapi.co/api/v2/pokemon?limit=905&offset=0/");

            for (int i = 0; i < firstPokemon.PokemonList.Length; i++) {
                conn.Insert(firstPokemon.PokemonList[i]);
            }
        }
    }
}
