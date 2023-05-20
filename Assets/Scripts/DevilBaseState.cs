using UnityEngine;

public abstract class DevilBaseState{   
    
    abstract public void EnterState(DevilStateManager devil);

    abstract public void UpdateState(DevilStateManager devil);

    abstract public void OnCollisionEnter(DevilStateManager devil);

}
