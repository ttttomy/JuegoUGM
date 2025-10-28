using UnityEngine;

public class Gun : MonoBehaviour
{
    public float range = 20f;
    public float verticalRange = 20f;
    public float fireRate;
    public float BigDamage = 2f;
    public float SmallDamage = 1f;

    private float nextTimeToFire;
    private BoxCollider gunTrigger;

    public LayerMask raycastLayerMask;

    public EnemyManager enemyManager;

    void Start()
    {
        gunTrigger = GetComponent<BoxCollider>();
        gunTrigger.size = new Vector3(1, verticalRange, range);
        gunTrigger.center = new Vector3(0, 0, range * 0.5f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)&& Time.time > nextTimeToFire)
        {
            Fire();
        }
    }

    void Fire()
    {    
        //Daño Enemigos
        foreach (var enemy in enemyManager.enemiesInTrigger)
        {
            //Obtiene dirección al enemigo
            var dir = enemy.transform.position - transform.position;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, range * 1.5f, raycastLayerMask))
            {
                if (hit.transform == enemy.transform)
                {
                    //Rango
                    float dist = Vector3.Distance(enemy.transform.position, transform.position);

                    if (dist > range * 0.5f)
                    {
                        //Aplica poco daño
                        enemy.TakeDamage(SmallDamage);
                    }
                    else
                    {
                        //Aplica mucho daño
                        enemy.TakeDamage(BigDamage);
                    }

                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Añade potencial de disparar al enemigo
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy)
        {
            enemyManager.AddEnemy(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Quita el potencial de disparar al enemigo
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy)
        {
            enemyManager.RemoveEnemy(enemy);
        }
    }
}
