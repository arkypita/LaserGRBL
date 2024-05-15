using System;
using System.Collections;

using SharpGL.SceneGraph.Collections;
using SharpGL.SceneGraph.Core;

namespace SharpGL.SceneGraph.ParticleSystems
{
    /// <summary>
    /// This is the main particle class, if you want specialised particles derive from it.
    /// </summary>
    [Serializable()]
    public abstract class Particle
    {
        /// <summary>
        /// This function should initialise the particle so that it's ready to tick.
        /// </summary>
        /// <param name="rand">The random number generator.</param>
        public abstract void Intialise(System.Random rand);

        /// <summary>
        /// This function moves the particle on a stage.
        /// </summary>
        /// <param name="rand">The random nunber generator.</param>
        public abstract void Tick(System.Random rand);

        /// <summary>
        /// This function draws the particle.
        /// </summary>
        /// <param name="gl"></param>
        public abstract void Draw(OpenGL gl);
    }

    /// <summary>
    /// A basic particle.
    /// </summary>
    public class BasicParticle : Particle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicParticle"/> class.
        /// </summary>
        public BasicParticle()
        {
        }

        /// <summary>
        /// This function initialises the basic particle.
        /// </summary>
        /// <param name="rand"></param>
        public override void Intialise(System.Random rand)
        {
            position.Set(0, 0, 0);
            velocity.Set(0, 0, 0);

            color.R = rand.Next(256) / 256.0f;
            color.G = rand.Next(1) / 256.0f;
            color.B = rand.Next(1) / 256.0f;

            life = (rand.Next(100) / 1000.0f) + 0.03f;
            direction.X += rand.Next(10) / 100.0f;
            direction.Y += rand.Next(10) / 100.0f;
            direction.Z += rand.Next(10) / 100.0f;
        }

        /// <summary>
        /// This function 'ticks' the particle, i.e moves it on a stage.
        /// </summary>
        /// <param name="rand">A random object.</param>
        public override void Tick(System.Random rand)
        {
            //	Randomise the direction.
            direction.X = directionRandomise.X - (2 * (float)rand.NextDouble() * directionRandomise.X);
            direction.Y = directionRandomise.Y - (2 * (float)rand.NextDouble() * directionRandomise.Y);
            direction.Z = directionRandomise.Z - (2 * (float)rand.NextDouble() * directionRandomise.Z);

            //	Now we randomise the color.
            color.R += colorRandomise.R - (2 * (float)rand.NextDouble() * colorRandomise.R);
            color.G += colorRandomise.G - (2 * (float)rand.NextDouble() * colorRandomise.G);
            color.B += colorRandomise.B - (2 * (float)rand.NextDouble() * colorRandomise.B);
            color.A += colorRandomise.A - (2 * (float)rand.NextDouble() * colorRandomise.A);

            //	First we update the velocity.
            velocity += direction;
            velocity += gravity;

            //	Now we move the particle.
            position += velocity;
            life -= lifespan;
        }

        public override void Draw(OpenGL gl)
        {
            gl.Color(color.R, color.G, color.B, life);

            gl.Begin(OpenGL.GL_TRIANGLE_STRIP);
            gl.Vertex(position.X + 0.1f, position.Y + 0.1f, position.Z + 0.1f);
            gl.Vertex(position.X - 0.1f, position.Y + 0.1f, position.Z);
            gl.Vertex(position.X + 0.1f, position.Y - 0.1f, position.Z);
            gl.Vertex(position.X - 0.1f, position.Y - 0.1f, position.Z);

            gl.End();
        }

        #region Member Data

        /// <summary>
        /// This is the vertex's current position in space.
        /// </summary>
        protected Vertex position = new Vertex(0, 0, 0);

        /// <summary>
        ///	This is the velocity, do not modify!
        /// </summary>
        protected Vertex velocity = new Vertex(0, 0, 0);

        /// <summary>
        /// This is the direction of the particle.
        /// </summary>
        protected Vertex direction = new Vertex(0, 0, 0);

        /// <summary>
        /// This shows the potential magnitude of the random effects of the direction.
        /// </summary>
        protected Vertex directionRandomise = new Vertex(0.1f, 0.1f, 0.1f);

        /// <summary>
        /// This is the gravity affecting the particle.
        /// </summary>
        protected Vertex gravity = new Vertex(0, -0.1f, 0);

        /// <summary>
        /// Particles colour.
        /// </summary>
        protected GLColor color = new GLColor(1, 0, 0, 0);

        /// <summary>
        /// How much the particles colour can change by each tick.
        /// </summary>
        protected GLColor colorRandomise = new GLColor(0.1f, 0.1f, 0.1f, 0);

        /// <summary>
        /// The life left of the particle.
        /// </summary>
        protected float life = 1;

        /// <summary>
        /// The lifespan of the particle.
        /// </summary>
        protected float lifespan = 0.01f;

        /// <summary>
        /// Does the particle only exist once?
        /// </summary>
        protected bool dieForever = false;

        #endregion

        #region Properties

        public Vertex Position
        {
            get { return position; }
            set { position = value; }
        }
        public Vertex Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        public Vertex Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        public Vertex DirectionRandomise
        {
            get { return directionRandomise; }
            set { directionRandomise = value; }
        }
        public Vertex Gravity
        {
            get { return gravity; }
            set { gravity = value; }
        }
        public GLColor Color
        {
            get { return color; }
            set { color = value; }
        }
        public GLColor ColorRandomise
        {
            get { return colorRandomise; }
            set { colorRandomise = value; }
        }
        public float Life
        {
            get { return life; }
            set { life = value; }
        }
        public float LifeSpan
        {
            get { return lifespan; }
            set { lifespan = value; }
        }
        public bool DieForever
        {
            get { return dieForever; }
            set { dieForever = value; }
        }

        #endregion
    }

    /// <summary>
    /// A Sphere particle.
    /// </summary>
    public class SphereParticle : BasicParticle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SphereParticle"/> class.
        /// </summary>
        public SphereParticle()
        {
        }

        /// <summary>
        /// Draws the specified gl.
        /// </summary>
        /// <param name="gl">The gl.</param>
        public override void Draw(OpenGL gl)
        {
            //	Create the sphere if need be.
            if (sphere == null)
                sphere = new Quadrics.Sphere();

            //	Set the properties.
            sphere.Transformation.TranslateX = position.X;
            sphere.Transformation.TranslateY = position.Y;
            sphere.Transformation.TranslateZ = position.Z;

            sphere.Render(gl, Core.RenderMode.Design);
        }

        protected static Quadrics.Sphere sphere = null;
    }
}