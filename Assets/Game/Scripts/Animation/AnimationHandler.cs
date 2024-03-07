using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    protected int _currentState;
    protected float _lockedTime;

    protected virtual void Update()
    {
        int state = GetState();

        if (state == _currentState)
        {
            return;
        }

        _currentState = state;
    }

    protected virtual int GetState()
    {
        return 0;
    }

    protected int LockState(int state, float time)
    {
        _lockedTime = Time.time + time;
        return state;
    }
}
