using UnityEngine;
using System.Collections;

public class AlertArea : MonoBehaviour {

	public float distance = 2;
	public Color color = Color.blue;
	Projector projector;

	// Use this for initialization
	void Start () {
		projector = GetComponent<Projector>();
		projector.material = Instantiate<Material>(projector.material);
		projector.material.SetFloat("_Angle",360);
	}
	
	// Update is called once per frame
	void Update () {
		projector.orthographicSize = Mathf.Max(0.1f,distance);
		projector.material.SetFloat("_MaxHeight",this.transform.position.y);
		projector.material.SetColor("_Color",color);
	}
}
