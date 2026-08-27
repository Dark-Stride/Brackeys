using UnityEngine;

public static class WorldSeedManager
{
    public static string currentSeed = "GMTK2024";
    public static int seedID;

    public static void Initialize()
    {
        seedID = currentSeed.GetHashCode();
        Random.InitState(seedID);
    }
}
