using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/tile.challenge";
    public static void UnlockLevel(int levelNumber)
    {
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream openStream = new FileStream(path, FileMode.Open);

            PlayerLevelData data = formatter.Deserialize(openStream) as PlayerLevelData;
            bool[] levelsStatus = data.levelsStatus;
            if (levelNumber > levelsStatus.Length) return;

            levelsStatus[levelNumber-1] = true;
            data.levelsStatus = levelsStatus;
            openStream.Close();

            FileStream createStream = new FileStream(path, FileMode.Create);
            formatter.Serialize(createStream, data);
            createStream.Close();
        }
        else
        {
            Debug.Log("Source file not found!");
        }
    }

    public static PlayerLevelData LoadLevel(int levelCount)
    {
        if (File.Exists(path))
        {
            Debug.Log("Loading Level Data");

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerLevelData data = formatter.Deserialize(stream) as PlayerLevelData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Creating new Level Data");

            PlayerLevelData data = new PlayerLevelData();
            bool[] result = new bool[levelCount];
            result[0] = true;
            data.levelsStatus = result;
            SaveLevelData(data);

            return data;
        }
    }

    static void SaveLevelData(PlayerLevelData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }
}