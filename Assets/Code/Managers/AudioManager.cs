using JetBrains.Annotations;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource sfx_Manager;
    public AudioClip explosionSFX;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlaySFX()
    {
        sfx_Manager.PlayOneShot(explosionSFX);
    }
}
