using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Spawn / Checkpoint")]
    public Transform initialSpawn;
    private Transform lastCheckpoint;

    [Header("Stats")]
    public float elapsedTime { get; private set; }
    public int deaths { get; private set; }

    [Header("Level State")]
    public bool levelStarted;
    public bool levelFinished;

    [Header("Respawn")]
    public UnityEvent OnRespawn;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }

    void Start()
    {
        elapsedTime = 0f;
        deaths = 0;
        lastCheckpoint = initialSpawn;

        // Timer does NOT run yet until start trigger
        levelStarted = false;
        levelFinished = false;
    }

    void Update()
    {
        if (levelStarted && !levelFinished)
            elapsedTime += Time.deltaTime;
    }

    public void StartLevel()
    {
        levelStarted = true;
        elapsedTime = 0f;
        deaths = 0;
        lastCheckpoint = initialSpawn;

        Debug.Log("[LevelManager] Level started");
    }

    public void FinishLevel()
    {
        if (levelFinished) return;

        levelFinished = true;
        levelStarted = false;

        SavePB();
        Debug.Log("[LevelManager] Level finished!");
        SceneManager.LoadScene("MainHub");
    }

    public void PlayerDied()
    {
        deaths++;
        Debug.Log("Player died");
        RespawnPlayer();
    }

    public void SetCheckpoint(Transform checkpointTransform)
    {
        if (checkpointTransform != null)
            lastCheckpoint = checkpointTransform;
    }

public void RespawnPlayer()
    {
        if (lastCheckpoint == null)
            lastCheckpoint = initialSpawn;

        Debug.Log($"[LevelManager] Respawning player to checkpoint: {lastCheckpoint.name}");
        Debug.Log($"[LevelManager] Checkpoint Position: {lastCheckpoint.position}");
        Debug.Log($"[LevelManager] Checkpoint Rotation: {lastCheckpoint.rotation.eulerAngles}");

        PlayerRespawner respawner = FindObjectOfType<PlayerRespawner>();
        if (respawner != null)
        {
            Debug.Log($"[LevelManager] Found PlayerRespawner on: {respawner.gameObject.name}");
            respawner.RespawnTo(lastCheckpoint.position, lastCheckpoint.rotation);
        }
        else
        {
            Debug.LogError("[LevelManager] PlayerRespawner not found!");
        }
    }

    void SavePB()
    {
        string key = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "_PB";

        string sceneName = SceneManager.GetActiveScene().name;
        string deathsKey = sceneName + "_Deaths";

        if (!PlayerPrefs.HasKey(key) || elapsedTime < PlayerPrefs.GetFloat(key))
        {
            PlayerPrefs.SetFloat(key, elapsedTime);
            PlayerPrefs.SetInt(deathsKey, deaths);
            PlayerPrefs.Save();
            Debug.Log($"[PB SAVED] Scene: Level1, Key: {key}, Time: {elapsedTime:F2}, Deaths: {deaths}");
        }
        else
        {
            Debug.Log($"[PB NOT SAVED] Existing PB is better. Key: {key}, Old PB: {PlayerPrefs.GetFloat(key):F2}, New Time: {elapsedTime:F2}");
        }
    }
}
