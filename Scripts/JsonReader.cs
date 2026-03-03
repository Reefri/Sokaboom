using Godot;
//using Godot.NativeInterop;
using System;
//using GodotDict = Godot.Collections.Dictionary;
//using GodotArray = Godot.Collections.Array;


using System.IO;
using System.Text.Json;
using System.Collections.Generic;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public static class JsonReader 
	{
        public static List<T> ReadJson<T>(string pPath)
        {
            JsonSerializerOptions lOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };


            using var lFile = Godot.FileAccess.Open(pPath, Godot.FileAccess.ModeFlags.Read);
            string lJsonString = lFile.GetAsText();

            List<T> lItems = JsonSerializer.Deserialize<List<T>>(lJsonString, lOptions);

            return lItems;
        }
    }
}
