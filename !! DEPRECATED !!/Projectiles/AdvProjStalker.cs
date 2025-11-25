using BMEffects_Remaster.Util;
using BrilliantSkies.Ftd.Game.Pools;
using UnityEngine;


namespace BMEffects_Remaster.Effects.Projectiles
{
    internal class AdvProjStalker : MonoBehaviour
    {
        public AdvPooledProjectile proj;
        public bool bExplode;

        public void Start()
        {
            bExplode = false;
        }

        public void Update()
        {
            if (!proj.gameObject.activeSelf)
            {
                if ((bool)BM_EffectUpdater.GetField("_explodedAlready", proj) && bExplode)
                {
                    bExplode = false;
                    ShellModel shellModel = (ShellModel)BM_EffectUpdater.GetField("ShellModel", proj);
                    shellModel.ExplosiveCharges.GetExplosionDamage();
                    shellModel.ExplosiveCharges.GetFlakExplosionDamage();
                    float num = shellModel.ExplosiveCharges.GetExplosionRadius();
                    float flakExplosionRadius = shellModel.ExplosiveCharges.GetFlakExplosionRadius();
                    num = Mathf.Max(num, flakExplosionRadius);
                    if (num > 20f)
                    {
                        BM_ExplosionsName type = BM_ExplosionsName.DistShockWave;
                        GameObject gameObject = BM_EffectCreator.Creator.CreateExplosion(type);
                        gameObject.transform.localScale = Vector3.one * num * 2f;
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
