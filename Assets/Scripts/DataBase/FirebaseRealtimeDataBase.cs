using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class FirebaseRealtimeDataBase : MonoBehaviour
{

    public static FirebaseRealtimeDataBase Instance;

    private DatabaseReference dbReference;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                dbReference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase Ready");
            }
            else
            {
                Debug.LogError($"Firebase Error Loading: {task.Result}");
            }
        });
    }

    public void RecordStatUpgrade(string statName)
    {
        if (dbReference == null) return;

        string playerName = PlayerPrefs.GetString("PlayerName", "Unknown_Hero");

        dbReference.Child("player_stats").Child(playerName).Child(statName).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            int currentCount = 0;
            if (task.IsCompleted && task.Result.Exists)
            {
                currentCount = int.Parse(task.Result.Value.ToString());
            }
            dbReference.Child("player_stats").Child(playerName).Child(statName).SetValueAsync(currentCount + 1);
        });

        dbReference.Child("stat_analytics").Child(statName).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            int currentCount = 0;
            if (task.IsCompleted && task.Result.Exists)
            {
                currentCount = int.Parse(task.Result.Value.ToString());
            }
            dbReference.Child("stat_analytics").Child(statName).SetValueAsync(currentCount + 1);

            Debug.Log($"{statName} for {playerName}");
        });
    }

    public void RecordGameCompletionTime(string playerName, float playTimeSeconds)
    {
        string newSessionKey = dbReference.Child("game_completions").Push().Key;

        dbReference.Child("game_completions").Child(newSessionKey).Child("playerName").SetValueAsync(playerName);
        dbReference.Child("game_completions").Child(newSessionKey).Child("time_seconds").SetValueAsync(playTimeSeconds);

        Debug.Log($"{playTimeSeconds}");
    }

public void RecordDeath(string monsterName)
    {
        if (dbReference == null) return;

        string playerName = PlayerPrefs.GetString("PlayerName", "Unknown_Hero");

        dbReference.Child("player_deaths").Child(playerName).Child(monsterName).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            int currentCount = 0;
            if (task.IsCompleted && task.Result.Exists)
            {
                currentCount = int.Parse(task.Result.Value.ToString());
            }
            dbReference.Child("player_deaths").Child(playerName).Child(monsterName).SetValueAsync(currentCount + 1);
        });

        dbReference.Child("death_analytics").Child(monsterName).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            int currentCount = 0;
            if (task.IsCompleted && task.Result.Exists)
            {
                currentCount = int.Parse(task.Result.Value.ToString());
            }
            dbReference.Child("death_analytics").Child(monsterName).SetValueAsync(currentCount + 1);

            Debug.Log($"{playerName} Death by {monsterName}");
        });
    }
}
