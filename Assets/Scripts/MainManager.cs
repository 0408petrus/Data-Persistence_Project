using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
#endif

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public static MainManager Instance;

    public Text ScoreText;
    public Text bestScoreText;
    public GameObject GameOverText;
    public GameObject score;
    public GameObject bestScore;

    private bool m_Started = false;
    private int m_Points;
    private int m_HighScore;
    private string m_PlayerName;
    private bool m_GameOver = false;

    private void Awake()
    {
        /*if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);*/

        LoadGame();
    }
    // Start is called before the first frame update
    void Start()
    {
            StartNew();
    }

    

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    [System.Serializable]
    class SaveData
    {
        public int m_SavePlayerScore;
        public string m_PlayerName;
        
    }
    private void StartNew()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }
    public void SaveGame()
    {
        // if m_Points (current points is higher), then save
        if (m_HighScore < m_Points)
        {
            SaveData data = new SaveData();

            data.m_SavePlayerScore = m_Points;
            data.m_PlayerName = UIMenuHandler.nameInput;
            m_PlayerName = data.m_PlayerName;
            string json = JsonUtility.ToJson(data);

            File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
            bestScoreText.text = "Best Score : " + data.m_PlayerName + ScoreText.text;
        }
    }

    public void LoadGame()
    {
        m_HighScore = 0;
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            m_HighScore = data.m_SavePlayerScore;
            m_PlayerName = data.m_PlayerName;

            bestScoreText.text = "Best Score : " + m_PlayerName + "Score: " + m_HighScore.ToString();
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public int GetHighScore()
    {
        return m_HighScore;
    }

    public string GetPlayerName()
    {
        return m_PlayerName;
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        SaveGame();
    }



}
