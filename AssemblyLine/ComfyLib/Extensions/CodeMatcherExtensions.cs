namespace ComfyLib;

using System.Collections.Generic;
using System.Reflection.Emit;

using HarmonyLib;

public static class CodeMatcherExtensions {
  public static CodeMatcher SaveInstruction(this CodeMatcher matcher, out CodeInstruction instruction) {
    instruction = matcher.Instruction;
    return matcher;
  }
}
