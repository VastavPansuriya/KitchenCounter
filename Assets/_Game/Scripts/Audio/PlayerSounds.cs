using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    private float footStepTimer;
    private const float footStepTimerMax = .1f;
    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void Update()
    {
        footStepTimer -= Time.deltaTime;
        if (footStepTimer < 0f)
        {
            footStepTimer = footStepTimerMax;
            if (player.IsWalking())
            {
                const int Volume = 1;
                SoundManager.Instance.PlayFootStepsSound(transform.position, Volume);
            }
        }
    }
}
