using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] internal Player player;
    internal Transform TR;
    public int AmmoLeft = 60;
    public float FireRate = 0.25f;
    public float ShootTimeStamp = 0f;
    public bool canShootNow = true;
    public GameObject BulletPrefab;
    internal AudioSource ShootSFX;
    void Awake()
    {
        ShootSFX = GetComponent<AudioSource>();
        TR = transform;
        player.UI.AmmoCountText.text = AmmoLeft.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = player.playerCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - player.GunHolder.position;

        //var angle = Vector3.Angle(mousePos, player.GunHolder.position);
        //Debug.Log(angle);
        //player.GunHolder.localEulerAngles = new Vector3(0f, 0f, angle);
        if (player.Facing_Right && mousePos.x > 0f)
        {
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            player.GunHolder.localEulerAngles = new Vector3(0f, 0f, Mathf.Clamp(rotZ, -89f, 89f));
        }
        else if (!player.Facing_Right && mousePos.x < 0f)
        {
            rotation.x = Mathf.Abs(rotation.x) * -1f;
            float rotZ = Mathf.Atan2(rotation.y, -rotation.x) * Mathf.Rad2Deg;
            player.GunHolder.localEulerAngles = new Vector3(0f, 0f, Mathf.Clamp(rotZ, -89f, 89f));
        }

        ShootTimer();

        if (Input.GetMouseButton(0) && canShootNow && AmmoLeft > 0)
        {
            Shoot();
        }
    }
    void ShootTimer()
    {
        if (ShootTimeStamp < FireRate && !canShootNow)
        {
            ShootTimeStamp += Time.deltaTime;
        }
        else if (ShootTimeStamp >= FireRate && !canShootNow)
        {
            ShootTimeStamp = 0f;
            canShootNow = true;
        }
    }
    public float ShootForce = 20f;
    void Shoot()
    {
        canShootNow = false;
        AmmoLeft--;
        ShootSFX.Play();
        player.UI.AmmoCountText.text = AmmoLeft.ToString();
        var bullet = Instantiate(BulletPrefab);
        bullet.transform.position = TR.position + (TR.right * 0.5f);
        bullet.transform.rotation = player.GunHolder.rotation;
        bullet.GetComponent<Rigidbody2D>().velocity = ShootForce * TR.right;
    }
}
