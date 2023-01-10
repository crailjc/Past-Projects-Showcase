using Newtonsoft.Json;
using PokemonProject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace Pokemon {
	public class FirstPokemon {

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("next")]
        public string nextURL { get; set; }

        [JsonProperty("previous")]
        public string lastURL { get; set; }

        [JsonProperty("results")]
		public firstPK[] PokemonList { get; set; }
    }

	public class firstPK
	{
        [JsonProperty("name")]
        public string PokemonName { get; set; }

        [JsonProperty("url")]
        public string PokemonUrl { get; set; }

        public int PokemonID { get; set; }

        public override string ToString(){
            return string.Format("{0} ({1})", PokemonName, PokemonUrl);
        }
    }

    public class PokemonData
    {
        [JsonProperty("abilities")]
        public ability[] abilities { get; set; }

        [JsonIgnore]
        [JsonProperty("base_experience")]
        public string exp { get; set; }

        [JsonIgnore]
        [JsonProperty("game_indices")]
        public string game_indices { get; set; }

        [JsonProperty("height")]
        public int height { get; set; }

        [JsonIgnore]
        [JsonProperty("held_items")]
        public string held_items { get; set; }

        [JsonProperty("id")]
        public int id { get; set; }

        [JsonIgnore]
        [JsonProperty("is_default")]
        public bool is_default { get; set; }

        [JsonIgnore]
        [JsonProperty("location_area_encounters")]
        public string area_encounters { get; set; }

        [JsonProperty("moves")]
        public moves[] moves { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonIgnore]
        [JsonProperty("order")]
        public string order { get; set; }

        [JsonIgnore]
        [JsonProperty("past_types")]
        public string past_types { get; set; }

        [JsonIgnore]
        [JsonProperty("species")]
        public string species { get; set; }

        [JsonIgnore]
        [JsonProperty("sprites")]
        public string sprites { get; set; }

        [JsonIgnore]
        [JsonProperty("stats")]
        public string stats { get; set; }

        [JsonProperty("types")]
        public typesInfo[] typeinfo { get; set; }

        [JsonProperty("weight")]
        public int weight{ get; set;}
    }

    public class typesInfo
    {
        [JsonProperty("slot")]
        public int slot { get; set; }
        [JsonProperty("type")]
        public detailedTypeInfo dtInfo { get; set; }
    }

    public class moveAP {
        [JsonProperty("accuracy")]
        public string acc { get; set; } = "None";

        // This variable is to help with moves 
        // that do not have an accuracy value
        public string strAcc {
            get {
                if (acc == null) {
                    return "No accuracy";
                } else {
                    return acc;
                }
            }
        }

        [JsonIgnore]
        [JsonProperty("contest_combos")]
        public string ccombos { get; set; }

        [JsonIgnore]
        [JsonProperty("contest_effects")]
        public string ceffects { get; set; }

        [JsonIgnore]
        [JsonProperty("contest_type")]
        public string contesttype { get; set; }

        [JsonIgnore]
        [JsonProperty("damage_class")]
        public string dclass { get; set; }

        [JsonIgnore]
        [JsonProperty("effect_chance")]
        public string echance { get; set; }

        [JsonIgnore]
        [JsonProperty("effect_changes")]
        public string echanges { get; set; }

        [JsonIgnore]
        [JsonProperty("effect_entries")]
        public string eentries { get; set; }

        [JsonIgnore]
        [JsonProperty("flavor_text_entries")]
        public string ftextentries { get; set; }

        [JsonIgnore]
        [JsonProperty("generation")]
        public string generation { get; set; }

        [JsonProperty("id")]
        public int id { get; set; }

        [JsonIgnore]
        [JsonProperty("learned_by_pokemon")]
        public string learned_by { get; set; }

        [JsonIgnore]
        [JsonProperty("machines")]
        public string machines { get; set; }

        [JsonIgnore]
        [JsonProperty("meta")]
        public string meta { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonIgnore]
        [JsonProperty("names")]
        public string names { get; set; }

        [JsonIgnore]
        [JsonProperty("past_values")]
        public string past_values { get; set; }

        [JsonProperty("power")]
        public string power { get; set; }

        // This is done for cases where there is no power
        public string strPower {
            get {
                if (power == null) {
                    return  "No power";
                } else {
                    return power;
                }
            }
        }

        [JsonProperty("pp")]
        public int pp { get; set; }

        [JsonProperty("priority")]
        public string priority { get; set; }

        [JsonIgnore]
        [JsonProperty("super_contest_effects")]
        public string sceffects { get; set; }

        [JsonProperty("target")]
        public targets target{ get; set; }

        [JsonProperty("type")]
        public moveType typeOfMove { get; set; }
    }
    
    public class moveType{
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("url")]
        public string url { get; set; }
    }


    public class targets {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("url")]
        public string url { get; set; }
    }

    public class detailedTypeInfo {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("url")]
        public string url { get; set; }

        public override string ToString()
        {
            return string.Format("{0}", name);
        }
    }

    public class moves {
        [JsonProperty("move")]
        public move moveInfo { get; set; }
        [JsonProperty("version_group_details")]
        public vGroupDetails[] version_group_details { get; set; }
    }

    public class vGroupDetails {
        [JsonProperty("level_learned_at")]
        public int lvlLearnedAt { get; set; }
        [JsonProperty("move_learn_method")]
        public moveLearnMethod moveMethod { get; set; }
        [JsonProperty("version_group")]
        public version_group versionGroup { get; set; }
    }

    public class version_group {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("url")]
        public string url { get; set; }
    }

    public class moveLearnMethod {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("url")]
        public string url { get; set; }
    }


    public class move {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("url")]
        public string url { get; set; }
    }


    public class items {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("url")]
        public string url { get; set; }

        [JsonProperty("version_details")]
        public versionDetails[] version_details { get; set; }

    }

    public class versionDetails {
        [JsonProperty("rarity")]
        public int rarity { get; set; }

        [JsonProperty("version")]
        public versionInfo vInfo { get; set; }

    }

    public class versionInfo {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("url")]
        public string url { get; set; }
    }


    public class game_indicies {
        [JsonProperty("game_index")]
        public int game_index { get; set; }

        [JsonProperty("version")]
        public gameVersion gameVer { get; set; }

    }

    public class gameVersion {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("url")]
        public string url { get; set; }
    }


    public class form {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("url")]
        public string url { get; set; }
    }


    public class ability {
        [JsonProperty("ability")]
        public subAbility abilityInfo { get; set; }

        [JsonProperty("is_hidden")]
        public bool isHidden { get; set; }

        [JsonProperty("slot")]
        public int slot { get; set; }
    }

    public class subAbility {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("url")]
        public string url { get; set; }
    }

}
