using static FrameAnimation;

public class PlayerAnimation
{
    public PlayerAnimation Instance;

    private FrameAnimation _frameAnimation;
    private Player _player;

    private bool _isAttackState;

    public PlayerAnimation(Player player)
    {
        Instance = this;
        _player = player;
    }

    public void Start()
    {
        _frameAnimation = _player.GetPlayerObjInstance().GetComponent<FrameAnimation>();
    }

    public void Play(FrameAnimation.FrameAnimationType frameAnimationType)
    {
        if (IsAttack()) return;

        if (frameAnimationType == _frameAnimation.curAnimationState) return;
        _frameAnimation.Play(frameAnimationType, null, true);
    }

    public void PlayAttack(float speed)
    {
        if (IsAttack()) return;
        _isAttackState = true;

        _frameAnimation.Play(FrameAnimationType.Attack, () =>
        {
            _isAttackState = false;
        }, false , speed);
    }

    public bool IsAttack() 
    {
        return _isAttackState;
    }
}