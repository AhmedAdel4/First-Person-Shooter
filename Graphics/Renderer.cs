using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;
using GlmNet;
using System.IO;
using Graphics._3D_Models;
using System.Windows.Forms;
using System.Media;

namespace Graphics
{
    class Renderer
    {
        #region Shader variable
        Shader sh;
        int transID;
        int viewID;
        int projID;
        int EyePositionID;
        int AmbientLightID;
        int DataID;

        mat4 ProjectionMatrix;
        mat4 ViewMatrix;
        public Camera cam;

        #endregion
       

        #region Skybox variables
        uint BottomfacevertexID;
        uint surface;
        uint UpfacevertexID;
        uint LeftfacevertexID;
        uint RightfacevertexID;
        uint FrontfacevertexID;
        uint BackfacevertexID;
        Texture Bottom;
        Texture Bottom2;

        Texture UP;
        Texture Left;
        Texture Right;
        Texture Front;
        Texture Back;
        mat4 modelmatrix;
        #endregion

        #region Models variables
       public Model3D car, car2 , gun , AWP;
        Model3D[] tree = new Model3D[100];
        Model3D building, building2, bullet , firstAid;
        public md2LOL m, s, b;
       public zombie_movement move_m;
        zombie_movement move_s;
        zombie_movement move_b;
        bullet bull;
        public int shot;
        bool die_m, die_s, die_b;
        vec3 kitPoint;
        public bool swapGun;
        public bool win;
        #endregion


        #region screens variables
        public bool is_game;
        // load scren
        Texture BackgroudTex;
        Texture StartTex;
        mat4 BackLoadMatrix;
        mat4 FrontLoadMatrix;

        // start screen
        Texture StartBackgroudTex;
        Texture StartStartTex;
        mat4 BackStartMatrix;
        mat4 FrontStartMatrix;
        #endregion


        #region health bar variables
        //2D over 3D variables
        Texture hp;
        Texture bhp;
        uint hpID;
        mat4 healthbar;
        mat4 backhealthbar;
        public bool lose;
        /// <summary>
        Texture load;
        mat4 loadmatrix;
        float scale;
        /// </summary>
        Shader shader2D;
        int mloc;
        float scalef;
        #endregion


