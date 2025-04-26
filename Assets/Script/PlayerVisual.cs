using UnityEngine;

public class PlayerVisual:MonoBehaviour
{
    private Animator animator;

    private const string IS_WOLK = "IsWolk";
    private const string IS_WOLK_LEFT = "IsWolkLeft";
    private const string IS_WOLK_STRAIGHT = "IsWolkStraight";
    private const string IS_WOLK_BACK = "IsWolkBack";

    private void Awake() {
        animator = GetComponent<Animator>();

    }

    private void Update() {
        animator.SetBool (IS_WOLK, Player.Instance.IsWolk());
        animator.SetBool (IS_WOLK_LEFT, Player.Instance.IsWolkLeft());
        animator.SetBool (IS_WOLK_STRAIGHT, Player.Instance.IsWolkStraight());
        animator.SetBool (IS_WOLK_BACK, Player.Instance.IsWolkBack());

        if (Player.Instance.IsRunning()){
            animator.speed = 1.5f;
        }
        else{
            animator.speed = 1f;
        }
    }
}
