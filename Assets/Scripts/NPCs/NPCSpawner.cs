using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("NPC Spawning")]
    [SerializeField] private int maxNpcs;
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private List<NPCContents> npcList = new();
    [SerializeField] private List<NPCController> npcActive = new();
    [SerializeField] private Transform player;

    [Header("Movement")]
    public NPCPath[] paths;
    public Transform[] spawnPoints;

    /*private void Start()
    {
        SpawnNPCs();
    }
    public void SpawnNPCs()
    {
        //Runs a for loop for the max num of NPCs to spawn
        for(int i = 0; i < maxNpcs; i++)
        {
            //Create new NPC Object
            var npc = npcPrefab.GetComponent<NPCController>();

            //Gets the NPC from the List
            npc.contents = npcList[i];
            int length = spawnPoints.Length;

            //If the NPC Path and SpawnPoints are null | Randomnly assign
            if (npcList[i].npcPath == null && npcList[i].npcSpawn == null)
            {
                if (length == 1)
                {
                    npcList[i].npcSpawn = spawnPoints[0];
                    npcList[i].npcPath = paths[0];
                }
                else
                {
                    int rng = Random.Range(0, length);
                    npcList[i].npcSpawn = spawnPoints[rng];
                    npcList[i].npcPath = paths[rng];
                }
            }

            //Set the player position for the NPC
            npc.player = player;

            //Set tits first target
            npc.currentTarget = npc.contents.npcPath.points[1];

            //Spawn in NPC at the spawn position | Add to list of Active NPCs
            Instantiate(npc, npcList[i].npcSpawn.position, npcList[i].npcSpawn.rotation);
            npcActive.Add(npc);
        } 
    }*/
}
