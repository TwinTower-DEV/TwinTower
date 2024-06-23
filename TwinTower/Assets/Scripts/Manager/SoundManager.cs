using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TwinTower;
using UnityEngine;

public class SoundManager : Manager<SoundManager>
    {
        AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
        float[] _volumes = new float[(int)Define.Sound.MaxCount];
        Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
        private int bgmsoundsize = -1;
        private int sesoundsize = -1;
        private float[] _volumeset = new float[]{ 0, 0.25f, 0.5f, 0.75f, 1.0f };
        //private float _masterVolume = DataManager.Instance.GameData.mastetVolume;

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        public void Init()
        {
            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = transform;
            }
            _volumes[(int)Define.Sound.Bgm] = _volumeset[DataManager.Instance.UIGameDatavalue.bgmcoursor];
            _volumes[(int)Define.Sound.Effect] = _volumeset[DataManager.Instance.UIGameDatavalue.secursor];
            _audioSources[(int)Define.Sound.Bgm].loop = true;
            RefreshSound();
        }
        public void Clear()
        {
            foreach (AudioSource audioSource in _audioSources)
            {
                audioSource.clip = null;
                audioSource.Stop();
            }
            _audioClips.Clear();
        }
        public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
        {
            AudioClip audioClip = GetOrAddAudioClip(path, type);
            Debug.Log(path);
            Play(audioClip, type, pitch);
        }

        public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
        {
            if (audioClip == null) return;

            if (type == Define.Sound.Bgm)
            {
                AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];
                if (audioSource.isPlaying)
                    audioSource.Stop();
                audioSource.pitch = pitch;
                audioSource.clip = audioClip;
                audioSource.Play();
            }
            else
            {
                AudioSource audioSource = ResourceManager.Instance.Instantiate("SFX/SoundEffect")
                    .GetComponent<AudioSource>();
                audioSource.clip = audioClip;
                //audioSource.volume = DataManager.Instance.GameData.effectVolume;
                audioSource.volume = _audioSources[(int)Define.Sound.Effect].volume;
                Debug.Log(audioSource.volume);
                StartCoroutine(PlayBgm(audioSource));
            }
        }
        private IEnumerator PlayBgm(AudioSource audioSource)
        {
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
            if (audioSource != null)
                Destroy(audioSource.gameObject);
        }
        public void StopBGM()
        {
            _audioSources[(int)Define.Sound.Bgm].Stop();
            _audioSources[(int)Define.Sound.Bgm].clip = null;
        }

        public void StopEffect()
        {
            _audioSources[(int)Define.Sound.Effect].Stop();
            _audioSources[(int)Define.Sound.Effect].clip = null;
        }
        AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
        {
            if (path.Contains("Sounds/") == false)
            {
                path = $"Sounds/sfx/{path}";
            }
            AudioClip audioClip = null;
            if (type == Define.Sound.Bgm)
            {
                audioClip = ResourceManager.Instance.Load<AudioClip>(path);
            }
            else
            {
                if (_audioClips.TryGetValue(path, out audioClip) == false)
                {
                    audioClip = ResourceManager.Instance.Load<AudioClip>(path);
                    _audioClips.Add(path, audioClip);
                }
            }

            if (audioClip == null)
            {
                Debug.Log($"AudioClip Missing! {path}");
            }

            return audioClip;
        }

        public void PreviewVolume_BGM(int bgmcursor)
        {
            _audioSources[(int)Define.Sound.Bgm].volume = _volumeset[bgmcursor];
            bgmsoundsize = bgmcursor;
        }

        public void PreviewVolume_SE(int secursor)
        {
            _audioSources[(int)Define.Sound.Effect].volume = _volumeset[secursor];
            sesoundsize = secursor;
        }

        public void ApplySoundVolume()
        {
            if (bgmsoundsize != -1)
                _volumes[(int)Define.Sound.Bgm] = _volumeset[bgmsoundsize];
            if (sesoundsize != -1)
                _volumes[(int)Define.Sound.Effect] = _volumeset[sesoundsize];

            bgmsoundsize = -1;
            sesoundsize = -1;
            RefreshSound();
        }

        public void CancelSetting()
        {
            RefreshSound();
        }
        private void RefreshSound()
        {
            for(int i = 0; i < (int)Define.Sound.MaxCount; i++)
            {
                _audioSources[i].volume = _volumes[i];
            }
        }
    }
