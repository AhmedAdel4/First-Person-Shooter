using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using Graphics._3D_Models;
using Tao.OpenGl;
using GlmNet;

namespace Graphics
{
    class zombie_movement
    {
        md2LOL model;
        float movement_x, movement_z;
        int count, val;
        float speed;
        Random rnd;
        bool dead;

        int max;
        
       public zombie_movement(md2LOL model, float movement_x, float movement_z, float speed)
        {
            this.model = model;
            this.model.StartAnimation(animType_LOL.RUN);
            this.movement_x = movement_x;
            this.movement_z = movement_z;
            this.count = 0;
            this.speed = speed;
            this.rnd = new Random();
            val = rnd.Next(1, 8);
            max = rnd.Next(300, 800);
            dead = false;
        }


        public vec2 beast_pos()
        {
            vec2 pos = new vec2(movement_x, movement_z);
            return pos;
        }

        public bool kill_player(Camera cam)
        {
            float x = cam.GetCameraPosition().x;
            float z = cam.GetCameraPosition().z;
            float distance = (float)Math.Sqrt( ( (x - movement_x) * (x - movement_x) ) + ( (z - movement_z) * (z - movement_z) ) );
            if (distance < 200 )
                return true;
            return false;
        }

        public void die(bool d)
        {
            if(d)
            {
                dead = true;
                model.TranslationMatrix = glm.translate(new mat4(1), new vec3(5000, 5000, 5000));
                movement_x = 5000;
                movement_z = 5000;
            }
        }



        public bool hit(Camera cam)
        {
            float x = cam.GetCameraPosition().x;
            float z = cam.GetCameraPosition().z;
            float distance = (float)Math.Sqrt(((x - movement_x) * (x - movement_x)) + ((z - movement_z) * (z - movement_z)));
            if (distance < 10)
                return true;
            else
                return false;
        }

        public void move_randomly(Camera cam)
        {
            if (!dead)
            {


                List<mat4> list = new List<mat4>();
                if (kill_player(cam))
                {
                    vec3 beast_pos = new vec3(movement_x, 0, movement_z);
                    vec3 dir_beast = beast_pos - cam.GetCameraPosition();
                    dir_beast = glm.normalize(dir_beast);


                    float mx = 0.4f * dir_beast.x;
                    float mz = 0.4f * dir_beast.z;

                    model.TranslationMatrix = glm.translate(new mat4(1), new vec3(movement_x -= mx, 0, movement_z -= mz));
                    list.Add(glm.rotate((float)((-90f / 180) * Math.PI), new vec3(1, 0, 0)));
                    list.Add(glm.rotate((3.1412f + cam.mAngleX) + 3.1412f, new vec3(0, 1, 0)));
                    mat4 matrix = MathHelper.MultiplyMatrices(list);
                    model.rotationMatrix = matrix;
                }
                else
                {

                    if (count > max)
                    {
                        count = 0;
                        val = rnd.Next(1, 8);
                    }




                    if (val == 1)
                    {
                        model.TranslationMatrix = glm.translate(new mat4(1), new vec3(movement_x += speed, 0, movement_z));
                        list.Add(glm.rotate((float)((-90f / 180) * Math.PI), new vec3(1, 0, 0)));
                        list.Add(glm.rotate((float)((-90f / 180) * Math.PI), new vec3(0, -1, 0)));
                        mat4 matrix = MathHelper.MultiplyMatrices(list);
                        model.rotationMatrix = matrix;
                    }
                    else if (val == 7)
                    {
                        model.TranslationMatrix = glm.translate(new mat4(1), new vec3(movement_x -= speed, 0, movement_z));
                        list.Add(glm.rotate((float)((-90f / 180) * Math.PI), new vec3(1, 0, 0)));
                        list.Add(glm.rotate((float)((-270f / 180) * Math.PI), new vec3(0, -1, 0)));
                        mat4 matrix = MathHelper.MultiplyMatrices(list);
                        model.rotationMatrix = matrix;
                    }
                    else if (val == 8)
                    {
                        model.TranslationMatrix = glm.translate(new mat4(1), new vec3(movement_x, 0, movement_z -= speed));
                        list.Add(glm.rotate((float)((-90f / 180) * Math.PI), new vec3(1, 0, 0)));
                        list.Add(glm.rotate((float)((-180f / 180) * Math.PI), new vec3(0, -1, 0)));
                        mat4 matrix = MathHelper.MultiplyMatrices(list);
                        model.rotationMatrix = matrix;
                    }
                    else if (val == 2)
                    {
                        model.TranslationMatrix = glm.translate(new mat4(1), new vec3(movement_x, 0, movement_z += speed));

                        list.Add(glm.rotate((float)((-90f / 180) * Math.PI), new vec3(1, 0, 0)));
                        list.Add(glm.rotate((float)((-360f / 180) * Math.PI), new vec3(0, -1, 0)));
                        mat4 matrix = MathHelper.MultiplyMatrices(list);
                        model.rotationMatrix = matrix;
                    }
                    else if (val == 3)
                    {
                        model.TranslationMatrix = glm.translate(new mat4(1), new vec3(movement_x += speed, 0, movement_z += speed));

                        list.Add(glm.rotate((float)((-90f / 180) * Math.PI), new vec3(1, 0, 0)));
                        list.Add(glm.rotate((float)((-45f / 180) * Math.PI), new vec3(0, -1, 0)));
                        mat4 matrix = MathHelper.MultiplyMatrices(list);
                        model.rotationMatrix = matrix;
                    }
                    else if (val == 4)
                    {
                        model.TranslationMatrix = glm.translate(new mat4(1), new vec3(movement_x += speed, 0, movement_z -= speed));

                        list.Add(glm.rotate((float)((-90f / 180) * Math.PI), new vec3(1, 0, 0)));
                        list.Add(glm.rotate((float)((-135f / 180) * Math.PI), new vec3(0, -1, 0)));
                        mat4 matrix = MathHelper.MultiplyMatrices(list);
                        model.rotationMatrix = matrix;
                    }
                    else if (val == 5)
                    {
                        model.TranslationMatrix = glm.translate(new mat4(1), new vec3(movement_x -= speed, 0, movement_z += speed));

                        list.Add(glm.rotate((float)((-90f / 180) * Math.PI), new vec3(1, 0, 0)));
                        list.Add(glm.rotate((float)((-315f / 180) * Math.PI), new vec3(0, -1, 0)));
                        mat4 matrix = MathHelper.MultiplyMatrices(list);
                        model.rotationMatrix = matrix;
                    }
                    else if (val == 6)
                    {
                        model.TranslationMatrix = glm.translate(new mat4(1), new vec3(movement_x -= speed, 0, movement_z -= speed));

                        list.Add(glm.rotate((float)((-90f / 180) * Math.PI), new vec3(1, 0, 0)));
                        list.Add(glm.rotate((float)((-225f / 180) * Math.PI), new vec3(0, -1, 0)));
                        mat4 matrix = MathHelper.MultiplyMatrices(list);
                        model.rotationMatrix = matrix;
                    }



                    model.scaleMatrix = glm.scale(new mat4(1), new vec3(0.8f, 0.8f, 0.8f));
                    count++;

                }
            }
        }
    }
}
