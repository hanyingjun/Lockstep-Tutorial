namespace vwengame.bephysics.example
{
    using UnityEngine;

    public class PlatformX : MonoBehaviour
    {
        public float speed = 1;
        public int m_dir = 1;
        private PhyEntityBox boxEntity = null;
        public float timer = 0;

        // Start is called before the first frame update
        void Start()
        {
            boxEntity = this.GetComponent<PhyEntityBox>();
        }

        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= 2)
            {
                timer = timer - 2;
                m_dir = m_dir * (-1);
            }
            boxEntity.Velocity = new BEPUutilities.Vector3(m_dir * speed, boxEntity.Velocity.Y, boxEntity.Velocity.Z);
        }
    }
}
