using BMEffects_Remaster.Effects.Projectiles;
using System.Collections.Generic;
using UnityEngine;


namespace BMEffects_Remaster.Effects.Projectiles
{
    public class SM_AutoKill : MonoBehaviour
    {
        public float KillTime = 5f;
        public List<GameObject> pool;
        public bool nowInit = true;
        public bool bCreate = true;
        private ParticleSystem ps;

        private void Start()
        {
            this.nowInit = true;
            base.gameObject.SetActive(false);
            this.bCreate = false;
        }

        public void Init()
        {
            this.nowInit = true;
            ps = base.GetComponent<ParticleSystem>();
            if (ps)
            {
                ps.time = 0f;
                ps.Play();
            }
            base.gameObject.SetActive(true);
            SM_LightController[] componentsInChildren = base.GetComponentsInChildren<SM_LightController>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                componentsInChildren[i].Init();
            }
        }

        private void LateUpdate()
        {
            this.nowInit = false;
            bool flag = this.KillTime < ps.time;
            bool flag1 = ps.particleCount <= 0;
            if (flag || flag1)
            {
                Destroy(this.gameObject);
            }
        }
    }
}