using UnityEngine;

public class CollisionBehaviour : MonoBehaviour
{
    // 当其他游戏对象开始与这个游戏对象的EdgeCollider2D碰撞时调用
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        int entityAttackMonoId = GetComponent<EntityBehaviour>().entityAttackMonoId;
        int entityBeAttackMonoId = GetComponent<EntityBehaviour>().entityBeAttackMonoId;
        int collisionEntityAttackMonoId = collision.GetComponent<EntityBehaviour>().entityAttackMonoId;

        if (entityBeAttackMonoId == -1 || entityBeAttackMonoId == collisionEntityAttackMonoId)
        {
            MessageManager.Instance.SendMessage(MessageConst.Battle_Collision, entityAttackMonoId, collisionEntityAttackMonoId);
        }
    }

    // 当其他游戏对象停止与这个游戏对象的EdgeCollider2D碰撞时调用
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("碰撞结束！与" + collision.gameObject.name + "OnTriggerStay2D");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("碰撞结束！与" + collision.gameObject.name + "OnTriggerExit2D");
    }
}
