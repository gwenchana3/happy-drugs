using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public class ConeController : MonoBehaviour
{
    public Material mat;
    public bool check;
    public float Hardness;
    public float ConeSize;


    public Vector3 deb;
    public Vector3 deb2;
    void Update()
    {
#if UNITY_EDITOR
        Shader.SetGlobalFloat("_HardnessLight", Hardness);
        Shader.SetGlobalFloat("_ConeMul", ConeSize);
#endif

        if (check && Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
        {
            Shader.SetGlobalVector("_Pos", hit.point);
            Shader.SetGlobalVector("_LPos", transform.position);

            deb = hit.point;
            deb2 = transform.position;
        }

        else
        {
            Shader.SetGlobalVector("_Pos", new Vector3(100,-100,100));
            Shader.SetGlobalVector("_LPos", new Vector3(100, -100, 100));
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(new Ray(transform.position, transform.forward));
        Gizmos.DrawSphere(deb, .1f);
    }
}
