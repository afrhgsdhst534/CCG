using UnityEngine;
using UnityEngine.UI;
public class VolumeControler : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle cardsSoundsUI;
    public bool cardsSounds;
    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("volume", volumeSlider.value);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;
        volumeSlider.onValueChanged.AddListener(SetVolume);
        cardsSoundsUI.onValueChanged.AddListener(OnToggleChanged);
    }
    private void OnToggleChanged(bool value)
    {
        cardsSounds = value;
    }
    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("volume", value);
    }
}