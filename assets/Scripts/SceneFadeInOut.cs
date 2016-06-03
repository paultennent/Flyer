using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFadeInOut : MonoBehaviour
{
	public Image FadeImg;
	public float fadeSpeed = 0.5f;
	public bool sceneStarting = true;

	private float fadePercent = 0;

	private Color startColor;

	void Awake()
	{
		FadeImg.rectTransform.localScale = new Vector2(Screen.width, Screen.height);
		startColor = FadeImg.color;
		fadePercent = 0f;
	}
	
	void Update()
	{
		// If the scene is starting...
		if (sceneStarting)
			// ... call the StartScene function.
			StartScene();
	}
	
	
	void FadeToClear()
	{
		// Lerp the colour of the image between itself and transparent.
		fadePercent += fadeSpeed * Time.deltaTime;
		FadeImg.color = Color.Lerp(startColor, Color.clear, fadePercent);
	}
	
	
	void FadeToBlack()
	{
		// Lerp the colour of the image between itself and black.
		fadePercent += fadeSpeed * Time.deltaTime;
		FadeImg.color = Color.Lerp(startColor, Color.black, fadePercent);
	}
	
	
	void StartScene()
	{
		// Fade the texture to clear.
		FadeToClear();
		
		// If the texture is almost clear...
		if (fadePercent > 0.95f)
		{
			// ... set the colour to clear and disable the RawImage.
			FadeImg.color = Color.clear;
			FadeImg.enabled = false;
			
			// The scene is no longer starting.
			sceneStarting = false;
			fadePercent=0;
		}
	}
	
	
	public void EndScene(string SceneNumber)
	{
		// Make sure the RawImage is enabled.
		FadeImg.enabled = true;
		
		// Start fading towards black.
		FadeToBlack();
		
		// If the screen is almost black...
		if (fadePercent >= 0.95f) {
			// ... reload the level
			fadePercent = 0;
			Application.LoadLevel (SceneNumber);
		}
	}
} 