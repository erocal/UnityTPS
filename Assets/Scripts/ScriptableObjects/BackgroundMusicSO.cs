using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundMusicSO", menuName = "Scriptable Objects/BackgroundMusicSO")]
public class BackgroundMusicSO : ScriptableObject
{

    [SerializeField] public BackgroundMusics[] backgroundMusics;

    [System.Serializable]
    public struct BackgroundMusics
    {

        [SerializeField] private AudioClip backgroundMusic;
        [SerializeField] private float volume;

        public AudioClip BackgroundMusic
        {
            get { return backgroundMusic; }
            private set { }
        }

        public float Volume
        {
            get { return volume; }
            private set { }
        }

    }

}
