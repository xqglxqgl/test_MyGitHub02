using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{

    /// <summary>
    /// Normalized
    /// </summary>
    public Vector2 NormalizedDirection { get; private set; }
    private void Update()
    {
        var xAxis = Input.GetAxis("Horizontal");
        var yAxis = Input.GetAxis("Vertical");

        Vector2 dir = new Vector2(xAxis, yAxis);
        dir = dir.normalized;
        this.NormalizedDirection = dir;
    }
}
