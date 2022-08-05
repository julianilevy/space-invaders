using UnityEngine;

namespace SpaceInvaders
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioClipIterator : GenericIterator<AudioClip>
    {
        private AudioSource _audioSource = null;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        protected override void OnGoToNextItem(int index)
        {
            PlayAudioClip(orderedItems[index]);
        }

        protected override void OnGoToLastItem()
        {
            PlayAudioClip(lastItem);
        }

        protected override void OnRestart()
        {
            PlayAudioClip(orderedItems[0]);
        }

        private void PlayAudioClip(AudioClip audioClip)
        {
            _audioSource.clip = audioClip;
            _audioSource.Play();
        }
    }
}