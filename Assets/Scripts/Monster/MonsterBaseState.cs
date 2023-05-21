using UnityEngine;

public abstract class MonsterBaseState{   
    
    abstract public void EnterState(MonsterStateManager monster);

    abstract public void UpdateState(MonsterStateManager monster);

    abstract public void OnCollisionEnter(MonsterStateManager monster);

}
