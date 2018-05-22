using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour {

    public static FlockManager instance;

    #region Fish Spawn Manager Variable
    [Header("Animal Spawn Manager")]
    public GameObject animal;
    public Vector3 dirLimit = new Vector3(5, 5, 5);
    public int fishNumber;
    public GameObject[] allFishes;
    public Vector3 goalPosition;
    #endregion

    #region Fish Settings Variables
    [Header("Animal Settings")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(0.0f, 20.0f)]
    public float neighbourDist;
    [Range(0.0f, 10.0f)]
    public float rotationSpeed;
    #endregion

    private void Awake()
    {
        MakeInstance();
    }

    // Use this for initialization
    void Start () {

        allFishes = new GameObject[fishNumber];
        for(int i = 0; i < fishNumber; i++)
        {
            Vector3 pos = this.transform.position + 
                new Vector3(Random.Range(-dirLimit.x, dirLimit.x), Random.Range(-dirLimit.y, dirLimit.y), Random.Range(-dirLimit.z, dirLimit.z));
            allFishes[i] = (GameObject)Instantiate(animal, pos, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (Random.Range(0, 100) < 10)
            goalPosition = this.transform.position + new Vector3(Random.Range(-dirLimit.x, dirLimit.x), Random.Range(-dirLimit.y, dirLimit.y), Random.Range(-dirLimit.z, dirLimit.z));
    }

    void MakeInstance()
    {
        if (instance == null)
            instance = this;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, neighbourDist);
    }
}
