using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recovery : MonoBehaviour
{
    public List<GameObject> Origin_Objects;

    public List<GameObject> instance_Objects;

    List<Vector3> positions = new();
    List<Quaternion> rotations = new();

    // Start is called before the first frame update
    void Start()
    {
        foreach (var obj in instance_Objects)
        {
            positions.Add(obj.transform.position);
            rotations.Add(obj.transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRecovery()
    {
        for(int i = 0; i < instance_Objects.Count; i++)
        {
            Destroy(instance_Objects[i]);
            instance_Objects[i] = Instantiate (Origin_Objects[i], positions[i], rotations[i]);
        }
        //Debug.Log("recovery");
    }
}
