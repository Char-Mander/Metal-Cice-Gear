using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum keyColor { BLUE, YELLOW }

[System.Serializable]
public class Key : MonoBehaviour
{
    public keyColor kColor;

    private void Start()
    {
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in childRenderers)
        {
            renderer.material.color = GetMyKeyColor(kColor);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.GetComponentInParent<Player>() != null)
        {
            Player.instance.AddKey(kColor);
            Destroy(this.gameObject);
        }
    }

    public static Color GetMyKeyColor(keyColor color) {
        switch(color){
            case keyColor.BLUE:
                return Color.blue;
                break;
            case keyColor.YELLOW:
                return Color.yellow;
                break;
        }
        return Color.white;
    }

}
