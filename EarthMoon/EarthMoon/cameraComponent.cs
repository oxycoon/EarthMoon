using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace EarthMoon
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class CameraComponent : Microsoft.Xna.Framework.GameComponent
    {
        private IInputHandler input;
        private GraphicsDeviceManager graphics;

        private Matrix projection;
        private Matrix view;

        private Vector3 camPos = new Vector3(0.0f, 0.0f, 3.0f);
        private Vector3 camTar = Vector3.Zero;

        private Vector3 camUpVec = Vector3.Up;

        private Vector3 camRef = new Vector3(0.0f, 0.0f, -0.0f);

        private float camYaw = 0.0f;

        private const float spinRate = 10.0f;

        public Matrix View
        {
            get { return view; }
            set { view = value;}
        }

        public Matrix Projection
        {
            get { return projection; }
            set { projection = value; }
        }

        public Vector3 CamPos
        {
            get { return camPos; }
            set 
            { 
                camPos = value;
                camRef = ((-1.0f) * camPos);
                camRef.Normalize();
            }
        }
        public Vector3 CamTar
        {
            get { return camTar; }
            set { camTar = value; }
        }

        public CameraComponent(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            graphics = (GraphicsDeviceManager)Game.Services.GetService(typeof(IGraphicsDeviceManager));
            input = (IInputHandler)game.Services.GetService(typeof(IInputHandler));

        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
            initCam();
        }

        private void initCam()
        {
            float aspectRatio = (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height;

            camRef = ((-1.0f) * camPos);
            camRef.Normalize();

            Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1.0f, 1000.0f, out projection);
            Matrix.CreateLookAt(ref camPos, ref camTar, ref camUpVec, out view);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            /*if(input.KeyboardState.IsKeyDown(Keys.))
            {

            }*/

            if(input.KeyboardState.IsKeyDown(Keys.Up))
            {

            }

            if(input.KeyboardState.IsKeyDown(Keys.Down))
            {

            }

            if(input.KeyboardState.IsKeyDown(Keys.Left))
            {
                camYaw = camYaw + (spinRate * timeDelta);
            }

            if(input.KeyboardState.IsKeyDown(Keys.Right))
            {
                camYaw = camYaw - (spinRate * timeDelta);
            }

            if(input.KeyboardState.IsKeyDown(Keys.W))
            {

            }

            if(input.KeyboardState.IsKeyDown(Keys.S))
            {

            }

            if (camYaw > 360)
            {
                camYaw -= 360;
            }
            else if (camYaw < 0)
            {
                camYaw += 360;
            }

            Matrix rotMatrix;
            Matrix.CreateRotationY(MathHelper.ToRadians(camYaw), out rotMatrix);

            Vector3 transformedRef;
            Vector3.Transform(ref camPos, ref rotMatrix, out transformedRef);

            base.Update(gameTime);
        }
    }
}
