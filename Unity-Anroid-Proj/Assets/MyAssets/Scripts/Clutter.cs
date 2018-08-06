using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Clutter : MonoBehaviour {

    public int ammount;
    [Range(0.00f,1.00f)]
    public float edgeLimit = 0.5f;
    [Range(1,25)]
    public int gridSize = 10;
    [Range(0.00f, 1.00f)]
    public float spawnNoise = 0.01f;
    public int searchLimit = 10;

    [System.Serializable]
    public class ClutterObject
    {
        public GameObject obj;
        [Range(0.00f,1.00f)]
        public float spawnChance;
        [HideInInspector]
        public float rangeStart;
        [Range(-360,360)]
        public int rotMinX = -360;
        [Range(-360, 360)]
        public int rotMaxX = 360;
        [Range(-360, 360)]
        public int rotMinY = -360;
        [Range(-360, 360)]
        public int rotMaxY = 360;
        [Range(-360, 360)]
        public int rotMinZ = -360;
        [Range(-360, 360)]
        public int rotMaxZ = 360;
    }

    public List<ClutterObject> clutterObjects;

    Vector3 size;
    Vector3 center;
    Vector3 cellBounds;

    List<List<GameObject>> cellsZ = new List<List<GameObject>>();
    List<GameObject> cellsX = new List<GameObject>();

    int total = 0, current = 0;

    void Start()
    {
        clutterObjects = clutterObjects.OrderBy(o => o.spawnChance).ToList();
        size = renderer.bounds.extents;
        center = renderer.bounds.center;
        cellBounds = new Vector3(renderer.bounds.size.x / gridSize, 0 + 0.1f, renderer.bounds.size.z / gridSize);
        GetRanges();
	}

    void GetRanges()
    {
        float range = 0, sum = 0;

        for (int i = 0; i < clutterObjects.Count; i++)
        {
            sum += clutterObjects[i].spawnChance;
        }

        for (int i = 0; i < clutterObjects.Count; i++)
        {
            range += 100 * (clutterObjects[i].spawnChance / sum);
            clutterObjects[i].rangeStart = range;
        }

        CreateGrid();
    }

    void CreateGrid()
    {
        GameObject cell = new GameObject();
        cell.AddComponent<BoxCollider>().size = cellBounds * 0.99f;
        cell.AddComponent<ClutterCell>();

        float x = center.x - size.x + (cellBounds.x / 2), y = center.z - size.z + (cellBounds.z / 2);

        for (int j = 0; j < (size.z * 2) / cellBounds.z; j++)
        {
            for (int i = 0; i < (size.x * 2) / cellBounds.x; i++)
            {
                GameObject CC = (GameObject)Instantiate(cell, new Vector3(x, transform.position.y, y), Quaternion.identity);
                CC.GetComponent<ClutterCell>().TestCollision(gameObject);
                cellsX.Add(CC);
                x += cellBounds.x;
                total++;
            }

            cellsZ.Add(cellsX.ToList());
            cellsX.Clear();
            y += cellBounds.z;
            x = center.x - size.x + (cellBounds.x / 2);
        }

        Destroy(cell);
    }

    public void Add()
    {
        current++;

        if (current == total)
        {
            total = 0;
            for (int i = 0; i < ammount; i++)
            {
                Spawn();
            }

            for (int i = 0; i < cellsZ.Count; i++)
            {
                for (int j = 0; j < cellsZ[i].Count; j++)
                {
                    Destroy(cellsZ[i][j]);
                }
            }
        }
    }

    void Spawn()
    {
        GameObject cell = null;
        bool found = false;
        int limit = 0;

        while (!found && limit < searchLimit)
        {
            int randZ = Random.Range(0, cellsZ.Count);
            int randX = Random.Range(0, cellsZ[randZ].Count);

            cell = cellsZ[randZ][randX];

            if (cell.GetComponent<ClutterCell>().state == ClutterCell.State.open)
            {
                found = true;
                cell.rigidbody.detectCollisions = false;
            }
            else
            {
                Destroy(cell);
                cellsZ[randZ].RemoveAt(randX);
                cell = null;
            }

            limit++;
        }

        if (cell == null)
            return;

        Vector3 cellCenter = cell.collider.bounds.center;

        float rndSpawnX = Random.Range(-(cellBounds.x - edgeLimit) + Random.Range(0, spawnNoise), (cellBounds.x - edgeLimit) + Random.Range(0, spawnNoise));
        float rndSpawnZ = Random.Range(-(cellBounds.z - edgeLimit) + Random.Range(0, spawnNoise), (cellBounds.z - edgeLimit) + Random.Range(0, spawnNoise));
        float rndObj = Random.Range(0.00f, 100.0f);

        for (int j = 0; j < clutterObjects.Count; j++)
        {
            if (clutterObjects[j].rangeStart >= rndObj)
            {
                Vector3 rotation = new Vector3();
                rotation.x = AngleClamp(Random.Range(-360, 360), clutterObjects[j].rotMinX, clutterObjects[j].rotMaxX);
                rotation.y = AngleClamp(Random.Range(-360, 360), clutterObjects[j].rotMinY, clutterObjects[j].rotMaxY);
                rotation.z = AngleClamp(Random.Range(-360, 360), clutterObjects[j].rotMinZ, clutterObjects[j].rotMaxZ);

                GameObject cltr = (GameObject)Instantiate(clutterObjects[j].obj, new Vector3(cellCenter.x + rndSpawnX, transform.position.y, cellCenter.z + rndSpawnZ), Quaternion.Euler(rotation));
                break;
            }
        }
    }

    float AngleClamp(float rotation, float min, float max)
    {
        if (rotation < -360)
        {
            rotation = rotation + 360;
        }
        if (rotation > 360)
        {
            rotation = rotation - 360;
        }
        rotation = Mathf.Clamp(rotation, min, max);
        return rotation;
    }
}
