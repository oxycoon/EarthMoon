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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private ContentManager content;
        private GraphicsDevice device;

        private SpriteFont font;

        private BasicEffect effect;

        private Matrix world, view, projection;

        private Vector3 camPos = new Vector3(700.0f, 10.0f, 0.0f);
        private Vector3 camTar = Vector3.Zero;
        private Vector3 camUpVec = Vector3.Up;

        private SpriteBatch spriteBatch;

        private Stack<Matrix> matrixStrack = new Stack<Matrix>();

        private float earthRotY = 0.0f;
        private float moonRotY = 0.0f;
        private float moonOrbitY = 0.0f;
        private float sunRotY = 0.0f;

        private Model mEarth;
        private Model mMoon;
        private Model mStar;

        // Planets
        private Planet[] planetArray = new Planet[8];
        
        private float re_mercury = 2439.7f, rp_mercury = 2439.7f;        //radius for mercury
        private float re_venus = 6051.8f, rp_venus = 6051.8f;            //radius for venus
        private float re_terra = 6378.1f, rp_terra = 6356.8f;            //radius for earth
        private float re_mars = 3396.2f, rp_mars = 3376.2f;              //radius for mars
        private float re_jupiter = 71492.0f, rp_jupiter = 66854.0f;      //radius for jupiter
        private float re_saturn = 60268.0f, rp_saturn = 54364.0f;        //radius for saturn
        private float re_uranus = 25559.0f, rp_uranus = 24973.0f;        //radius for uranus
        private float re_neptune = 24764.0f, rp_neptune = 24341.0f;      //radius for neptune

        private float dc_mercury = 6.98f * (float)Math.Pow(10, 7), df_mercury = 4.60f * (float)Math.Pow(10, 7);     //distance from sol at closest and furthest
        private float dc_venus = 1.075f * (float)Math.Pow(10, 8), df_venus = 1.098f * (float)Math.Pow(10, 8);       //distance from sol at closest and furthest
        private float dc_terra = 1.471f * (float)Math.Pow(10, 8), df_terra = 1.521f * (float)Math.Pow(10, 8);       //distance from sol at closest and furthest
        private float dc_mars = 2.067f * (float)Math.Pow(10, 8), df_mars = 2.491f * (float)Math.Pow(10, 8);         //distance from sol at closest and furthest
        private float dc_jupiter = 7.409f * (float)Math.Pow(10, 8), df_jupiter = 8.157f * (float)Math.Pow(10, 8);   //distance from sol at closest and furthest
        private float dc_saturn = 1.384f * (float)Math.Pow(10, 9), df_saturn = 1.503f * (float)Math.Pow(10, 9);     //distance from sol at closest and furthest
        private float dc_uranus = 2.739f * (float)Math.Pow(10, 9), df_uranus = 3.003f * (float)Math.Pow(10, 9);     //distance from sol at closest and furthest
        private float dc_neptune = 4.456f * (float)Math.Pow(10, 9), df_neptune = 4.546f * (float)Math.Pow(10, 9);   //distance from sol at closest and furthest

        private double rot_mercury = 1407.5;        //time in hours taken to spin once around it's own axis. Sideral day
        private double rot_venus = 5832.0;          //time in hours taken to spin once around it's own axis. Sideral day
        private double rot_terra = 23.93;           //time in hours taken to spin once around it's own axis. Sideral day
        private double rot_mars = 24.66;
        private double rot_jupiter = 9.0;
        private double rot_saturn = 10.75;
        private double rot_uranus = 17.24;
        private double rot_neptune = 16.11;

        // speed of the planets in km/s
        private float speed_mercury = 47.8725f;
        private float speed_venus = 35.0214f;
        private float speed_terra = 29.7859f;
        private float speed_mars = 24.1309f;
        private float speed_jupiter = 13.0697f;
        private float speed_saturn = 9.6724f;
        private float speed_uranus = 6.8352f;
        private float speed_neptune = 5.4778f;
        
        /*
        private Planet Mercury;
        private Planet Venus;
        private Planet Earth;
        private Planet Mars;
        private Planet Jupiter;
        private Planet Saturn;
        private Planet Uranus;
        private Planet Neptun;
        */
        private Model temp;

        private KeyboardState kbState;
        private bool isFullScreen = false;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            /*
            Mercury =   new Planet("Mercury", temp, 0.5f, new Vector3(5.0f, 0.0f, 0.0f), 0.4f, 4.4f);
            Venus =     new Planet("Venus", temp, 0.5f, new Vector3(10.0f, 0.0f, 0.0f), 0.4f, 4.4f);
            Earth =     new Planet("Earth", temp, 0.5f, new Vector3(15.0f, 0.0f, 0.0f), 0.4f, 4.4f);
            Mars =      new Planet("Mars", temp, 0.5f, new Vector3(20.0f, 0.0f, 0.0f), 0.4f, 4.4f);
            Jupiter =   new Planet("Jupiter", temp, 0.5f, new Vector3(25.0f, 0.0f, 0.0f), 0.4f, 4.4f);
            Saturn =    new Planet("Saturn", temp, 0.5f, new Vector3(30.0f, 0.0f, 0.0f), 0.4f, 4.4f);
            Uranus =    new Planet("Uranus", temp, 0.5f, new Vector3(35.0f, 0.0f, 0.0f), 0.4f, 4.4f);
            Neptun =    new Planet("Neptun", temp, 0.5f, new Vector3(40.0f, 0.0f, 0.0f), 0.4f, 4.4f);
            */
            //                          Name        Model Scale    Distance from sun                RS    OS
            planetArray[0] = new Planet("Mercury", temp, 1f, new Vector3(50.0f, 0.0f, 0.0f), 0.4f, 0.7f, 0);
            planetArray[1] = new Planet("Venus", temp, 1f, new Vector3(75.0f, 0.0f, 0.0f), 0.4f, 0.6f, 0);

            planetArray[2] = new Planet("Earth", temp, 1.0f, new Vector3(100.0f, 0.0f, 0.0f), 0.06f, 0.01f, 1);
            planetArray[2].MoonArray[0] = new Moon("The Moon", temp, 0.5f, new Vector3(2.0f, 0.0f, 0.0f), 0.1f, 0.1f);

            planetArray[3] = new Planet("Mars", temp, 1f, new Vector3(125.0f, 0.0f, 0.0f), 0.4f, 0.05f, 2);
            planetArray[3].MoonArray[0] = new Moon("Deimos", temp, 0.5f, new Vector3(2.0f, 0.0f, 0.0f), 0.1f, 2.0f);
            planetArray[3].MoonArray[1] = new Moon("Phobos", temp, 0.5f, new Vector3(4.0f, 0.0f, 0.0f), 0.1f, 0.1f);

            planetArray[4] = new Planet("Jupiter", temp, 1f, new Vector3(150.0f, 0.0f, 0.0f), 0.4f, 0.04f, 4);
            planetArray[4].MoonArray[0] = new Moon("Lo", temp, 0.5f, new Vector3(1.0f, 0.0f, 0.0f), 0.1f, 1.0f);
            planetArray[4].MoonArray[1] = new Moon("Europa", temp, 0.5f, new Vector3(3.0f, 0.0f, 0.0f), 0.1f, 0.2f);
            planetArray[4].MoonArray[2] = new Moon("Genymede", temp, 0.5f, new Vector3(5.0f, 0.0f, 0.0f), 0.1f, 0.7f);
            planetArray[4].MoonArray[3] = new Moon("Callisto", temp, 0.5f, new Vector3(7.0f, 0.0f, 0.0f), 0.1f, 0.1f);

            planetArray[5] = new Planet("Saturn", temp, 1f, new Vector3(175.0f, 0.0f, 0.0f), 0.4f, 0.03f, 9);
            planetArray[5].MoonArray[0] = new Moon("Mimas", temp, 0.5f, new Vector3(1.0f, 0.0f, 0.0f), 0.1f, 0.1f);
            planetArray[5].MoonArray[1] = new Moon("Enceladus", temp, 0.5f, new Vector3(2.0f, 0.0f, 0.0f), 0.1f, 0.2f);
            planetArray[5].MoonArray[2] = new Moon("Tethys", temp, 0.5f, new Vector3(3.0f, 0.0f, 0.0f), 0.1f, 0.3f);
            planetArray[5].MoonArray[3] = new Moon("Dione", temp, 0.5f, new Vector3(4.0f, 0.0f, 0.0f), 0.1f, 0.4f);
            planetArray[5].MoonArray[4] = new Moon("Rhea", temp, 0.5f, new Vector3(5.0f, 0.0f, 0.0f), 0.1f, 0.5f);
            planetArray[5].MoonArray[5] = new Moon("Titan", temp, 0.5f, new Vector3(6.0f, 0.0f, 0.0f), 0.1f, 0.6f);
            planetArray[5].MoonArray[6] = new Moon("Hyperion", temp, 0.5f, new Vector3(7.0f, 0.0f, 0.0f), 0.1f, 0.7f);
            planetArray[5].MoonArray[7] = new Moon("Lapetus", temp, 0.5f, new Vector3(8.0f, 0.0f, 0.0f), 0.1f, 0.8f);
            planetArray[5].MoonArray[8] = new Moon("Phoebe", temp, 0.5f, new Vector3(9.0f, 0.0f, 0.0f), 0.1f, 0.9f);

            planetArray[6] = new Planet("Uranus",   temp,  1f, new Vector3(200.0f, 0.0f, 0.0f), 0.4f, 0.02f, 6);
            planetArray[6].MoonArray[0] = new Moon("Puck", temp, 0.5f, new Vector3(1.0f, 0.0f, 0.0f), 0.1f, 0.2f);
            planetArray[6].MoonArray[1] = new Moon("Miranda", temp, 0.5f, new Vector3(2.0f, 0.0f, 0.0f), 0.1f, 0.5f);
            planetArray[6].MoonArray[2] = new Moon("Ariel", temp, 0.5f, new Vector3(3.0f, 0.0f, 0.0f), 0.1f, 0.3f);
            planetArray[6].MoonArray[3] = new Moon("Umbriel", temp, 0.5f, new Vector3(4.0f, 0.0f, 0.0f), 0.1f, 0.6f);
            planetArray[6].MoonArray[4] = new Moon("Titania", temp, 0.5f, new Vector3(5.0f, 0.0f, 0.0f), 0.1f, 0.1f);
            planetArray[6].MoonArray[5] = new Moon("Oberon", temp, 0.5f, new Vector3(6.0f, 0.0f, 0.0f), 0.1f, 0.4f);

            planetArray[7] = new Planet("Neptun",   temp,  1f, new Vector3(225.0f, 0.0f, 0.0f), 0.4f, 0.01f, 3);
            planetArray[7].MoonArray[0] = new Moon("Proteus", temp, 0.5f, new Vector3(1.0f, 0.0f, 0.0f), 0.1f, 0.5f);
            planetArray[7].MoonArray[1] = new Moon("Triton", temp, 0.5f, new Vector3(2.0f, 0.0f, 0.0f), 0.1f, 0.2f);
            planetArray[7].MoonArray[2] = new Moon("Nereid", temp, 0.5f, new Vector3(3.0f, 0.0f, 0.0f), 0.1f, 1.0f);
             
            this.IsFixedTimeStep = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            InitDevice();
            InitCamera();
        }

        private void InitDevice()
        {
            device = graphics.GraphicsDevice;

            //Setter størrelse på framebuffer:
            graphics.PreferredBackBufferWidth = 1400;
            graphics.PreferredBackBufferHeight = 800;
            graphics.IsFullScreen = isFullScreen;
            graphics.ApplyChanges();

            Window.Title = "En enkel kube...";

            //Initialiserer Effect-objektet:
            effect = new BasicEffect(graphics.GraphicsDevice);
            effect.VertexColorEnabled = false;
        }

        private void InitCamera()
        {
            float aspectRatio = (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height;
            Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.01f, 1000.0f, out projection);
            Matrix.CreateLookAt(ref camPos, ref camTar, ref camUpVec, out view);
            effect.Projection = projection;
            effect.View = view;
            effect.VertexColorEnabled = true;

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            font = Content.Load<SpriteFont>("font");

            // TODO: use this.Content to load your game content here
            mStar = Content.Load<Model>("sphere");
            (mStar.Meshes[0].Effects[0] as BasicEffect).EnableDefaultLighting();

            //Mercury.PlanetModel = Content.Load<Model>("sphere");
            //(Mercury.PlanetModel.Meshes[0].Effects[0] as BasicEffect).EnableDefaultLighting();

            foreach(Planet p in planetArray)
            {
                p.PlanetModel = Content.Load<Model>("sphere");
                (p.PlanetModel.Meshes[0].Effects[0] as BasicEffect).EnableDefaultLighting();
            }
 

            /*
            mEarth = Content.Load<Model>("sphere");
            mMoon = Content.Load<Model>("sphere");
            (mEarth.Meshes[0].Effects[0] as BasicEffect).EnableDefaultLighting();
            (mMoon.Meshes[0].Effects[0] as BasicEffect).EnableDefaultLighting();*/
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            kbState = Keyboard.GetState();

            if (kbState.IsKeyDown(Keys.Down))
            {
                camPos.X += 10.0f;
                camTar.X += 10.0f;  
            }

            if (kbState.IsKeyDown(Keys.Up))
            {
                camPos.X -= 10.0f;
                camTar.X -= 10.0f;
            }

            if (kbState.IsKeyDown(Keys.Left))
            {
                camPos.Z += 10.0f;
                camTar.Z += 10.0f;
            }

            if (kbState.IsKeyDown(Keys.Right))
            {
                camPos.Z -= 10.0f;
                camTar.Z -= 10.0f;
            }

            if (kbState.IsKeyDown(Keys.Q))
            {
                camPos.Y += 0.2f;
                camTar.Y += 0.2f;
            }

            if (kbState.IsKeyDown(Keys.E))
            {
                camPos.Y -= 0.2f;
                camTar.Y -= 0.2f;
            }

            if (kbState.IsKeyDown(Keys.Escape))
            {
                if(isFullScreen)
                {
                    isFullScreen = false;
                }
                else
                {
                    isFullScreen = true;
                }
                graphics.IsFullScreen = isFullScreen;
                graphics.ApplyChanges();
            }


            InitCamera();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            this.DrawStar(gameTime);
            foreach (Planet p in planetArray)
            {
                this.DrawPlanet(gameTime, p);
                
                if (p.MoonArray.Length > 0)
                {
                    foreach (Moon m in p.MoonArray)
                    {
                        this.DrawMoon(gameTime, m);
                        matrixStrack.Pop();
                    }
                }
                
                matrixStrack.Pop();
            }
            
            //this.DrawEarth(gameTime);
            //this.DrawMoon(2.0f, 0.0f);

            //matrixStrack.Pop();
            //matrixStrack.Pop();
            spriteBatch.Begin();
            float y = 0.0f;
            spriteBatch.DrawString(font, planetArray[2].PlanetName + ": " + planetArray[2].MoonArray[0].MoonOrbitY, new Vector2(0.0f, 0.0f), Color.WhiteSmoke);
            foreach (Planet p in planetArray)
            {
                spriteBatch.DrawString(font, p.PlanetName + ": " + p.PlanetOrbitY, new Vector2(0.0f, y += 20), Color.WhiteSmoke);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void DrawStar(GameTime gameTime)
        {
            Matrix matScale, matRotateY, matTrans;

            matScale = Matrix.CreateScale(1.0f);
            matTrans = Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);
            

            matRotateY = Matrix.CreateRotationY(sunRotY);
            sunRotY += (float)gameTime.ElapsedGameTime.Milliseconds / 5000.0f;
            sunRotY = sunRotY % (float)(2 * Math.PI);

            world = matScale * matRotateY * matTrans;
            matrixStrack.Push(world);

            effect.World = world;
            mStar.Draw(world, view, projection);
        }

        private void DrawPlanet(GameTime gameTime, Planet planet)
        {
            Matrix matRotateY, matScale, matOrbTranslation, matOrbRotation;
            Matrix _world = matrixStrack.Peek();

            matScale = Matrix.CreateScale(planet.PlanetScale);


            matRotateY = Matrix.CreateRotationY(planet.PlanetRotY);
            planet.PlanetRotY += (float)gameTime.ElapsedGameTime.Milliseconds / 50.0f;
            planet.PlanetRotY = planet.PlanetRotY % (float)(2 * Math.PI);

            matOrbTranslation = Matrix.CreateTranslation(planet.PlanetDistanceToSun);
            planet.PlanetOrbitY += (planet.PlanetOrbitSpeed / 60); // * (float)gameTime.ElapsedGameTime.Milliseconds / 50000.0f)
            planet.PlanetOrbitY = planet.PlanetOrbitY % (float)(2 * Math.PI);
            matOrbRotation = Matrix.CreateRotationY(planet.PlanetOrbitY);


            world = matScale * matRotateY * matOrbTranslation * matOrbRotation * _world;
            matrixStrack.Push(world);

            effect.World = world;
            mStar.Draw(world, view, projection);

        }

        private void DrawMoon(GameTime gameTime, Moon moon)
        {
            Matrix matRotateY, matScale, matOrbTranslation, matOrbRotation;
            Matrix _world = matrixStrack.Peek();

            matScale = Matrix.CreateScale(moon.MoonScale);


            matRotateY = Matrix.CreateRotationY(moon.MoonRotY);
            moon.MoonRotY += (float)gameTime.ElapsedGameTime.Milliseconds / 5000.0f;
            moon.MoonRotY = moon.MoonRotY % (float)(2 * Math.PI);

            matOrbTranslation = Matrix.CreateTranslation(moon.MoonDistanceToSun);
            moon.MoonOrbitY += (moon.MoonOrbitSpeed / 60); //  * (float)gameTime.ElapsedGameTime.Milliseconds / 5000.0f 
            moon.MoonOrbitY = moon.MoonOrbitY % (float)(2 * Math.PI);
            matOrbRotation = Matrix.CreateRotationY(moon.MoonOrbitY);


            world = matScale * matRotateY * matOrbTranslation * matOrbRotation * _world;
            matrixStrack.Push(world);

            effect.World = world;
            mStar.Draw(world, view, projection);
        }

        /*
        private void DrawEarth(GameTime gameTime)
        {

            Matrix matScale, matRotateY, matTrans;
            //Skaleringsmatrise:
            matScale = Matrix.CreateScale(0.5f);
            //Translasjonsmatrise:
            matTrans = Matrix.CreateTranslation(0.0f, 0.0f, -3.0f);
            //Rotasjonsmatrise (y-akse):
            matRotateY = Matrix.CreateRotationY(earthRotY);
            earthRotY += (float)gameTime.ElapsedGameTime.Milliseconds / 5000.0f;
            earthRotY = earthRotY % (float)(2 * Math.PI);
            //Kumulativ world-matrise;
            world = matScale * matRotateY * matTrans;
            //Legger på matrisestack:
            matrixStrack.Push(world);
            //Setter jordas world-matrise:
            effect.World = world;
            //Tegner jorda:
            mEarth.Draw(world, view, projection);
        }

        private void DrawMoon(GameTime gameTime)
        {
            Matrix matScale, matRotateY, matTrans;
            //Skaleringsmatrise:
            matScale = Matrix.CreateScale(0.5f);
            //Translasjonsmatrise:
            matTrans = Matrix.CreateTranslation(0.0f, 0.0f, -3.0f);
            //Rotasjonsmatrise (y-akse):
            matRotateY = Matrix.CreateRotationY(earthRotY);
            earthRotY += (float)TargetElapsedTime.Milliseconds / 5000.0f;
            earthRotY = earthRotY % (float)(2 * Math.PI);
            //Kumulativ world-matrise;
            world = matScale * matRotateY * matTrans;
            //Legger på matrisestack:
            matrixStrack.Push(world);
            //Setter jordas world-matrise:
            effect.World = world;
            //Tegner jorda:
            mMoon.Draw(world, view, projection);
        }
        private void DrawMoon(float orbitX, float orbitY)
        {
            //Leser toppen av matrisestacken (world-matrisa for jorda):
            Matrix _world = matrixStrack.Peek();
            Matrix matRotateY, matScale, matOrbTranslation, matOrbRotation;
            //Skaleringsmatrisa:
            matScale = Matrix.CreateScale(0.5f);
            //Rotasjonsmatrisa (om y):
            matRotateY = Matrix.CreateRotationY(moonRotY);
            moonRotY += 0.2f / 400.0f;
            moonRotY = moonRotY % (float)(2 * Math.PI);
            //Månen skal gå i bane - Orbit - en translasjon etterfulgt av en rotasjon:
            matOrbTranslation = Matrix.CreateTranslation(orbitX, orbitY, 0.0f);
            moonOrbitY += (48.5f/60) * (float)TargetElapsedTime.Milliseconds / 50.0f;
            moonOrbitY = moonOrbitY % (float)(2 * Math.PI);
            matOrbRotation = Matrix.CreateRotationY(moonOrbitY);
            //Kumulativ world-transformasjonen - inkludert jordas world-transformasjon:
            world = matScale * matRotateY * matOrbTranslation * matOrbRotation * _world;
            //Legger på stack:
            matrixStrack.Push(world);
            //Setter world-matrise for månen:
            effect.World = world;
            //Tegner månen:
            mMoon.Draw(world, view, projection);
        }
        */

    }
}
