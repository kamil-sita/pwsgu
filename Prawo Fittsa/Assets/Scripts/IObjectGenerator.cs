using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines interface for object generation
/// </summary>
public abstract class IObjectGenerator : MonoBehaviour
{
    /// <summary>
    /// Generates objects, according to settings of this ObjectGenerator
    /// </summary>
    public abstract void GenerateObjects();
    /// <summary>
    /// Provides a reference to ClickableManager
    /// </summary>
    public abstract void SetManager(ClickableManager cm);
}
