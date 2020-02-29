using GlmNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
namespace Graphics
{
    class bullet
    {
        Model3D bull;
        float movement_x, movement_z, movement_y;
        vec3 camDir;

        
        


        public bullet(Model3D bullet , Camera cam)
        {
            this.bull = bullet;
            this.movement_x = cam.GetCameraPosition().x;
            this.movement_y = cam.GetCameraPosition().y;
            this.movement_z = cam.GetCameraPosition().z;
            this.bull.transmatrix = glm.translate(new mat4(1), new vec3(movement_x , movement_y-5, movement_z ));
            this.bull.rotmatrix = glm.rotate(3.1412f + cam.mAngleX, new vec3(0, 1, 0));
        }

        public vec2 GetBulletPosition()
        {
            //gunShot = new SoundPlayer("D:\\death_jack_02.wav");
            //gunShot.PlayLooping();
            vec2 pos = new vec2(movement_x, movement_z);
            return pos;
        }
        public void fire(Camera cam)
        {

            float mx =  4* cam.GetLookDirection().x;
            float mz =  4* cam.GetLookDirection().z;
            bull.transmatrix = glm.translate(new mat4(1), new vec3(movement_x += mx, movement_y, movement_z += mz));
            bull.rotmatrix = glm.rotate(3.1412f + cam.mAngleX, new vec3(0, 1, 0));
        }

        public void reset(Camera cam)
        {
            movement_x = cam.GetCameraPosition().x;
            movement_y = cam.GetCameraPosition().y;
            movement_z = cam.GetCameraPosition().z;
            bull.transmatrix = glm.translate(new mat4(1), new vec3(movement_x, movement_y - 5, movement_z));
            bull.rotmatrix = glm.rotate(3.1412f + cam.mAngleX, new vec3(0, 1, 0));
        }


    }
}
