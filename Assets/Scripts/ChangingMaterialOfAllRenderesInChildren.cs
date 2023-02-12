using UnityEngine;

public class ChangingMaterialOfAllRenderesInChildren : MonoBehaviour
{
    [SerializeField] private GameObject GameObjectWithRenderers;
    private Renderer[] AllRenderersOfChildren;

    private void Awake()
    {
        AllRenderersOfChildren = GameObjectWithRenderers.GetComponentsInChildren<Renderer>();
    }

    public void ChangeMaterialOfAllRenderesInChildren(Material NewMaterial)
    {
        foreach(Renderer oneRenderer in AllRenderersOfChildren)
        {
            oneRenderer.material = NewMaterial;
        }
    }
}