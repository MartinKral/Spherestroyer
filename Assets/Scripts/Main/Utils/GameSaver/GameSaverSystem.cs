using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

#if UNITY_DOTSRUNTIME
using Unity.Tiny.GameSave;

[AlwaysUpdateSystem]
[UpdateAfter(typeof(GameSaveSystem))]
#endif

public class GameSaverSystem : SystemBase
{
    public bool IsReady { get; private set; } = false;

#if UNITY_DOTSRUNTIME
    private readonly string mainSaveFileName = "main-save-file";
    private GameSaveSystem gameSaveSystem;

    protected override void OnCreate()
    {
        gameSaveSystem = World.GetExistingSystem<GameSaveSystem>();
    }

    protected override void OnStartRunning()
    {
        EntityManager.AddComponentData(
            EntityManager.CreateEntity(),
            new GameSaveReadFromPersistentStorageRequest(mainSaveFileName, 0));
    }
#else
    private GameSaverSystemMock gameSaveSystem;

    protected override void OnCreate()
    {
        gameSaveSystem = World.GetExistingSystem<GameSaverSystemMock>();
        IsReady = true;
    }

#endif

    protected override void OnUpdate()
    {
        if (IsReady) return;

#if UNITY_DOTSRUNTIME
        Entities.ForEach((Entity e, ref GameSaveReadFromPersistentStorageRequest readRequest) => {
            if ((GameSaverResult)(int)readRequest.result == GameSaverResult.Success)
            {
                IsReady = true;
            }
        }).WithStructuralChanges().Run();
#endif
    }

    public GameSaverResult Read<T>(string key, ref T value) where T : unmanaged
    {
        return (GameSaverResult)(int)gameSaveSystem.Read(key, ref value);
    }

    public GameSaverResult Read<T>(string key, out T value, in T defaultValue) where T : unmanaged
    {
        return (GameSaverResult)(int)gameSaveSystem.Read(key, out value, defaultValue);
    }

    public GameSaverResult Write<T>(string key, T value) where T : unmanaged
    {
        return (GameSaverResult)(int)gameSaveSystem.Write(key, value);
    }

    public void Save()
    {
#if UNITY_DOTSRUNTIME
        EntityManager.AddComponentData(
            EntityManager.CreateEntity(),
            new GameSaveWriteToPersistentStorageRequest(mainSaveFileName, 0));
#endif
    }
}

public class GameSaverSystemMock : SystemBase
{
    protected override void OnUpdate()
    {
    }

    public GameSaverResult Read<T>(FixedString64 key, ref T value) where T : unmanaged
    {
        Logger.Log($"[FAKE] Tried to read {key}");
        return GameSaverResult.Success;
    }

    public GameSaverResult Read<T>(FixedString64 key, out T value, in T defaultValue) where T : unmanaged
    {
        Logger.Log($"[FAKE] Tried to read {key}");
        value = defaultValue;
        return GameSaverResult.Success;
    }

    public GameSaverResult Write<T>(FixedString64 key, T value) where T : unmanaged
    {
        Logger.Log($"[FAKE] Tried to write {key}");
        return GameSaverResult.Success;
    }
}

public class MockGameSaveReadFromPersistentStorageRequest
{
    public GameSaverResult result;
}

public enum GameSaverResult
{
    NotStarted,
    ReadInProgress,
    WriteInProgress,
    Success,
    ErrorNotFound,
    ErrorType,
    ErrorDataSize,
    ErrorReadFailed,
    ErrorMultipleRequestsToSameFile,
    ErrorWriteFailed
}
