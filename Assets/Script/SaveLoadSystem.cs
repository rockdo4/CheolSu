using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;

public class SaveLoadSystem
{
    public enum Mods
    {
        Json,
        Binary,
        EncryptedBinary,
    };

    public static Mods FileMode { get; } = Mods.Json;
    public static int SaveDataVersion { get; } = 1;
    private static string[] SaveSlotFileNames =
    {
        "Save0.json",
        "Save1.json",
        "Save2.json"
    };
    private static string AutoSaveFileName { get; } = "AutoSave.txt";

    public static string SaveDirectory
    {
        get
        {
            return $"{Application.persistentDataPath}/Save";
        }
    }

    public static void AutoSave(SaveData data)
    {
        Save(data, AutoSaveFileName);
    }

    public static SaveData AutoLoad()
    {
        return Load(AutoSaveFileName);
    }

    public static void Save(SaveData data, int slot)
    {
        Save(data, SaveSlotFileNames[slot]);
    }

    public static SaveData Load(int slot)
    {
        return Load(SaveSlotFileNames[slot]);
    }

    public static void Save(SaveData data, string filename)
    {
        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }

        var path = Path.Combine(SaveDirectory, filename);
        using (var writer = new JsonTextWriter(new StreamWriter(path)))
        {
            var serialize = new JsonSerializer();
            serialize.Converters.Add(new PlayerStatusConverter());
            serialize.Converters.Add(new ItemListConverter());
            serialize.Converters.Add(new SkillInfoConverter());
            serialize.Converters.Add(new GoldConverter());
            serialize.Converters.Add(new EnhanceConverter());
            serialize.Serialize(writer, data);
        }
    }

    public static SaveData Load(string filename)
    {
        var path = Path.Combine(SaveDirectory, filename);
        if (!File.Exists(path))
            return null;

        SaveData data = null;

        int version = 0;
        var json = File.ReadAllText(path);

        using (var reader = new JsonTextReader(new StringReader(json)))
        {
            var jObj = JObject.Load(reader);
            version = jObj["Version"].Value<int>();
        }

        using (var reader = new JsonTextReader(new StringReader(json)))
        {
            var serialize = new JsonSerializer();
            serialize.Converters.Add(new PlayerStatusConverter());
            serialize.Converters.Add(new ItemListConverter());
            serialize.Converters.Add(new SkillInfoConverter());
            serialize.Converters.Add(new GoldConverter());
            serialize.Converters.Add(new EnhanceConverter());

            switch (version)
            {
                case 1:
                    data = serialize.Deserialize<SaveDataV1>(reader);
                    break;
            }
        }

        while (data.Version < SaveDataVersion)
        {
            //data = data.VersionUp();
        }


        return data;
    }
}
