using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LineIntersectionDrawer : MonoBehaviour
{
    public float aX;
    public float aY;
    public float bX;
    public float bY;
    public float cX;
    public float cY;
    public float dX;
    public float dY;
    [Header("Компоненты для отрисовки линий")] 
    public LineRenderer lineRenderer1; 
    public LineRenderer lineRenderer2;
    [Header("Спрайт для обозначения точки пересечения")]
    public GameObject intersectionMarker;
    [Header("Пределы отрисовки линий по оси X")]
    public float xMin = -10f;
    public float xMax = 10f;

    public DragObject _dragPointControllerPrefab;
    DragObject _dragPointA;
    DragObject _dragPointB;
    DragObject _dragPointC;
    DragObject _dragPointD;
    void Start()
    {
        _dragPointA = Instantiate(_dragPointControllerPrefab);
        _dragPointB = Instantiate(_dragPointControllerPrefab);
        _dragPointC = Instantiate(_dragPointControllerPrefab);
        _dragPointD = Instantiate(_dragPointControllerPrefab);

        _dragPointA.transform.position = Vector3.left;
        _dragPointB.transform.position = Vector3.right;
        _dragPointC.transform.position = Vector3.up;
        _dragPointD.transform.position = Vector3.down;
    }
    private void Update()
    {
        aX = _dragPointA.transform.position.x;
        aY = _dragPointA.transform.position.y;
        bX = _dragPointB.transform.position.x;
        bY = _dragPointB.transform.position.y;
        cX = _dragPointC.transform.position.x;
        cY = _dragPointC.transform.position.y;
        dX = _dragPointD.transform.position.x;
        dY = _dragPointD.transform.position.y;
        DrawLinesAndIntersection();
    }
    public void DrawLinesAndIntersection()
    {
        lineRenderer1.positionCount = 2;
        lineRenderer2.positionCount = 2;

        lineRenderer1.SetPosition(0, new(aX,aY,0));
        lineRenderer1.SetPosition(1, new(bX,bY,0));

        lineRenderer2.SetPosition(0, new(cX, cY, 0));
        lineRenderer2.SetPosition(1, new(dX, dY, 0));


        if (IntersectionUtil.TryGetIntersection(new Vector2(aX,aY), new Vector2(bX, bY), new Vector2(cX, cY), new Vector2(dX, dY), out Vector2 intersection))
        {
            lineRenderer2.SetPosition(1, intersection);
            if (intersectionMarker != null)
            {
                intersectionMarker.transform.position = intersection;
                intersectionMarker.SetActive(true);
            }
        }
        else
        {
            if (intersectionMarker != null)
            {
                intersectionMarker.SetActive(false);
            }
            Debug.Log("Линии параллельны или совпадают – точки пересечения нет.");
        }
    }
}