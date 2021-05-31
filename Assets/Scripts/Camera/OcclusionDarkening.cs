using UnityEngine;

public class OcclusionDarkening : MonoBehaviour {

	// How bright areas in shadow should be (roughly - precision issues)
	public float ShadowBrightness = 0.5f;
	// Radius to take samples in
	public float SampleRadius = 10;

	// Resolution of the distance map for occlusion rendering
	public int OcclusionResolution = 1000;
	// Amount of samples to take smoothing the shadows
	public int Samples = 50;
	

	// Material to use for drawing
	Material mat;
	
	// Dimensions of the camera
	// May not actually be the dimensions of the camera
	// Don't try to use this elsewhere
	Vector2 camDims;

	// Direction vectors
	Vector2[] dirs;

	// Sample offset vectors
	Vector2[] sampleOffsets;
	// Sample scales

	// Texture holding the shadows
	RenderTexture shadows;

	GameObject player;

	void Start() {
		// Generate sample positions using vogel's method of generating points on a circle
		sampleOffsets = new Vector2[Samples];
		float a = Mathf.PI * (3 - Mathf.Sqrt(5));
		for(int i = 0; i < Samples; i++) {
			float r = Mathf.Sqrt((float) i / Samples) * SampleRadius;
			float theta = a * (i - 1);
			sampleOffsets[Samples - i - 1] = new Vector2(Mathf.Cos(theta) * r, Mathf.Sin(theta) * r);
		}

		shadows = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);

		// Calculate all the direction vectors only once for giga speed
		dirs = Vision.GetDirections(OcclusionResolution);

		// Get player
		player = GameObject.Find("Player");

		// I stole this code from Unity's docs lol
		// Get the material
		mat = new Material(Shader.Find("Hidden/Internal-Colored"));
		// Idk
		mat.hideFlags = HideFlags.HideAndDontSave;
		// Turn on alpha blending
		mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
		// Turn backface culling off
		mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
		// Turn off depth writes
		mat.SetInt("_ZWrite", 0);
	}

	// Map a world position to a position on the screen for use in GL calls
	// Function name is short because I have to call it a lot and I value my fingers
	Vector2 GLS(Vector2 pos) {
		return (pos - (Vector2) Camera.main.transform.position + camDims / 2) / camDims;
	}

	void OnPostRender() {
		// Render the shadows to the shadows texture
		RenderTexture.active = shadows;
		GL.Clear(false, true, new Color(0, 0, 0, 0));
		// Update camera dimensions
		// Is this necessary? I guess it will be if the camera ever zooms in or out
		camDims = new Vector2(2 * Camera.main.orthographicSize * Camera.main.aspect, 2 * Camera.main.orthographicSize);
		// This does... uh, something matrix-related
		GL.PushMatrix();
		// I think
		GL.LoadOrtho();
		// Use the material we set earlier
		mat.SetPass(0);
		GL.Begin(GL.QUADS);
		// Set the shadow color
		float alpha = Mathf.Sqrt(1 - Mathf.Pow(ShadowBrightness, 1f / Samples));
		GL.Color(new Color(0, 0, 0, alpha));
		float[] dists = Vision.GetPlayerDepths(dirs);
		for(int i = 0; i < dists.Length; i++) {
			// Draw a quad as part of the shadow
			GL.Vertex(GLS((Vector2) player.transform.position + dirs[i] * dists[i]));
			GL.Vertex(GLS((Vector2) player.transform.position + dirs[(i + 1) % dirs.Length] * dists[(i + 1) % dists.Length]));
			GL.Vertex(GLS((Vector2) player.transform.position + dirs[(i + 1) % dirs.Length] * 50));
			GL.Vertex(GLS((Vector2) player.transform.position + dirs[i] * 50));
		}
		GL.End();
		GL.PopMatrix();
		// Switch back to the other thing for regular rendering
		RenderTexture.active = null;

	}

	void OnGUI() {
		// Draw the shadow texture with a bunch of offsets to make it soft
		for(int i = 0; i < Samples; i++) {
			// Use this one instead if you need more control
			/*GUI.DrawTexture(new Rect(sampleOffsets[i].x, sampleOffsets[i].y, Screen.width, Screen.height),
					shadows, ScaleMode.ScaleToFit, true, 0, new Color(0.5f, 0.5f, 0.5f, 0.01f), 0, 0);*/
			GUI.DrawTexture(new Rect(sampleOffsets[i].x, sampleOffsets[i].y, Screen.width, Screen.height), shadows);
		}
	}
}
