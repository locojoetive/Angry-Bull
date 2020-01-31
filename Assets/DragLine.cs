using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragLine : MonoBehaviour
{
    private LineRenderer line;
    private PlayerMoveAndRotate player;
    private Camera mainCamera;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoveAndRotate>();
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (PlayerMoveAndRotate.dragging) {
            Plane dragLinePlane = new Plane(-mainCamera.transform.forward, transform.position);
            Ray startRay = mainCamera.ScreenPointToRay(PlayerMoveAndRotate.lastCapturedMovingTouchPosition),
                endRay = mainCamera.ScreenPointToRay(PlayerMoveAndRotate.playerScreenPosition);
            float rayDistance = mainCamera.nearClipPlane;
            line.SetPosition(1, startRay.GetPoint(rayDistance));
            line.SetPosition(0, endRay.GetPoint(rayDistance));
            Vector2 worldToScreenDirection = PlayerMoveAndRotate.lastCapturedMovingTouchPosition - PlayerMoveAndRotate.playerScreenPosition;
            worldToScreenDirection.x = (worldToScreenDirection.x) / (0.5f * mainCamera.scaledPixelWidth);
            worldToScreenDirection.y = (worldToScreenDirection.y) / (0.5f * mainCamera.scaledPixelHeight);

            Debug.Log(worldToScreenDirection);
        } else
        {
            line.SetPosition(1, transform.position);
            line.SetPosition(0, transform.position);
        }
    }
}
