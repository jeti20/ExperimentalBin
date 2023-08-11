using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wire : MonoBehaviour
{
    [SerializeField] Transform startTransform, endTransform;
    [SerializeField] int segemntCount = 10;
    [SerializeField] float totalLength = 10;

    [SerializeField] float totalWeight = 10;
    [SerializeField] float drag = 1;
    [SerializeField] float angularDrag = 1;

    Transform[] segemnts;
    [SerializeField] Transform segmentParent;

    private void Start()
    {   
        segemnts = new Transform[segemntCount]; //creating array with the positions of segments 
        GenerateSegments();
    }

    //creating gizmos based on the segments
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < segemnts.Length; i++)
        {
            Gizmos.DrawWireSphere(segemnts[i].position, 0.1f);
        }
    }

    //Generating segments 
    private void GenerateSegments()
    {
        JoinSegment(startTransform, null, true); //first one segment is not conected to previous because it is first, also kintic
        //first segemnt
        Transform prevTransofrm = startTransform;

        //direction where segments should go
        Vector3 direction = (endTransform.position - startTransform.position);

        //creating segments
        for (int i = 0; i < segemntCount; i++)
        {
            GameObject segment = new GameObject($"segment_{i}"); //creating new segment with name with the number
            segment.transform.SetParent(segmentParent); //assign parent frin which it goes
            segemnts[i] = segment.transform;

            //moving segments between start and end
            Vector3 pos = prevTransofrm.position + (direction / segemntCount);//divaiding the distance between start-end by the amount of segments
            segment.transform.position = pos;   

            JoinSegment(segment.transform, prevTransofrm);

            prevTransofrm = segment.transform; //przypisywanie kolejno segmentom, ze kazdy koljeny wraz z spawnowaniem kolejnego staje sie poprzednim
        }
        JoinSegment(endTransform, prevTransofrm, false);          
    }

    private void JoinSegment(Transform current, Transform connectedTransform, bool isKinetic = false) //start must be kinetic
    {
        Rigidbody rigidbody = current.AddComponent<Rigidbody>();
        rigidbody.isKinematic = isKinetic;
        rigidbody.mass = totalWeight / segemntCount;
        rigidbody.drag = drag;
        rigidbody.angularDrag = angularDrag;
    }
}
