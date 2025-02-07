using UnityEngine;

public class PlayerVisual:MonoBehaviour
{
    private Animator animator;

    private const string IS_WOLK = "IsWolk";

    private void Awake() {
        animator = GetComponent<Animator>();

    }

    private void Update() {
        animator.SetBool (IS_WOLK, Player.Instance.IsWolk());
    }
}