        public void Initialize()
        {
            

            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");
            shader2D = new Shader(projectPath + "\\Shaders\\2Dvertex.vertexshader", projectPath + "\\Shaders\\2Dfrag.fragmentshader");


            #region 3D Models initialization
            swapGun = false;
            die_m = false;
            die_s = false;
            die_b = false;
            win = false;
            shot = 0;
            //=--------------------------zombie

            m = new md2LOL(projectPath + "\\ModelFiles\\zombie.md2");
            //
            move_m = new zombie_movement(m,0,0,0.1f);

            s = new md2LOL(projectPath + "\\ModelFiles\\zombie.md2");
            //
            move_s = new zombie_movement(s, -50, 0, 0.1f);

            b = new md2LOL(projectPath + "\\ModelFiles\\zombie.md2");
            //
            move_b = new zombie_movement(b, 50, -250, 0.1f);

            /////////////////////////////car
            gun = new Model3D();
            gun.LoadFile(projectPath + "\\ModelFiles\\obj", 3, "m4a1_s.obj");

            AWP = new Model3D();
            AWP.LoadFile(projectPath + "\\ModelFiles\\GUN2\\Dragon_Lore"  , 3 , "AWP_Dragon_Lore.obj");

            firstAid = new Model3D();
            firstAid.LoadFile(projectPath + "\\ModelFiles\\First Aid",3, "Kit.obj");

            /// bullet
            bullet = new Model3D();
            bullet.LoadFile(projectPath + "\\ModelFiles\\bullet", 2, "bullet.obj");


            car = new Model3D();
            car.LoadFile(projectPath + "\\ModelFiles\\car", 3, "dpv.obj");
            car.scalematrix = glm.scale(new mat4(1), new vec3(0.15f, 0.15f, 0.15f));
            car.transmatrix = glm.translate(new mat4(1), new vec3(50, 1, 0));
            car.rotmatrix = glm.rotate(3.1412f, new vec3(0, 1, 0));
            /////////////////////////////car2
            car2 = new Model3D();
            car2.LoadFile(projectPath + "\\ModelFiles\\car", 3, "dpv.obj"); car2.scalematrix = glm.scale(new mat4(1), new vec3(0.15f, 0.15f, 0.15f));
            car2.transmatrix = glm.translate(new mat4(1), new vec3(-160, 1, -500));
            car2.rotmatrix = glm.rotate(3.1412f, new vec3(0, 1, 0));
            //------------------------building
            building = new Model3D();
            building.LoadFile(projectPath + "\\ModelFiles\\building", 3, "Building 02.obj");
            building.scalematrix = glm.scale(new mat4(1), new vec3(75.15f, 50.15f, 50.15f));
            building.transmatrix = glm.translate(new mat4(1), new vec3(0, 1, 0));
            building.rotmatrix = glm.rotate(3.1412f, new vec3(0, 1, 0));
            //--------------------------------building 2
            building2 = new Model3D();
            building2.LoadFile(projectPath + "\\ModelFiles\\building", 3, "Building 02.obj");
            building2.scalematrix = glm.scale(new mat4(1), new vec3(75.15f, 50.15f, 50.15f));
            building2.transmatrix = glm.translate(new mat4(1), new vec3(0, 1, -1000));
            building2.rotmatrix = glm.rotate(3.1412f, new vec3(0, 1, 0));

            //To do 1.2 ==> Initialize "tree" 3D Model
            for (int i = 0; i < 10; i++)
            {
                tree[i] = new Model3D();
                tree[i].LoadFile(projectPath + "\\ModelFiles\\tree", 2, "Tree.obj");
                tree[i].scalematrix = glm.scale(new mat4(1), new vec3(20f, 20f, 20f));
                tree[i].transmatrix = glm.translate(new mat4(1), new vec3(300, 1, -100 - (i * 150)));

            }
            for (int i = 0; i < 10; i++)
            {
                tree[i + 10] = new Model3D();
                tree[i + 10].LoadFile(projectPath + "\\ModelFiles\\tree", 2, "Tree.obj");
                tree[i + 10].scalematrix = glm.scale(new mat4(1), new vec3(20f, 20f, 20f));
                tree[i + 10].transmatrix = glm.translate(new mat4(1), new vec3(-300, 1, -100 - (i * 150)));

            }

             kitPoint = new vec3(1, -9, 220);
            firstAid.transmatrix = glm.translate(new mat4(1), new vec3(1, -9,150 ));
            firstAid.scalematrix = glm.scale(new mat4(1), new vec3(10f, 10f, 10f));

            #endregion




            #region Skybox and Ground Vertices
            Bottom = new Texture(projectPath + "\\Textures\\bottom.png", 2);
            Bottom2 = new Texture(projectPath + "\\Textures\\Ground.jpg", 2);

            UP = new Texture(projectPath + "\\Textures\\top.png", 2);
            Left = new Texture(projectPath + "\\Textures\\left.png", 2);
            Right = new Texture(projectPath + "\\Textures\\right.png", 2);
            Front = new Texture(projectPath + "\\Textures\\back.png", 2);
            Back = new Texture(projectPath + "\\Textures\\front.png", 2);

            float[] Ground = {
                     -1,0,-1,
                     0,0,1,
                     0,0,
                     0,1,0,//normal

                     1,0,-1,
                     0,0,1,
                     1,0,
                     0,1,0,

                     1,0,1,
                     0,0,1,
                     1,1,
                     0,1,0,

                     1,0,1,
                     0,0,1,
                     1,1,
                     0,1,0,

                     -1,0,-1,
                     0,0,1,
                     0,0,
                     0,1,0,

                    -1,0,1,
                     0,0,1,
                     0,1,
                     0,1,0
                };

                float[] Bottom_face = {
                     -1,-1,-1,
                     0,0,1,
                     0,0,
                     0,1,0,//normal

                     1,-1,-1,
                     0,0,1,
                     1,0,
                     0,1,0,

                     1,-1,1,
                     0,0,1,
                     1,1,
                     0,1,0,

                     1,-1,1,
                     0,0,1,
                     1,1,
                     0,1,0,

                     -1,-1,-1,
                     0,0,1,
                     0,0,
                     0,1,0,

                    -1,-1,1,
                     0,0,1,
                     0,1,
                     0,1,0
                };

                float[] Up_face = {
                     -1,1,1,
                     0,0,1,
                     0,0,
                     0,-1,0,//normal

                     -1,1,-1,
                     0,0,1,
                     0,1,
                     0,-1,0,

                     1,1,1,
                     0,0,1,
                     1,0,
                     0,-1,0,

                     1,1,1,
                     0,0,1,
                     1,0,
                     0,-1,0,

                     -1,1,-1,
                     0,0,1,
                     0,1,
                     0,-1,0,

                     1,1,-1,
                     0,0,1,
                     1,1,
                     0,-1,0
                };

                float[] Left_face = {
                     -1,1,1,
                     0,0,1,
                     0,0,
                     1,0,0,//normal

                     -1,1,-1,
                     0,0,1,
                     1,0,
                     1,0,0,

                     -1,-1,-1,
                     0,0,1,
                     1,1,
                     1,0,0,

                     -1,-1,-1,
                     0,0,1,
                     1,1,
                     1,0,0,

                     -1,1,1,
                     0,0,1,
                     0,0,
                     1,0,0,

                    -1,-1,1,
                     0,0,1,
                     0,1,
                     1,0,0
                };

                float[] Right_face = {
                     1,1,1,
                     0,0,1,
                     1,0,
                     -1,0,0,//normal

                     1,1,-1,
                     0,0,1,
                     0,0,
                     -1,0,0,

                     1,-1,-1,
                     0,0,1,
                     0,1,
                     -1,0,0,

                     1,-1,-1,
                     0,0,1,
                     0,1,
                     -1,0,0,

                     1,1,1,
                     0,0,1,
                     1,0,
                     -1,0,0,

                    1,-1,1,
                     0,0,1,
                     1,1,
                     -1,0,0
                };

                float[] Back_face = {
                     -1,1,1,
                     0,0,1,
                     1,0,
                     0,0,-1,//normal

                     1,1,1,
                     0,0,1,
                     0,0,
                     0,0,-1,

                     -1,-1,1,
                     0,0,1,
                     1,1,
                     0,0,-1,

                     -1,-1,1,
                     0,0,1,
                     1,1,
                     0,0,-1,

                     1,-1,1,
                     0,0,1,
                     0,1,
                     0,0,-1,

                     1,1,1,
                     0,0,1,
                     0,0,
                     0,0,-1
                };

                float[] Front_face = {
                     -1,1,-1,
                     0,0,1,
                     0,0,
                     0,0,1,//normal

                     1,1,-1,
                     0,0,1,
                     1,0,
                     0,0,1,

                     -1,-1,-1,
                     0,0,1,
                     0,1,
                     0,0,1,

                     -1,-1,-1,
                     0,0,1,
                     0,1,
                     0,0,1,

                     1,1,-1,
                     0,0,1,
                     1,0,
                     0,0,1,

                     1,-1,-1,
                     0,0,1,
                     1,1,
                     0,0,1
                };

            BottomfacevertexID = GPU.GenerateBuffer(Bottom_face);
            surface = GPU.GenerateBuffer(Ground);

            UpfacevertexID = GPU.GenerateBuffer(Up_face);
            LeftfacevertexID = GPU.GenerateBuffer(Left_face);
            RightfacevertexID = GPU.GenerateBuffer(Right_face);
            FrontfacevertexID = GPU.GenerateBuffer(Front_face);
            BackfacevertexID = GPU.GenerateBuffer(Back_face);

            modelmatrix = glm.scale(new mat4(1), new vec3(1500, 1500, 1500));


            #endregion

            #region Health Bar init
            //2D control
            hp = new Texture(projectPath + "\\Textures\\HP.bmp", 9);
            bhp = new Texture(projectPath + "\\Textures\\BackHP.bmp", 10);
            load = new Texture(projectPath + "\\Textures\\HP.bmp", 9);
            lose = false;
            is_game = false;


            float[] squarevertices = {
                -1,1,0,
                0,0,

                1,-1,0,
                1,1,

                -1,-1,0,
                0,1,

                1,1,0,
                1,0,

                -1,1,0,
                0,0,

                1,-1,0,
                1,1
            };

            hpID = GPU.GenerateBuffer(squarevertices);
            backhealthbar = MathHelper.MultiplyMatrices(new List<mat4>(){
                glm.scale(new mat4(1), new vec3(0.5f,0.1f, 1)), glm.translate(new mat4(1),new vec3(-0.5f,0.9f,0)) });
            scalef = 1;
            scale = 1;

            #endregion


            #region Load screen vertices and texture vertices

            BackgroudTex = new Texture(projectPath + "\\Textures\\background.png", 1);
            StartTex = new Texture(projectPath + "\\Textures\\load.jpg", 1);


            BackLoadMatrix = MathHelper.MultiplyMatrices(new List<mat4>(){
                  glm.translate(new mat4(1),new vec3(0,0,0)) });

            FrontLoadMatrix = MathHelper.MultiplyMatrices(new List<mat4>(){
                glm.scale(new mat4(1), new vec3(0.3f,0.3f, 1)), glm.translate(new mat4(1),new vec3(0,-0.4f,0)) });
            #endregion

            #region Start screen vertices and texture vertices

            StartBackgroudTex = new Texture(projectPath + "\\Textures\\Startbackground.jpg", 1);
            StartStartTex = new Texture(projectPath + "\\Textures\\start.png", 1);


            BackStartMatrix = MathHelper.MultiplyMatrices(new List<mat4>(){
                 glm.translate(new mat4(1),new vec3(0,0,0)) });

            FrontStartMatrix = MathHelper.MultiplyMatrices(new List<mat4>(){
                glm.scale(new mat4(1), new vec3(0.3f,0.3f, 1)), glm.translate(new mat4(1),new vec3(-0.6f,-0.6f,0)) });
            #endregion





            #region Send data to 2D shader
            shader2D.UseShader();
            mloc = Gl.glGetUniformLocation(shader2D.ID, "model");
            #endregion




            #region Send data to 3D shader
            sh.UseShader();
            Gl.glClearColor(0, 0, 0.4f, 1);
            cam = new Camera();
            cam.Reset(0, 34, 100, 0, 0, 0, 0, 1, 0);
            bull = new bullet(bullet, cam);
            ProjectionMatrix = cam.GetProjectionMatrix();
            ViewMatrix = cam.GetViewMatrix();

            transID = Gl.glGetUniformLocation(sh.ID, "trans");
            projID = Gl.glGetUniformLocation(sh.ID, "projection");
            viewID = Gl.glGetUniformLocation(sh.ID, "view");
            //ambientLight
            //============
            AmbientLightID = Gl.glGetUniformLocation(sh.ID, "aL");
            vec3 ambientLight = new vec3(0.8f, 0.8f, 0.8f);
            Gl.glUniform3fv(AmbientLightID, 1, ambientLight.to_array());


            //LightPosition
            //==============
            int LightPositionID = Gl.glGetUniformLocation(sh.ID, "LightPosition_worldspace");
            vec3 lightPosition = new vec3(1.0f, 20f, 4.0f);
            Gl.glUniform3fv(LightPositionID, 1, lightPosition.to_array());

            //eye position.
            EyePositionID = Gl.glGetUniformLocation(sh.ID, "EyePosition_worldspace");
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glDepthFunc(Gl.GL_LESS);

            //attenuation & specularExponent
            //==================================
            float attenuation = 200, specularExponent = 500;
            vec2 data = new vec2(attenuation, specularExponent);
            DataID = Gl.glGetUniformLocation(sh.ID, "data");
            Gl.glUniform2fv(DataID, 1, data.to_array());
            #endregion


        }


        
        public void Draw(bool start)
        {
            
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            if(is_game)
            {
               
                #region Drawing 3D world


                #region game
                sh.UseShader();
              

                Gl.glUniformMatrix4fv(projID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());
                Gl.glUniformMatrix4fv(viewID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
                Gl.glUniform3fv(EyePositionID, 1, cam.GetCameraPosition().to_array());


                
                move_m.move_randomly(cam);
               
            
                move_b.move_randomly(cam);
            
                move_s.move_randomly(cam);


                m.Draw(transID);
                b.Draw(transID);
                s.Draw(transID);
                car.Draw(transID);
                car2.Draw(transID);
                
                
                // bullet section
                bullet.scalematrix = glm.scale(new mat4(1), new vec3(0.3f, 0.3f, 0.3f));
                bullet.Draw(transID);
                
                if (shot == 1)
                {
                    bull.fire(cam);
                    float distancem = (float)Math.Sqrt(((bull.GetBulletPosition().x - move_m.beast_pos().x) * (bull.GetBulletPosition().x - move_m.beast_pos().x)) + ((bull.GetBulletPosition().y - move_m.beast_pos().y) * (bull.GetBulletPosition().y - move_m.beast_pos().y)));
                    float distanceb = (float)Math.Sqrt(((bull.GetBulletPosition().x - move_b.beast_pos().x) * (bull.GetBulletPosition().x - move_b.beast_pos().x)) + ((bull.GetBulletPosition().y - move_b.beast_pos().y) * (bull.GetBulletPosition().y - move_b.beast_pos().y)));
                    float distances = (float)Math.Sqrt(((bull.GetBulletPosition().x - move_s.beast_pos().x) * (bull.GetBulletPosition().x - move_s.beast_pos().x)) + ((bull.GetBulletPosition().y - move_s.beast_pos().y) * (bull.GetBulletPosition().y - move_s.beast_pos().y)));
                    if (distancem <= 20)
                    {
                        die_m = true;
                        move_m.die(true);
                        bull.reset(cam);
                        shot = 0;
                    }
                    if (distances <= 20)
                    {
                        die_s = true;
                        move_s.die(true);
                        bull.reset(cam);
                        shot = 0;
                    }
                    if (distanceb <= 20)
                    {
                        move_b.die(true);
                        bull.reset(cam);
                        shot = 0;
                        die_b = true;
                    }
                }
                else
                {
                    bull.reset(cam);
                }

                if(swapGun)
                {
                    AWP.scalematrix = glm.scale(new mat4(1), new vec3(0.06f, 0.06f, 0.06f));
                    vec3 p = cam.GetCameraPosition();
                    AWP.rotmatrix = glm.rotate(3.1412f + cam.mAngleX, new vec3(0, 1, 0));
                    float x = p.x;
                    float y = p.y;
                    float z = p.z;
                    AWP.transmatrix = glm.translate(new mat4(1), new vec3(x,y-2,z));
                    AWP.Draw(transID);
                }
                else
                {

                    // gun section
                    gun.scalematrix = glm.scale(new mat4(1), new vec3(0.06f, 0.06f, 0.06f));
                    vec3 p = cam.GetCameraPosition();
                    gun.rotmatrix = glm.rotate(3.1412f + cam.mAngleX, new vec3(0, 1, 0));
                    gun.transmatrix = glm.translate(new mat4(1), p);
                    gun.Draw(transID);
                }


                firstAid.Draw(transID);

                
                
                   
               


                for (int j = 0; j < 20; j++)
                {
                    tree[j].Draw(transID);
                }
                building.Draw(transID);

                building2.Draw(transID);


                Draw_side_face(surface, modelmatrix, Bottom2);
                Draw_side_face(BottomfacevertexID, modelmatrix, Bottom);

                Draw_side_face(UpfacevertexID, modelmatrix, UP);
                Draw_side_face(LeftfacevertexID, modelmatrix, Left);

                Draw_side_face(RightfacevertexID, modelmatrix, Right);
                Draw_side_face(FrontfacevertexID, modelmatrix, Front);
                Draw_side_face(BackfacevertexID, modelmatrix, Back);
                
                #endregion


                #region Health Bar 
                //disable depthtest (no z value in 2D)
                Gl.glDisable(Gl.GL_DEPTH_TEST);
                //use 2D shader
                shader2D.UseShader();
                //decrease the health bar by scaling down the 2D front square
                float distance = (float)Math.Sqrt(((cam.GetCameraPosition().x - kitPoint.x) * (cam.GetCameraPosition().x - kitPoint.x)) + ((cam.GetCameraPosition().z - kitPoint.z) * (cam.GetCameraPosition().z - kitPoint.z)));
                if (distance < 20)
                {
                    scalef = 1;
                }
                
                if (move_m.hit(cam) || move_b.hit(cam) || move_s.hit(cam))
                {
                    scalef -= 0.0005f;
                    if (scalef <= 0)
                        lose = true;
                }


                healthbar = MathHelper.MultiplyMatrices(new List<mat4>() {
                     glm.scale(new mat4(1), new vec3(0.48f*scalef, 0.1f, 1)), glm.translate(new mat4(1), new vec3(-0.5f-((1-scalef)*0.48f), 0.9f, 0)) });
                Draw2D_picture(hpID, backhealthbar, bhp, hp, healthbar);
                //re-enable the depthtest to draw the other 3D objects in the scene
                Gl.glEnable(Gl.GL_DEPTH_TEST);
                #endregion


                #endregion

                if(die_b && die_m && die_s)
                {
                    win = true;
                }
            }
            else
            {
                #region Drawing 2D world
                //2D controls
                //disable depthtest (no z value in 2D)
                Gl.glDisable(Gl.GL_DEPTH_TEST);
                //use 2D shader
                shader2D.UseShader();

                #region Screens
                if (start)
                {
                    Draw2D_picture(hpID, BackLoadMatrix, BackgroudTex, StartTex, FrontLoadMatrix);
                    scale -= 0.0005f;
                    if (scale < 0)
                        is_game = true;

                    loadmatrix = MathHelper.MultiplyMatrices(new List<mat4>() {
                           glm.scale(new mat4(1), new vec3(0.25f*scale, 0.07f, 1)), glm.translate(new mat4(1), new vec3(-0.27f-((0.0001f-scale)*0.25f), -0.55f, 0)) });
                    Draw2D_picture(hpID, backhealthbar, null, load, loadmatrix);

                }
                else
                {

                    Draw2D_picture(hpID, BackStartMatrix, StartBackgroudTex, StartStartTex, FrontStartMatrix);
                }
                #endregion



                //re-enable the depthtest to draw the other 3D objects in the scene
                Gl.glEnable(Gl.GL_DEPTH_TEST);

                #endregion

            }
           

        }

        public void Draw2D_picture(uint vertsID, mat4 Back_transformations, Texture Back_texture, Texture Front_texture, mat4 Front_Transformations)
        {
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, vertsID);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 5 * sizeof(float), (IntPtr)0);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 5 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            //background 
            if( Back_texture != null)
            {
                Gl.glUniformMatrix4fv(mloc, 1, Gl.GL_FALSE, Back_transformations.to_array());
                Back_texture.Bind();
                Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);

            }

            //Front
            Gl.glUniformMatrix4fv(mloc, 1, Gl.GL_FALSE, Front_Transformations.to_array());
            Front_texture.Bind();
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);
        }

        public void Draw_side_face(uint vertexID, mat4 modelmatrix, Texture texture)
        {
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, vertexID);
            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, modelmatrix.to_array());
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 11 * sizeof(float), IntPtr.Zero);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 11 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 11 * sizeof(float), (IntPtr)(6 * sizeof(float)));
            Gl.glEnableVertexAttribArray(3);
            Gl.glVertexAttribPointer(3, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 11 * sizeof(float), (IntPtr)(8 * sizeof(float)));
            texture.Bind();
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);
        }

        public void Update(float deltaTime)
        {
            cam.UpdateViewMatrix();
            ProjectionMatrix = cam.GetProjectionMatrix();
            ViewMatrix = cam.GetViewMatrix();

            m.UpdateExportedAnimation();
            s.UpdateExportedAnimation();
            b.UpdateExportedAnimation();


        }
   
        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}