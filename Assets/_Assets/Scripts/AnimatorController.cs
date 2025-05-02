using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Player player;

    // Update is called once per frame
    void Update() 
    {
        if (player.IsWalking()) {
            animator.SetBool("IsWalking", true);
        } else {
            animator.SetBool("IsWalking", false);
        }
    }
}
