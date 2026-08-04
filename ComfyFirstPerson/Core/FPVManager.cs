namespace ComfyFirstPerson;

using UnityEngine;

using static PluginConfig;
using static ComfyFirstPerson;

public sealed class FPVManager {
  private static bool isFirstPerson = false;
  private static bool isInitialized = false;

   private static void changeFOV(int delta) {
    GameCamera gameCamera = GameCamera.instance;

    if (gameCamera == null) {
      return;
    }

    gameCamera.m_fov += delta;
    ShowMessage($"Changed fov to: {gameCamera.m_fov}");

  }
  public static bool CanChangePOV() {
    return (!Chat.instance || !Chat.instance.HasFocus())
          && !Console.IsVisible()
          && !InventoryGui.IsVisible()
          && !StoreGui.IsVisible()
          && !Menu.IsVisible()
          && !Minimap.IsOpen()
          && !Player.m_localPlayer.InCutscene()
          && !Player.m_localPlayer.InPlaceMode();
  }

  public static void IncreaseFOV() {
    changeFOV(1);
  }

  public static void Initialize() {
    GameCamera gameCamera = GameCamera.instance;

    if (gameCamera == null) {
      return;
    }

    MinDist = gameCamera.m_minDistance;
    MaxDist = gameCamera.m_maxDistance;
    ZoomSens = gameCamera.m_zoomSens;

    isInitialized = true;
  }

  public static void DecreaseFOV() {
    changeFOV(-1);
  }

  public static bool IsFirstPerson() {
    return isFirstPerson;
  }

  public static bool IsInitialized() {
    return isInitialized;
  }

  public static void SetActiveCameraFields() {
    GameCamera gameCamera = GameCamera.instance;
    Player localPlayer = Player.m_localPlayer;

    if (gameCamera == null || localPlayer == null) {
      return;
    }

    OldOffset = gameCamera.m_3rdOffset;
    OldFps = gameCamera.m_fpsOffset;

    gameCamera.m_3rdOffset = new Vector3(0f, 0f, 0f);
    gameCamera.m_fpsOffset = Vector3.zero;
    gameCamera.m_minDistance = 0f;
    gameCamera.m_maxDistance = 0f;
    gameCamera.m_zoomSens = 0f;
    gameCamera.m_nearClipPlaneMax = 0.02f;
    gameCamera.m_nearClipPlaneMin = 0.01f;
    gameCamera.m_fov = DefaultFov.Value;
    localPlayer.m_head.localScale = Vector3.zero;
    localPlayer.m_eye.localScale = Vector3.zero;
  }

  public static void SetDisabledCameraFields() {
    GameCamera gameCamera = GameCamera.instance;
    Player localPlayer = Player.m_localPlayer;

    if (gameCamera == null || localPlayer == null) {
      return;
    }
    gameCamera.m_3rdOffset = OldOffset;
    gameCamera.m_fpsOffset = OldFps;
    gameCamera.m_minDistance = MinDist;
    gameCamera.m_maxDistance = MaxDist;
    gameCamera.m_zoomSens = ZoomSens;
    gameCamera.m_fov = 65f;

    localPlayer.m_head.localScale = Vector3.one;
    localPlayer.m_eye.localScale = Vector3.one;
  }

  public static void SetCameraPosition() {
    GameCamera gameCamera = GameCamera.instance;
    Player localPlayer = Player.m_localPlayer;

    if (gameCamera == null || localPlayer == null) {
      return;
    }

    gameCamera.transform.position = new Vector3(
        Player.m_localPlayer.m_head.position.x + 0.15f,
        Player.m_localPlayer.m_head.position.y + 0.30f,
        Player.m_localPlayer.m_head.position.z);
  }

  public static void ToggleFirstPerson() {
    isFirstPerson = !isFirstPerson;

    if (isFirstPerson) {
      SetActiveCameraFields();
      return;
    }

    SetDisabledCameraFields();
  }
}
