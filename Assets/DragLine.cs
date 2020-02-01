using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragLine : MonoBehaviour
{
    private LineRenderer line;
    private BullMove player;
    private Camera mainCamera;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<BullMove>();
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (TouchHandler.dragging) {
            /*Ray startRay = mainCamera.ScreenPointToRay(BullMove.playerScreenPosition),
                endRay = mainCamera.ScreenPointToRay(BullMove.playerScreenPosition + TouchHandler.dragForceInScreenSpace);
            float rayDistance = mainCamera.nearClipPlane;
            */
            line.SetPosition(0, player.transform.position);
            line.SetPosition(1, CameraOrbit.projectedReference);
            
        } else
        {
            line.SetPosition(1, transform.position);
            line.SetPosition(0, transform.position);
        }
    }
}
