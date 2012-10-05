using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EarthMoon
{
    class Moon
    {
        private String moonName;
        private Model moonModel;
        private Vector3 moonDistanceToSun;

        private float moonScale;

        private float moonRotSpeed;
        private float moonRotY = 0.0f;

        private float moonOrbitSpeed;
        private float moonOrbitY = 0.0f;

        private int orbitCount = 0;

        

        public Moon(String name, float scale, Vector3 distance, float rotSpeed, float orbitSpeed)
        {
            this.moonName = name;
            //this.moonModel = model;
            this.moonScale = scale;
            this.moonDistanceToSun = distance;
            this.moonRotSpeed = rotSpeed;
            this.moonOrbitSpeed = orbitSpeed;

        }

        public String MoonName
        {
            get { return moonName; }
            set { moonName = value; }
        }

        public Model MoonModel
        {
            get { return moonModel; }
            set { moonModel = value; }
        }

        public Vector3 MoonDistanceToSun
        {
            get { return moonDistanceToSun; }
            set { moonDistanceToSun = value; }
        }

        public float MoonRotY
        {
            get { return moonRotY; }
            set { moonRotY = value; }
        }

        public float MoonOrbitSpeed
        {
            get { return moonOrbitSpeed; }
            set { moonOrbitSpeed = value; }
        }

        public float MoonRotSpeed
        {
            get { return moonRotSpeed; }
            set { moonRotSpeed = value; }
        }

        public float MoonOrbitY
        {
            get { return moonOrbitY; }
            set { moonOrbitY = value; }
        }

        public float MoonScale
        {
            get { return moonScale; }
            set { moonScale = value; }
        }

        public int OrbitCount
        {
            get { return orbitCount; }
            set { orbitCount = value; }
        }
    }
}
