using UnityEngine;
using System.Collections.Generic;


public sealed class SoundManager : MonoBehaviour
{
   
    
        private static SoundManager _instance;

        public static SoundManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("SoundManager instance is null! Is it in the scene?");
                }
                return _instance;
            }
        }

        [Header("Audio Sources")]
        public AudioSource musicSource;
        public AudioSource sfxSource;

        [Header("Default Audio Clips")]
        public AudioClip defaultButtonClick;
        public AudioClip defaultBackgroundMusic;

        [Header("Volume Settings")]
        public float musicVolume = 0.5f;
        public float sfxVolume = 0.7f;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);

                // สร้าง AudioSource ถ้ายังไม่มี
                if (musicSource == null)
                {
                    musicSource = gameObject.AddComponent<AudioSource>();
                    musicSource.loop = true;
                    musicSource.volume = musicVolume;
                }

                if (sfxSource == null)
                {
                    sfxSource = gameObject.AddComponent<AudioSource>();
                    sfxSource.volume = sfxVolume;
                }

                // เล่นเพลงพื้นหลัง
                if (defaultBackgroundMusic != null)
                {
                    PlayMusic(defaultBackgroundMusic);
                }

                Debug.Log("🎵 SoundManager initialized!");
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // ✅ เล่นเพลงพื้นหลัง
        public void PlayMusic(AudioClip clip)
        {
            if (clip == null || musicSource == null) return;

            musicSource.clip = clip;
            musicSource.Play();
            Debug.Log("🎵 Playing music: " + clip.name);
        }

        // ✅ หยุดเพลง
        public void StopMusic()
        {
            if (musicSource != null)
            {
                musicSource.Stop();
            }
        }

        // ✅ เล่นเสียง Effect
        public void PlaySFX(AudioClip clip)
        {
            if (clip == null || sfxSource == null)
            {
                Debug.LogWarning("❌ Cannot play SFX: clip or source is null");
                return;
            }

            sfxSource.PlayOneShot(clip, sfxVolume);
            Debug.Log("🔊 Playing SFX: " + clip.name);
        }

        // ✅ เล่นเสียง Effect ที่ตำแหน่งใดก็ได้
        public void PlaySFXAtPosition(AudioClip clip, Vector3 position, float volume = 1f)
        {
            if (clip == null)
            {
                Debug.LogWarning("❌ Cannot play SFX at position: clip is null");
                return;
            }

            AudioSource.PlayClipAtPoint(clip, position, volume * sfxVolume);
            Debug.Log("🔊 Playing SFX at position: " + clip.name);
        }

        // ✅ ปรับ Volume เพลง
        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);

            if (musicSource != null)
            {
                musicSource.volume = musicVolume;
            }
        }

        // ✅ ปรับ Volume เสียง Effect
        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);

            if (sfxSource != null)
            {
                sfxSource.volume = sfxVolume;
            }
        }

        // ✅ เล่นเสียงปุ่ม
        public void PlayButtonClick()
        {
            if (defaultButtonClick != null)
            {
                PlaySFX(defaultButtonClick);
            }
        }
    
}