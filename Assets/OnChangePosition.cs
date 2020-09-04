using UnityEngine;

public class OnChangePosition : MonoBehaviour
{
    public PolygonCollider2D hole2DCollider;
    public PolygonCollider2D ground2DCollider;
    public MeshCollider GeneratedMeshCollider;
    public Collider groundCollider;
    private Mesh generatedMesh;
    public float initialScale = 0.5f;

    private void Start()
    {
        //忽略障碍物和生成的地面网格的碰撞
        GameObject[] allGOS = FindObjectsOfType(typeof(GameObject)) as GameObject[];

        foreach(var item in allGOS)
        {
            Physics.IgnoreCollision(item.GetComponent<Collider>(), GeneratedMeshCollider, true);
        } 
    }

    public void FixedUpdate()
    {
         if(transform.hasChanged)
        {
            transform.hasChanged = false;
            hole2DCollider.transform.position = new Vector2(transform.position.x, transform.position.z);
            hole2DCollider.transform.localScale = transform.localScale * initialScale;
            MakeHole();
            Make3DMeshCollider();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        Physics.IgnoreCollision(other, groundCollider, true);
        Physics.IgnoreCollision(other, GeneratedMeshCollider, false);

    }

    private void OnTriggerExit(Collider other)
    {
        Physics.IgnoreCollision(other, groundCollider, false);
        Physics.IgnoreCollision(other, GeneratedMeshCollider, true);
    }


    private void MakeHole()
    {
        Vector2[] points = hole2DCollider.GetPath(0);

        for(var i = 0; i < points.Length; i++)
        {
            //points[i] += (Vector2)hole2DCollider.transform.position;
            points[i] = hole2DCollider.transform.TransformPoint(points[i]);
        }

        ground2DCollider.pathCount = 2;
        ground2DCollider.SetPath(1, points);

    }

    private void Make3DMeshCollider()
    {
        if (null != generatedMesh)
            Destroy(generatedMesh);

        generatedMesh = ground2DCollider.CreateMesh(true, true);
        GeneratedMeshCollider.sharedMesh = generatedMesh;
    }


}
