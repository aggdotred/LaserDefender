using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemyPrefab;
    public float height = 10f;
    public float width = 5f;
    public float speed = 5.0f;
    public float spawnDelay = 0.5f;

    private float xMin;
    private float xMax;
    private bool movingRight = true;
	

	void Start () {

        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xMin = leftMost.x;
        xMax = rightMost.x;

        SpawnUntilFull();
    }

    public void SpawnEnemies()
    {
        foreach (Transform child in transform)
        {
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = child;
        }
    }

    void SpawnUntilFull()
    {
        Transform freePosition = NextFreePosition();
        if (freePosition)
        {
            GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = freePosition;
        }
        if (NextFreePosition())
        {
            Invoke("SpawnUntilFull", spawnDelay);
        }
        
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
    }
	
	
	void Update () {
        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        } else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        
        // check if formation is going off screen
        float rightEdgeOfFormation = transform.position.x + (0.5f * width);
        float leftEdgeOfFormation = transform.position.x - (0.5f * width);

        if (leftEdgeOfFormation < xMin)
        {
            movingRight = true;
        } else if (rightEdgeOfFormation > xMax)
        {
            movingRight = false;
        }

        if (AllMembersDead())
        {
            Debug.Log("Empty formation");
            SpawnUntilFull();

        }
    }

    Transform NextFreePosition()
    {
        foreach (Transform childPositionGameObject in transform)
        {
            if (childPositionGameObject.childCount == 0)
            {
                return childPositionGameObject;
            }
        }
        return null;
    }

    bool AllMembersDead()
    {
        foreach(Transform childPositionGameObject in transform)
        {
            if (childPositionGameObject.childCount > 0){
                return false;
            }
        }
        return true;
    }
}
