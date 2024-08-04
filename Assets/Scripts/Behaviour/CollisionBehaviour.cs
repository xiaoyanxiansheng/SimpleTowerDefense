using UnityEngine;

public class CollisionBehaviour : MonoBehaviour
{
    // ��������Ϸ����ʼ�������Ϸ�����EdgeCollider2D��ײʱ����
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

    // ��������Ϸ����ֹͣ�������Ϸ�����EdgeCollider2D��ײʱ����
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("��ײ��������" + collision.gameObject.name + "OnTriggerStay2D");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("��ײ��������" + collision.gameObject.name + "OnTriggerExit2D");
    }
}
