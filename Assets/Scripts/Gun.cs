using UnityEngine;


public class Gun : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] private Transform bulletExitPoint; // mermýnýn fýrlatýlacagý yer
    [SerializeField] private LayerMask layer;
    [SerializeField] private GunData data;
    public Gun nextGun => data.NextGun;
    private RaycastHit hit;
    [SerializeField] private float _attackRate;
    public int level;
    private void Start()
    {
        level = data.Level;
        _attackRate = data.AttackRate;
    }
    private void Update()
    {
        Debug.DrawRay(transform.position, -transform.right*10, Color.red);
        _attackRate -= Time.deltaTime;
        if(_attackRate < 0)
        {
            Fire(TargetBrick());
        }
    }
    public Brick TargetBrick()
    {
        if (Physics.Raycast(transform.position, -transform.right, out hit, 100f, layer))
        {
            if (hit.collider.tag == "Brick")
            {
                hit.collider.enabled = false;
                return hit.collider.gameObject.GetComponent<Brick>();
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
        obj.GetComponent<Bullet>().targetBrick = targetBrick;
        obj.GetComponent<Bullet>().bulletExitPoint = bulletExitPoint;
        obj.transform.localRotation = this.transform.localRotation;
        obj.transform.position = bulletExitPoint.position;
        obj.SetActive(true);
        obj.GetComponent<Bullet>().earnedMoney = data.EarndMoneyPerFire;    
        //StartCoroutine(obj.GetComponent<Bullet>().Fire(targetBrick, bulletExitPoint));
    }
}
