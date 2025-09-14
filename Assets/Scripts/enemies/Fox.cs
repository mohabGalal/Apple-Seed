using UnityEngine;

public class fox : BaseEnemy
{
    public GameObject BulletPreFab;
    public Transform BulletSpawner;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    override protected void Update()
    {

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < 6)
        {
            Attack();
        }
    }
    protected override void Move() { }
    public override void Attack() {

        Instantiate(BulletPreFab, BulletSpawner.position, Quaternion.identity);


    
     
    
    
    }
}
