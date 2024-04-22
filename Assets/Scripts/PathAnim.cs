using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAnim : MonoBehaviour
{
    Transform player;
    Transform target;
    [SerializeField]
    DrumShakes drumSeq;
    float t;
    [SerializeField]
    float pathSpeed = 1;
    [SerializeField]
    float pathSize = 5;
    void Start()
    {
        player = transform.GetChild(0);
        target = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        player.position = ControlFunctions.Path(t,pathSpeed, pathSize);
        target.position = ControlFunctions.Path(t + 1f, pathSpeed, pathSize);
        float shake = Mathf.Cos(drumSeq.cameraShake * 500);
        Vector3 targ_pos = target.position + new Vector3(0, shake, 0);
        player.LookAt(targ_pos);
    }
}
