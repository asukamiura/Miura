using Player;

public class PlayerAttackNormal : IState<PlayerStateID>
{
    readonly PlayerCore core;
    readonly AnimationController animationController;
    readonly PlayerAttackNormalController normalAttackController;
    readonly AttackAssist attackAssist;
    PlayerAttackData currentAttackData;
    int currentStep = 1;
    InputReceiver Input => InputReceiver.Instance;
    bool isComboContinue = false;

    public PlayerStateID StateID => PlayerStateID.AttackNormal;

    public PlayerAttackNormal(PlayerCore core, AnimationController animationController, PlayerAttackNormalController normalAttackController, AttackAssist attackAssist)
    {
        this.core = core;
        this.animationController = animationController;
        this.normalAttackController = normalAttackController;
        this.attackAssist = attackAssist;
    }

    public void Enter()
    {
        PlayAnimation();
        animationController.SetRootMotion(true);
        normalAttackController.OnHitDetectionPerformed += HandlePerformHit;
    }

    public void Update()
    {
        // 現在の段が最終段か？
        bool isLastStep = currentStep >= normalAttackController.GetMaxComboCount();

        // 次の段に行けるか判定(最終段でなければ入力を受け付ける)
        if (!isLastStep && !isComboContinue && Input.AttackNormal && animationController.IsTimeElapsed(currentAttackData.AnimationStateName, currentAttackData.ComboEnableTime))
        {
            isComboContinue = true;
        }

        // ガードステートに遷移
        if (Input.Guard)
        {
            core.StateMachine.ChangeState(PlayerStateID.Guard);
            return;
        }

        // ダッシュステートに遷移
        if (Input.Dash)
        {
            core.StateMachine.ChangeState(PlayerStateID.Dash);
            return;
        }

        // 次の段に遷移する処理
        if (isComboContinue && animationController.IsTimeElapsed(currentAttackData.AnimationStateName, currentAttackData.TransitionTime))
        {
            currentStep++;
            PlayAnimation();
            isComboContinue = false;
            return;
        }

        // 次の段に行かない、既に最終段である場合、リセット時間でLocomotionに遷移
        if ((!isComboContinue || isLastStep) && animationController.IsTimeElapsed(currentAttackData.AnimationStateName, currentAttackData.ComboResetTime))
        {
            core.StateMachine.ChangeState(PlayerStateID.Locomotion);
        }
    }

    public void FixedUpdate() { }

    public void Exit()
    {
        currentStep = 1;
        attackAssist.StopAssist();
        animationController.SetRootMotion(false);
        normalAttackController.OnHitDetectionPerformed -= HandlePerformHit;
    }

    // アニメーションを再生
    void PlayAnimation()
    {
        currentAttackData = normalAttackController.GetCurrentAttackData(currentStep);

        animationController.PlayAniamtion(currentAttackData.AnimationStateName, currentAttackData.TransitionDuration, 0, currentAttackData.TimeOffset);
    }

    void HandlePerformHit()
    {
        normalAttackController.ExecuteAttack(currentAttackData, core.AttackPower, core.InPowerUp, core.CenterTransform, animationController.Animator);
    }
}
