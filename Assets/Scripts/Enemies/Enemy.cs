using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Enemy : Damagable
{
    public Vector3 velocity;

    public Transform target;
    public CharacterController controller { get; private set; }

    private void Awake() {
        controller = GetComponent<CharacterController>();
        target = PollingStation.Get<PlayerController>().transform;
    }

    public void FixedUpdate() {
        if (!controller.isGrounded) {
            velocity += Physics.gravity * Time.fixedDeltaTime;
            controller.Move(velocity * Time.fixedDeltaTime);
        }
        else {
            velocity.y = 0;
        }
    }
}
