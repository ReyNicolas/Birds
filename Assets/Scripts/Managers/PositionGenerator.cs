using System.Xml.Schema;
using UnityEngine;

public class PositionGenerator
{
    float margin = 1f;
    float xMin, xMax;
    float yMin, yMax;
    Camera mainCamera;

    public void SetDimension()
    {
        mainCamera = Camera.main;

        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        xMin = mainCamera.transform.position.x - cameraWidth / 2;
        xMax = mainCamera.transform.position.x + cameraWidth / 2;

        yMin = mainCamera.transform.position.y - cameraHeight / 2;
        yMax = mainCamera.transform.position.y + cameraHeight / 2;
    }

    public Vector2 ReturnABorderPosition()
    {
        var num = Random.Range(1, 5);

        switch (num)
        {
            case 1:
                return new Vector2(xMin, Random.Range(yMin + margin, yMax - margin));
            case 2:
                return new Vector2(xMax, Random.Range(yMin + margin, yMax - margin));
            case 3:
                return new Vector2(Random.Range(xMin + margin, xMax - margin), yMin);
            default:
                return new Vector2(Random.Range(xMin + margin, xMax - margin), yMax);
        }        
    }

    public Vector2 ReturnABorderPositionToMove(Vector2 vectorBorderStartPosition)
    {
        if (vectorBorderStartPosition.x == xMin) return new Vector2(xMax + margin, Random.Range(yMin + margin, yMax - margin));
        if (vectorBorderStartPosition.x == xMax) return new Vector2(xMin - margin, Random.Range(yMin + margin, yMax - margin));
        if (vectorBorderStartPosition.y == yMin) return new Vector2(Random.Range(xMax + margin, xMax - margin), yMax + margin);
        return new Vector2(Random.Range(xMin + margin, xMax - margin), yMin - margin);
    }
    public void AssignPosition(GameObject gameObject)
    {
        gameObject.transform.position = ReturnPosition();
    }
    Vector2 ReturnPosition()
    {
        while (true)
        {
            var position = GenerateRandomPosition();
            if (!Physics2D.BoxCast(position, Vector2.one, 0, Vector2.zero, 0.5f)) return position;
        }
    }
    Vector2 GenerateRandomPosition()
    {
        var position = mainCamera.transform.position;
        return new Vector2(position.x + Random.Range(xMin + margin, xMax - margin), position.y + Random.Range(yMin + margin, yMax - margin));
    }
}
