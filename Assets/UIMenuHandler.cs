using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
#endif

public class UIMenuHandler : MonoBehaviour
{
    public static string nameInput;
    [SerializeField] TextMeshProUGUI bestScoreText;
    private int m_HighScore;
    private string m_PlayerName;
    // Start is called before the first frame update
    void Start()
    {
        DataLoaded();
        /*bestScore = MainManager.Instance.GetHighScore();
        playerName = MainManager.Instance.GetPlayerName();
        bestScoreText.text = "Best Score: " + playerName + " Score: " + bestScore;*/
    }

    [System.Serializable]
    class SaveData
    {
        public int m_SavePlayerScore;
        public string m_PlayerName;

    }
    void DataLoaded() 
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            m_HighScore = data.m_SavePlayerScore;
            m_PlayerName = data.m_PlayerName;

            bestScoreText.text = "Best Score: " + m_PlayerName + "Score: " + m_HighScore;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEndEditInputInputField(string input)
    {
        nameInput = input+" ";
        Debug.Log(nameInput);
    }

    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
    //MainManager.Instance.SaveGame();
#if UNITY_EDITOR
    EditorApplication.ExitPlaymode();
#else
    Application.Quit();
#endif

    }
}
