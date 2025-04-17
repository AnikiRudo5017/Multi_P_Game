using Fusion;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Fusion Setup")]
    public NetworkRunner RunnerPrefab;          // Prefab của NetworkRunner
    public string GameModeIdentifier = "Shooter"; // Định danh chế độ game
    public int MaxPlayerCount = 8;              // Số người chơi tối đa

    [Header("UI Setup")]
    public TMP_InputField NicknameText;         // Input field cho tên người chơi
    public TMP_InputField RoomText;             // Input field cho tên phòng
    public TextMeshProUGUI StatusText;          // Text hiển thị trạng thái

    private NetworkRunner _runnerInstance;      // Instance của NetworkRunner

    // Khởi động game khi nhấn nút Start
    public async void StartGame()
    {
        // Ngắt kết nối hiện tại nếu có
        await Disconnect();

        // Lưu tên người chơi vào PlayerPrefs
        PlayerPrefs.SetString("PlayerName", NicknameText.text);

        // Tạo instance của NetworkRunner
        _runnerInstance = Instantiate(RunnerPrefab);

        // Thêm NetworkSceneManagerDefault để tránh log thông báo
        if (_runnerInstance.GetComponent<INetworkSceneManager>() == null)
        {
            _runnerInstance.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        // Thêm listener cho sự kiện ngắt kết nối
        var events = _runnerInstance.GetComponent<NetworkEvents>();
        events.OnShutdown.AddListener(OnShutdown);

        // Thiết lập scene hiện tại
        var sceneInfo = new NetworkSceneInfo();
        sceneInfo.AddSceneRef(SceneRef.FromIndex(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex));

        // Cấu hình tham số khởi động
        var startArguments = new StartGameArgs()
        {
            GameMode = GameMode.Shared,              // Chế độ multiplayer chia sẻ
            SessionName = RoomText.text,             // Tên phòng từ input
            PlayerCount = MaxPlayerCount,            // Số người chơi tối đa
            SessionProperties = new Dictionary<string, SessionProperty> { ["GameMode"] = GameModeIdentifier },
            Scene = sceneInfo                        // Scene hiện tại
        };

        // Cập nhật trạng thái UI
        StatusText.text = "Connecting...";

        // Khởi động game và đợi kết quả
        var startTask = _runnerInstance.StartGame(startArguments);
        await startTask;

        // Xử lý kết quả
        if (startTask.Result.Ok)
        {
            StatusText.text = "Connected!";
            this.gameObject.SetActive(false);
        }
        else
        {
            StatusText.text = $"Connection Failed: {startTask.Result.ShutdownReason}";
        }
    }

    // Ngắt kết nối khi cần
    private async Task Disconnect()
    {
        if (_runnerInstance == null)
            return;

        StatusText.text = "Disconnecting...";

        var events = _runnerInstance.GetComponent<NetworkEvents>();
        events.OnShutdown.RemoveListener(OnShutdown);

        await _runnerInstance.Shutdown();
        _runnerInstance = null;
    }

    // Xử lý ngắt kết nối bất ngờ
    private void OnShutdown(NetworkRunner runner, ShutdownReason reason)
    {
        StatusText.text = $"Disconnected: {reason}";
        _runnerInstance = null;
        gameObject.SetActive(true); // Hiện lại UI khi ngắt kết nối
    }

    // Khởi tạo giá trị mặc định
    private void OnEnable()
    {
        var nickname = PlayerPrefs.GetString("PlayerName");
        if (string.IsNullOrEmpty(nickname))
        {
            nickname = "Player" + Random.Range(10000, 100000);
        }
        NicknameText.text = nickname;
        RoomText.text = "Room" + Random.Range(100, 1000); // Tên phòng mặc định
        StatusText.text = "";
    }
}