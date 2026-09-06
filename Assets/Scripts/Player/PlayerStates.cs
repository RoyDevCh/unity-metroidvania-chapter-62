using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player p, PlayerStateMachine sm) : base(p, sm) { }
    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGrounded) { stateMachine.ChangeState(player.AirState); player.Jump(); return; }
        if (Input.GetKeyDown(KeyCode.LeftShift) && player.TryStartDash()) return;
        if (Input.GetKeyDown(KeyCode.J)) { player.BeginAttack(); return; }
        if (Input.GetKeyDown(KeyCode.Q)) { stateMachine.ChangeState(player.CounterAttackState); return; }
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.01f) { stateMachine.ChangeState(player.MoveState); return; }
        if (!player.IsGrounded) stateMachine.ChangeState(player.AirState);
        player.SetVelocity(0f, player.GetComponent<Rigidbody2D>().velocity.y);
    }
}

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player p, PlayerStateMachine sm) : base(p, sm) { }
    public override void Update()
    {
        base.Update();
        float x = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(x) < 0.01f) { stateMachine.ChangeState(player.IdleState); return; }
        if (x != 0f) player.Face(x > 0f ? 1 : -1);
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGrounded) { player.Jump(); stateMachine.ChangeState(player.AirState); return; }
        if (Input.GetKeyDown(KeyCode.LeftShift) && player.TryStartDash()) return;
        if (Input.GetKeyDown(KeyCode.J)) { player.BeginAttack(); return; }
        if (Input.GetKeyDown(KeyCode.Q)) { stateMachine.ChangeState(player.CounterAttackState); return; }
        if (!player.IsGrounded) { stateMachine.ChangeState(player.AirState); return; }
        player.SetVelocity(x * player.MoveSpeed, player.GetComponent<Rigidbody2D>().velocity.y);
    }
}

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player p, PlayerStateMachine sm) : base(p, sm) { }
    public override void Update()
    {
        base.Update();
        float x = Input.GetAxisRaw("Horizontal");
        if (x != 0f) player.Face(x > 0f ? 1 : -1);
        if (Input.GetKeyDown(KeyCode.LeftShift) && player.TryStartDash()) return;
        if (Input.GetKeyDown(KeyCode.Q)) { stateMachine.ChangeState(player.CounterAttackState); return; }
        if (Mathf.Abs(x) > 0.01f) player.SetVelocity(x * player.MoveSpeed, player.GetComponent<Rigidbody2D>().velocity.y);
        if (player.IsTouchingWall && !player.IsGrounded && player.GetComponent<Rigidbody2D>().velocity.y < 0f)
        {
            stateMachine.ChangeState(player.WallSlideState);
            return;
        }
        if (player.IsGrounded && player.GetComponent<Rigidbody2D>().velocity.y <= 0.1f)
        {
            if (Mathf.Abs(x) > 0.01f) stateMachine.ChangeState(player.MoveState);
            else stateMachine.ChangeState(player.IdleState);
        }
    }
}

public class PlayerDashState : PlayerState
{
    private int direction;
    public PlayerDashState(Player p, PlayerStateMachine sm) : base(p, sm) { }
    public override void Enter()
    {
        base.Enter();
        direction = Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.01f ? (Input.GetAxisRaw("Horizontal") > 0f ? 1 : -1) : player.FacingDirection;
        player.Face(direction);
        player.SetVelocity(player.DashSpeed * direction, 0f);
    }
    public override void Update()
    {
        base.Update();
        player.SetVelocity(player.DashSpeed * direction, 0f);
        if (stateTimer >= player.DashDuration)
        {
            if (player.IsGrounded) stateMachine.ChangeState(player.IdleState);
            else stateMachine.ChangeState(player.AirState);
        }
    }
}

public class PlayerAttackState : PlayerState
{
    private bool hit;
    private int comboStage;
    public PlayerAttackState(Player p, PlayerStateMachine sm) : base(p, sm) { }
    public override void Enter() { base.Enter(); hit = false; player.SetZeroVelocity(); }
    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();
        if (!hit && stateTimer >= 0.12f) { player.PerformAttack(1f + comboStage * 0.15f); hit = true; }
        if (stateTimer >= 0.34f)
        {
            if (player.IsGrounded) stateMachine.ChangeState(player.IdleState);
            else stateMachine.ChangeState(player.AirState);
        }
    }
    public void SetComboStage(int stage) { comboStage = stage; }
}

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player p, PlayerStateMachine sm) : base(p, sm) { }
    public override void Update()
    {
        base.Update();
        player.SetVelocity(0f, -2.2f);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.WallJumpState);
            return;
        }
        if (player.IsGrounded || !player.IsTouchingWall) stateMachine.ChangeState(player.AirState);
    }
}

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player p, PlayerStateMachine sm) : base(p, sm) { }
    public override void Enter()
    {
        base.Enter();
        int awayFromWall = -player.FacingDirection;
        player.Face(awayFromWall);
        player.SetVelocity(awayFromWall * 8f, player.JumpForce);
    }
    public override void Update()
    {
        base.Update();
        if (stateTimer > 0.18f) stateMachine.ChangeState(player.AirState);
    }
}

public class PlayerCounterAttackState : PlayerState
{
    private bool checkedHit;
    public PlayerCounterAttackState(Player p, PlayerStateMachine sm) : base(p, sm) { }
    public override void Enter()
    {
        base.Enter();
        checkedHit = false;
        player.BeginCounterWindow();
        player.SetZeroVelocity();
    }
    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();
        if (!checkedHit && Input.GetKeyDown(KeyCode.Q)) { checkedHit = true; player.TryCounter(); }
        if (stateTimer >= 2f)
        {
            if (player.IsGrounded) stateMachine.ChangeState(player.IdleState);
            else stateMachine.ChangeState(player.AirState);
        }
    }

    public override void Exit()
    {
        player.EndCounterWindow();
        base.Exit();
    }
}
