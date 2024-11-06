using UnityEngine;

namespace Radical
{
    public struct Bone
    {
        public string name; 
        public Transform bone; 
        public Quaternion restRotation; 
        public int side;

        public Bone(Transform bone, string name, int side = -1)
        {
            this.name = name;
            this.bone = bone;
            if (bone == null)
            {
                Debug.Log("No bone found for " + name);
                this.name = "None";
                restRotation = Quaternion.identity;
                
                this.side = 0;
            }
            else
            {
                restRotation = bone.localRotation;
                this.side = side;
            }
        }
        public void ApplyRotation(float[] rotationValues)
        {
            if (bone == null)
            {
                Debug.Log(name + ": I have no bone");
                return;// Quaternion.identity;
            }
            float x = rotationValues[1];
            float y = rotationValues[2] * side;
            float z = rotationValues[3] * side;
            float w = rotationValues[0];
            Quaternion rotation = new Quaternion(x, y, z, w);
            bone.localRotation = restRotation * rotation;
            //return result;
        }

        public Quaternion GetActualRotation(float[] rotationValues)
        {
            float x = rotationValues[1];
            float y = rotationValues[2] * side;
            float z = rotationValues[3] * side;
            float w = rotationValues[0];
            Quaternion rotation = new Quaternion(x, y, z, w);
            return restRotation * rotation;
        }

        public void ApplyRotation(Quaternion rotation)
        {
            // baked frames store the definite rotation, so we don't need to calculate
            bone.localRotation = restRotation * rotation;
        }

        //retargetting
        public void Retarget(Transform child)
        {
            bone.position = child.position;
            bone.localRotation = restRotation;
            child.SetParent(bone, true);
        }
    }
}
