using System.Collections;
using System.Linq;
using UnityEngine;

public class RockSpawnController : MonoBehaviour {
    public float radius;
    public float density;
    public float destroyCheckFrequency;

    public GameObject prefab;
    
    private Player player;
    private Vector3 lastPlayerPos;

    private void Start() {
        player = Player.instance;
    }

    private void Update() {
        SpawnRock();

        lastPlayerPos = player.transform.position;
    }

    // Check this wiki page for explanation: https://github.com/Elgirhath/Mars/wiki/Random-spawn-objects
    private void SpawnRock()
    {
        Vector3 moveVector = player.transform.position - lastPlayerPos;
        moveVector.y = 0f;

        float moveDistance = moveVector.magnitude;
        float sinBeta = Mathf.Sqrt(1f - 0.25f * Mathf.Pow(moveDistance / radius, 2));
        float alpha = Mathf.Asin(sinBeta) * 2f;
        float newArea = radius * radius * (Mathf.PI - alpha + Mathf.Sin(alpha));
        float probability = Mathf.Clamp01(newArea * density);

        if (Random.value > probability) return;

        float angle = Random.Range(0f, alpha) - alpha / 2f;

        Vector3 dir = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.up) * moveVector.normalized;
        Vector3 pos3d = dir.normalized * radius + player.transform.position;
        pos3d.y = Terrain.activeTerrain.SampleHeight(pos3d);

        var zones = Zone.GetZonesInPoint<SpawnControlZone>(pos3d);
        float multiplier = zones.Any() ? zones.Min(x => x.multiplier) : 1f;
        if (Random.value > multiplier) return;

        GameObject rock = Instantiate(prefab, pos3d, Quaternion.identity, transform);
        StartCoroutine(CheckForDestroy(rock));
    }

    private IEnumerator CheckForDestroy(GameObject rock)
    {
        for (;;)
        {
            if (rock == null)
                yield break;

            Vector3 differenceVector3 = rock.transform.position - player.transform.position;
            differenceVector3.y = 0f;
            float distance2d = differenceVector3.magnitude;

            if (distance2d > radius)
            {
                Destroy(rock);
                yield break;
            }
            yield return new WaitForSeconds(destroyCheckFrequency);
        }
    }
}