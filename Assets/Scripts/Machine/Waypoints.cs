using UnityEngine;

public class Waypoints : MonoBehaviour
{
    internal Vector3[] _positions = null;
    /// <summary>
    /// 子物体的世界坐标数组
    /// </summary>
    internal Vector3[] Positions
    {
        get
        {
            if(_positions == null && transform.childCount > 0)
            {
                _positions = new Vector3[transform.childCount];
                for (int i = 0; i < _positions.Length; i++)
                {
                    Transform child = transform.GetChild(i);
                    _positions[i] = child.position;
                }
            }
            return _positions;
        }
    }

    internal Vector3[] _localPositions = null;
    /// <summary>
    /// 子物体的本地坐标数组
    /// </summary>
    internal Vector3[] LocalPositions
    {
        get
        {
            if (_localPositions == null && transform.childCount > 0)
            {
                _localPositions = new Vector3[transform.childCount];
                for (int i = 0; i < _localPositions.Length; i++)
                {
                    Transform child = transform.GetChild(i);
                    _localPositions[i] = child.localPosition;
                }
            }
            return _localPositions;
        }
    }
}