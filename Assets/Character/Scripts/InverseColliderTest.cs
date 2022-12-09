using System.Linq;
using UnityEngine;

public class InverseColliderTest : MonoBehaviour
{
    [SerializeField]
    private float colliderSize;

    private bool isActivated = false;
    private GameObject Cylinder;
    private GameObject Quad;

    private void OnTriggerEnter(Collider c)
    {
        if (!isActivated && c.gameObject.CompareTag("Player"))
        {
            isActivated = true;
            CreateInverseCollider();
        }
    }

    private void CreateInverseCollider()
    {
        // Cylinderの生成
        Cylinder = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // MeshColliderの作成
        Quad = GameObject.CreatePrimitive(PrimitiveType.Cube);

        Cylinder.transform.position = transform.position;
        Quad.transform.position = transform.position;

        Cylinder.transform.SetParent(transform);
        Quad.transform.SetParent(transform);

        Cylinder.transform.localScale = new Vector3(13.5f, colliderSize, 24.5f);
        Quad.transform.localScale = new Vector3(14f, 10f, 0.5f);

        // Colliderオブジェクトの描画は不要なのでRendererを消す
        Destroy(Cylinder.GetComponent<MeshRenderer>());
        Destroy(Quad.GetComponent<MeshRenderer>());

        // 元々存在するColliderを削除
        Collider[] cylinderColliders = Cylinder.GetComponents<Collider>();

        for (int i = 0; i < cylinderColliders.Length; i++)
        {
            Destroy(cylinderColliders[i]);
        }

        // メッシュの面を逆にしてからMeshColliderを設定
        var mesh1 = Cylinder.GetComponent<MeshFilter>().mesh;
        mesh1.triangles = mesh1.triangles.Reverse().ToArray();
        Cylinder.AddComponent<MeshCollider>();

        Quad.layer = 12;
    }
}