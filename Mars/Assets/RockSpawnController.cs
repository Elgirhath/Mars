using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RockSpawnController : MonoBehaviour {
    public float distance;
    public int density;
    public GameObject prefab;
    
    private List<GameObject> rocks = new List<GameObject>();
    private float spawnFOV = 180f; // Items will be spawned in front of the player and will disappear behind
    private PlayerController player;

    private void Start() {
        player = PlayerController.instance;
    }

    // Update is called once per frame
    void Update() {
        if (rocks.Count < density) {
            SpawnRock();
        }

        foreach (var rock in rocks.ToList()) {
            Vector3 dist2dVec = player.transform.position - rock.transform.position;
            dist2dVec.y = 0f;
            float dist2d = dist2dVec.magnitude;
            
            if (dist2d > distance + 1e-7) {
                RemoveRock(rock);
            }
        }
    }

    void SpawnRock() {
        float angle = Random.Range(0f, spawnFOV);
        angle -= spawnFOV / 2f;
        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.up) * player.transform.forward;
        Vector3 pos3d = dir.normalized * distance + player.transform.position;
        pos3d.y = Terrain.activeTerrain.SampleHeight(pos3d);
        
        GameObject rock = Instantiate(prefab, pos3d, Quaternion.identity, transform);
        rocks.Add(rock);
    }

    void RemoveRock(GameObject rock) {
        rocks.Remove(rock);
        Destroy(rock);
    }
}
