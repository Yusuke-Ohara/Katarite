using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private skiing_movement skimove;
    public GameObject Particle_right;
    public GameObject Particle_left;
    private ParticleSystem Particle_property_right;
    private ParticleSystem Particle_property_left;
   
    // Start is called before the first frame update
    void Start()
    {
        skimove = GetComponent<skiing_movement>();
        Particle_property_right = Particle_right.GetComponentInChildren<ParticleSystem>();
        Particle_property_left = Particle_left.GetComponentInChildren<ParticleSystem>();
        //EmissionModule = Particle_property.emission;

    }

    // Update is called once per frame
    void Update()
    {
        Particle_right.transform.rotation = Quaternion.Euler(0, 90 * skimove.direction_into_particle(), 0);
        Particle_left.transform.rotation = Quaternion.Euler(0, 90 * skimove.direction_into_particle(), 0);
        var em_right = Particle_property_right.emission;
        var em_left = Particle_property_left.emission;
        if (skimove.ground_judge() == true)
        {
            if (skimove.direction_into_particle() < 0)
            {
                em_right.rateOverTime = -720 * skimove.direction_into_particle();
            }
            else if (skimove.direction_into_particle() > 0)
            {
                em_left.rateOverTime = 720 * skimove.direction_into_particle() * skimove.direction_into_particle();
            }
        }
        else
        {
            em_right.rateOverTime = 0;
            em_left.rateOverTime = 0;
        }
    }
}
