/*
*���j�F�c�N�[��MV��json�t�@�C���`���ƌ݊���������悤�ɍ쐬����
*/

using System.IO;
using UnityEngine;

/*
*Actor.json�ɍ��킹��
*�c�N�[��MV ver1.3.4�
*/

public class CharacterObject{

    public int id = 0;
    public string battterName = "";
    public int characterIndex = 0;
    public string characterName = "";
    public int classId = 0;
    public int[] equips;
    public int faceIndex = 0;
    public string faceName = "";
    public int[] traits;
    public int initialLevel = 0;
    public int maxLevel = 0;
    public string name = "";
    public string nickname = "";
    public string note = "";
    public string profile = "";

}

public class CharacterDataSingleton:MonoBehaviour{

    public void GetCharacterData() {

        string folderpath = Application.dataPath + "/Resources/data/";
        string filePath = folderpath + "Actors.json";

        if (!File.Exists(filePath)) return;

        var jsonText = File.ReadAllText(filePath);
        var json = LitJson.JsonMapper.ToObject<CharacterObject[]>(jsonText);

    }
}
