using UnityEngine;

public class PlayParticle : MonoBehaviour
{
    public ParticleSystem[] particlesToPlay;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (var p in particlesToPlay)
            {
                p.Play();
            }
        }
    }
}
