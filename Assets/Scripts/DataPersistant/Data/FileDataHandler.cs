using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "ivnsjaw";
    private readonly string backupExtension = ".bak";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load(string profileId, bool allowRestoreFromBackup = true)
    {
        // return nếu profileId = null
        if (profileId == null)
            return null;

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try 
            {
                // Load data từ file
                string dataToLoad = "";

                
                using(FileStream fileStream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // Mã hóa data
                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                // giải tuần tự hóa từ Json thành data game

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e) 
            {

                // Có lỗi xảy ra trong khi load
                // tiến hành backup lại file từ backup file
                // load lại GameData 
                if (allowRestoreFromBackup)
                {
                    Debug.LogWarning("Thất bại khi cố gắng load file, đang load lại");
                    bool rollBackSuccess = AttemtRollback(fullPath);
                    if (rollBackSuccess)
                    {
                        // Đang cố load lại
                        loadedData = Load(profileId, false);
                    }
                }
                // Backup lại file không thành công
                else
                {
                    Debug.Log("Lỗi khi cố load file tại: " + fullPath);
                }
            }
        }
        return loadedData;
    }

    public void Save(GameData data, string profileId)
    {
        // return nếu profileId = null
        if (profileId == null)
            return;

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        string backupFilePath = fullPath + backupExtension;

        try
        {
            // tạo thư mục nếu như chưa có thư mục
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serialize C# data game object thành Json
            string dataToStore  = JsonUtility.ToJson(data, true);

            // optionally encrypt the data
            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            // Viết xuống file 
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

            // kiểm tra data vừa lưa có load được không
            GameData verifiedGameData = Load(profileId);
            // if file data vừa lưu load thành công, backup lại
            if (verifiedGameData != null)
            {
                File.Copy(fullPath,backupFilePath,true);
            }
            // lỗi, ném ngoại lệ
            else
            {
                throw new Exception("Data file load không load được, backup không được tạo!");
            }

        }
        catch (Exception e)
        {
            Debug.LogError("Lỗi khi cố lưu game data tới file: " + fullPath + "\n" + e);
        }
    }

    public void Delete(string profileId)
    {
        // base case - return nếu profileId null 
        if (profileId == null)
            return;

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);

        try
        {
            // Đảm bảo đường dẫn tồn tại 
            if (File.Exists(fullPath))
            {
                // Xóa thư mục profileId
                Directory.Delete(Path.GetDirectoryName(fullPath), true);
            }
            else
            {
                Debug.LogWarning("Đang cố xóa profileId nhưng dữ liệu không tồn tại tại đường dẫn: " + fullPath);
            }
        }catch(Exception e)
        {
            Debug.LogWarning("Có lỗi xảy ra trong khi cố xóa profileId: "+profileId + " tại đường dẫn: "+ fullPath);
        }
    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        // Load toàn bộ thư mục con trong thư mục truyền vào
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            // defensive programming - kiểm tra nếu file data file tồn tại
            // nếu không, thư mục này không phải là một profile và bỏ qua
            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Bỏ qua thư mục này vì nó không chứa dữ liệu: " + profileId);
                continue;
            }

            // Load data 
            GameData profileData = Load(profileId);
            // defensive programming - chắc chắn rằng profile không null
            // nếu null thì có gì đó sai
            if (profileData !=null)
            {
                profileDictionary.Add(profileId, profileData);
            }
            else
            {
                Debug.LogError("Đang load profile data nhưng có gì đó không đúng, ProfileId: " + profileId);
            }
        }
        return profileDictionary;
    }

    public string GetMostRecentlyUpdatedProfileId() 
    {
        string mostRecentProfileId = null;

        Dictionary<string, GameData> profilesGameData = LoadAllProfiles();
        foreach (KeyValuePair<string, GameData> pair in profilesGameData) 
        {
            string profileId = pair.Key;
            GameData gameData = pair.Value;

            // skip this entry if the gamedata is null
            if (gameData == null) 
            {
                continue;
            }

            // if this is the first data we've come across that exists, it's the most recent so far
            if (mostRecentProfileId == null) 
            {
                mostRecentProfileId = profileId;
            }
            // otherwise, compare to see which date is the most recent
            else 
            {
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesGameData[mostRecentProfileId].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);
                // the greatest DateTime value is the most recent
                if (newDateTime > mostRecentDateTime) 
                {
                    mostRecentProfileId = profileId;
                }
            }
        }
        return mostRecentProfileId;
    }

    // the below is a simple implementation of XOR encryption
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }

    private bool AttemtRollback(string fullPath)
    {
        bool succel = false;

        string backupFilePath = fullPath + backupExtension;

        try
        {
            if (File.Exists(backupFilePath))
            {
                File.Copy(backupFilePath, fullPath, true);
                succel = true;
                Debug.Log("Đã quay lại backup file tại :" + backupFilePath);
            }
            // file backup không tồn tại để backup
            else
            {
                throw new Exception("Đang cố backup lại file data từ backup file, nhưng không tìm thấy backup file.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Xảy ra lỗi gì đó khi đang cố gắng backup lại file : " + backupFilePath + "\n" + ex);
        }

        return succel;
    }
}
