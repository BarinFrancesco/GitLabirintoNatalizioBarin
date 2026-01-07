using UnityEngine;

public class MiniMapFullView : MonoBehaviour
{
    [SerializeField] private int lato ;       
    [SerializeField] private Camera miniMapCamera; 
    [SerializeField] private float altezzaExtra; 

    void Start()
    {
        if (miniMapCamera == null)
            miniMapCamera = GetComponent<Camera>();

        float centroX = (lato - 1) * 10 / 2f;
        float centroZ = (lato - 1) * 10 / 2f;


        float altezza = (lato * 10 / 2f) + altezzaExtra;


        miniMapCamera.transform.position = new Vector3(centroX, altezza, centroZ);
        miniMapCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);


        miniMapCamera.orthographic = true;
        miniMapCamera.orthographicSize = (lato * 10 / 2f) + altezzaExtra; // metà lato in unità + margine
    }
}
