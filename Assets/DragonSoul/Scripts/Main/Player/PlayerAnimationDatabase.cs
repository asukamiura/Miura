using System.Collections.Generic;
namespace Player
{
    public class PlayerAnimationDatabase
    {
        public static readonly Dictionary<PlayerStateID, AnimationConfig> Configs = new Dictionary<PlayerStateID, AnimationConfig>
        {
            { PlayerStateID.Idle, new AnimationConfig{ animationName = "Idle", transitionDuration = 0.5f, layer = 0, offset = 0} },
            { PlayerStateID.Move, new AnimationConfig{ animationName = "Move", transitionDuration = 0.5f, layer = 0, offset = 0} },
            { PlayerStateID.Dash, new AnimationConfig{ animationName = "Dash", transitionDuration = 0.5f, layer = 0, offset = 0} },
            { PlayerStateID.Dodge, new AnimationConfig{ animationName = "", transitionDuration = 0.5f, layer = 0, offset = 0} },
            { PlayerStateID.Guard, new AnimationConfig{ animationName = "", transitionDuration = 0.5f, layer = 0, offset = 0} },
            { PlayerStateID.Block, new AnimationConfig{ animationName = "", transitionDuration = 0.5f, layer = 0, offset = 0} },
            { PlayerStateID.Damage, new AnimationConfig{ animationName = "", transitionDuration = 0.5f, layer = 0, offset = 0} },
            { PlayerStateID.Dead, new AnimationConfig{ animationName = "", transitionDuration = 0.5f, layer = 0, offset = 0} },
            { PlayerStateID.AttackNormal1, new AnimationConfig{ animationName = "AttackNormal1", transitionDuration = 0.1f, layer = 0, offset = 0} },
            { PlayerStateID.AttackNormal2, new AnimationConfig{ animationName = "", transitionDuration = 0.5f, layer = 0, offset = 0} },
            { PlayerStateID.AttackNormal3, new AnimationConfig{ animationName = "", transitionDuration = 0.5f, layer = 0, offset = 0} },
            { PlayerStateID.AttackSpecial1_1, new AnimationConfig{ animationName = "", transitionDuration = 0.5f, layer = 0, offset = 0} },
            { PlayerStateID.AttackSpecial1_2, new AnimationConfig{ animationName = "", transitionDuration = 0.5f, layer = 0, offset = 0} },
            { PlayerStateID.AttackSpecial1_3, new AnimationConfig{ animationName = "", transitionDuration = 0.5f, layer = 0, offset = 0} },
            { PlayerStateID.AttackSpecial2, new AnimationConfig{ animationName = "", transitionDuration = 0.5f, layer = 0, offset = 0} },
        };
    }
}
