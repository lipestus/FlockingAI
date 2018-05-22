using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAI : MonoBehaviour {

    float speed;
    private bool goOppositeDir = false;
    private float fishSensor = 1f;
    private Animator anim;
    private float CycOffset;
	// Use this for initialization
	void Start () {

        anim = GetComponent<Animator>();
        speed = Random.Range(FlockManager.instance.minSpeed, FlockManager.instance.maxSpeed);
        CycOffset = Random.Range(0.0f, 1.0f);
        anim.SetFloat("CycleOffset", CycOffset);
	}
	
	// Update is called once per frame
	void Update () {

        // draw a rectangular bound to prevent fish running away
        Bounds bounds = new Bounds(FlockManager.instance.transform.position, FlockManager.instance.dirLimit * 2);

        RaycastHit hit = new RaycastHit();
        Vector3 direction = Vector3.zero;
        

        if (!bounds.Contains(transform.position))
        {
            goOppositeDir = true;
            direction = FlockManager.instance.transform.position - transform.position;  
        }
        else if (Physics.Raycast(transform.position, this.transform.forward * fishSensor, out hit))
        {
            Debug.DrawRay(transform.position, this.transform.forward * fishSensor, Color.red);
            goOppositeDir = true;
            // get the predicted vector.z and the raycast normal to return the outgoing vector
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }
        else
        {
            goOppositeDir = false;
        }

        if(goOppositeDir)
        {
            // Tell to fishes come back to the flock manager position
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), FlockManager.instance.rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 10)
                speed = Random.Range(FlockManager.instance.minSpeed, FlockManager.instance.maxSpeed);

            if (Random.Range(0, 100) < 25)
                FlockRules();
        }

        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void FlockRules()
    {
        GameObject[] gos;
        gos = FlockManager.instance.allFishes;

        Vector3 vCentre = Vector3.zero;
        Vector3 vAvoid = Vector3.zero;
        float globalSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;

        foreach(GameObject go in gos)
        {
            if(go != this.gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                if(nDistance <= FlockManager.instance.neighbourDist)
                {
                    vCentre += go.transform.position;
                    groupSize++;

                    if(nDistance < 1.0f)
                    {
                        vAvoid = vAvoid + (this.transform.position - go.transform.position);
                    }

                    FishAI goGameObj = go.GetComponent<FishAI>();
                    globalSpeed = globalSpeed + goGameObj.speed;
                }
            }
        }

        if(groupSize > 0)
        {
            vCentre = vCentre / groupSize + (FlockManager.instance.goalPosition - this.transform.position);
            speed = globalSpeed / groupSize;

            Vector3 direction = (vCentre + vAvoid) - transform.position;
            if(direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), FlockManager.instance.rotationSpeed * Time.deltaTime);
            }
        }
    }
}
