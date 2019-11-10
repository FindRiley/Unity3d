using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct frame {
    public Vector3 dir;
    public float dis;

    public frame(Vector3 dir_, float dis_) {
        dir = dir_;
        dis = dis_;
    }
}

public class Path : MonoBehaviour {
    public ParticleSystem particle;
    public List<Vector3> pos;

    AnimationCurve curveX = new AnimationCurve();
    AnimationCurve curveY = new AnimationCurve();
    AnimationCurve curveZ = new AnimationCurve();
    Queue<frame> frames = new Queue<frame>();

    void Start() {
        if (pos.Count < 1)
            return;
        float totalDistance = 0, curDis = 0;
        Vector3 dir;
        particle.transform.position = new Vector3(0, 0, -2);
        for (int i = 1; i < pos.Count; ++i) {
            curDis = Vector3.Distance(pos[i], pos[i - 1]);
            dir = pos[i] - pos[i - 1];
            //dir.Normalize();
            frames.Enqueue(new frame(dir, curDis));

            totalDistance += curDis;
        }
        float time = 0;
        while (frames.Count > 0) {
            frame data = frames.Dequeue();
            curveX.AddKey(new Keyframe(time, data.dir.x, float.PositiveInfinity, float.PositiveInfinity));
            curveY.AddKey(new Keyframe(time, data.dir.y, float.PositiveInfinity, float.PositiveInfinity));
            curveZ.AddKey(new Keyframe(time, data.dir.z, float.PositiveInfinity, float.PositiveInfinity));
            time += data.dis / totalDistance;
        }

        var velocity = particle.velocityOverLifetime;
        velocity.enabled = true;

        float scale = 50 / particle.startLifetime;
        velocity.space = ParticleSystemSimulationSpace.Local;
        velocity.x = new ParticleSystem.MinMaxCurve(1, curveX);
        velocity.y = new ParticleSystem.MinMaxCurve(1, curveY);
        velocity.z = new ParticleSystem.MinMaxCurve(1, curveZ);
    }
}
