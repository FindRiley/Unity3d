using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory {
    public GameObject disk;
    public static DiskFactory diskFactory = new DiskFactory();
    public float baseSpeedX = 25, maxSpeedY = 8, maxSpeedZ = 5;

    private Dictionary<int, Disk> used = new Dictionary<int, Disk>();   // flying disks
    private List<Disk> free = new List<Disk>();     // disks not in use
    int count;

    private DiskFactory() {
        disk = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Disk"));
        disk.AddComponent<Disk>();
        disk.SetActive(false);
        count = 0;
    }

    public void FreeDisk() {
        foreach (Disk di in used.Values) {
            if (!di.gameObject.activeSelf) {
                used.Remove(di.GetInstanceID());
                free.Add(di);
                return;
            }
        }
    }

    public Disk GetDisk(int round) {
        FreeDisk();
        Disk newDisk;
        GameObject diskObject = null;
        if (free.Count > 0) {
            diskObject = free[0].gameObject;
            free.Remove(free[0]);
        } else {
            ++count;
            diskObject = GameObject.Instantiate<GameObject>(disk, Vector3.zero, Quaternion.identity);
        }
        diskObject.SetActive(true);
        newDisk = diskObject.AddComponent<Disk>();

        float startDir = Random.Range(-1.0f, 1.0f);   // (-1, 0) left; (0, 1) right
        float speedX = Random.Range(baseSpeedX + round * 3, baseSpeedX + round * 10);        // rule of speedx
        float startX = Random.Range(75, 100);
        if (startDir > 0) {
            speedX = -speedX;   // begin from right, startX > 0, speedX < 0
        } else {
            startX = -startX;   // begin from left, starX < 0, speedX > 0
        }
        /* set random color(r, g, b), speed(x, y, z), startPoint(x, y, z), rotated Direction(x, y, z) */
        newDisk.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        newDisk.Speed = new Vector3(speedX, Random.Range(0.0f, maxSpeedY), Random.Range(-maxSpeedZ, maxSpeedZ));
        newDisk.StartPoint = new Vector3(startX, Random.Range(5, 35), Random.Range(25, 45));
        newDisk.Direction = new Vector3(Random.Range(-75, -25), 0, 0);
        used.Add(newDisk.GetInstanceID(), newDisk);
        newDisk.name = "disk" + count.ToString();
        newDisk.setSpeedX(speedX);
        return newDisk;
    }
}
