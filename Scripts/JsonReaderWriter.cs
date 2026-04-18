using Godot;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public abstract class JsonReaderWriter
	{
        private const string RES_JSONFOLDER_PATH = "res://data/";
        private const string USER_JSONFOLDER_PATH = "user://data/";


        public static List<T> ReadJsonToList<T>(string pPath)
        {
            using Godot.FileAccess lFile = Godot.FileAccess.Open(USER_JSONFOLDER_PATH+pPath, Godot.FileAccess.ModeFlags.Read) ?? 
                Godot.FileAccess.Open(RES_JSONFOLDER_PATH + pPath, Godot.FileAccess.ModeFlags.Read);

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
             Godot.FileAccess lFile = Godot.FileAccess.Open(USER_JSONFOLDER_PATH + pPath, Godot.FileAccess.ModeFlags.Write);

            if (lFile == null)
            {

                // Create folder if it doesn't exist
                if (!DirAccess.Open("user://").DirExists("data"))
                {
                    DirAccess.Open("user://").MakeDir("data");
                }

                string sourcePath = ProjectSettings.GlobalizePath(RES_JSONFOLDER_PATH + pPath);
                string destPath = ProjectSettings.GlobalizePath(USER_JSONFOLDER_PATH);


                File.Copy(sourcePath, destPath, false);

                lFile = Godot.FileAccess.Open(USER_JSONFOLDER_PATH + pPath, Godot.FileAccess.ModeFlags.Write);

            }



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
