using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; set; }
    // セーブファイル名
    const string FileName = "/savedata.data";
    // セーブデータのデフォルト値
    const float DefaultVolumeMaster = 0.5f;
    const float DefaultVolumeBgm = 0.5f;
    const float DefaultVolumeSe = 0.5f;

    FileStream file;
    BinaryFormatter bf;
    string filePath;

    void Awake()
    {
        filePath = Application.dataPath + FileName;
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        transform.parent = null;
        DontDestroyOnLoad(gameObject);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void InitFileSave()
    {
        bf = new BinaryFormatter();
        file = File.Create(filePath);
    }

    void InitFileLoad()
    {
        bf = new BinaryFormatter();
        file = File.Open(filePath, FileMode.Open);
    }

    void CloseFile()
    {
        file.Close();
        file = null;
    }

    public bool SaveDataCheck()
    {
        if (File.Exists(filePath)) { return true; }
        return false;
    }

    public void CreateSaveData()
    {
        try
        {
            InitFileSave();

            SaveData data = new SaveData();
            data.volMaster = DefaultVolumeMaster;
            data.volBgm = DefaultVolumeBgm;
            data.volSe = DefaultVolumeSe;
            data.highScores = new int[] { 0, 0, 0 };
            data.bestRanks = new int[] { 4, 4, 4 };

            bf.Serialize(file, data);
        }
        catch (IOException)
        {
            Debug.LogError("failed to open file");
        }
        finally
        {
            if (file != null) { CloseFile(); }
        }
    }

    public void SaveAudio(float vm, float vb, float vs)
    {
        try
        {
            SaveData data;

            if (SaveDataCheck())
            {
                InitFileLoad();
                data = bf.Deserialize(file) as SaveData;
                CloseFile();
            }
            else
            {
                data = new SaveData();
            }

            data.volMaster = vm;
            data.volBgm = vb;
            data.volSe = vs;
            InitFileSave();
            bf.Serialize(file, data);
        }
        catch (IOException)
        {
            Debug.LogError("failed to open file");
        }
        finally
        {
            if (file != null) { CloseFile(); }
        }
    }

    public void LoadAudio(ref float vm, ref float vb, ref float vs)
    {
        try
        {
            InitFileLoad();

            SaveData data = bf.Deserialize(file) as SaveData;
            vm = data.volMaster;
            vb = data.volBgm;
            vs = data.volSe;
        }
        catch (IOException)
        {
            Debug.LogError("failed to open file");
        }
        finally
        {
            if (file != null) { CloseFile(); }
        }
    }

    public void SaveHighScore(int stageNum, int newScore)
    {
        try
        {
            SaveData data;

            if (SaveDataCheck())
            {
                InitFileLoad();
                data = bf.Deserialize(file) as SaveData;
                CloseFile();
            }
            else
            {
                data = new SaveData();
            }

            if (data.highScores[stageNum] < newScore)
            {
                data.highScores[stageNum] = newScore;
            }

            InitFileSave();
            bf.Serialize(file, data);
        }
        catch (IOException)
        {
            Debug.LogError("failed to open file");
        }
        finally
        {
            if (file != null) { CloseFile(); }
        }
    }

    public int LoadHighScore(int stageNum)
    {
        int highScore = 0;
        try
        {
            InitFileLoad();

            SaveData data = bf.Deserialize(file) as SaveData;

            highScore = data.highScores[stageNum];

        }
        catch (IOException)
        {
            Debug.LogError("failed to open file");
        }
        finally
        {
            if (file != null) { CloseFile(); }
        }
        return highScore;
    }

    public void SaveBestRank(int stageNum, int newRank)
    {
        try
        {
            SaveData data;

            if (SaveDataCheck())
            {
                InitFileLoad();
                data = bf.Deserialize(file) as SaveData;
                CloseFile();
            }
            else
            {
                data = new SaveData();
            }

            if (data.bestRanks[stageNum] > newRank)
            {
                data.bestRanks[stageNum] = newRank;
            }

            InitFileSave();
            bf.Serialize(file, data);
        }
        catch (IOException)
        {
            Debug.LogError("failed to open file");
        }
        finally
        {
            if (file != null) { CloseFile(); }
        }
    }

    public int LoadBestRank(int stageNum)
    {
        int bestRank = 0;
        try
        {
            InitFileLoad();

            SaveData data = bf.Deserialize(file) as SaveData;

            bestRank = data.bestRanks[stageNum];

        }
        catch (IOException)
        {
            Debug.LogError("failed to open file");
        }
        finally
        {
            if (file != null) { CloseFile(); }
        }
        return bestRank;
    }
}
