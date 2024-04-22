namespace Aurality;

using UnityEngine;

public static class ZSFXUtils {
  public static bool IsBuzzingSfx(ZSFX sfx) {
    if (sfx.name == "sfx_deathsquito_attack(Clone)") {
      return true;
    }

    if (sfx.m_audioClips == null || sfx.m_audioClips.Length <= 0) {
      return false;
    }

    foreach (AudioClip audioClip in sfx.m_audioClips) {
      if (audioClip && audioClip.name == "Insect_Wasp_WingsLoop3") {
        return true;
      }
    }

    return false;
  }
}
