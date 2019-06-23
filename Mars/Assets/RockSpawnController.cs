using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RockSpawnController : MonoBehaviour {
    public float distance;
    public int density;
    
    [Tooltip("FOV at which rocks can be spawned")]
    public float spawnFOV = 120f;
    
    public GameObject prefab;
    
    private List<GameObject> rocks = new List<GameObject>();
    private PlayerController player;
    private Vector3 lastPlayerPos;
    private Vector3 playerMovement = Vector3.zero;

    private void Start() {
        player = PlayerController.instance;
    }

    // Update is called once per frame
    void Update() {
        if (rocks.Count < density) {
            SpawnRock();
        }

        Validate();
        foreach (var rock in rocks.ToList()) {
            Vector3 dist2dVec = player.transform.position - rock.transform.position;
            dist2dVec.y = 0f;
            float dist2d = dist2dVec.magnitude;
            
            if (dist2d > distance + 1e-7) {
                Destroy(rock);
            }
        }

        lastPlayerPos = player.transform.position;
    }

    void SpawnRock() {
        /*
         * Spawns a rock in a distance, in front of a player (in terms of movement, not camera view)
         */
        
        float angle = Random.Range(0f, spawnFOV);
        angle -= spawnFOV / 2f;
        Vector3 moveVector = player.transform.position - lastPlayerPos;
        moveVector.y = 0f;
        if (moveVector.magnitude > 1e-8)
            playerMovement = moveVector.normalized;
        else if (playerMovement.magnitude < 1e-8) {
            playerMovement = new Vector3(Random.value, 0f, Random.value).normalized;
        }
        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.up) * playerMovement;
        Vector3 pos3d = dir.normalized * distance + player.transform.position;
        pos3d.y = Terrain.activeTerrain.SampleHeight(pos3d);
        
        GameObject rock = Instantiate(prefab, pos3d, Quaternion.identity, transform);
        rocks.Add(rock);
    }

    void Validate() {
        /*
         * Checks if any of the rocks were destroyed (eg. by collecting it) and removes from the list
         */
        
        List<GameObject> rocksCopy = rocks.ToList();
        foreach (var rock in rocksCopy) {
            if (rock == null) {
                rocks.Remove(rock);
            }
        }
    }
}
