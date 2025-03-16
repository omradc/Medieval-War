using UnityEngine;

public class DrawLineRenderer : MonoBehaviour
{
    public LineRenderer square;
    public LineRenderer formationIndicator;
    private Vector2 startPoint;
    private Vector2 endPoint;
    const float lineWidth = 300;
    [SerializeField] Camera cam;
    void Start()
    {
        InitializeLineRenderer(square, 4, true);
    }
    void InitializeLineRenderer(LineRenderer lineRenderer, int positionCount, bool loop)
    {
        lineRenderer.positionCount = positionCount;
        lineRenderer.loop = loop; // Çizgiyi döngüsel hale getir
    }
    #region Square
    public void InitializeDrawSquare()
    {
        startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition); // İlk fare pozisyonunu al
    }
    public void DrawSquare()
    {
        endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Şu anki fare pozisyonunu al
        Vector3[] corners = new Vector3[5];

        corners[0] = new Vector3(startPoint.x, startPoint.y, 0);
        corners[1] = new Vector3(endPoint.x, startPoint.y, 0);
        corners[2] = new Vector3(endPoint.x, endPoint.y, 0);
        corners[3] = new Vector3(startPoint.x, endPoint.y, 0);
        corners[4] = corners[0]; // Başlangıç noktasına geri dön

        square.SetPositions(corners); // Kareyi çiz
    }
    public void ResetSquare()
    {
        Vector3[] corners = new Vector3[5];

        corners[0] = Vector3.zero;
        corners[1] = Vector3.zero;
        corners[2] = Vector3.zero;
        corners[3] = Vector3.zero;
        corners[4] = Vector3.zero;

        square.SetPositions(corners); // Kareyi sıfırla
    }
    #endregion

    public void DrawFormationIndicator(/*GameObject targetImage,*/ Camera cam, float lineWidth = 500)
    {
        formationIndicator.positionCount = 2;
        formationIndicator.SetPosition(0, (Vector2)cam.transform.position);
        formationIndicator.SetPosition(1, (Vector2)cam.ScreenToWorldPoint(Input.mousePosition));
        formationIndicator.widthMultiplier = cam.orthographicSize / lineWidth;
    }
    public void DynamicLineRendererWidthness()
    {
        square.widthMultiplier = cam.orthographicSize / lineWidth;
    }


}
