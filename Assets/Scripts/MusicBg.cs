using Internal;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class MusicBg : MonoBehaviour, IRegistrable
{
	[SerializeField] private AudioClip[] _music;
	private                  int         _current = -1;
	private                  AudioSource _audioSource;
	
	public                   void        Register()   => Locator.Register(typeof(MusicBg), this);
	public                   void        Unregister() => Locator.Unregister(typeof(MusicBg));
	private                  void        Awake()
	{
		Register();
		_audioSource = GetComponent<AudioSource>();
	}
	private                  void        OnDestroy()  => Unregister();
	public void Play(int index = -1)
	{
		if (index < 0)
		{
			do
				index = Random.Range(0, _music.Length);
			while (_current == index);
		}
		_current = index;
		_audioSource.Stop();
		_audioSource.clip = _music[_current];
		_audioSource.Play();
	}
	private void Update()
	{
		if (_audioSource.time >= _audioSource.clip.length) Play();
	}

}
