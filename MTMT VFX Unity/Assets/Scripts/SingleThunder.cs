using UnityEngine;


namespace MTMTVFX.Internal
{
    public class SingleThunder : MonoBehaviour
    {
        private LineRenderer Line;
        public float Width = 1f;
        public float Height = 10f;
        public float MoveLength = 0.1f;
        private Vector3[] PosBuf = new Vector3[8];
        private Vector3[] MoveVec = new Vector3[8];

        private void Start()
        {
            Line = GetComponent<LineRenderer>();
            GetComponent<Renderer>().material.SetTextureOffset("_MaskTex", new Vector2(0f, UnityEngine.Random.value));
            SetThunder();
        }

        private void SetThunder()
        {
            for (int i = 0; i < 8; i++)
            {
                float num = i / 8f;
                PosBuf[i].x = UnityEngine.Random.Range(-Width, Width) * num;
                PosBuf[i].z = UnityEngine.Random.Range(-Width, Width) * num;
                PosBuf[i].y = num * Height;
                Line.SetPositions(PosBuf);
                Line.widthMultiplier = 10f;
                MoveVec[i] = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1)) * num * MoveLength * 100f;
            }
        }

        private void Update()
        {
            for (int i = 0; i < 8; i++)
            {
                float num = (8 - i - 1) / 8f;
                Vector3[] posBuf = PosBuf;
                int num2 = i;
                posBuf[num2].y = posBuf[num2].y - Time.deltaTime * num * 50f;
                PosBuf[i] += MoveVec[i] * Time.deltaTime;
                Line.SetPositions(PosBuf);
            }
        }
    }
}
