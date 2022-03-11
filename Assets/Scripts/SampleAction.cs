using TMPro;
using UnityEngine;

public class SampleAction : MonoBehaviour
{
    public TextMeshProUGUI alertText;
    private AudioSource audioSource;
    public AudioClip sound1;
    public AudioClip sound2;
    public string sentText = "Send Mosaic!!!";
    public string receivedText = "Received Mosaic!!!";

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SentMosaic()
    {
        audioSource.PlayOneShot(sound1);
        alertText.text = sentText;
    }
    
    public void ReceiveMosaic()
    {
        audioSource.PlayOneShot(sound2);
        alertText.text = receivedText;
    }
}
