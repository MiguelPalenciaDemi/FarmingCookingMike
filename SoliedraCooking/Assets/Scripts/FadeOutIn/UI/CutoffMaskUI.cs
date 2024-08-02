using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;


public class CutoffMaskUI : Image
{
    private static readonly int StencilComp = Shader.PropertyToID("_StencilComp");

    public override Material materialForRendering
    {
        get
        {
            var renderingMaterial = new Material(base.materialForRendering);
            renderingMaterial.SetInt(StencilComp,(int)CompareFunction.NotEqual); //This "invert" the image. It makes mask to work inversely.
            return renderingMaterial;

        }
    }
}
