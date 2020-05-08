using System.Collections;
using System.Linq;
using Assets.Air;
using UnityEngine;

namespace Assets.Scripts.RandomSpawning
{
    public class RockSpawnController : MonoBehaviour
    {
        public float radius;
        public float density;
        public float destroyCheckFrequency;

        public GameObject prefab;

        private Player.Player player;
        private Vector3 lastPlayerPosition;

        private Vector3 moveVector
        {
            get
            {
                var vector = player.transform.position - lastPlayerPosition;
                vector.y = 0f;
                return vector;
            }
        }

        private void Start()
        {
            player = Player.Player.instance;
        }

        private void Update()
        {
            var spawnObjectAmount = GetNumberOfObjectsToSpawn();
            for (int i = 0; i < spawnObjectAmount; i++)
            {
                Spawn();
            }

            lastPlayerPosition = player.transform.position;
        }

        // Check this wiki page for explanation: https://github.com/Elgirhath/Mars/wiki/Random-spawn-objects
        private void Spawn()
        {
            float spawnFov = GetSpawnFov();
            var spawnPosition = GenerateSpawnPosition(spawnFov);

            if (Random.value > GetSpawnProbabilityInPoint(spawnPosition, spawnFov)) return;

            GameObject rock = Instantiate(prefab, spawnPosition, Quaternion.identity, transform);
            StartCoroutine(CheckForDestroy(rock));
        }

        private int GetNumberOfObjectsToSpawn()
        {
            var spawnFov = GetSpawnFov();
            var randomSample = GenerateSpawnPosition(spawnFov);
            return Mathf.CeilToInt(GetSpawnProbabilityInPoint(randomSample, spawnFov));
        }


        private float GetSpawnFov()
        {
            float moveDistance = moveVector.magnitude;
            float sinBeta = Mathf.Sqrt(1f - 0.25f * Mathf.Pow(moveDistance / radius, 2));
            return Mathf.Asin(sinBeta) * 2f;
        }

        private Vector3 GenerateSpawnPosition(float spawnFov)
        {
            float angle = Random.Range(0f, spawnFov) - spawnFov / 2f;
            Vector3 spawnDirection = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.up) * moveVector.normalized;
            Vector3 spawnPosition = spawnDirection.normalized * radius + player.transform.position;
            spawnPosition.y = Terrain.activeTerrain.SampleHeight(spawnPosition);

            return spawnPosition;
        }

        private float GetDensityInPoint(Vector3 point)
        {
            var zones = Zone.GetZonesInPoint<SpawnControlZone>(point).Where(zone => zone.spawnController == this);
            float densityInPoint = zones.Any() ? zones.Min(x => x.density) : density;
            return densityInPoint;
        }

        private float GetSpawnProbabilityInPoint(Vector3 point, float spawnFov)
        {
            float densityInPoint = GetDensityInPoint(point);
            float newArea = radius * radius * (Mathf.PI - spawnFov + Mathf.Sin(spawnFov));
            return Mathf.Clamp(newArea * densityInPoint, 0f, Mathf.Infinity);
        }

        private IEnumerator CheckForDestroy(GameObject spawnObject)
        {
            for (;;)
            {
                if (spawnObject == null)
                    yield break;

                Vector3 differenceVector3 = spawnObject.transform.position - player.transform.position;
                differenceVector3.y = 0f;
                float distance2d = differenceVector3.magnitude;

                if (distance2d > radius)
                {
                    Destroy(spawnObject);
                    yield break;
                }
                yield return new WaitForSeconds(destroyCheckFrequency);
            }
        }
    }
}