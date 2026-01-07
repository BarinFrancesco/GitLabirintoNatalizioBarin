using UnityEngine;

public class Cella : MonoBehaviour
{
    [SerializeField]
    private GameObject _leftWall;

    [SerializeField]
    private GameObject _rigthtWall;

    [SerializeField]
    private GameObject _frontWall;

    [SerializeField]
    private GameObject _backWall;

    [SerializeField]
    private GameObject _unvisitedBlock;

    public bool IsVisited { get; private set; }

    public void Visita()
    {
        IsVisited = true;
        _unvisitedBlock.SetActive(false);
    }

    public void DisattivaMuroDestra()
    {
        _rigthtWall.SetActive(false);
    }

    public void DisattivaMuroSinistra()
    {
        _leftWall.SetActive(false);
    }

    public void DisattivaMuroDavanti()
    {
        _frontWall.SetActive(false);
    }

    public void DisattivaMuroDietro()
    {
        _backWall.SetActive(false);
    }


    //per modificare la cella nel caso fosse cella Vittoria
    public void ImpostaMateriale(Material materiale)
    {
        if (materiale == null)
            return;

        ApplicaMateriale(_leftWall, materiale);
        ApplicaMateriale(_rigthtWall, materiale);
        ApplicaMateriale(_frontWall, materiale);
        ApplicaMateriale(_backWall, materiale);
        ApplicaMateriale(_unvisitedBlock, materiale);
    }

    private void ApplicaMateriale(GameObject obj, Material materiale)//modifichiamo graficamente la cella per renderla riconoscibile dall'utente
    {
        if (obj == null)
            return;

        Renderer r = obj.GetComponentInChildren<Renderer>();
        if (r != null)
            r.sharedMaterial = materiale;
    }

}
