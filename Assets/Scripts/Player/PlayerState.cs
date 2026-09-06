using UnityEngine;

public abstract class PlayerState
{
    protected readonly Player player;
    protected readonly PlayerStateMachine stateMachine;
    protected float stateTimer;

    protected PlayerState(Player player, PlayerStateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { stateTimer = 0f; }
    public virtual void Update() { stateTimer += Time.deltaTime; }
    public virtual void Exit() { }
}
