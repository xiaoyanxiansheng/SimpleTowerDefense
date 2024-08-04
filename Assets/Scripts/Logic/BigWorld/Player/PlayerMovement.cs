using TMPro;
using UnityEngine;

public class PlayerMovement
{
    private float _turnSpeed = 5f;
    private Vector2 m_Movement;
    //private Quaternion m_Rotation = Quaternion.identity;
    private Player _player;

    public PlayerMovement(Player player)
    {
        _player = player;
    }

    public void Start()
    {
        
    }

    public void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //判断如果没有输入再获取摇杆的值
        horizontal = horizontal == 0 ? BigWorldManager.Instance.VariableJoystick.Horizontal : horizontal;
        vertical = vertical == 0 ? BigWorldManager.Instance.VariableJoystick.Vertical : vertical;

        m_Movement.Set(horizontal, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalINput = !Mathf.Approximately(vertical, 0f);

        //根据水平和垂直数值，如果有一个移动就代表着行走
        bool isWalking = hasHorizontalInput || hasVerticalINput;
        if (isWalking) _player.GetPlayerAnimation().Play(FrameAnimation.FrameAnimationType.Move);
        else _player.GetPlayerAnimation().Play(FrameAnimation.FrameAnimationType.Idle);

        Vector2 curPosition = _player.GetPosition();
        _player.SetPosition(CalNextPosition(Player.Instance.GetBoxCollider2D(), curPosition, m_Movement * _turnSpeed));
        if(horizontal != 0) _player.SetRotation(horizontal > 0 ? Vector3.zero : new Vector3(0, 180, 0));
    }

    private Vector2 CalNextPosition(BoxCollider2D boxCollider , Vector2 position , Vector2 modifyPosition)
    {
        int collision = BigWorldBoxColliderManager.Instance.CalCollision(position + modifyPosition + Player.Instance.GetBoxCollider2DPosition(), boxCollider.size);

        if (collision == 1 || collision == 2) modifyPosition.x = 0;
        if (collision == 3 || collision == 4) modifyPosition.y = 0;
        return position + modifyPosition;
    }
}