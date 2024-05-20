using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class BlackHole : MonoBehaviour
{
    public Transform BH;
    public float rad = 3f;
    public float blackR1 = 1f; // радиус круга внутри которого черный цвет
    public float blackR2 = 4f; // радиус круга с которого начинается блендинг фона (до круга с радиусом blackR1)
    public float ior = 0.38f; // коэффициент преломления

    private Material material;

    // Creates a private material used to the effect
    void OnEnable()
    {
        material = new Material(Shader.Find("BlackHole"));
    }


    // Постпроцесс
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        //Находим позицию черной дыры в экранных координатах
        Vector2 screenPos = new Vector2(
            this.GetComponent<Camera>().WorldToScreenPoint(BH.position).x / this.GetComponent<Camera>().pixelWidth,
            1 - this.GetComponent<Camera>().WorldToScreenPoint(BH.position).y / this.GetComponent<Camera>().pixelHeight);


        float distance = Vector3.Distance(BH.transform.position, this.transform.position); 
        material.SetFloat("_dist", distance);
        material.SetVector("screenPos", screenPos);
        material.SetFloat("IOR", ior);
        material.SetFloat("black_r1", blackR1/distance);
        material.SetFloat("black_r2", blackR2/distance);
        material.SetFloat("rad", rad);

        Graphics.Blit(source, destination, material);
    }
}
