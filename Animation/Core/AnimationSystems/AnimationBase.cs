﻿using g2.Animation.Core.ParticleSystems;
using g2.Animation.Core.Timing;
using g2.Datastructures.Geometry;
using System.Diagnostics;
using System.Timers;

namespace g2.Animation.Core.AnimationSystems;
// ToDo: Add Boundary for canvas maybe move checking for boundaries in box like quadtree?  or BoundaryCheckc class?  efficiant boundary checks (k d tree?)

public class AnimationBase
{
    private const int PARTICLESCOUNT = 500;

    private readonly FPSCounter fpsCounter;
    private readonly Quadrant quadrant;
    private readonly Particle[] particles;

    private bool updateRunning;
    private bool fixedUpdateRunning;

    public AnimationBase(FPSCounter fpsCounter, double width, double height)
    {
        this.fpsCounter = fpsCounter;
        quadrant = new(0, 0, width, height);
        particles = new Particle[PARTICLESCOUNT];

        Random random = new();

        for (int i = 0; i < PARTICLESCOUNT; i++)
        {
            double x = (random.NextDouble() * width);
            double y = (random.NextDouble() * height);

            Particle particle = new(x, y, 5, 5, quadrant)
            {
                //Velocity = new Vector2D((random.NextDouble() * 150) - 75, (random.NextDouble() * 150) - 75),
                Velocity = new Vector2D(100, 0),

                Acceleration = new Vector2D(0, 0)
            };

            particles[i] = particle;
        }
    }

    public Particle[] Particles
    {
        get => particles;
    }

    public Task Loop()
    {
        return Task.Run(() =>
        {
            // ToDo: remove discard an only return completetd if both returned completed?
            _ = UpdateAsync();
            _ = FixedUpdateAsync();
        });
    }

    private async Task UpdateAsync()
    {
        updateRunning = true;

        await Task.Run(() =>
        {
            while (updateRunning)
            {
                Time.Delta();

                fpsCounter.Update();




                _ = (UpdateComplete?.Invoke(null, EventArgs.Empty));
            }
        });
    }

    private async Task FixedUpdateAsync()
    {
        fixedUpdateRunning = true;
        Time.StarPeriodicTimer(1 / 100.0);

        await Task.Run(async () =>
        {
            while (fixedUpdateRunning && Time.PeriodicTimer is not null && await Time.PeriodicTimer.WaitForNextTickAsync())
            {
                Time.FixedDelta();

                fpsCounter.FixedUpdate();

                for (int i = 0; i < particles.Length; i++)
                {
                    particles[i].FixedUpdate();
                    particles[i].CheckBoundaries();
                }

                _ = (FixedUpdateComplete?.Invoke(null, EventArgs.Empty));
                //Debug.WriteLine($"FixedUpdate: {DateTime.Now:O} \t FixedDetlatatime: {Time.FixedDeltaTime:G35}");
            }
        });
    }

    public void Pause()
    {
        updateRunning = false;
        fixedUpdateRunning = false;
    }

    public event Func<object?, EventArgs, Task>? FixedUpdateComplete;
    public event Func<object?, EventArgs, Task>? UpdateComplete;

}

