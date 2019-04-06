using UniRx;
using UnityEngine;
using System.Collections;

namespace Player
{
    public class PlayerColorMono : MonoBehaviour, IPlayerColor
    {
        [SerializeField] private Color m_Default, m_Damaged;
        [SerializeField] private float m_FlashingSpeed = 5f;
        private Material m_Material;
        private Coroutine m_Coroutine;

        private bool m_Init = false;

        public void Bind(Health health)
        {
            if (!m_Init) Init();
            health.Immortal
                .Subscribe(SetFlashing);
        }

        public void Reset()
        {
            if (m_Coroutine != null)
            {
                StopCoroutine(m_Coroutine);
                m_Coroutine = null;
            }
            m_Material.color = m_Default;
        }

        private void SetFlashing(bool state)
        {
            if (state)
            {
                m_Coroutine = StartCoroutine(Flashing());
            }
            else
            {
                if (m_Coroutine != null)
                {
                    StopCoroutine(m_Coroutine);
                    m_Coroutine = null;
                }
                m_Material.color = m_Default;
            }
        }

        private void Awake()
        {
            if (!m_Init) Init();
        }

        private void Init()
        {
            var rend = GetComponent<Renderer>();
            m_Material = rend.material;
            m_Init = true;
        }

        private IEnumerator Flashing()
        {
            float timer = 0f, sign = 1f;
            while (true)
            {
                timer += Time.deltaTime * m_FlashingSpeed * sign;
                if (timer > 1f || timer < 0f)
                {
                    timer = Mathf.Clamp01(timer);
                    sign *= -1f;
                }
                m_Material.color = Color.Lerp(m_Default, m_Damaged, timer);
                yield return null;
            }
        }
    }
}