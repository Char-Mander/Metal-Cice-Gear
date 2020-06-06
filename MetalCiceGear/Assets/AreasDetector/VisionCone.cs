using UnityEngine;
using System.Collections;

public class VisionCone : MonoBehaviour {

	public float angle = 30;
	public Color color = Color.blue;
	Projector projector;

	// Use this for initialization
	void Start () {
		projector = GetComponent<Projector>();
		projector.material = Instantiate<Material>(projector.material);
	}
	
	// Update is called once per frame
	void Update () {
		projector.material.SetFloat("_Angle",angle);
		projector.material.SetFloat("_MaxHeight",this.transform.position.y);
		projector.material.SetColor("_Color",color);
	}
}
