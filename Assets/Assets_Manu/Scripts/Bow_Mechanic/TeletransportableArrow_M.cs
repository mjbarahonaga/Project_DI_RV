using MEC;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TeletransportableArrow_M : Arrow_M
{
    public float OffsetY = 1f;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void CheckCollision()
    {
        if (Physics.Linecast(_lastPosition, Tip.position, out RaycastHit hit))
        {
            if ((_layersToAvoidOnHit.value & (1 << hit.collider.gameObject.layer)) > 0) return;
            if (hit.transform.TryGetComponent(out Collider collider))
            {
                _rb.interpolation = RigidbodyInterpolation.None;
                transform.parent = hit.transform;
                Stop();
                Teleport(hit.point);
            }
        }
    }

    public void Teleport(Vector3 pos)
    {
        FadeController.Instance.StartFadeIn(0.5f, () =>
        {
            var player = GameManager_Teleport.Instance.GetPlayer;
            player.transform.position = new Vector3(pos.x,pos.y + OffsetY, pos.z);
            FadeController.Instance.StartFadeOut(0.5f);
        },
        true);
        
    }

    protected override void Stop(bool hitted = true)
    {
        base.Stop(hitted);
    }
    protected override IEnumerator<float> UpdateCoroutine()
    {
        return base.UpdateCoroutine();
    }

    public override void Release(float value)
    {
        base.Release(value);
    }

    protected override void MaskAndFire(float power)
    {
        base.MaskAndFire(power);
    }

    protected override IEnumerator<float> ToDestroy(float time)
    {
        return base.ToDestroy(time);
    }
}
