using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Diagnostics;
using System;

namespace Graphics
{
    public partial class GraphicsForm : Form
    {
        Renderer renderer = new Renderer();
        Thread MainLoopThread;
        public static bool start = false;
        public static int count = 0;

        float deltaTime;
        public GraphicsForm()
        {
            InitializeComponent();
            simpleOpenGlControl1.InitializeContexts();            
            initialize();
            deltaTime = 0.005f;
            MainLoopThread = new Thread(MainLoop);
            MainLoopThread.Start();

        }
        void initialize()
        {
            renderer.Initialize();   
        }
        void MainLoop()
        {
            while (true)
            {
                
                if (renderer.lose || renderer.win)
                    this.Close();
                renderer.Draw(start);
                renderer.Update(deltaTime);
                simpleOpenGlControl1.Refresh();
            }
        }
        private void GraphicsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            renderer.CleanUp();
            MainLoopThread.Abort();
        }

        private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
        {
            renderer.Draw(start);
            renderer.Update(deltaTime);
        }

        private void simpleOpenGlControl1_KeyPress(object sender, KeyPressEventArgs e)
        {

            float speed = 10f;
            if (e.KeyChar == 'a')
            {
                float z = renderer.cam.GetCameraPosition().z;
                float x = renderer.cam.GetCameraPosition().x;
                if (x > -1485.264 && x < 1485.264 && z < 1485.264 && z > -1485.264)
                {
                    renderer.cam.Strafe(-speed);
                }
                else
                {
                    renderer.cam.Strafe(speed);
                }
            }
               
            if (e.KeyChar == 'd')
            {
                float z = renderer.cam.GetCameraPosition().z;
                float x = renderer.cam.GetCameraPosition().x;
                if (x > -1485.264 && x < 1485.264 && z < 1485.264 && z > -1485.264)
                {
                    renderer.cam.Strafe(speed);
                }
                else
                {
                    renderer.cam.Strafe(-speed);
                }
            }
                
            if (e.KeyChar == 's')
            {
                float z = renderer.cam.GetCameraPosition().z;
                float x = renderer.cam.GetCameraPosition().x;
                if (x > -1485.264 && x < 1485.264 && z < 1485.264 && z > -1485.264)
                {
                    renderer.cam.Walk(-speed);
                }
                else
                {
                    renderer.cam.Walk(speed);
                }
            }
                
            if (e.KeyChar == 'w')
            {
                float z = renderer.cam.GetCameraPosition().z;
                float x = renderer.cam.GetCameraPosition().x;
                if (x > -1485.264 && x < 1485.264 && z < 1485.264 && z > -1485.264)
                {
                    renderer.cam.Walk(speed);
                }
                else
                {
                    renderer.cam.Walk(-speed);
                }
            }

            if (e.KeyChar == 'e')
            {
                renderer.shot++;
                if (renderer.shot > 2)
                    renderer.shot = 1;
            }
         
            if(e.KeyChar == 'q')
            {
                if (renderer.swapGun == true)
                {
                    renderer.swapGun = false;
                }
                else
                {
                    renderer.swapGun = true;
                }
            }
            if (e.KeyChar == 'z')
                renderer.cam.Fly(-speed);
            if (e.KeyChar == 'c')
                renderer.cam.Fly(speed);

        }

        float prevX, prevY;


        private void simpleOpenGlControl1_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (renderer.is_game) 
            {
                Cursor.Current = Cursors.Cross;
                float speed = 0.05f;
                float delta = e.X - prevX;
                if (delta > 2)
                    renderer.cam.Yaw(-speed);
                else if (delta < -2)
                    renderer.cam.Yaw(speed);


             

                MoveCursor();

            }
        }

     

     

        private void simpleOpenGlControl1_MouseClick(object sender, MouseEventArgs e)
        {

            if ((e.X > 40 && e.X < 279) && (e.Y > 289 && e.Y < 416))
            {
                start = true;
                

            }
           
        }

        private void GraphicsForm_Load(object sender, EventArgs e)
        {

        }

        private void simpleOpenGlControl1_Load(object sender, EventArgs e)
        {

        }

        private void MoveCursor()
        {
            this.Cursor = new Cursor(Cursor.Current.Handle);
            Point p = PointToScreen(simpleOpenGlControl1.Location);
            Cursor.Position = new Point(simpleOpenGlControl1.Size.Width/2+p.X, simpleOpenGlControl1.Size.Height/2+p.Y);
            Cursor.Clip = new Rectangle(this.Location, this.Size);
            prevX = simpleOpenGlControl1.Location.X+simpleOpenGlControl1.Size.Width/2;
            prevY = simpleOpenGlControl1.Location.Y + simpleOpenGlControl1.Size.Height / 2;
        }
    }
}
