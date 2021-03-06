using UnityEngine;

namespace Enemy
{
    public class EnemyBehavior : MonoBehaviour, IEnemy
    {
        [HideInInspector] public GameObject trackedObject;
        [HideInInspector] public GameObject groupParent;
        private AudioSource AS;

        [HideInInspector] public string currentState;
        [HideInInspector] public float health;
        [HideInInspector] public int score;
        [HideInInspector] public float scale;

        // bullet stuff
        [HideInInspector] public GameObject bulletPrefab;
        [HideInInspector] public float bulletSpeed, bulletDamage, bulletLifetime;
        [HideInInspector] public float fireRate, fireRateOffset;
        [HideInInspector] public float minTargetDistance, maxTargetDistance;
        private float _fireTimer;
    
        // swarming stuff
        [HideInInspector] public float swarmRadius, rotationSpeed;
        [HideInInspector] public Vector3 rotationAxis;

        [HideInInspector] public GameObject Explosion;


        private void Start()
        {
            AS = GetComponent<AudioSource>();
            fireRate += fireRateOffset;
            _fireTimer = Time.time;
            var center = groupParent.transform.position;
            var transformPosition = (Random.insideUnitSphere - center).normalized * swarmRadius + center;
            transform.position = transformPosition;
            transform.localScale = Vector3.one * scale;
        }

        private void Update()
        {
            var thisPosition = transform.position;
        
            // swarm around group parent
            var center = groupParent.transform.position;
            transform.RotateAround(center, rotationAxis, rotationSpeed * Time.deltaTime);
            var desiredPosition = (thisPosition - center).normalized * swarmRadius + center;
            thisPosition = desiredPosition;

            // check for range when pathing
            var targetPosition = trackedObject.transform.position;
            var toTarget = targetPosition - thisPosition;
            var distanceToTarget = toTarget.magnitude;
            var dot = Vector3.Dot(targetPosition, toTarget);
        
            if (currentState == "Pathing" && (distanceToTarget < minTargetDistance || distanceToTarget > maxTargetDistance || dot < 0))
            { 
                transform.LookAt(groupParent.transform.position + groupParent.transform.forward);
                _fireTimer = Time.time;
                return;
            }
        
            // look at player
            transform.LookAt(targetPosition);
        
            // fire
            if (Time.time - _fireTimer > fireRate)
            {
                AS.Play();
                _fireTimer = Time.time;
                var localTransform = transform;
                var bullet = Instantiate(bulletPrefab,
                    localTransform.position,
                    bulletPrefab.transform.rotation * localTransform.rotation).GetComponent<EnemyBullet>();
                bullet.speed = bulletSpeed;
                bullet.damage = bulletDamage;
                bullet.direction = localTransform.forward;
                bullet.lifeTime = bulletLifetime;
            }
        }

        public void TakeDamage(float damage)
        {
            health -= damage;
            if (health <= 0)
            {
                // Die
                Instantiate(Explosion, transform.position, transform.rotation);

                Destroy(gameObject);
            }
        }

        public float GetHealth() => health;
        
        public int GetScore() => score;
        
        public void SetTarget(GameObject target) => trackedObject = target;
    }
}
