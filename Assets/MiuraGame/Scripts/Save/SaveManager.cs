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
    const int DefaultClearStageNum = 0;
    const int DefaultHighScore = 0;

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
            int csn = LoadClearStageNum();

            InitFileSave();

            SaveData data = new SaveData();
            data.volMaster = vm;
            data.volBgm = vb;
            data.volSe = vs;
            data.clearStageNum = csn;

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

    public void SaveClearStageNum(int csn)
    {
        try
        {
            float vm = 0.5f;
            float vb = 0.5f;
            float vs = 0.5f;
            LoadAudio(ref vm, ref vb, ref vs);

            InitFileSave();

            SaveData data = new SaveData();
            data.volMaster = vm;
            data.volBgm = vb;
            data.volSe = vs;
            data.clearStageNum = csn;

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

    public int LoadClearStageNum()
    {
        int rp = 0;
        try
        {
            InitFileLoad();

            SaveData data = bf.Deserialize(file) as SaveData;
            rp = data.clearStageNum;

        }
        catch (IOException)
        {
            Debug.LogError("failed to open file");
        }
        finally
        {
            if (file != null) { CloseFile(); }
        }
        return rp;
    }

    public void SaveHighScore(int stageNum, int highScore)
    {
        try
        {
            float vm = 0.5f;
            float vb = 0.5f;
            float vs = 0.5f;
            LoadAudio(ref vm, ref vb, ref vs);
            int csn = LoadClearStageNum();

            InitFileSave();

            SaveData data = new SaveData();
            data.volMaster = vm;
            data.volBgm = vb;
            data.volSe = vs;
            data.clearStageNum = csn;
            data.highScore[stageNum] = highScore;

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

            highScore = data.highScore[stageNum];

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
}
