using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
	public string NextSceneName;
	public GameObject PasueMenu;
	public GameObject ControlButton;
	public AudioMixer AM;
	public Slider slider;

	private void Start()
	{
		slider.value = recordValue;
	}

	public void PlayGame()
	{
		SceneManager.LoadScene(NextSceneName);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void UIEnable()
	{
		GameObject.Find("Canvas/MainMenu/UI").SetActive(true);
	}

	public void PauseMenu()
	{
		PasueMenu.SetActive(true);
		ControlButton.SetActive(false);
		Time.timeScale = 0;
	}

	public void ExitGame()
	{
		PasueMenu.SetActive(false);
		ControlButton.SetActive(true);
		Time.timeScale = 1;
	}

	public static float recordValue;
	public void SetVolume(float val)
	{
		recordValue = val;
		AM.SetFloat("MainVolume", val);
	}
}
