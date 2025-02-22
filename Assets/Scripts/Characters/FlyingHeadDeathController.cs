public class FlyingHeadDeathController : DeathController
{
    protected override void ProtectedOnDeath()
    {
        GetComponent<ShadowClonesController>()?.DestroyAllClones();
    }
}
