using ComfyGizmo;

static class Assert
{
  public static void True(bool condition, string message)
  {
    if (!condition)
    {
      throw new InvalidOperationException(message);
    }
  }
}

sealed record TestPiece(string PrefabName, string DisplayName);

static class Program
{
  public static void Main()
  {
    PieceSelectionCache<string> cache = new(value => value);
    cache.Rebuild(["wood_wall", "stone_wall"]);

    Assert.True(cache.TryGet("wood_wall", out string selected) && selected == "wood_wall",
        "The cache must return the selected build piece by stable key.");
    Assert.True(cache.Matches(["wood_wall", "stone_wall"]),
        "The cache must recognize an unchanged available-piece set.");
    Assert.True(!cache.Matches(["wood_wall", "iron_wall"]),
        "The cache must invalidate when content changes without a count change.");

    cache.Rebuild(["wood_wall", null, "stone_wall", ""]);
    Assert.True(cache.Count == 2,
        "The cache must skip null and empty keys while rebuilding.");

    PieceSelectionCache<TestPiece> identityCache = new(piece => piece.PrefabName);
    identityCache.Rebuild([
      new TestPiece("wood_wall", "Wall"),
      new TestPiece("stone_wall", "Wall")
    ]);
    Assert.True(identityCache.TryGet(new TestPiece("stone_wall", "Wall"), out TestPiece identityMatch)
        && identityMatch.PrefabName == "stone_wall",
        "The cache must distinguish pieces with the same display name by prefab identity.");

    Console.WriteLine("PieceSelectionCache contract: 5 assertions passed.");
  }
}
