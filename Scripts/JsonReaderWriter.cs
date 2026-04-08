using Godot;


using System.Text.Json;
using System.Collections.Generic;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public abstract class JsonReaderWriter
	{
        private const string JSONFOLDER_PATH = "res://data/";


        public static List<T> ReadJsonToList<T>(string pPath)
        {
            using Godot.FileAccess lFile = Godot.FileAccess.Open(JSONFOLDER_PATH+pPath, Godot.FileAccess.ModeFlags.Read);

            if (lFile == null)
            {
                GD.Print("Could not open the file.");
                return new List<T>();
            }

            JsonSerializerOptions lSerialyzingOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };


            string lJsonString = lFile.GetAsText();

            List<T> lItems = JsonSerializer.Deserialize<List<T>>(lJsonString, lSerialyzingOptions);

            lFile.Close();

            return lItems;
        }

        public static void WriteListToJson<T>(string pPath, List<T> pObjectList)
        {

            using Godot.FileAccess lFile = Godot.FileAccess.Open(JSONFOLDER_PATH + pPath, Godot.FileAccess.ModeFlags.Write);

            if (lFile == null)
            {
                GD.Print("Could not open the file.");
                return;
            }

            JsonSerializerOptions lSerialyzingOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            string lSerializedAccountList = JsonSerializer.Serialize(pObjectList);



            lFile.StoreString(lSerializedAccountList);

            lFile.Close();
        }


    }
    
}
