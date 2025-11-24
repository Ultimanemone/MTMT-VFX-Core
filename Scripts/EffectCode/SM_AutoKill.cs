using System.Collections.Generic;
using BM_EffectUpdate.EffectCode;
using UnityEngine;

namespace MTMTVFX.Mono
{
    public class SM_AutoKill : MonoBehaviour
    {
        public float KillTime = 5f;
        public List<GameObject> pool;
        public bool nowInit = true;
        public bool bCreate = true;

        private void Start()
        {
            this.nowInit = true;
            base.gameObject.SetActive(false);
            this.bCreate = false;
        }

        public void Init()
        {
            this.nowInit = true;
            this.NowTime = 0f;
            if (base.GetComponent<ParticleSystem>())
            {
                base.GetComponent<ParticleSystem>().time = 0f;
                base.GetComponent<ParticleSystem>().Play();
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
            this.deltaTime = Time.deltaTime;
            this.NowTime += this.deltaTime;
            if (this.KillTime < this.NowTime)
            {
                base.gameObject.SetActive(false);
            }
        }
    }
}
