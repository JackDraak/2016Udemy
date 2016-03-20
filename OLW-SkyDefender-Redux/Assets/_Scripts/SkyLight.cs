using UnityEngine;
using System.Collections;

public class SkyLight : MonoBehaviour {

	void Start () {
		Vector4 skyColour = new Vector4 (1f, 0.957f, 0.839f, 1f); //  FF F4 D6 FF 
		GameObject lightGameObject = new GameObject("SkyLight");
        Light lightComp = lightGameObject.AddComponent<Light>();
		lightComp.color = skyColour;
        lightComp.type = LightType.Directional;

		lightComp.intensity = 0.86f;
		lightComp.bounceIntensity = 1f;
		lightComp.shadowStrength = 1f;
		lightComp.shadowBias = 0.05f;
		lightComp.shadowNormalBias = 0.4f;
		lightComp.shadowNearPlane = 0.2f;
		lightComp.cookieSize = 10f;

        lightGameObject.transform.position = new Vector3(0, 5, 0);
	}
}
