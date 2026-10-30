using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Nguồn phát")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AutoWeapon weapon;
    [SerializeField] private Experience experience;

    [Header("Clip")]
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip enemyDied;
    [SerializeField] private AudioClip levelUp;
    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip lose;
    [SerializeField] private AudioClip shoot;
    [SerializeField] private AudioClip gem;

    [SerializeField, Range(0f, 1f)] private float musicVolume = 0.4f;
    [SerializeField, Range(0f, 1f)] private float sfxVolume = 0.8f;

    private GameStateMachine stateMachine;
    private float lastEnemyDeathSfxTime;
    private const float EnemyDeathSfxCooldown = 0.04f;   // chống hàng chục tiếng chồng nhau cùng frame

    private void Start()
    {
        stateMachine = GameStateMachine.Instance;
        stateMachine.OnStateChanged += HandleStateChanged;

        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.Play();
        weapon.OnFired += () => PlaySfx(shoot, 0.3f);
        experience.OnXpGained += () => PlaySfx(gem, 0.5f);
    }

    private void OnEnable()
    {
        Enemy.Died += HandleEnemyDied;
        GameEvents.LevelUp += HandleLevelUp;
    }

    private void OnDisable()
    {
        Enemy.Died -= HandleEnemyDied;
        GameEvents.LevelUp -= HandleLevelUp;
        if (stateMachine != null) stateMachine.OnStateChanged -= HandleStateChanged;
    }

    public void PlaySfx(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume * volumeScale);
    }

    private void HandleEnemyDied(Enemy _)
    {
        // Time.unscaledTime vì timeScale có thể bằng 0
        if (Time.unscaledTime - lastEnemyDeathSfxTime < EnemyDeathSfxCooldown) return;
        lastEnemyDeathSfxTime = Time.unscaledTime;
        PlaySfx(enemyDied, 0.7f);
    }

    private void HandleLevelUp(int _) => PlaySfx(levelUp);

    private void HandleStateChanged(GameState previous, GameState next)
    {
        if (next == GameState.Win) { musicSource.Stop(); PlaySfx(win); }
        else if (next == GameState.Lose) { musicSource.Stop(); PlaySfx(lose); }
    }
}