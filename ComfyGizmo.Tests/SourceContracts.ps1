$ErrorActionPreference = 'Stop'

$repo = Split-Path -Parent $PSScriptRoot
$hammer = Get-Content (Join-Path $repo 'ComfyGizmo\Core\HammerTableManager.cs') -Raw
$plugin = Get-Content (Join-Path $repo 'ComfyGizmo\ComfyGizmo.cs') -Raw
$rotation = Get-Content (Join-Path $repo 'ComfyGizmo\Core\RotationManager.cs') -Raw
$game = Get-Content (Join-Path $repo 'ComfyGizmo\Patches\GamePatch.cs') -Raw
$player = Get-Content (Join-Path $repo 'ComfyGizmo\Patches\PlayerPatch.cs') -Raw
$gizmos = Get-Content (Join-Path $repo 'ComfyGizmo\Core\Gizmos.cs') -Raw
$ghostGizmo = Get-Content (Join-Path $repo 'ComfyGizmo\Core\GhostGizmo.cs') -Raw
$abstractRotator = Get-Content (Join-Path $repo 'ComfyGizmo\Core\Rotators\AbstractRotator.cs') -Raw

$failures = [System.Collections.Generic.List[string]]::new()

function Require-NotContains([string] $text, [string] $value, [string] $name) {
  if ($text.Contains($value)) {
    $script:failures.Add("${name}: found '$value'")
  }
}

function Require-Contains([string] $text, [string] $value, [string] $name) {
  if (-not $text.Contains($value)) {
    $script:failures.Add("${name}: missing '$value'")
  }
}

Require-NotContains $hammer 'm_availablePiecesByCategory' 'category-layout independence'
Require-NotContains $hammer 'm_selectedCategory =' 'stateful category selection'
Require-NotContains $hammer 'Hud.m_instance.UpdatePieceList' 'legacy HUD refresh'
Require-Contains $hammer 'player.SetSelectedPiece(selectedPiece)' 'current Player selection API'
Require-Contains $hammer 'Utils.GetPrefabName(piece.gameObject)' 'vanilla prefab identity'

Require-Contains $plugin 'ManualLogSource' 'plugin-owned log source'
Require-Contains $rotation 'public static bool IsInitialized' 'rotation initialization state'
Require-Contains $rotation 'public static bool TryInitialize()' 'guarded rotation initialization'
Require-Contains $rotation 'AreRotatorsAlive' 'world-transition rotator validation'
Require-Contains $rotation 'if (IsInitialized && !AreRotatorsAlive())' 'stale rotator recovery'
Require-Contains $game 'RotationManager.TryInitialize();' 'non-fatal game startup initialization'
Require-Contains $player 'if (!RotationManager.TryInitialize())' 'placement initialization guard'
Require-Contains $rotation 'if (!IsInitialized)' 'rotation fallback guard'
Require-Contains $gizmos 'public bool IsAlive' 'gizmo lifetime check'
Require-Contains $gizmos '_gizmoInstances.Remove(this)' 'gizmo registry cleanup'
Require-Contains $ghostGizmo 'public bool IsAlive' 'ghost gizmo lifetime check'
Require-Contains $abstractRotator 'public bool IsAlive' 'rotator lifetime check'
Require-Contains $player 'if (!matcher.IsValid)' 'transpiler miss fallback'
Require-Contains $player 'return code;' 'transpiler original-instruction fallback'
Require-Contains $player 'm_availablePieces.Count > 0' 'non-empty available-piece guard'
Require-NotContains $player '.ThrowIfInvalid(' 'throwing transpiler'

if ($failures.Count -gt 0) {
  $failures | ForEach-Object { Write-Output "FAIL: $_" }
  exit 1
}

Write-Output 'Source compatibility contracts: all assertions passed.'
