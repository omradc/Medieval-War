using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawRectangle : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector2 startPoint;
    private Vector2 endPoint;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 5; // Kare için 4 köşe ve başlangıç noktasına dönüş
        lineRenderer.loop = true; // Çizgiyi döngüsel hale getir
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Sol fare tuşuna basıldığında
        {
            startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition); // İlk fare pozisyonunu al
        }
        if (Input.GetMouseButton(1)) // Fare tuşu basılı tutulduğunda
        {
            endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Şu anki fare pozisyonunu al
            DrawSquare();
        }
        if (Input.GetMouseButtonUp(1))
            ResetSquare();

    }

    void DrawSquare()
    {
        Vector3[] corners = new Vector3[5];

        corners[0] = new Vector3(startPoint.x, startPoint.y, 0);
        corners[1] = new Vector3(endPoint.x, startPoint.y, 0);
        corners[2] = new Vector3(endPoint.x, endPoint.y, 0);
        corners[3] = new Vector3(startPoint.x, endPoint.y, 0);
        corners[4] = corners[0]; // Başlangıç noktasına geri dön

        lineRenderer.SetPositions(corners); // Kareyi çiz
    }
    void ResetSquare()
    {
        Vector3[] corners = new Vector3[5];

        corners[0] = Vector3.zero;
        corners[1] = Vector3.zero;
        corners[2] = Vector3.zero;
        corners[3] = Vector3.zero;
        corners[4] = Vector3.zero;

        lineRenderer.SetPositions(corners); // Kareyi çiz
    }
}
