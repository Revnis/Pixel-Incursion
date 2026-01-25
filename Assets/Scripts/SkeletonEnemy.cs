public class SkeletonEnemy : BaseEnemy
{
    protected override void Awake()
    {
        base.Awake();
        moveSpeed = 1.2f;
        detectRange = 2.5f;
    }
}
