using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public Transform BGMtrs;
    public Transform SFXtrs;

    //오디오 클립 경로
    private const string AUDIO_PATH = "Audio";
    
    private Dictionary<EnumData.BGM, AudioSource> _BGMPlayer = new Dictionary<EnumData.BGM, AudioSource>();
    private Dictionary<EnumData.SFX, AudioSource> _SFXPlayer = new Dictionary<EnumData.SFX, AudioSource>();

    // 현재 재생중인 BGM
    private AudioSource _currBGMSource;


    protected override void Init()
    {
        base.Init();
        LoadBGMPlayer();
        LoadSFXPlayer();
    }

    //Audio 폴더내 BGM 오디오 클립 딕셔너리에 저장
    private void LoadBGMPlayer()
    {
        for (int i = 0; i < (int)EnumData.BGM.COUNT; i++)
        {
            var audioName = ((EnumData.BGM)i).ToString();
            var pathStr = $"{AUDIO_PATH}/{audioName}";
            var audioClip = Resources.Load(pathStr, typeof(AudioClip)) as AudioClip;

            if (!audioClip)
            {
                continue;
            }

            var newGameObject = new GameObject(audioName);
            var newAudioSource = newGameObject.AddComponent<AudioSource>();
            newAudioSource.clip = audioClip;
            newAudioSource.loop = true;
            newAudioSource.volume = 0.2f;
            newAudioSource.playOnAwake = false;
            newGameObject.transform.parent = BGMtrs;

            _BGMPlayer[(EnumData.BGM)i] = newAudioSource;
        }
    }

    //Audio 폴더내 SFX 오디오 클립 딕셔너리에 저장
    private void LoadSFXPlayer()
    {
        for(int i = 0; i < (int)EnumData.SFX.COUNT; i++)
        {
            var audioName = ((EnumData.SFX)i).ToString();
            var pathStr = $"{AUDIO_PATH}/{audioName}";
            var audioClip = Resources.Load(pathStr, typeof(AudioClip)) as AudioClip;

            if(!audioClip)
            {
                continue;
            }

            var newGameObject = new GameObject(audioName);
            var newAudioSource = newGameObject.AddComponent<AudioSource>();
            newAudioSource.clip = audioClip;
            newAudioSource.loop = false;
            newAudioSource.volume = 0.2f;
            newAudioSource.playOnAwake = false;
            newGameObject.transform.parent = SFXtrs;

            _SFXPlayer[(EnumData.SFX)i] = newAudioSource;
        }
    }
    // BGM 함수
    #region [BGM 함수]

    public void PlayBGM(EnumData.BGM bgm)
    {
        // BGM 재생 함수
        // Audio 폴더에 있는 오디오클립명으로 매개변수를 받습니다
        // ex) abcd 이름의 오디오클립 재생시 BGM.abcd


        //현재 재생중인 BGM 제거
        if (_currBGMSource)
        {
            _currBGMSource.Stop();
            _currBGMSource = null;
        }

        // BGMPlayer에 오디오클립이 있는지 확인
        if(!_BGMPlayer.ContainsKey(bgm))
        {
            return;
        }

        _currBGMSource = _BGMPlayer[bgm];
        _currBGMSource.Play();
    }

    public void PauseBGM()
    {
        //BGM 일시정지 함수


        if (_currBGMSource)
        {
            _currBGMSource.Pause();
        }
    }

    public void ResumeBGM()
    {
        //BGM 일시정지 해제함수


        if(_currBGMSource)
        {
            _currBGMSource.UnPause();
        }
    }

    public void StopBGM()
    {
        //BGM 정지 함수


        if (_currBGMSource)
        {
            _currBGMSource.Stop();
        }
    }

    #endregion


    // SFX 함수 
    #region [SFX 함수]

    public void PlaySFX(EnumData.SFX sfx)
    {
        // SFX 재생 함수
        // Audio 폴더에 있는 오디오클립명으로 매개변수를 받습니다
        // ex) abcd 이름의 오디오클립 재생시 SFX.abcd


        if (!_SFXPlayer.ContainsKey(sfx))
        {
            return;
        }
        _SFXPlayer[sfx].Play();
    }

    #endregion

    //인게임 오디오설정 관련 함수
    #region

    #endregion
}
