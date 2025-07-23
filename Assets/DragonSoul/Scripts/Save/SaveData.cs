//セーブデータクラス
[System.Serializable]
public class SaveData
{
    public float volMaster;
    public float volBgm;
    public float volSe;
    public int[] highScores = new int[3];
    public int[] bestRanks = new int[3];
}
