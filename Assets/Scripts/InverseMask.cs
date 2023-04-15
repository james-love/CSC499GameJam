using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class InverseMask : Image
{
    private Material inverseMaterial;
    public override Material materialForRendering
    {
        get
        {
            inverseMaterial ??= new(base.materialForRendering);
            inverseMaterial.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            return inverseMaterial;
        }
    }
}
