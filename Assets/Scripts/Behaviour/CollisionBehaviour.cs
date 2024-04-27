using UnityEngine;

[ExecuteAlways]
public class CollisionBehaviour : MonoBehaviour
{
    // ��������Ϸ����ʼ�������Ϸ�����EdgeCollider2D��ײʱ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        MessageManager.Instance.SendMessage(MessageConst.Battle_Collision,GetComponent<EntityBehaviour>().entityMonoId, collision.GetComponent<EntityBehaviour>().entityMonoId);
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
