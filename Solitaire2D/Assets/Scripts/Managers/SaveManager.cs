using System.IO;
using System.Text;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static string savesFolder;

    private static void InitializeFolder()
    {
        if (savesFolder == null || savesFolder.Length == 0)
        {
            savesFolder = Application.dataPath;
            if (Application.isEditor)
                savesFolder = Path.GetFullPath(savesFolder + "/..");
            savesFolder += "/Saves";

            if (!Directory.Exists(savesFolder))
                Directory.CreateDirectory(savesFolder);
        }
    }

    public static void SaveSkins(SkinData data)
    {
        InitializeFolder();

        string savePath = $"{savesFolder}/skins.dat";
        string saveData = JsonUtility.ToJson(data);

        if (!File.Exists(savePath))
            File.Create(savePath);

        using (StreamWriter sw = new StreamWriter(savePath))
        { 
            sw.Write(saveData);
        }
    }

    public static void LoadSkins(out SkinData data)
    {
        data = null;

        InitializeFolder();

        string loadPath = $"{savesFolder}/skins.dat";
        string loadData = string.Empty;

        if (!File.Exists(loadPath))
            return;

        using (FileStream fs = new FileStream(loadPath, FileMode.Open))
        {
            using (StreamReader sr = new StreamReader(fs))
            {
                loadData = sr.ReadToEnd();
            }
        }

        data = JsonUtility.FromJson<SkinData>(loadData);
    }
}
