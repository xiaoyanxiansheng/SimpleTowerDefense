
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : EnemyCommon
{
    private EntityBehaviour _entityBehaviour;
    private List<Vector2> _jumpPosPaths = new List<Vector2>();

    private int _jumpCellStep = 2;
    private int _curPathIndex = 0;
    private int _nextPathIndex = 0;
    private float _cellPassTime = 0;

    protected override List<Vector2> CalMovePosPaths(ref List<Vector2> movePosPaths, Vector2 startPos, Vector2 endPos)
    {
        movePosPaths = base.CalMovePosPaths(ref movePosPaths, startPos, endPos);
        _jumpPosPaths.Clear();
        for(int i = 0; i < movePosPaths.Count / _jumpCellStep; i++)
        {
            _jumpPosPaths.Add(movePosPaths[i * _jumpCellStep]);
        }
        if(movePosPaths.Count % _jumpCellStep != 0)
        {
            _jumpPosPaths.Add(movePosPaths[movePosPaths.Count - 1]);
        }
        return _jumpPosPaths;
    }

    //public override void EnterBattle(Vector2 startPos, Vector2 endPos)
    //{
    //    movePathComponent.CalculateMovePosPaths(startPos, endPos);
    //    _movePaths = movePathComponent.GetMovePaths();
    //    GameObject obj = ResourceManager.GetGameObjectById(GetEntityInstanceId());
    //    obj.transform.localPosition = startPos;
    //    obj.gameObject.SetActive(true);
    //    _entityBehaviour = GetEntityBehaviour();
    //    _curPathIndex = 0;
    //    _nextPathIndex = 0;
    //    MoveFinishCall();
    //}

    //protected override void OnUpdate(float delta)
    //{
    //    if (_curPathIndex == _nextPathIndex) return;

    //    // X
    //    Vector2 endPos = _movePaths[_nextPathIndex];
    //    movePathComponent.SetStart(GetPos()).SetEnd(endPos).SetSpeed(GetMoveSpeed()).SetMoveFinishCall(MoveFinishCall).Move();

    //    // Y
    //    _cellPassTime += delta;
    //    float y = _entityBehaviour.animationCurve.Evaluate(_cellPassTime * _entityBehaviour.animationSpeed);

    //}

    //protected override void MoveCellFinishCall()
    //{
    //    _curPathIndex = _nextPathIndex;
    //    _nextPathIndex = Mathf.Max(_curPathIndex + _jumpCellStep, _movePaths.Count - 1);

    //    base.MoveCellFinishCall();
    //}

    //private void MoveFinishCall()
    //{
    //    _curPathIndex = _nextPathIndex;
    //    _nextPathIndex = Mathf.Max(_curPathIndex + _jumpCellStep, _movePaths.Count - 1);
    //}
}