using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSpawnHandler : MonoBehaviour
{

    public GameObject Arena;
    public static Vector3 LastArenaCenter;
    public static Vector3 PlayerPosition;
    public static bool SpawnNewArena = false;
    public float ArenaCenetrOffset = 70f;

    private List<Vector3> arenaCentersList = new List<Vector3>();
    private Vector3 newArenaCenter;
    


    void Update()
    {
        if (SpawnNewArena)
        {
            SpawnArena();
        }
    }

    private void SpawnArena()
    {
        float xOffset = LastArenaCenter.x - PlayerPosition.x;
        float zOffset = LastArenaCenter.z - PlayerPosition.z;


        if (Mathf.Abs(xOffset) > Mathf.Abs(zOffset))
        {
            if (xOffset > 0)
            {
                newArenaCenter = new Vector3(LastArenaCenter.x - ArenaCenetrOffset, LastArenaCenter.y, LastArenaCenter.z);
            }
            else
            {
                newArenaCenter = new Vector3(LastArenaCenter.x + ArenaCenetrOffset, LastArenaCenter.y, LastArenaCenter.z);
            }
        }
        else
        {
            if (zOffset > 0)
            {
                newArenaCenter = new Vector3(LastArenaCenter.x, LastArenaCenter.y, LastArenaCenter.z - ArenaCenetrOffset);
            }
            else
            {
                newArenaCenter = new Vector3(LastArenaCenter.x, LastArenaCenter.y, LastArenaCenter.z + ArenaCenetrOffset);
            }
        }

        if (!arenaCentersList.Contains(newArenaCenter))
        {
            Instantiate(Arena, newArenaCenter, Arena.transform.rotation);
            arenaCentersList.Add(newArenaCenter);
            SpawnNewArena = false;
        }


    }
}
