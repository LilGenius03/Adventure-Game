using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class DataFileHandler
{
    public string dataDirectoryPath;
    public string dataFileName;

    public DataFileHandler(string directoryPath, string fileName)
    {
        dataDirectoryPath = directoryPath;
        dataFileName = fileName;
    }

    public void Save(GameData data, int slot)
    {
        string fileName = dataFileName + slot.ToString();
        string directoryPath = Path.Combine(dataDirectoryPath, fileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(directoryPath));
            string saveData = JsonUtility.ToJson(data, true);

            using(FileStream stream = new FileStream(directoryPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(saveData);
                }
            }
        }
        catch
        {

        }
    }

    public GameData Load(int slot)
    {
        string fileName = dataFileName + slot.ToString();
        string directoryPath = Path.Combine(dataDirectoryPath, fileName);
        GameData data = null;

        if(File.Exists(directoryPath))
        {
            try
            {
                string saveData = "";
                using (FileStream stream = new FileStream(directoryPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        saveData = reader.ReadToEnd();
                    }
                }

                data = JsonUtility.FromJson<GameData>(saveData);
            }
            catch
            {

            }
        }

        return data;
    }
}
