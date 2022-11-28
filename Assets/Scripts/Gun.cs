using UnityEngine;


public class Gun : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] private Transform bulletExitPoint; // mermýnýn fýrlatýlacagý yer
    [SerializeField] private LayerMask layer;
    [SerializeField] private GunData data;
    private RaycastHit hit;
    [SerializeField] private float _attackRate;
    private void Start()
    {
        _attackRate = data.AttackRate;
    }
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.up*10, Color.red);
        _attackRate -= Time.deltaTime;
        if(_attackRate < 0)
        {
            Fire(TargetBrick());
        }
    }
    private Brick TargetBrick()
    {
        if (Physics.Raycast(transform.position, transform.up, out hit, 100f, layer))
        {
            if (hit.collider.tag == "Brick")
            {
                hit.collider.enabled = false;
                return hit.collider.GetComponent<Brick>();
            }
        }
        return null;
    }
    private void Fire(Brick targetBrick)
    {
        if (targetBrick == null)
            return;
        _attackRate = data.AttackRate;
        var obj = ObjectPool.Instance.GetPooledObject(0);
        obj.transform.localRotation = this.transform.localRotation;
        obj.transform.position = bulletExitPoint.position;
        obj.SetActive(true);
        StartCoroutine(obj.GetComponent<Bullet>().Fire(targetBrick, bulletExitPoint));
    }
}
