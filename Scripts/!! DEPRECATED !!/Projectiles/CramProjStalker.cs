using BMEffects_Remaster.Util;
using BrilliantSkies.Ftd.Game.Pools;
using UnityEngine;


namespace BMEffects_Remaster.Effects.Projectiles
{
    internal class CramProjStalker : MonoBehaviour
    {
        public PooledCramProjectile proj;
        public bool bExplode;

        public void Start()
        {
            bExplode = false;
        }

        public void Update()
        {
            if (!proj.gameObject.activeSelf)
            {
                if (bExplode)
                {
                    bExplode = false;
                    float explosiveDamage = proj._pState.ExplosiveDamage;
                    float explosiveRadius = proj._pState.ExplosiveRadius;
                    if (explosiveRadius > 20f)
                    {
                        BM_ExplosionsName type = BM_ExplosionsName.DistShockWave;
                        GameObject gameObject = BM_EffectCreator.Creator.CreateExplosion(type);
                        gameObject.transform.localScale = Vector3.one * explosiveRadius * 2f;
                        gameObject.transform.localPosition = proj.myTransform.position;
                        return;
                    }
                }
            }
            else
            {
                bExplode = true;
            }
        }
    }
}
